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
        await schemeUpdateJob.UpdateScheme(HttpContext.RequestAborted);
        await mockDataJob.Fill(HttpContext.RequestAborted);

        return Ok();
    }

    private readonly SchemeUpdateJob schemeUpdateJob;
    private readonly PopulateWithMockDataJob mockDataJob;
}