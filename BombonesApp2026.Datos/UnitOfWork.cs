namespace BombonesApp2026.Datos
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BombonesDbContext _context;

        public UnitOfWork(BombonesDbContext context)
        {
            _context = context;
        }

        public void Commit()
        {
            _context.SaveChanges();
        }
    }
}
