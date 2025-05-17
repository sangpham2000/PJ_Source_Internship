using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using PJ_Source_GV.Models.Entities;
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
        var result = await db.QueryAsync<StandardEntity>("SELECT * FROM Standard");
        var dtoList = result.Select(e => new StandardDto
        {
            Id = e.id,
            Name = e.name,
            NormalizedName = e.normalized_name,
            CreatedAt = e.created_at,
            UpdatedAt = e.updated_at,
            CreatedBy = e.created_by,
            UpdatedBy = e.updated_by
        }).ToList();
        return dtoList;
    }

    public StandardDto GetById(string id)
    {
        using var db = Connection;
        return db.QueryFirstOrDefault<StandardDto>("SELECT * FROM Standard WHERE id = @Id", new { Id = id });
    }

    public async Task<int> Add(StandardDto StandardDto)
    {
        using var db = Connection;
        var sql = @"
            INSERT INTO Standard (name, normalized_name, created_at, updated_at, created_by, updated_by)
            VALUES (@Name, @NormalizedName, @CreatedAt, @UpdatedAt, @CreatedBy, @UpdatedBy)";
        var result = await db.ExecuteAsync(sql, StandardDto);
        return result;
    }

    public void Update(StandardDto StandardDto)
    {
        using var db = Connection;
        var sql = @"
            UPDATE Standard
            SET name = @Name,
                normalized_name = @NormalizedName,
                updated_at = @UpdatedAt,
                updated_by = @UpdatedBy
            WHERE id = @Id";
        db.Execute(sql, StandardDto);
    }

    public void Delete(string id)
    {
        using var db = Connection;
        db.Execute("DELETE FROM Standard WHERE id = @Id", new { Id = id });
    }
}