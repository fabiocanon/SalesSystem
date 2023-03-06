using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SalesSystem.Models;
using System.Diagnostics;

namespace SalesSystem.Controllers
{
    public class HomeController : Controller
    {
        IServiceProvider _serviceProvider;
        public HomeController(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<IActionResult> Index()
        {
            await CrearRolAsync(_serviceProvider);

            return View();
        }

        public async Task<IActionResult> Privacy()
        {
            await CrearRolAsync(_serviceProvider);
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private async Task CrearRolAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            String[] rolesName = { "Admin", "User", "Consulta" };

            foreach (var item in rolesName)
            {
                var rolExiste = await roleManager.RoleExistsAsync(item);

                if (!rolExiste)
                {
                    await roleManager.CreateAsync(new IdentityRole(item));
                }
            }
        }
    }
}