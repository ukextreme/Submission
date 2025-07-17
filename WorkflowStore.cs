public class WorkflowStore
{
    public ConcurrentDictionary<string, WorkflowDefinition> Definitions { get; } = new();
    public ConcurrentDictionary<string, WorkflowInstance> Instances { get; } = new();
    
    public void AddDefinition(WorkflowDefinition definition) => 
        Definitions.TryAdd(definition.Id, definition);
    
    public void AddInstance(WorkflowInstance instance) => 
        Instances.TryAdd(instance.Id, instance);
}