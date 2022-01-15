using System.Collections.Immutable;
using Dapr;
using Microsoft.AspNetCore.Mvc;

namespace Consumer.Controllers;

[ApiController]
public class GrantsPromiseController : Controller
{
    private readonly ILogger<GrantsPromiseController> _logger;
    private static ImmutableList<Promises.MyPromise> _promises = ImmutableList<Promises.MyPromise>.Empty;
    public GrantsPromiseController(ILogger<GrantsPromiseController> logger) => _logger = logger;
        //Subscribe to a topic      
    // [Topic("test-pubsub", "GrantsPromise")]
    [HttpPost("sub")]
    public void getCheckout(Promises.MyPromise promise)
    {
        _logger.LogInformation("Subscriber received : {promiseId}", promise.Id);
        _promises = _promises.Add(promise);
    }

    [HttpGet("get-promises")]
    public IEnumerable<Promises.MyPromise> GetPromises() => _promises;
}
