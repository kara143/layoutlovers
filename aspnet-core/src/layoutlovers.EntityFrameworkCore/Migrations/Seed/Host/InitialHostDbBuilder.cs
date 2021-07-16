using layoutlovers.EntityFrameworkCore;

namespace layoutlovers.Migrations.Seed.Host
{
    public class InitialHostDbBuilder
    {
        private readonly layoutloversDbContext _context;

        public InitialHostDbBuilder(layoutloversDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            new DefaultEditionCreator(_context).Create();
            new DefaultLanguagesCreator(_context).Create();
            new HostRoleAndUserCreator(_context).Create();
            new DefaultSettingsCreator(_context).Create();
            new DefaultCategoryCreator(_context).Create();
            _context.SaveChanges();
        }
    }
}
