using System.Data; 

namespace BlackBox.Application.Common.Interfaces
{
    public interface IBlackBoxReadOnlyContext
    { 
        Task<T?> QueryFirstOrDefaultAsync<T>(string sql, object? param = null, bool isStoredProcedure = false, 
                IDbTransaction? transaction = null, CancellationToken cancellationToken = default);

        Task<List<T>> QueryMultipleAsync<T>(string sql, object? param = null, bool isStoredProcedure = false, 
                IDbTransaction? transaction = null, CancellationToken cancellationToken = default);
         
        Task<int> ExecuteAsync(string sql, object? param = null, bool isStoredProcedure = false, 
                IDbTransaction? transaction = null, CancellationToken cancellationToken = default);
        
        Task<T?> QueryScalarAsync<T>(string sql, object? param = null, bool isStoredProcedure = false, 
                IDbTransaction? transaction = null, CancellationToken cancellationToken = default);

        Task<dynamic?> ExecuteWithTableParamsAsync<T>(string sql, object? param = null,
            Dictionary<string, DataTable>? tableParams = null, bool isStoredProcedure = false, 
                IDbTransaction? transaction = null, CancellationToken cancellationToken = default); 
         
    }
}
