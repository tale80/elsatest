using Elsa.ActivityResults;
using Elsa.Attributes;
using Elsa.Services;
using Elsa.Services.Models;

namespace ElsaTest.Server.Activities;

[Activity(
    Category = "Read",
    DisplayName = "Read/Wait",
    Description = "Wait the read of a specified number of items before to continue"
)]
public class ReadActivity : Activity
{
    [ActivityInput(Hint = "The number of items to wait before to return the list")]
    public int NbItemsAwaited { get; set; } = 3;

    [ActivityInput(Hint = "The item id read")]
    public Guid itemId { get; set; }

    [ActivityOutput(Hint = "list of read item ID in DB")]
    public List<Guid> lstItems { get; set; }= new List<Guid>();
    
    protected override async ValueTask<IActivityExecutionResult> OnExecuteAsync(ActivityExecutionContext context)
    {
        // Instruct the workflow runner to suspend the workflow.
        return await ExecuteInternal(context);
    }

    protected override async ValueTask<IActivityExecutionResult> OnResumeAsync(ActivityExecutionContext context)
    {
        return await ExecuteInternal(context);
    }

    private async ValueTask<IActivityExecutionResult> ExecuteInternal(ActivityExecutionContext context)
    {
        lstItems.Add(itemId);
        if (lstItems.Count == NbItemsAwaited)
        {
            Console.WriteLine($"Received {lstItems.Count} on {NbItemsAwaited} items. Done");
            return Done();
        }
        else
        {
            Console.WriteLine($"Received {lstItems.Count} on {NbItemsAwaited} items. Wait");
            return Suspend();
        }
    }
}