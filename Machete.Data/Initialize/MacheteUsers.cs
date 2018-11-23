using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Machete.Data
{
    public static class MacheteUsers
    {

        public static void Initialize(MacheteContext DB)
        {
            // TODO: A lot of the missing arguments exist in other places, we have to pull them into this class
            var rm = new RoleManager<IdentityRole>
               (new RoleStore<IdentityRole>(DB), null, null, null, null); // TODO: This is bad, very bad
            rm.CreateAsync(new IdentityRole("Administrator")).Wait();
            rm.CreateAsync(new IdentityRole("Manager")).Wait();
            rm.CreateAsync(new IdentityRole("Check-in")).Wait();
            rm.CreateAsync(new IdentityRole("PhoneDesk")).Wait();
            rm.CreateAsync(new IdentityRole("Teacher")).Wait();
            rm.CreateAsync(new IdentityRole("User")).Wait();
            rm.CreateAsync(new IdentityRole("Hirer")).Wait(); // This role is used exclusively for the online hiring interface

            var um = new UserManager<MacheteUser>(
                new UserStore<MacheteUser>(DB), null, null, null, null, null, null, null, null); // TODO: This is bad, very bad
            var admin = new MacheteUser
            {
                UserName = "jadmin",
                IsApproved = true,
                Email = "jciispam@gmail.com"
            };
            var user = new MacheteUser
            {
                UserName = "juser",
                IsApproved = true,
                Email = "user@there.org"
            };
            um.CreateAsync(admin, "ChangeMe").Wait();
            um.AddToRoleAsync(admin, "Administrator").Wait(); //Default Administrator, edit to change
            um.AddToRoleAsync(admin, "Teacher").Wait(); //Required to make tests work
            um.CreateAsync(user, "ChangeMe").Wait();
            um.AddToRoleAsync(admin, "User").Wait(); //Default Administrator, edit to change
            
            DB.Commit();
        }

    }
}
