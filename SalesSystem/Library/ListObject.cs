using Microsoft.AspNetCore.Identity;
using SalesSystem.Data;

namespace SalesSystem.Library
{
    public class ListObject
    {
        public LUsersRoles _usersRoles;
        
        public IdentityError _identityError;
        public ApplicationDbContext _context;
        public IWebHostEnvironment _environment;

        public RoleManager<IdentityRole> _roleManager;
        public UserManager<IdentityUser> _userManager;
        public SignInManager<IdentityUser> _signInManager;
    }
}
