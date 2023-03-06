using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SalesSystem.Library
{
    public class LUsersRoles
    {
        public List<SelectListItem> getRoles(RoleManager<IdentityRole> roleManager)
        {
            List<SelectListItem> _selectLis = new List<SelectListItem>();

            var roles = roleManager.Roles.ToList();

            roles.ForEach(item => {
                _selectLis.Add(new SelectListItem
                {
                    Value = item.Id,
                    Text = item.Name
                });
            });
         
            return _selectLis;
        }

    }
}
