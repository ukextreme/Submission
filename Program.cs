var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<WorkflowStore>();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();

// Workflow Definition Endpoints
app.MapPost("/definitions", (WorkflowDefinition definition, WorkflowStore store) =>
{
    try {
        TransitionValidator.ValidateDefinition(definition);
        store.AddDefinition(definition);
        return Results.Created($"/definitions/{definition.Id}", definition);
    }
    catch (InvalidOperationException ex) {
        return Results.Problem(ex.Message, statusCode: 400);
    }
});

app.MapGet("/definitions", (WorkflowStore store) => 
    Results.Ok(store.Definitions.Values));

// Workflow Instance Endpoints
app.MapPost("/instances", (string definitionId, WorkflowStore store) =>
{
    if (!store.Definitions.TryGetValue(definitionId, out var def))
        return Results.NotFound("Definition not found");
    
    var instance = new WorkflowInstance(
        Id: Guid.NewGuid().ToString(),
        definitionId,
        CurrentStateId: def.InitialState.Id
    );
    
    store.AddInstance(instance);
    return Results.Created($"/instances/{instance.Id}", instance);
});

app.MapPost("/instances/{id}/actions", 
    (string id, string actionName, WorkflowStore store) =>
{
    if (!store.Instances.TryGetValue(id, out var instance) 
        return Results.NotFound("Instance not found");
    
    var definition = store.Definitions[instance.WorkflowDefinitionId];
    var currentState = definition.States.First(s => s.Id == instance.CurrentStateId);
    
    if (currentState.IsFinal)
        return Results.Conflict("Cannot transition from final state");
    
    if (!TransitionValidator.IsValidTransition(definition, instance.CurrentStateId, actionName))
        return Results.BadRequest("Invalid transition");
    
    var transition = definition.Transitions.First(t => t.Name == actionName);
    var updated = instance with { CurrentStateId = transition.ToStateId };
    store.Instances[id] = updated;
    
    return Results.Ok(updated);
});

app.Run();