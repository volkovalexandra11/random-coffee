using Microsoft.AspNetCore.Mvc;
using RandomCoffeeServer.Domain.Hosting.Jobs;

namespace RandomCoffeeServer.Controllers.JobControllers;

public class RoundMakerJobController : ControllerBase
{
    public RoundMakerJobController(RoundsMakerJob roundsMakerJob)
    {
        this.roundsMakerJob = roundsMakerJob;
    }

    [Route("/")]
    [Route("/make-rounds")]
    [HttpPost]
    public async Task<IActionResult> StartRound()
    {
        await roundsMakerJob.StartAsync(HttpContext.RequestAborted);
        await roundsMakerJob.StopAsync(HttpContext.RequestAborted);
        return Ok();
    }

    private readonly RoundsMakerJob roundsMakerJob;
}