using Elsa.Activities.RabbitMq;
using ElsaTest.Server.Activities;
using ElsaTest.Server.Workflow;

var builder = WebApplication.CreateBuilder(args);

var elsaSection = builder.Configuration.GetSection("Elsa");

// Elsa services.
builder.Services
    .AddElsa(elsa => elsa
        //.UseEntityFrameworkPersistence(ef => ef.UseSqlite())
        .AddConsoleActivities()
        .AddHttpActivities(elsaSection.GetSection("Server").Bind)
        //.AddQuartzTemporalActivities()
        .AddWorkflowsFrom<TestWorkFlow>()
        .AddActivitiesFrom<ReadActivity>()
    );

// Elsa API endpoints.
builder.Services.AddElsaApiEndpoints();

// For Dashboard.
builder.Services.AddRazorPages();

var app = builder.Build();
app
    .UseStaticFiles() // For Dashboard.
    .UseHttpActivities()
    .UseRouting()
    .UseEndpoints(endpoints =>
    {
        // Elsa API Endpoints are implemented as regular ASP.NET Core API controllers.
        endpoints.MapControllers();

        // For Dashboard.
        endpoints.MapFallbackToPage("/_Host");
    });

app.Run();