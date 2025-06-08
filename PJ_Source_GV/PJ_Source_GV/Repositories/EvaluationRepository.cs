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
using PJ_Source_GV.Models.Vocabulary;

namespace PJ_Source_GV.Repositories
{
    public class EvaluationRepository : IEvaluationRepository
    {
        private readonly string _connectionString;

        public EvaluationRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        private IDbConnection Connection => new SqlConnection(_connectionString);

        public async Task<List<EvaluationDto>> GetAll()
        {
            using var db = Connection;
            var evaluationsDict = new Dictionary<int, EvaluationDto>();
            var sessionsDict = new Dictionary<int, EvaluationSessionDto>();

            var sql = @"
                SELECT 
                    e.id, e.name, e.[desc], e.start_date, e.end_date, e.created_at, e.updated_at, e.created_by, e.updated_by,
                    es.id AS session_id, es.evaluation_id, es.created_at, es.updated_at, es.created_by, es.updated_by,
                    es.[desc] AS description,
                    ess.standard_id
                FROM std_evaluation e
                LEFT JOIN std_evaluation_session es ON e.id = es.evaluation_id
                LEFT JOIN std_evaluation_session_standard ess ON es.id = ess.evaluation_session_id
                ORDER BY e.id, es.id";

            await db.QueryAsync<EvaluationEntity, EvaluationSessionEntity, int?, EvaluationEntity>(
                sql,
                (evaluation, session, standardId) =>
                {
                    if (evaluation?.id == null)
                        return evaluation;

                    // Get or create EvaluationDto
                    if (!evaluationsDict.TryGetValue(evaluation.id.Value, out var evaluationDto))
                    {
                        evaluationDto = EvaluationMapper.ToDto(evaluation);
                        evaluationDto.Sessions = new List<EvaluationSessionDto>();
                        evaluationsDict.Add(evaluation.id.Value, evaluationDto);
                    }

                    // Process session if it exists
                    EvaluationSessionDto sessionDto = null;
                    if (session?.id != null)
                    {
                        if (!sessionsDict.TryGetValue(session.id.Value, out sessionDto))
                        {
                            sessionDto = EvaluationSessionMapper.ToDto(session);
                            sessionDto.StandardIds = new List<int>();
                            evaluationDto.Sessions.Add(sessionDto);
                            sessionsDict.Add(session.id.Value, sessionDto);
                        }
                    }

                    // Add standardId to session if applicable
                    if (sessionDto != null && standardId.HasValue && !sessionDto.StandardIds.Contains(standardId.Value))
                    {
                        sessionDto.StandardIds.Add(standardId.Value);
                    }

                    return evaluation;
                },
                splitOn: "session_id,standard_id");

            return evaluationsDict.Values.ToList();
        }

        public async Task<EvaluationDto?> GetById(int id)
        {
            using var db = Connection;
            EvaluationDto? evaluationDto = null;
            var sessionsDict = new Dictionary<int, EvaluationSessionDto>();

            var sql = @"
                SELECT 
                    e.id, e.name, e.[desc] AS description, e.start_date, e.end_date, e.created_at, e.updated_at, e.created_by, e.updated_by,
                    es.id AS session_id, es.evaluation_id, es.created_at, es.updated_at, es.created_by, es.updated_by,
                    ess.standard_id,
                    cc.id AS cell_id, cc.column_id, cc.evaluation_id, cc.value, cc.[order], cc.created_at, cc.updated_at, cc.created_by, cc.updated_by
                FROM std_evaluation e
                LEFT JOIN std_evaluation_session es ON e.id = es.evaluation_id
                LEFT JOIN std_evaluation_session_standard ess ON es.id = ess.evaluation_session_id
                LEFT JOIN std_column_cell cc ON e.id = cc.evaluation_id
                WHERE e.id = @Id
                ORDER BY es.id, cc.[order]";

            await db.QueryAsync<EvaluationEntity, EvaluationSessionEntity, int?, ColumnCellEntity, EvaluationEntity>(
                sql,
                (evaluation, session, standardId, cell) =>
                {
                    if (evaluationDto == null && evaluation != null)
                    {
                        evaluationDto = EvaluationMapper.ToDto(evaluation);
                        evaluationDto.Sessions = new List<EvaluationSessionDto>();
                    }

                    EvaluationSessionDto sessionDto = null;
                    if (session?.id != null)
                    {
                        if (!sessionsDict.TryGetValue(session.id.Value, out sessionDto))
                        {
                            sessionDto = EvaluationSessionMapper.ToDto(session);
                            sessionDto.StandardIds = new List<int>();
                            sessionDto.Cells = new List<ColumnCellDto>();
                            evaluationDto.Sessions.Add(sessionDto);
                            sessionsDict.Add(session.id.Value, sessionDto);
                        }
                    }

                    if (sessionDto != null && standardId.HasValue && !sessionDto.StandardIds.Contains(standardId.Value))
                    {
                        sessionDto.StandardIds.Add(standardId.Value);
                    }

                    if (cell?.id != null)
                    {
                        var cellDto = ColumnCellMapper.ToDto(cell);
                        if (sessionDto != null && !sessionDto.Cells.Any(c => c.Id == cell.id))
                        {
                            sessionDto.Cells.Add(cellDto);
                        }
                        else if (!evaluationDto.Sessions.Any(s => s.Cells.Any(c => c.Id == cell.id)))
                        {
                            if (!evaluationDto.Sessions.Any())
                            {
                                evaluationDto.Sessions.Add(new EvaluationSessionDto
                                {
                                    Cells = new List<ColumnCellDto>(),
                                    StandardIds = new List<int>()
                                });
                            }
                            evaluationDto.Sessions[0].Cells.Add(cellDto);
                        }
                    }

                    return evaluation;
                },
                new { Id = id },
                splitOn: "session_id,standard_id,cell_id");

            return evaluationDto;
        }
        
        public async Task<List<EvaluationSessionDto>> GetSessionsByEvaluationId(int evaluationId)
        {
            using var db = Connection;
            var sessionsDict = new Dictionary<int, EvaluationSessionDto>();

            // Validate evaluation exists
            var evaluationExists = await db.ExecuteScalarAsync<int>(
                "SELECT COUNT(*) FROM std_evaluation WHERE id = @EvaluationId",
                new { EvaluationId = evaluationId });

            if (evaluationExists == 0)
            {
                return new List<EvaluationSessionDto>(); // Return empty list if evaluation doesn't exist
            }

            var sql = @"
                SELECT 
                    es.id, es.evaluation_id, es.[desc], es.created_at, es.updated_at, es.created_by, es.updated_by, es.status,
                    ess.standard_id, s.id, s.name,
                    cc.id, cc.column_id, cc.evaluation_session_id, cc.value, cc.[order], cc.created_at, cc.updated_at, cc.created_by, cc.updated_by
                FROM std_evaluation_session es
                LEFT JOIN std_evaluation_session_standard ess ON es.id = ess.evaluation_session_id
                LEFT JOIN std_standard s ON ess.standard_id = s.id
                LEFT JOIN std_column_cell_history cc ON es.id = cc.evaluation_session_id
                WHERE es.evaluation_id = @EvaluationId
                ORDER BY es.id DESC, cc.[order] ASC";

            await db.QueryAsync<EvaluationSessionEntity, StandardEntity, ColumnCellEntity, EvaluationSessionEntity>(
                sql,
                (session, standardInfo, cell) =>
                {
                    if (session?.id == null)
                        return session;

                    // Get or create EvaluationSessionDto
                    if (!sessionsDict.TryGetValue(session.id.Value, out var sessionDto))
                    {
                        sessionDto = EvaluationSessionMapper.ToDto(session);
                        sessionDto.StandardIds = new List<int>();
                        sessionDto.StandardNames = new List<string>(); // Initialize StandardNames
                        sessionDto.Cells = new List<ColumnCellDto>();
                        sessionsDict.Add(session.id.Value, sessionDto);
                    }

                    // Add standardId and standardName if applicable
                    if (standardInfo?.id.HasValue == true && !sessionDto.StandardIds.Contains(standardInfo.id.Value))
                    {
                        sessionDto.StandardIds.Add(standardInfo.id.Value);
                        if (!string.IsNullOrEmpty(standardInfo.name))
                        {
                            sessionDto.StandardNames.Add(standardInfo.name);
                        }
                    }

                    // Add cell if applicable
                    if (cell?.id != null && !sessionDto.Cells.Any(c => c.Id == cell.id))
                    {
                        sessionDto.Cells.Add(ColumnCellMapper.ToDto(cell));
                    }

                    return session;
                },
                new { EvaluationId = evaluationId },
                splitOn: "standard_id,id");

            return sessionsDict.Values.ToList();
        }

        public async Task<int> Add(EvaluationDto evaluationDto)
        {
            using var db = Connection;
            db.Open();
            using var transaction = db.BeginTransaction();

            try
            {
                // Validate input
                if (string.IsNullOrEmpty(evaluationDto.Name) || evaluationDto.StartDate == null || evaluationDto.EndDate == null)
                {
                    throw new ArgumentException("Name, StartDate, and EndDate are required.");
                }

                if (evaluationDto.StartDate >= evaluationDto.EndDate)
                {
                    throw new ArgumentException("StartDate must be earlier than EndDate.");
                }

                // Insert evaluation
                var evaluationSql = @"
                    INSERT INTO std_evaluation (name, [desc], start_date, end_date, created_at, updated_at, created_by, updated_by)
                    OUTPUT INSERTED.id
                    VALUES (@Name, @Desc, @StartDate, @EndDate, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, @CreatedBy, @UpdatedBy)";

                var evaluationId = await db.QuerySingleAsync<int>(evaluationSql, evaluationDto, transaction);
                evaluationDto.Id = evaluationId;

                transaction.Commit();
                return evaluationId;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception($"Error adding evaluation: {ex.Message}", ex);
            }
        }

        public async Task<int> AddSession(EvaluationSessionDto sessionDto)
        {
            using var db = Connection;
            db.Open();
            using var transaction = db.BeginTransaction();

            try
            {
                // Check if evaluation exists
                var evaluationExists = await db.ExecuteScalarAsync<int>(
                    "SELECT COUNT(*) FROM std_evaluation WHERE id = @EvaluationId",
                    new { EvaluationId = sessionDto.EvaluationId },
                    transaction);

                if (evaluationExists == 0)
                {
                    throw new ArgumentException($"Evaluation ID {sessionDto.EvaluationId } does not exist.");
                }

                // Insert evaluation session
                var sessionSql = @"
                    INSERT INTO std_evaluation_session (evaluation_id, [desc], created_at, updated_at, created_by, updated_by, status)
                    OUTPUT INSERTED.id
                    VALUES (@EvaluationId, @Desc, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, @CreatedBy, @UpdatedBy, 0)";

                var sessionId = await db.QuerySingleAsync<int>(sessionSql, new
                {
                    EvaluationId = sessionDto.EvaluationId ,
                    sessionDto.Desc,
                    sessionDto.CreatedBy,
                    sessionDto.UpdatedBy
                }, transaction);
                sessionDto.Id = sessionId;
                sessionDto.EvaluationId = sessionDto.EvaluationId ;

                // Validate and insert standard associations
                if (sessionDto.StandardIds?.Any() == true)
                {
                    // Check if standards exist
                    var existingStandardIds = await db.QueryAsync<int>(
                        "SELECT id FROM std_standard WHERE id IN @StandardIds",
                        new { StandardIds = sessionDto.StandardIds },
                        transaction);

                    var missingStandards = sessionDto.StandardIds.Except(existingStandardIds).ToList();
                    if (missingStandards.Any())
                    {
                        throw new ArgumentException($"Invalid standard IDs: {string.Join(", ", missingStandards)}");
                    }

                    var standardSql = @"
                        INSERT INTO std_evaluation_session_standard (evaluation_session_id, standard_id, created_at, updated_at, created_by, updated_by)
                        VALUES (@EvaluationSessionId, @StandardId, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, @CreatedBy, @UpdatedBy)";

                    foreach (var standardId in sessionDto.StandardIds)
                    {
                        await db.ExecuteAsync(standardSql, new
                        {
                            EvaluationSessionId = sessionId,
                            StandardId = standardId,
                            sessionDto.CreatedBy,
                            sessionDto.UpdatedBy
                        }, transaction);
                    }
                }

             
                transaction.Commit();
                return sessionId;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception($"Error adding evaluation session: {ex.Message}", ex);
            }
        }

        public async Task<int> AddEvaluationValueAsync(EvaluationSessionDto sessionDto, SessionStatus mode)
        {
            using var db = Connection;
            db.Open();
            using var transaction = db.BeginTransaction();
            try
            {
                 // Insert cells
                 if (sessionDto.Tables?.Any() == true)
                {
                    var sqlDeleteSessionValues = @"
                        DELETE FROM std_column_cell_history
                        WHERE evaluation_session_id = @EvaluationSessionId";
                    await db.ExecuteAsync(sqlDeleteSessionValues, new { EvaluationSessionId = sessionDto.Id }, transaction);
                    
                    foreach (var item in sessionDto.Tables)
                    {
                        if (sessionDto.Status == SessionStatus.Edit)
                        {
                            var sqlTableHistory = @"
                                INSERT INTO std_standard_table_history (
                                    table_id,                               
                                    evaluation_session_id,
                                    standard_id,
                                    name,
                                    created_at,
                                    updated_at,
                                    created_by,
                                    updated_by
                                )
                                SELECT
                                    t.id,
                                    @EvaluationSessionId,
                                    t.standard_id,
                                    t.name,
                                    t.created_at,
                                    t.updated_at,
                                    0,
                                    0
                                FROM std_standard_table t
                                WHERE t.id = @TableId";
                            await db.ExecuteAsync(sqlTableHistory, new { EvaluationSessionId = sessionDto.Id, TableId = item.tableId }, transaction);
                            
                            var sqlTableColumnHistory = @"
                                 INSERT INTO std_table_column_history (
                                    column_id,                                  
                                    table_id,
                                    evaluation_session_id,
                                    name,
                                    [order],
                                    created_at,
                                    updated_at,
                                    created_by,
                                    updated_by,
                                    type
                                )
                                SELECT
                                    c.id,
                                    @TableId,
                                    @EvaluationSessionId,
                                    c.name,
                                    c.[order],
                                    c.created_at,
                                    c.updated_at,
                                    0,
                                    0,
                                    c.type
                                FROM std_table_column c
                                INNER JOIN std_standard_table t ON c.table_id = t.id
                                INNER JOIN std_evaluation_session_standard ess ON t.standard_id = ess.standard_id
                                WHERE c.table_id = @TableId AND ess.evaluation_session_id = @EvaluationSessionId";
                            await db.ExecuteAsync(sqlTableColumnHistory, new { EvaluationSessionId = sessionDto.Id, TableId = item.tableId }, transaction);
                        }
                        
                        var updateSql = @"UPDATE std_evaluation_session SET status = @Status WHERE id = @Id";
                        if (mode == SessionStatus.Draft)
                        {
                            await db.ExecuteAsync(updateSql, new { sessionDto.Id, Status = SessionStatus.Draft }, transaction);
                        }

                        if (mode == SessionStatus.Saved)
                        {
                            await db.ExecuteAsync(updateSql, new { sessionDto.Id, Status = SessionStatus.Saved }, transaction);
                        }

                        var rows = item.rows;
                        if (rows?.Any() == true)
                        {
                            var order = 0;
                            foreach (var row in item.rows)
                            {
                                var cells = row.Cells;
                                if (cells?.Any() == true)
                                {
                                    foreach (var cell in cells)
                                    {
                                        if (cell.ColumnId == null || string.IsNullOrEmpty(cell.Value))
                                        {
                                            throw new ArgumentException($"A cell is missing ColumnId or Value.");
                                        }

                                        cell.Order = order;
                                        cell.EvaluationSessionId = sessionDto.Id;

                                        var cellSql = @"
                                            INSERT INTO std_column_cell_history 
                                            (column_id, value, [order], created_at, updated_at, created_by, updated_by, evaluation_session_id)
                                            VALUES 
                                            (@ColumnId, @Value, @Order, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, @CreatedBy, @UpdatedBy, @EvaluationSessionId)";
            
                                        await db.ExecuteAsync(cellSql, cell, transaction);
                                    }
                                }
                                order++;
                            }

                        }
                    }
                }
                transaction.Commit();
                return sessionDto.Id ?? 0;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception($"Error adding evaluation session: {ex.Message}", ex);
            }
        }

        public async Task<int> Update(EvaluationDto evaluationDto)
        {
            if (evaluationDto.Id == null)
            {
                throw new ArgumentException("Evaluation ID cannot be null for update operation");
            }

            using var db = Connection;
            db.Open();
            using var transaction = db.BeginTransaction();

            try
            {
                // Validate input
                if (string.IsNullOrEmpty(evaluationDto.Name) || evaluationDto.StartDate == null || evaluationDto.EndDate == null)
                {
                    throw new ArgumentException("Name, StartDate, and EndDate are required.");
                }

                if (evaluationDto.StartDate >= evaluationDto.EndDate)
                {
                    throw new ArgumentException("StartDate must be earlier than EndDate.");
                }

                if (evaluationDto.UpdatedBy == null)
                {
                    throw new ArgumentException("UpdatedBy is required.");
                }

                // Check if evaluation exists
                var evaluationExists = await db.ExecuteScalarAsync<int>(
                    "SELECT COUNT(*) FROM std_evaluation WHERE id = @Id",
                    new { evaluationDto.Id },
                    transaction);

                if (evaluationExists == 0)
                {
                    throw new ArgumentException($"Evaluation ID {evaluationDto.Id} does not exist.");
                }

                // Update evaluation
                var evaluationSql = @"
                    UPDATE std_evaluation
                    SET name = @Name, 
                        [desc] = @Description, 
                        start_date = @StartDate, 
                        end_date = @EndDate, 
                        updated_at = CURRENT_TIMESTAMP, 
                        updated_by = @UpdatedBy
                    WHERE id = @Id";

                await db.ExecuteAsync(evaluationSql, evaluationDto, transaction);

                // Note: Sessions, standards, and cells are not updated here; use AddSession or a separate UpdateSession method

                transaction.Commit();
                return 1;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception($"Error updating evaluation: {ex.Message}", ex);
            }
        }

        public async Task<bool> Delete(int id)
        {
            using var db = Connection;
            db.Open();
            using var transaction = db.BeginTransaction();

            try
            {
                // Check if evaluation exists
                var evaluationExists = await db.ExecuteScalarAsync<int>(
                    "SELECT COUNT(*) FROM std_evaluation WHERE id = @Id",
                    new { Id = id },
                    transaction);

                if (evaluationExists == 0)
                {
                    return false;
                }

                var sql = "DELETE FROM std_evaluation WHERE id = @Id";
                var rowsAffected = await db.ExecuteAsync(sql, new { Id = id }, transaction);
                transaction.Commit();
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception($"Error deleting evaluation ID {id}: {ex.Message}", ex);
            }
        }

        public async Task SaveEvaluationHistory(int evaluationId, int userId)
        {
            using var db = Connection;
            try
            {
                await db.ExecuteAsync("sp_SaveEvaluationHistory @evaluation_id, @user_id",
                    new { evaluation_id = evaluationId, user_id = userId });
            }
            catch (Exception ex)
            {
                throw new Exception($"Error saving evaluation history for evaluation ID {evaluationId}: {ex.Message}", ex);
            }
        }

        public async Task<List<EvaluationHistoryDto>> GetEvaluationHistory(int evaluationId)
        {
            using var db = Connection;
            var sql = @"
                SELECT 
                    e.id AS EvaluationId,
                    e.name AS EvaluationName,
                    s.name AS StandardName,
                    th.name AS TableName,
                    ch.name AS ColumnName,
                    cch.value AS CellValue,
                    cch.created_at AS EvaluationTime
                FROM std_evaluation e
                JOIN std_evaluation_session es ON e.id = es.evaluation_id
                JOIN std_evaluation_session_standard ess ON es.id = ess.evaluation_session_id
                JOIN std_standard s ON ess.standard_id = s.id
                JOIN std_standard_table_history th ON th.evaluation_id = e.id AND th.standard_id = s.id
                JOIN std_table_column_history ch ON ch.table_id = th.id AND ch.evaluation_id = e.id
                JOIN std_column_cell_history cch ON cch.column_id = ch.id AND cch.evaluation_id = e.id
                WHERE e.id = @EvaluationId
                ORDER BY th.id, ch.[order], cch.[order]";

            var result = await db.QueryAsync<EvaluationHistoryDto>(sql, new { EvaluationId = evaluationId });
            return result.ToList();
        }
    }
}