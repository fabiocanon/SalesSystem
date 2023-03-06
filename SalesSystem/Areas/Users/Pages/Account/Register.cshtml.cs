using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SalesSystem.Areas.Users.Models;
using SalesSystem.Data;
using SalesSystem.Library;
using System.Data;

namespace SalesSystem.Areas.Users.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private SignInManager<IdentityUser> _signInManager;
        private UserManager<IdentityUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;
        private ApplicationDbContext _context;
        private LUsersRoles _usersRole;
        private static InputModel _dataInput;
        private Uploadimage _uploadimage;
        private IWebHostEnvironment _environment;
        public RegisterModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext context,
            IWebHostEnvironment environment)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _environment = environment;
            _usersRole = new LUsersRoles();
            _uploadimage = new Uploadimage();
        }

        public void OnGet()
        {
            if (_dataInput != null)
            {
                Input = _dataInput;
                Input.rolesLista = _usersRole.getRoles(_roleManager);
                Input.AvatarImage = null;
            }
            else
            {
                Input = new InputModel
                {
                    rolesLista = _usersRole.getRoles(_roleManager)
                };
            }
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel : InputModelRegister
        {
            public IFormFile AvatarImage { get; set; }
            [TempData]
            public string ErrorMessage { get; set; }

            public List<SelectListItem> rolesLista { get; set; }
        }

        public async Task<IActionResult> OnPost()
        {
            if (await SaveAsync())
            {
                return Redirect("/Users/Users/?area=Users");
            }
            else
            {
                return Redirect("/Users/Register");
            }
        }

        private async Task<bool> SaveAsync()
        {
            _dataInput = Input;
            var valor = false;

            if (!Input.Role.Equals("Seleccione un role"))
            {
                var userList = _userManager.Users.Where(u => u.Email.Equals(Input.Email)).ToList();

                if (userList.Count.Equals(0))
                {
                    var strategy = _context.Database.CreateExecutionStrategy();
                    await strategy.ExecuteAsync(async () =>
                    {
                        using (var transaction = _context.Database.BeginTransaction())
                        {
                            try
                            {
                                var user = new IdentityUser
                                {
                                    UserName = Input.Email,
                                    Email = Input.Email,
                                    PhoneNumber = Input.PhoneNumber
                                };

                                var result = await _userManager.CreateAsync(user, Input.Password);

                                if (result.Succeeded)
                                {
                                    await _userManager.AddToRoleAsync(user, Input.Role);
                                    var dataUser = _userManager.Users.Where(u => u.Email.Equals(Input.Email)).ToList().Last();
                                    var imageByte = await _uploadimage.ByteAvatarImageAsync(Input.AvatarImage, _environment, "images/images/default.png");

                                    var t_user = new TUsers
                                    {
                                        Name = Input.Name,
                                        LastName = Input.LastName,
                                        NID = Input.NID,
                                        Email = Input.Email,
                                        IdUser = dataUser.Id,
                                        Image = imageByte,
                                    };

                                    await _context.AddAsync(t_user);
                                    _context.SaveChanges();

                                    transaction.Commit();
                                    _dataInput = null;
                                    valor = true;
                                
                                }
                                else
                                {
                                    foreach (var item in result.Errors)
                                    {
                                        _dataInput.ErrorMessage = item.Description;
                                    }
                                    valor = false;
                                    transaction.Rollback();
                                }

                            }
                            catch (Exception ex)
                            {
                                _dataInput.ErrorMessage = ex.Message + " " + ex.InnerException;
                                transaction.Rollback();
                                valor = false;
                            }
                        }
                    });
                }
                else
                {
                    _dataInput.ErrorMessage = $"El {Input.Email} ya esta registrado.";
                    valor = false;
                }

            }
            else
            {
                _dataInput.ErrorMessage = "Seleccione un rol";
                valor = false;
            }

            return valor;
        }
    }
}
