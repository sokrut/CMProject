using ContactManager.Persistance.Entities;
using ContactManager.Persistance.Models;
using Dapper;
using System.Data;
using System.Text;

namespace ContactManager.Persistance.Reposetories
{
    public class ManagerRepository
    {
        private readonly IDbConnection _dbConnection;

        public ManagerRepository(IDbConnection databaseConnection)
        {
                _dbConnection = databaseConnection;
        }

        public async Task<ManagerEntity?> GetManagerById(int managerId)
        {
            const string query = @"
                SELECT *
                FROM [dbo].[contact_manager]
                WHERE [Id] = @managerId";

            return await _dbConnection.QueryFirstOrDefaultAsync<ManagerEntity>(query, new { managerId });
        }

        public async Task<IEnumerable<ManagerEntity>> GetManagers(ManagerFilterOptions filter, int limit, int offset, string? sort)
        {
            var query = $@"
                SELECT
                    [Id]
                    ,[Name]
                    ,[Married]
                    ,[PhoneNumber]
                    ,[BirthDate]
                    ,[Salary]
                FROM [dbo].[contact_manager]
                   {GetFilterExpression(filter)}
                ORDER BY {GetSortExpression(sort)}
                OFFSET @offset ROWS
                FETCH NEXT @limit ROWS ONLY;";

            return (await _dbConnection.QueryAsync<ManagerEntity>(query,
            new
            {
                limit,
                offset,
                filter.Name,
                filter.Married,
                filter.PhoneNumber,
                filter.SalaryTo,
                filter.SalaryFrom,
                filter.BirthDateFrom,
                filter.BirthDateTo,
            })).ToList();
        }

        public async Task<IEnumerable<ManagerEntity>> GetManagersInfoForCsv(ManagerFilterOptions filter)
        {
            var query = $@"
                SELECT
                    [Id]
                    ,[Name]
                    ,[Married]
                    ,[PhoneNumber]
                    ,[BirthDate]
                    ,[Salary]
                FROM [dbo].[contact_manager]
                   {GetFilterExpression(filter)}";

            return (await _dbConnection.QueryAsync<ManagerEntity>(query,
            new
            {
                filter.Name,
                filter.Married,
                filter.PhoneNumber,
                filter.SalaryTo,
                filter.SalaryFrom,
                filter.BirthDateFrom,
                filter.BirthDateTo,
            })).ToList();
        }

        public async Task<int> GetManagerCount(ManagerFilterOptions filter)
        {
            var query = $@"
                SELECT COUNT(*)
                FROM [dbo].[contact_manager] 
                   {GetFilterExpression(filter)}";

            return (await _dbConnection.QueryFirstOrDefaultAsync<int>(query,
            new
            {
                filter.Name,
                filter.Married,
                filter.PhoneNumber,
                filter.SalaryTo,
                filter.SalaryFrom,
                filter.BirthDateFrom,
                filter.BirthDateTo
            }));
        }

        public async Task<int> CreateManager(ManagerEntity manager)
        {
            const string query = @"
            INSERT INTO [dbo].[contact_manager]
                ([Name]
                ,[Married]
                ,[PhoneNumber]
                ,[BirthDate]
                ,[Salary])
            VALUES
            (   @Name,
                @Married,
                @PhoneNumber,
                @BirthDate,
                @Salary);
            SELECT SCOPE_IDENTITY()";

            return await _dbConnection.ExecuteScalarAsync<int>(query, manager);
        }

        public async Task<int> UpdateManager(ManagerEntity managerEntity)
        {
            const string query = @"
                UPDATE [dbo].[contact_manager]
                    set [Name] = @Name,
                        [Married] = @Married,
                        [PhoneNumber] = @PhoneNumber,
                        [BirthDate] = @BirthDate,
                        [Salary] = @Salary
                WHERE Id = @Id;";

            return await _dbConnection.ExecuteScalarAsync<int>(query, managerEntity);
        }

        public async Task<int> DeleteManager(int id)
        {
            const string query = @"
                DELETE FROM [dbo].[contact_manager]
                WHERE Id = @id;";

            return await _dbConnection.ExecuteScalarAsync<int>(query, new { id });
        }

        private static string GetSortExpression(string? sort)
        {
            if (string.IsNullOrEmpty(sort))
            {
                return $"[Id] DESC";
            }

            var sortParam = sort.Split(':');
            var sortBy = (!string.IsNullOrEmpty(sortParam[0])) ? sortParam[0] : "Id";
            var sortDirection = ((sortParam.Length > 1) && !string.IsNullOrEmpty(sortParam[1])) ? sortParam[1].ToUpper() : "DESC";

            return $"[{sortBy}] {sortDirection}";
        }

        private static string GetFilterExpression(ManagerFilterOptions filter)
        {
            var sb = new StringBuilder();
            var first = "WHERE";

            if (!string.IsNullOrEmpty(filter.Name))
            {
                sb.Append(first);
                sb.Append($" [Name] LIKE @Name ");
                first = "AND";
            }

            if (!string.IsNullOrEmpty(filter.PhoneNumber))
            {
                sb.Append(first);
                sb.Append($" [PhoneNumber] LIKE @PhoneNumber ");
                first = "AND";
            }

            if (filter.Married.HasValue)
            {
                sb.Append(first);
                sb.Append($" [Married] >= @Married ");
                first = "AND";
            }

            if (filter.SalaryFrom.HasValue)
            {
                sb.Append(first);
                sb.Append($" [Salary] >= @SalaryFrom ");
                first = "AND";
            }

            if (filter.SalaryTo.HasValue)
            {
                sb.Append(first);
                sb.Append($" [Salary] <= @SalaryTo ");
                first = "AND";
            }

            if (filter.BirthDateFrom.HasValue)
            {
                sb.Append(first);
                sb.Append($" [BirthDate] >= @BirthDateFrom ");
                first = "AND";
            }

            if (filter.BirthDateTo.HasValue)
            {
                sb.Append(first);
                sb.Append($" [BirthDate] <= @BirthDateTo ");
            }

            return sb.ToString();
        }
    }
}