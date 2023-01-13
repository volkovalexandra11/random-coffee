using Microsoft.AspNetCore.Mvc;
using RandomCoffeeServer.Domain.Hosting.Jobs;

namespace RandomCoffeeServer.Controllers.JobControllers;

public class DatabaseJobsController : ControllerBase
{
    public DatabaseJobsController(SchemeUpdateJob schemeUpdateJob, PopulateWithMockDataJob mockDataJob)
    {
        this.schemeUpdateJob = schemeUpdateJob;
        this.mockDataJob = mockDataJob;
    }

    [Route("/")]
    [Route("/update-db")]
    [HttpPost]
    public async Task<IActionResult> UpdateDb()
    {
        await schemeUpdateJob.StartAsync(HttpContext.RequestAborted);
        await schemeUpdateJob.StopAsync(HttpContext.RequestAborted);

        await mockDataJob.StartAsync(HttpContext.RequestAborted);
        await mockDataJob.StopAsync(HttpContext.RequestAborted);

        return Ok();
    }

    private readonly SchemeUpdateJob schemeUpdateJob;
    private readonly PopulateWithMockDataJob mockDataJob;
}