using Elsa.Activities.Console;
using Elsa.Activities.ControlFlow;
using Elsa.Activities.Http;
using Elsa.Activities.Http.Models;
using Elsa.ActivityResults;
using Elsa.Builders;
using ElsaTest.Server.Activities;

namespace ElsaTest.Server.Workflow;

public class TestWorkFlow : IWorkflow
{
    public void Build(IWorkflowBuilder builder)
    {
        builder
            .WithDisplayName("Wait items")
            .HttpEndpoint(activity =>
                activity
                    .WithPath("/start")
                    .WithMethod(HttpMethod.Post.Method))
            .HttpEndpoint(activity =>
                activity
                    .WithPath("/read")
                    .WithMethod(HttpMethod.Post.Method)
                    .WithReadContent()
                    .WithTargetType<Guid>())
            .Then<ReadActivity>(activity =>
            {
                activity.Set(x => x.NbItemsAwaited, 3);
                activity.Set(x => x.itemId, Guid.Parse("a8698816-bb6c-489d-8544-165bbebc408f"));
            })
            .WriteLine(context =>
            {
                var lstItems = context.GetInput<List<Guid>>();
                
                return $"Item read : {String.Join(" | ", lstItems)}";
            });
    }
}