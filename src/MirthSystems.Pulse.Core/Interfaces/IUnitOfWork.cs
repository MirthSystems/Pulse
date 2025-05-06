namespace MirthSystems.Pulse.Core.Interfaces
{
    /// <summary>
    /// Interface for the Unit of Work pattern that coordinates operations across multiple repositories.
    /// </summary>
    /// <remarks>
    /// <para>This interface provides a single point of access to all repositories.</para>
    /// <para>It ensures that all database operations within a business transaction are:</para>
    /// <para>- Consistent: All operations succeed or fail as a unit</para>
    /// <para>- Atomic: Either all changes are saved or none are saved</para>
    /// <para>- Isolated: Changes are not visible to other operations until committed</para>
    /// <para>It implements IDisposable to properly release database connections and resources.</para>
    /// </remarks>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Gets the venue repository for accessing venue entities.
        /// </summary>
        /// <remarks>
        /// <para>The venue repository provides operations for managing venue data, including:</para>
        /// <para>- Basic CRUD operations</para>
        /// <para>- Venue-specific queries</para>
        /// <para>- Location-based searches</para>
        /// </remarks>
        IVenueRepository Venues { get; }
        
        /// <summary>
        /// Gets the special repository for accessing special promotion entities.
        /// </summary>
        /// <remarks>
        /// <para>The special repository provides operations for managing special promotion data, including:</para>
        /// <para>- Basic CRUD operations</para>
        /// <para>- Special-specific queries</para>
        /// <para>- Filtering by type, venue, and time</para>
        /// </remarks>
        ISpecialRepository Specials { get; }
        
        /// <summary>
        /// Gets the operating schedule repository for accessing venue hours entities.
        /// </summary>
        /// <remarks>
        /// <para>The operating schedule repository provides operations for managing venue business hours, including:</para>
        /// <para>- Basic CRUD operations</para>
        /// <para>- Schedule-specific queries</para>
        /// <para>- Checking if venues are currently open</para>
        /// </remarks>
        IOperatingScheduleRepository OperatingSchedules { get; }
        
        /// <summary>
        /// Gets the address repository for accessing location entities.
        /// </summary>
        /// <remarks>
        /// <para>The address repository provides operations for managing address data, including:</para>
        /// <para>- Basic CRUD operations</para>
        /// <para>- Address standardization</para>
        /// <para>- Geospatial queries</para>
        /// </remarks>
        IAddressRepository Addresses { get; }

        /// <summary>
        /// Saves all pending changes to the database as a single transaction.
        /// </summary>
        /// <returns>The number of database records affected.</returns>
        /// <remarks>
        /// <para>This method commits all pending changes across all repositories to the database.</para>
        /// <para>If any operation fails, all changes are rolled back to maintain data consistency.</para>
        /// <para>The return value indicates how many records were inserted, updated, or deleted.</para>
        /// <para>This method should be called after completing a logical group of repository operations.</para>
        /// </remarks>
        Task<int> SaveChangesAsync();
    }
}
