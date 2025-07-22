// Controllers/ProductsController.cs

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Starter_v1.BasicInfra;
using Starter_v1.Context;
using Starter_v1.Entities;
using Starter_v1.Infra;

namespace Starter_v1.Controllers;

/// <summary>
/// Some Data Controller class 
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class SomeDataController : ControllerBase
{
    private readonly ILogger<SomeDataController> _logger;
    private readonly IDataAccessLogger _dataAccessLogger;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="dataAccessLogger"></param>
    public SomeDataController(
        ILogger<SomeDataController> logger,
        IDataAccessLogger dataAccessLogger)
    {
        _logger = logger;
        _dataAccessLogger = dataAccessLogger;
    }

    /// <summary>
    /// Gets all biomedical data records
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        _logger.LogInformation("Retrieving all biomedical data records");

        try
        {
            // Your data retrieval logic here
            var data = new List<object>(); // Replace with actual data

            _dataAccessLogger.LogDataAccess(
                userId: "current-user-id", 
                action: "READ_ALL", 
                resourceType: "BiomedicaData", 
                resourceId: "N/A");

            return Ok(data);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving biomedical data records");
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Exports biomedical data
    /// </summary>
    [HttpPost("export")]
    public async Task<IActionResult> ExportData([FromBody] ExportRequest request)
    {
        _logger.LogInformation("Exporting biomedical data for user {User Id}", "current-user-id");

        try
        {
            // Your data export logic here
            int recordCount = 100; // Replace with actual count

            _dataAccessLogger.LogDataExport(
                userId: "current-user-id", 
                dataType: "BiomedicalData", 
                recordCount: recordCount);

            return Ok("Data exported successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error exporting biomedical data");
            return StatusCode(500, "Internal server error");
        }
    }
}

