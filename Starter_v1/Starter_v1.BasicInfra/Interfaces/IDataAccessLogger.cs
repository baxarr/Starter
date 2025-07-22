namespace Starter_v1.BasicInfra;

public interface IDataAccessLogger
{
    void LogDataAccess(string userId, string action, string resourceType, string resourceId);
    void LogDataExport(string userId, string dataType, int recordCount);
    void LogComplianceEvent(string eventType, string details);
}