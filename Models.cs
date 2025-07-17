public record StateDefinition(
    string Id, 
    bool IsInitial = false,
    bool IsFinal = false
);

public record TransitionDefinition(
    string Name,
    List<string> FromStateIds,
    string ToStateId
);

public record WorkflowDefinition(
    string Id,
    List<StateDefinition> States,
    List<TransitionDefinition> Transitions
)
{
    public StateDefinition InitialState => States.Single(s => s.IsInitial);
};

public record WorkflowInstance(
    string Id,
    string WorkflowDefinitionId,
    string CurrentStateId,
    List<string> TransitionHistory // Added
);