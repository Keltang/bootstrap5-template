using BlackBox.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace BlackBox.Application.Common.Interfaces
{
    public interface IBlackBoxContext
    { 
        DatabaseFacade Database { get; }

        #region Entities
        DbSet<Grade> Grades { get; }

        #endregion 


        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        Task<int> ExecuteAction(string sqlStatement, CancellationToken cancellationToken);
        void UpdateEntity(object entity);
        Task<int> SaveChangesExplicitTrackingAsync(CancellationToken cancellationToken = new CancellationToken());
    }
}
