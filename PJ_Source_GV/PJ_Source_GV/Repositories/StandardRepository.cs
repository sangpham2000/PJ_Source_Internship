using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using PJ_Source_GV.Models.Entities;
using PJ_Source_GV.Models.Mapper;
using PJ_Source_GV.Models.Models.Dtos;

namespace PJ_Source_GV.Repositories;

public class StandardRepository : IStandardRepository
{
    private readonly string _connectionString;

    public StandardRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    private IDbConnection Connection => new SqlConnection(_connectionString);

    public async Task<List<StandardDto>> GetAll()
    {
        using var db = Connection;
        var standardsDict = new Dictionary<int, StandardDto>();

        var sql = @"
            SELECT 
                s.id, s.name, s.normalized_name, s.created_at, s.updated_at, s.created_by, s.updated_by,
                t.id, t.standard_id, t.name, 
                t.created_at, t.updated_at, 
                t.created_by, t.updated_by,
                c.id, c.table_id, c.name, c.[order], 
                c.type, c.created_at, 
                c.updated_at, c.created_by, 
                c.updated_by
            FROM std_standard s
            LEFT JOIN std_standard_table t ON s.id = t.standard_id
            LEFT JOIN std_table_column c ON t.id = c.table_id
            ORDER BY s.id, t.id, c.[order]";

        await db.QueryAsync<StandardEntity, StandardTableEntity, TableColumnEntity, StandardEntity>(
            sql,
            (standard, table, column) =>
            {
                if (standard?.id == null)
                    return standard;

                // Get or create StandardDto
                if (!standardsDict.TryGetValue(standard.id.Value, out var standardDto))
                {
                    standardDto = StandardMapper.ToDto(standard);
                    standardDto.Tables = new List<StandardTableDto>();
                    standardsDict.Add(standard.id.Value, standardDto);
                }

                // Process table if it exists
                if (table?.id != null)
                {
                    // Look for existing table in the current standard
                    var existingTable = standardDto.Tables
                        .FirstOrDefault(t => t?.Id == table.id);

                    // If table doesn't exist yet, add it to the standard
                    if (existingTable == null)
                    {
                        var tableDto = StandardTableMapper.ToDto(table);
                        tableDto.Columns = new List<TableColumnDto>();
                        standardDto.Tables.Add(tableDto);
                        existingTable = tableDto;
                    }

                    // Process column if it exists
                    if (column?.id != null && existingTable != null)
                    {
                        // Check if column already exists in the table
                        bool columnExists = existingTable.Columns
                            .Any(c => c?.Id == column.id);

                        // Only add the column if it doesn't already exist
                        if (!columnExists)
                        {
                            var columnDto = TableColumnMapper.ToDto(column);
                            existingTable.Columns.Add(columnDto);
                        }
                    }
                }

                return standard;
            },
            splitOn: "id,id");

        return standardsDict.Values.ToList();
    }

    public async Task<StandardDto?> GetById(int id)
    {
        using var db = Connection;
        StandardDto? standardDto = null;
        var tablesDict = new Dictionary<int, StandardTableDto>();

        var sql = @"
            SELECT 
                s.id, s.name, s.normalized_name, s.created_at, s.updated_at, s.created_by, s.updated_by,
                t.id, t.standard_id, t.name, 
                t.created_at, t.updated_at, 
                t.created_by, t.updated_by,
                c.id, c.table_id, c.name, c.order, 
                c.type, c.created_at, 
                c.updated_at, c.created_by, 
                c.updated_by
            FROM std_standard s
            LEFT JOIN std_standard_table t ON s.id = t.standard_id
            LEFT JOIN std_table_column c ON t.id = c.table_id
            WHERE s.id = @Id
            ORDER BY t.id, c.order";

        await db.QueryAsync<StandardEntity, StandardTableEntity, TableColumnEntity, StandardEntity>(
            sql,
            (standard, table, column) =>
            {
                // Initialize standard if needed
                if (standardDto == null && standard != null)
                {
                    standardDto = StandardMapper.ToDto(standard);
                    standardDto.Tables = new List<StandardTableDto>();
                }

                // Process table if it exists
                if (table?.id != null && standardDto != null)
                {
                    // Try to get the table from dictionary or add it
                    if (!tablesDict.TryGetValue(table.id.Value, out var tableDto))
                    {
                        tableDto = StandardTableMapper.ToDto(table);
                        tableDto.Columns = new List<TableColumnDto>();
                        standardDto.Tables.Add(tableDto);
                        tablesDict.Add(table.id.Value, tableDto);
                    }

                    // Process column if it exists
                    if (column?.id != null)
                    {
                        var columnDto = TableColumnMapper.ToDto(column);
                        tableDto.Columns.Add(columnDto);
                    }
                }

                return standard;
            },
            new { Id = id },
            splitOn: "id,id");

        return standardDto;
    }

    public async Task<List<StandardDto>> GetByIds(List<int> ids)
    {
        if (ids == null || !ids.Any())
        {
            return new List<StandardDto>();
        }

        using var db = Connection;
        var standardsDict = new Dictionary<int, StandardDto>();

        var sql = @"
            SELECT 
                s.id, s.name, s.normalized_name, s.created_at, s.updated_at, s.created_by, s.updated_by,
                t.id, t.standard_id, t.name,
                t.created_at, t.updated_at, 
                t.created_by, t.updated_by,
                c.id, c.table_id, c.name, c.[order], 
                c.type, c.created_at, 
                c.updated_at, c.created_by, 
                c.updated_by
            FROM std_standard s
            LEFT JOIN std_standard_table t ON s.id = t.standard_id
            LEFT JOIN std_table_column c ON t.id = c.table_id
            WHERE s.id IN @Ids
            ORDER BY s.id, t.id, c.[order]";

        await db.QueryAsync<StandardEntity, StandardTableEntity, TableColumnEntity, StandardEntity>(
            sql,
            (standard, table, column) =>
            {
                if (standard?.id == null)
                    return standard;

                // Get or create StandardDto
                if (!standardsDict.TryGetValue(standard.id.Value, out var standardDto))
                {
                    standardDto = StandardMapper.ToDto(standard);
                    standardDto.Tables = new List<StandardTableDto>();
                    standardsDict.Add(standard.id.Value, standardDto);
                }

                // Process table if it exists
                if (table?.id != null)
                {
                    // Look for existing table in the current standard
                    var existingTable = standardDto.Tables
                        .FirstOrDefault(t => t?.Id == table.id);

                    // If table doesn't exist yet, add it to the standard
                    if (existingTable == null)
                    {
                        var tableDto = StandardTableMapper.ToDto(table);
                        tableDto.Columns = new List<TableColumnDto>();
                        standardDto.Tables.Add(tableDto);
                        existingTable = tableDto;
                    }

                    // Process column if it exists
                    if (column?.id != null)
                    {
                        // Check if column already exists in the table
                        bool columnExists = existingTable.Columns
                            .Any(c => c?.Id == column.id);

                        // Only add the column if it doesn't already exist
                        if (!columnExists)
                        {
                            var columnDto = TableColumnMapper.ToDto(column);
                            existingTable.Columns.Add(columnDto);
                        }
                    }
                }

                return standard;
            },
            new { Ids = ids },
            splitOn: "id,id");

        return standardsDict.Values.ToList();
    }
    
    public async Task<List<StandardDto>> GetHistoryByEvaluationSessionId(List<int> ids,int evaluationSessionId)
    {
        if (ids == null || !ids.Any())
        {
            return new List<StandardDto>();
        }
        
        using var db = Connection;
        var standardsDict = new Dictionary<int, StandardDto>();

        var sql = @"
            SELECT 
                s.id, s.name, s.normalized_name, s.created_at, s.updated_at, s.created_by, s.updated_by,
                th.table_id AS id, th.standard_id, th.name, th.evaluation_session_id,
                th.created_at, th.updated_at, th.created_by, th.updated_by,
                ch.column_id AS id, ch.table_id, ch.name, ch.[order], ch.type,
                ch.created_at, ch.updated_at, ch.created_by, ch.updated_by
            FROM std_standard s
            INNER JOIN std_evaluation_session_standard ess ON s.id = ess.standard_id
            LEFT JOIN std_standard_table_history th ON s.id = th.standard_id AND th.evaluation_session_id = ess.evaluation_session_id
            LEFT JOIN std_table_column_history ch ON th.table_id = ch.table_id AND ch.evaluation_session_id = th.evaluation_session_id
            WHERE ess.evaluation_session_id = @EvaluationSessionId AND s.id IN @Ids
            ORDER BY s.id, th.id, ch.[order]";

        await db.QueryAsync<StandardEntity, StandardTableEntity, TableColumnEntity, StandardEntity>(
            sql,
            (standard, tableHistory, columnHistory) =>
            {
                if (standard?.id == null)
                    return standard;

                // Get or create StandardDto
                if (!standardsDict.TryGetValue(standard.id.Value, out var standardDto))
                {
                    standardDto = StandardMapper.ToDto(standard);
                    standardDto.Tables = new List<StandardTableDto>();
                    standardsDict.Add(standard.id.Value, standardDto);
                }

                // Process table history if it exists
                if (tableHistory?.id != null)
                {
                    // Look for existing table in the current standard
                    var existingTable = standardDto.Tables
                        .FirstOrDefault(t => t?.Id == tableHistory.id);

                    // If table doesn't exist yet, add it to the standard
                    if (existingTable == null)
                    {
                        var tableDto = StandardTableMapper.ToDto(tableHistory);
                        tableDto.Columns = new List<TableColumnDto>();
                        standardDto.Tables.Add(tableDto);
                        existingTable = tableDto;
                    }

                    // Process column history if it exists
                    if (columnHistory?.id != null)
                    {
                        // Check if column already exists in the table
                        bool columnExists = existingTable.Columns
                            .Any(c => c?.Id == columnHistory.id);

                        // Only add the column if it doesn't already exist
                        if (!columnExists)
                        {
                            var columnDto = TableColumnMapper.ToDto(columnHistory);
                            existingTable.Columns.Add(columnDto);
                        }
                    }
                }

                return standard;
            },
            new { EvaluationSessionId = evaluationSessionId, Ids = ids  },
            splitOn: "id,id");

        return standardsDict.Values.ToList();
    }
    public async Task<int> Add(StandardDto standardDto)
    {
        using var db = Connection;
        db.Open();
        using var transaction = db.BeginTransaction();

        try
        {
            // Normalize name
            standardDto.NormalizedName = standardDto.Name?.ToLowerInvariant();
            
            // Insert standard
            var standardSql = @"
                INSERT INTO std_standard (name, normalized_name, created_at, updated_at, created_by, updated_by)
                OUTPUT INSERTED.id
                VALUES (@Name, @NormalizedName, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, @CreatedBy, @UpdatedBy)";

            var standardId = await db.QuerySingleAsync<int>(standardSql, standardDto, transaction);
            standardDto.Id = standardId;

            // Insert tables
            if (standardDto.Tables.Count > 0)
            {
                foreach (var table in standardDto.Tables.Where(t => t != null))
                {
                    var tableSql = @"
                        INSERT INTO std_standard_table (standard_id, name, created_at, updated_at, created_by, updated_by)
                        OUTPUT INSERTED.id
                        VALUES (@StandardId, @Name, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, @CreatedBy, @UpdatedBy)";

                    table.StandardId = standardId;
                    var tableId = await db.QuerySingleAsync<int>(tableSql, table, transaction);
                    table.Id = tableId;

                    // Insert columns
                    if (table.Columns.Count > 0)
                    {
                        for (int i = 0; i < table.Columns.Count; i++)
                        {
                            var column = table.Columns[i];
                            if (column != null)
                            {
                                // Set the order based on position in list
                                column.Order = i;
                                
                                var columnSql = @"
                                    INSERT INTO std_table_column (table_id, name, [order], type, created_at, updated_at, created_by, updated_by)
                                    OUTPUT INSERTED.id
                                    VALUES (@TableId, @Name, @Order, @Type, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, @CreatedBy, @UpdatedBy)";

                                column.TableId = tableId;
                                var columnId = await db.QuerySingleAsync<int>(columnSql, column, transaction);
                                column.Id = columnId;
                            }
                        }
                    }
                }
            }

            // Update total_table in std_standard
            var updateTotalTableSql = @"
                UPDATE std_standard
                SET total_table = (SELECT COUNT(*) FROM std_standard_table WHERE standard_id = @StandardId)
                WHERE id = @StandardId";

            await db.ExecuteAsync(updateTotalTableSql, new { StandardId = standardId }, transaction);

            transaction.Commit();
            return standardId;
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            throw new Exception($"Error adding standard: {ex.Message}", ex);
        }
    }

    public async Task<int> Update(StandardDto standardDto)
    {
        if (standardDto.Id == null)
        {
            throw new ArgumentException("Standard ID cannot be null for update operation");
        }

        using var db = Connection;
        db.Open();
        using var transaction = db.BeginTransaction();

        try
        {
            // Normalize name
            standardDto.NormalizedName = standardDto.Name?.ToLowerInvariant();
            
            // Update standard
            var standardSql = @"
                UPDATE std_standard
                SET name = @Name, 
                    normalized_name = @NormalizedName, 
                    updated_at = CURRENT_TIMESTAMP, 
                    updated_by = @UpdatedBy
                WHERE id = @Id";

            await db.ExecuteAsync(standardSql, standardDto, transaction);

            // Handle tables - first get existing tables
            var existingTableIds = await db.QueryAsync<int>(
                "SELECT id FROM std_standard_table WHERE standard_id = @StandardId",
                new { StandardId = standardDto.Id },
                transaction);

            var currentTableIds = new List<int>();
            if (standardDto.Tables.Count > 0)
            {
                // Process each table
                foreach (var table in standardDto.Tables.Where(t => t != null))
                {
                    table.StandardId = standardDto.Id;
                    
                    if (table.Id == null)
                    {
                        // Insert new table
                        var tableSql = @"
                            INSERT INTO std_standard_table (standard_id, name, created_at, updated_at, created_by, updated_by)
                            OUTPUT INSERTED.id
                            VALUES (@StandardId, @Name, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, @CreatedBy, @UpdatedBy)";

                        var tableId = await db.QuerySingleAsync<int>(tableSql, table, transaction);
                        table.Id = tableId;
                    }
                    else
                    {
                        // Update existing table
                        var tableSql = @"
                            UPDATE std_standard_table
                            SET name = @Name, 
                                updated_at = CURRENT_TIMESTAMP, 
                                updated_by = @UpdatedBy
                            WHERE id = @Id";

                        await db.ExecuteAsync(tableSql, table, transaction);
                        currentTableIds.Add(table.Id.Value);
                    }

                    if (table.Id.HasValue)
                    {
                        // Handle columns for this table
                        await UpdateTableColumns(db, transaction, table);
                    }
                }
            }

            // Delete tables that are no longer in the data
            var tablesToDelete = existingTableIds.Where(id => !currentTableIds.Contains(id)).ToList();
            if (tablesToDelete.Any())
            {
                await db.ExecuteAsync(
                    "DELETE FROM std_standard_table WHERE id IN @TableIds",
                    new { TableIds = tablesToDelete },
                    transaction);
            }

            // Update total_table count
            var updateTotalTableSql = @"
                UPDATE std_standard
                SET total_table = (SELECT COUNT(*) FROM std_standard_table WHERE standard_id = @StandardId)
                WHERE id = @StandardId";

            await db.ExecuteAsync(updateTotalTableSql, new { StandardId = standardDto.Id }, transaction);

            transaction.Commit();
            return 1; // Successful update
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            throw new Exception($"Error updating standard: {ex.Message}", ex);
        }
    }

    private async Task UpdateTableColumns(IDbConnection db, IDbTransaction transaction, StandardTableDto table)
    {
        // Get existing columns
        var existingColumnIds = await db.QueryAsync<int>(
            "SELECT id FROM std_table_column WHERE table_id = @TableId",
            new { TableId = table.Id },
            transaction);

        var currentColumnIds = new List<int>();

        // Process each column
        if (table.Columns != null)
        {
            for (int i = 0; i < table.Columns.Count; i++)
            {
                var column = table.Columns[i];
                if (column != null)
                {
                    column.TableId = table.Id;
                    column.Order = i; // Set order based on position
                    
                    if (column.Id == null)
                    {
                        // Insert new column
                        var columnSql = @"
                            INSERT INTO std_table_column (table_id, name, [order], type, created_at, updated_at, created_by, updated_by)
                            OUTPUT INSERTED.id
                            VALUES (@TableId, @Name, @Order, @Type, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, @CreatedBy, @UpdatedBy)";

                        var columnId = await db.QuerySingleAsync<int>(columnSql, column, transaction);
                        column.Id = columnId;
                    }
                    else
                    {
                        // Update existing column
                        var columnSql = @"
                            UPDATE std_table_column
                            SET name = @Name, 
                                [order] = @Order, 
                                type = @Type, 
                                updated_at = CURRENT_TIMESTAMP, 
                                updated_by = @UpdatedBy
                            WHERE id = @Id";

                        await db.ExecuteAsync(columnSql, column, transaction);
                        if (column.Id.HasValue)
                        {
                            currentColumnIds.Add(column.Id.Value);
                        }
                    }
                }
            }
        }

        // Delete columns that are no longer in the list
        var columnsToDelete = existingColumnIds.Where(id => !currentColumnIds.Contains(id)).ToList();
        if (columnsToDelete.Any())
        {
            await db.ExecuteAsync(
                "DELETE FROM std_table_column WHERE id IN @ColumnIds",
                new { ColumnIds = columnsToDelete },
                transaction);
        }
    }

    public async Task<bool> Delete(int id)
    {
        using var db = Connection;
        try
        {
            // Note: Assuming ON DELETE CASCADE is configured in the database schema
            var rowsAffected = await db.ExecuteAsync("DELETE FROM std_standard WHERE id = @Id", new { Id = id });
            return rowsAffected > 0;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error deleting standard ID {id}: {ex.Message}", ex);
        }
    }
}