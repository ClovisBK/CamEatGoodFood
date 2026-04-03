namespace AuthService.Repositories.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRatingRepository Ratings { get; }
        IRecipeRepository Recipes { get; }
        Task<int> CompleteAsync();
    }
}
