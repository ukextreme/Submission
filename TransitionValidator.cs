public static class TransitionValidator
{
    public static void ValidateDefinition(WorkflowDefinition definition)
    {
        // Validate states
        if (definition.States.Count == 0)
            throw new InvalidOperationException("At least one state required");
        
        if (definition.States.Count(s => s.IsInitial) != 1)
            throw new InvalidOperationException("Exactly one initial state required");
        
        // Validate transitions
        var stateIds = definition.States.Select(s => s.Id).ToHashSet();
        foreach (var t in definition.Transitions)
        {
            if (!stateIds.Contains(t.ToStateId))
                throw new InvalidOperationException($"Invalid toState: {t.ToStateId}");
            
            if (t.FromStateIds.Exists(id => !stateIds.Contains(id)))
                throw new InvalidOperationException("Invalid fromState in transition");
        }
    }

    public static bool IsValidTransition(
        WorkflowDefinition definition,
        string currentStateId,
        string actionName)
    {
        var transition = definition.Transitions
            .FirstOrDefault(t => t.Name == actionName);
        
        return transition != null && 
               transition.FromStateIds.Contains(currentStateId) &&
               !definition.States.First(s => s.Id == currentStateId).IsFinal;
    }
}