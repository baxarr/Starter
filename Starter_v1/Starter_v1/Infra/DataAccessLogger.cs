// Services/DataAccessLogger.cs

using Starter_v1.BasicInfra;

namespace Starter_v1.Infra;

/// <summary>
/// DataAccessLogger class
/// </summary>
public class DataAccessLogger : IDataAccessLogger
{
    private readonly ILogger<DataAccessLogger> _logger;

    /// <summary>
    /// DataAccessLogger ctor
    /// </summary>
    /// <param name="logger"></param>
    public DataAccessLogger(ILogger<DataAccessLogger> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="action"></param>
    /// <param name="resourceType"></param>
    /// <param name="resourceId"></param>
    public void LogDataAccess(string userId, string action, string resourceType, string resourceId)
    {
        _logger.LogInformation("AUDIT: User {UserId} performed {Action} on {ResourceType} {ResourceId}", 
            userId, action, resourceType, resourceId);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="dataType"></param>
    /// <param name="recordCount"></param>
    public void LogDataExport(string userId, string dataType, int recordCount)
    {
        _logger.LogWarning("DATA_EXPORT: User {UserId} exported {RecordCount} records of type {DataType}", 
            userId, recordCount, dataType);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="eventType"></param>
    /// <param name="details"></param>
    public void LogComplianceEvent(string eventType, string details)
    {
        _logger.LogInformation("COMPLIANCE: {EventType} - {Details}", eventType, details);
    }
}