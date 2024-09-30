using BlackBox.Application.Common.Interfaces;
using System.Data; 
using Microsoft.Data.SqlClient;
using BlackBox.Application.Configurations;
using Dapper;

namespace BlackBox.Infrastructure.Persistence.Contexts
{
    public class BlackBoxReadOnlyContext : IBlackBoxReadOnlyContext
    {
        private readonly IDbConnection _connection;

        public BlackBoxReadOnlyContext(DbConnectionConfig dbConfig)
        {
            _connection = new SqlConnection(dbConfig.ConnectionString);
            Dapper.SqlMapper.Settings.CommandTimeout = 6000;
        }

        public async Task<T?> QueryFirstOrDefaultAsync<T>(string sql, object? param = null, bool isStoredProcedure = false,
                IDbTransaction? transaction = null, CancellationToken cancellationToken = default)
        {
            var commandType = isStoredProcedure ? CommandType.StoredProcedure : CommandType.Text;

            return await _connection.QueryFirstOrDefaultAsync<T>(sql, param, transaction, null, commandType);
        }

        public async Task<List<T>> QueryMultipleAsync<T>(string sql, object? param = null, bool isStoredProcedure = false,
                    IDbTransaction? transaction = null, CancellationToken cancellationToken = default)
        {
            var commandType = isStoredProcedure ? CommandType.StoredProcedure : CommandType.Text;

            var query = await _connection.QueryMultipleAsync(sql, param, transaction, null, commandType);

            return query.Read<T>().ToList();
        }


        public async Task<int> ExecuteAsync(string sql, object? param = null, bool isStoredProcedure = false, IDbTransaction? transaction = null,
          CancellationToken cancellationToken = default)
        {
            var commandType = isStoredProcedure ? CommandType.StoredProcedure : CommandType.Text;
            var query = await _connection.ExecuteAsync(sql, param, transaction, commandType: commandType);

            return query;
        }

        public async Task<T?> QueryScalarAsync<T>(string sql, object? param = null, bool isStoredProcedure = false, IDbTransaction? transaction = null,
          CancellationToken cancellationToken = default)
        {
            var commandType = isStoredProcedure ? CommandType.StoredProcedure : CommandType.Text;
            var query = await _connection.ExecuteScalarAsync(sql, param, transaction, commandType: commandType);

            if (query == null) return default;

            return (T)query;
        }

        public async Task<dynamic?> ExecuteWithTableParamsAsync<T>(string sql,
            object? param = null, Dictionary<string, DataTable>? tableParams = null,
            bool isStoredProcedure = false, IDbTransaction? transaction = null,
          CancellationToken cancellationToken = default)
        {
            param ??= new { };
            tableParams ??= new Dictionary<string, DataTable>();
            var args = new DynamicParameters(param);

            foreach (var dataTable in tableParams)
            {
                args.Add(dataTable.Key, dataTable.Value.AsTableValuedParameter(dataTable.Value.TableName));
            }

            var commandType = isStoredProcedure ? CommandType.StoredProcedure : CommandType.Text;

            //Type type = typeof(T);
            // i.e if type.IsList and returnValueKind is not Kind.List, throw exception; same for other combinations

            // return (await _connection.QueryMultipleAsync(sql, args, transaction, commandType: commandType)).Read<T>().ToList();
            //return await _connection.QueryFirstOrDefaultAsync<T>(sql, args, transaction, commandType: commandType);

            return await _connection.ExecuteAsync(sql, args, transaction, commandType: commandType);
        }

    }
}
