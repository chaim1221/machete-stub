using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Machete.Data
{
    public static class MacheteUsers
    {

        public static void Initialize(MacheteContext DB)
        {
            IdentityResult ir;

            // TODO: A lot of the missing arguments exist in other places, we have to pull them into this class
            var rm = new RoleManager<IdentityRole>
               (new RoleStore<IdentityRole>(DB), null, null, null, null); // TODO: This is bad, very bad
            ir = rm.CreateAsync(new IdentityRole("Administrator")).Result;
            ir = rm.CreateAsync(new IdentityRole("Manager")).Result;
            ir = rm.CreateAsync(new IdentityRole("Check-in")).Result;
            ir = rm.CreateAsync(new IdentityRole("PhoneDesk")).Result;
            ir = rm.CreateAsync(new IdentityRole("Teacher")).Result;
            ir = rm.CreateAsync(new IdentityRole("User")).Result;
            ir = rm.CreateAsync(new IdentityRole("Hirer")).Result; // This role is used exclusively for the online hiring interface

            var um = new UserManager<MacheteUser>(
                new UserStore<MacheteUser>(DB), null, null, null, null, null, null, null, null); // TODO: This is bad, very bad
            var admin = new MacheteUser()
            {
                UserName = "jadmin",
                IsApproved = true,
                Email = "jciispam@gmail.com"
            };
            var user = new MacheteUser()
            {
                UserName = "juser",
                IsApproved = true,
                Email = "user@there.org"
            };
            ir = um.CreateAsync(admin, "ChangeMe").Result;
            ir = um.AddToRoleAsync(admin, "Administrator").Result; //Default Administrator, edit to change
            ir = um.AddToRoleAsync(admin, "Teacher").Result; //Required to make tests work
            ir = um.CreateAsync(user, "ChangeMe").Result;
            ir = um.AddToRoleAsync(admin, "User").Result; //Default Administrator, edit to change
            DB.Commit();
        }

    }
}
