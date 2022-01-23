namespace GraphQLParser.AST;

/// <summary>
/// AST node for <see cref="ASTNodeKind.FieldDefinition"/>.
/// </summary>
public class GraphQLFieldDefinition : GraphQLTypeDefinition, IHasDirectivesNode, IHasArgumentsDefinitionNode
{
    /// <inheritdoc/>
    public override ASTNodeKind Kind => ASTNodeKind.FieldDefinition;

    /// <summary>
    /// Arguments for this field definition.
    /// </summary>
    public GraphQLArgumentsDefinition? Arguments { get; set; }

    /// <summary>
    /// Nested <see cref="GraphQLType"/> AST node with field type.
    /// </summary>
    public GraphQLType Type { get; set; } = null!;

    /// <inheritdoc/>
    public GraphQLDirectives? Directives { get; set; }
}

internal sealed class GraphQLFieldDefinitionWithLocation : GraphQLFieldDefinition
{
    private GraphQLLocation _location;

    public override GraphQLLocation Location
    {
        get => _location;
        set => _location = value;
    }
}

internal sealed class GraphQLFieldDefinitionWithComment : GraphQLFieldDefinition
{
    private GraphQLComment? _comment;

    public override GraphQLComment? Comment
    {
        get => _comment;
        set => _comment = value;
    }
}

internal sealed class GraphQLFieldDefinitionFull : GraphQLFieldDefinition
{
    private GraphQLLocation _location;
    private GraphQLComment? _comment;

    public override GraphQLLocation Location
    {
        get => _location;
        set => _location = value;
    }

    public override GraphQLComment? Comment
    {
        get => _comment;
        set => _comment = value;
    }
}