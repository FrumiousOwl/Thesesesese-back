using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using srrf.Data;
using srrf.Models;

namespace srrf.MachineLearning;
public class AnomalyLogService
{
    private readonly Context _context;

    public AnomalyLogService(Context context)
    {
        _context = context;
    }

    public async Task SaveAnomalyLogAsync(AuditLogModel log, bool isAnomaly)
    {
        var anomalyLog = new AnomalyLog
        {
            Email = log.Email,
            Role = log.Role,
            Entity = log.EntityName,
            Action = log.Action,
            IsAnomaly = isAnomaly,
            Timestamp = DateTime.UtcNow
        };

        _context.AnomalyLogs.Add(anomalyLog);
        await _context.SaveChangesAsync();
    }

    public async Task<List<AnomalyLog>> GetAllAnomaliesAsync()
    {
        return await _context.AnomalyLogs.OrderByDescending(a => a.Timestamp).ToListAsync();
    }
}

