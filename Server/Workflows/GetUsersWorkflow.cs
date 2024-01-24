using System.Dynamic;
using Elsa.Common.Contracts;
using Elsa.Http;
using Elsa.Scheduling.Activities;
using Elsa.Workflows;
using Elsa.Workflows.Activities;
using Elsa.Workflows.Contracts;
using Elsa.Workflows.Models;

namespace Spent.Server.Workflows;

public partial class GetUsersWorkflow : WorkflowBase
{
    [AutoInject]
    private readonly ISystemClock _systemClock = default!;

    protected override void Build(IWorkflowBuilder builder)
    {
        var responseVariable = builder.WithVariable<ExpandoObject>();
        var currentUserVariable = builder.WithVariable<ExpandoObject>();

        builder.Root = new Sequence
        {
            Activities =
            {
                new Cron
                {
                    CronExpression = new("0 1/1 * * * ?"),
                    CanStartWorkflow = true
                },
                new WriteLine(
                    new Input<string>($"Heartbeat workflow triggered at {_systemClock.UtcNow.LocalDateTime}")),
                new SendHttpRequest
                {
                    Url = new(new Uri("https://reqres.in/api/users")),
                    Method = new(HttpMethods.Get),
                    ParsedContent = new(responseVariable)
                }
            }
        };
    }
}
