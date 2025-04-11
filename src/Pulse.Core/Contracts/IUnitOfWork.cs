namespace Pulse.Core.Contracts
{
    /// <summary>
    /// Unit of Work interface for managing transactions across repositories
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Repository for venue data
        /// </summary>
        IVenueRepository Venues { get; }

        /// <summary>
        /// Repository for venue type data
        /// </summary>
        IVenueTypeRepository VenueTypes { get; }

        /// <summary>
        /// Repository for special data
        /// </summary>
        ISpecialRepository Specials { get; }

        /// <summary>
        /// Repository for operating schedule data
        /// </summary>
        IOperatingScheduleRepository BusinessHours { get; }

        /// <summary>
        /// Repository for tag data
        /// </summary>
        ITagRepository Tags { get; }

        /// <summary>
        /// Repository for tag-to-special link data
        /// </summary>
        ITagToSpecialLinkRepository TagToSpecialLinks { get; }

        /// <summary>
        /// Saves all changes made through the repositories to the database
        /// </summary>
        /// <returns>Number of affected records</returns>
        Task<int> SaveChangesAsync();

        /// <summary>
        /// Executes a function within a transaction and returns a result
        /// </summary>
        /// <typeparam name="TResult">The type of the result</typeparam>
        /// <param name="operation">The operation to execute</param>
        /// <returns>The result of the operation</returns>
        Task<TResult> ExecuteInTransactionAsync<TResult>(Func<Task<TResult>> operation);

        /// <summary>
        /// Executes an action within a transaction
        /// </summary>
        /// <param name="operation">The operation to execute</param>
        Task ExecuteInTransactionAsync(Func<Task> operation);
    }
}
