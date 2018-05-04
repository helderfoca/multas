using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using multas.Models;
using Owin;

namespace multas
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

            // invoca a função que vai criar a base de dados
            iniciaAplicacao();
        }


        /// <summary>
        /// cria, caso não existam, as Roles de suporte à aplicação: Veterinario, Funcionario, Dono
        /// cria, nesse caso, também, um utilizador...
        /// </summary>
        private void iniciaAplicacao()
        {

            ApplicationDbContext db = new ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));

            // criar a Role 'Agentes'
            if (!roleManager.RoleExists("Agentes"))
            {
                // não existe a 'role'
                // então, criar essa role
                var role = new IdentityRole();
                role.Name = "Agentes";
                roleManager.Create(role);
            }

               // criar a Role 'Condutores'
            if (!roleManager.RoleExists("Condutores"))
            {
                // não existe a 'role'
                // então, criar essa role
                var role = new IdentityRole();
                role.Name = "Condutores";
                roleManager.Create(role);
            }



            // criar um utilizador 'Agentes'
            var user = new ApplicationUser();
            user.UserName = "tania@mail.pt";
            user.Email = "tania@mail.pt";
            //user.Nome = "Luís Freitas";
            string userPWD = "123_Asd";
            var chkUser = userManager.Create(user, userPWD);

            //Adicionar o Utilizador à respetiva Role-Agentes-
            if (chkUser.Succeeded)
            {
                var result1 = userManager.AddToRole(user.Id, "Agentes");
            }
        }

        // https://code.msdn.microsoft.com/ASPNET-MVC-5-Security-And-44cbdb97
        
    }
}
