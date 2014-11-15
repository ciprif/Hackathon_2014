using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using ProjektB.Web.Infrastructure;
using System.Threading;

namespace ProjektB.Web.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

    }

    public class ToDo
    {
        public int ToDoId { get; set; }

        public string Payload { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<ToDo> ToDos { get; set; }

        public DbSet<Team> Teams { get; set; }

        public DbSet<UserDetail> UserDetails { get; set; }

        public DbSet<FitnessProvider> FitnessProviders { get; set; }

    }

    public class Repository
    {
        private const string DBContextKey = "DB_CONTEXT";
        private const string DBTransactionKey = "DB_TRANSACTION";

        /// <summary>
        /// Do not use dirrectly in the code. Use the Context property instead.
        /// </summary>
        private ApplicationDbContext _context;

        public UnitOfWork UoW { get; set; }

        protected ApplicationDbContext Context
        {
            get
            {
                ApplicationDbContext context = UoW.Get<ApplicationDbContext>(DBContextKey);

                if (context == null)
                {
                    context = new ApplicationDbContext();
                    UoW[DBContextKey] = context;
                    DbContextTransaction tran = context.Database.BeginTransaction();
                    UoW[DBTransactionKey] = tran;
                }

                return context;
            }
        }

        public IDbSet<ToDo> ToDos { get { return Context.ToDos; } }
        public IDbSet<Team> Teams { get { return Context.Teams; } }
        public IDbSet<UserDetail> UserDetails { get { return Context.UserDetails; } }
        public IDbSet<FitnessProvider> FitnessProviders { get { return Context.FitnessProviders; } }

        public int SaveChanges()
        {
            return Context.SaveChanges();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await Context.SaveChangesAsync();
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return await Context.SaveChangesAsync(cancellationToken);
        }
    }
}