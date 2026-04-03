using AuthService.Data;
using AuthService.Repositories.Interfaces;

namespace AuthService.Repositories.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        public IRatingRepository Ratings { get; private set; }

        public IRecipeRepository Recipes { get; private set; }

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            Recipes = new RecipeRepository(context);
            Ratings = new RatingRepository(context);
        }

        public async Task<int> CompleteAsync()
            => await _context.SaveChangesAsync();

        public void Dispose() => _context.Dispose();
    }
}
