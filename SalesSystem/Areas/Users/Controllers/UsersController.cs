using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SalesSystem.Areas.Users.Models;
using SalesSystem.Data;
using SalesSystem.Library;
using SalesSystem.Models;

namespace SalesSystem.Areas.Users.Controllers
{
    [Area("Users")]

    public class UsersController : Controller
    {
        private SignInManager<IdentityUser> _signInManager;
        private LUser _user;
        private static DataPaginador<InputModelRegister> models;

        public UsersController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext context)
        {
            _signInManager = signInManager;
            _user = new LUser(userManager, signInManager, roleManager, context);
        }
        public IActionResult Users(int id, String filtrar)
        {
            //if (_signInManager.IsSignedIn(User))
            //{
            Object[] objects  = new Object[3];

            var data = _user.getTUsuariosAsync(filtrar, 0);

            if (data.Result.Count > 0)
            {
                var url = Request.Scheme + "://" + Request.Host.Value;

                objects = new LPaginador<InputModelRegister>().paginador(data.Result, id, 30, "Users", "Users", "Users", url);
            }
            else
            {
                objects[0] = "No hay datos";
                objects[1] = "No hay datos";
                objects[2] = new List<InputModelRegister>();
            }
            models = new DataPaginador<InputModelRegister>
            {
                List = (List<InputModelRegister>)objects[2],
                Pagi_info = (String)objects[0],
                Pagi_navegacion = (String)objects[1],
                Input = new InputModelRegister()
            };
            return View(models);
            //}
            //else
            //{
            //    return Redirect("/");
            //}


        }
    }
}
