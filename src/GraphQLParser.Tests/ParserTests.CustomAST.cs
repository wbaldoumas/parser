using GraphQLParser.Exceptions;

namespace GraphQLParser.Tests;

public class ParserTestsCustomAST
{
    [Fact]
    public void Should_Throw_On_Comment()
    {
        string text = "# comment";
        Should.Throw<NotSupportedException>(() => text.Parse<GraphQLComment>());
    }

    [Theory]
    [InlineData("null", ASTNodeKind.NullValue)]
    [InlineData("1", ASTNodeKind.IntValue)]
    [InlineData("1.1", ASTNodeKind.FloatValue)]
    [InlineData("\"abc\"", ASTNodeKind.StringValue, "abc")]
    [InlineData("\"escaped \\n\\r\\b\\t\\f\"", ASTNodeKind.StringValue, "escaped \n\r\b\t\f")]
    [InlineData("true", ASTNodeKind.BooleanValue)]
    [InlineData("RED", ASTNodeKind.EnumValue)]
    [InlineData("[ 1, 2, 3]", ASTNodeKind.ListValue)]
    [InlineData("{ a: 1, b: \"abc\", c: RED, d: $id }", ASTNodeKind.ObjectValue)]
    [InlineData("$id", ASTNodeKind.Variable)]
    public void Should_Parse_Value_Literal_But_Not_Entire_Document(string text, ASTNodeKind kind, string expected = null)
    {
        Should.Throw<GraphQLSyntaxErrorException>(() => Parser.Parse(text));

        var value = text.Parse<GraphQLValue>();
        value.ShouldNotBeNull();
        value.Kind.ShouldBe(kind);
        if (expected != null)
            ((GraphQLStringValue)value).Value.ShouldBe(expected);
    }

    [Fact]
    public void Should_Parse_Variable()
    {
        string text = "$id";
        var ast = text.Parse<GraphQLVariable>().ShouldNotBeNull();
        ast.Name.Value.ShouldBe("id");
    }

    [Fact]
    public void Should_Parse_Argument()
    {
        string text = "id: 5";
        var ast = text.Parse<GraphQLArgument>().ShouldNotBeNull();
        ast.Name.Value.ShouldBe("id");
    }

    [Fact]
    public void Should_Parse_Arguments()
    {
        string text = "(id: 5 code: abc)";
        var ast = text.Parse<GraphQLArguments>().ShouldNotBeNull();
        ast.Items.Count.ShouldBe(2);
    }

    [Fact]
    public void Should_Parse_Description()
    {
        string text = "\"blablalba\"";
        var ast = text.Parse<GraphQLDescription>().ShouldNotBeNull();
        ast.Value.ShouldBe("blablalba");
    }

    [Fact]
    public void Should_Parse_Directive()
    {
        string text = "@my";
        var ast = text.Parse<GraphQLDirective>().ShouldNotBeNull();
        ast.Name.Value.ShouldBe("my");
    }

    [Fact]
    public void Should_Parse_Directives()
    {
        string text = "@my @your";
        var ast = text.Parse<GraphQLDirectives>().ShouldNotBeNull();
        ast.Items.Count.ShouldBe(2);
    }

    [Fact]
    public void Should_Parse_Field()
    {
        string text = "name";
        var ast = text.Parse<GraphQLField>().ShouldNotBeNull();
        ast.Name.Value.ShouldBe("name");
    }

    [Fact]
    public void Should_Parse_SelectionSet()
    {
        string text = "{ a b }";
        var ast = text.Parse<GraphQLSelectionSet>().ShouldNotBeNull();
        ast.Selections.Count.ShouldBe(2);
    }

    [Fact]
    public void Should_Parse_ArgumentsDefinition()
    {
        string text = "(size: Int)";
        var definition = text.Parse<GraphQLArgumentsDefinition>().ShouldNotBeNull();
        definition.Items.Count.ShouldBe(1);
    }

    [Fact]
    public void Should_Parse_InputValueDefinition()
    {
        string text = "size: Int";
        var definition = text.Parse<GraphQLInputValueDefinition>().ShouldNotBeNull();
        definition.Name.Value.ShouldBe("size");
    }

    [Fact]
    public void Should_Parse_DirectiveDefinition()
    {
        string text = "directive @my on FIELD";
        var definition = text.Parse<GraphQLDirectiveDefinition>().ShouldNotBeNull();
        definition.Name.Value.ShouldBe("my");
    }

    [Fact]
    public void Should_Parse_EnumTypeDefinition()
    {
        string text = "enum Color { RED }";
        var definition = text.Parse<GraphQLEnumTypeDefinition>().ShouldNotBeNull();
        definition.Name.Value.ShouldBe("Color");
    }

    [Fact]
    public void Should_Parse_EnumValueDefinition()
    {
        string text = "RED";
        var definition = text.Parse<GraphQLEnumValueDefinition>().ShouldNotBeNull();
        definition.Name.Value.ShouldBe("RED");
    }

    [Fact]
    public void Should_Parse_EnumValuesDefinition()
    {
        string text = "{ RED GREEN }";
        var definition = text.Parse<GraphQLEnumValuesDefinition>().ShouldNotBeNull();
        definition.Items.Count.ShouldBe(2);
    }

    [Fact]
    public void Should_Parse_FieldDefinition()
    {
        string text = "name: String";
        var definition = text.Parse<GraphQLFieldDefinition>().ShouldNotBeNull();
        definition.Name.Value.ShouldBe("name");
    }

    [Fact]
    public void Should_Parse_FieldsDefinition()
    {
        string text = "{ name: String age: Int }";
        var definition = text.Parse<GraphQLFieldsDefinition>().ShouldNotBeNull();
        definition.Items.Count.ShouldBe(2);
    }

    [Fact]
    public void Should_Parse_FragmentDefinition()
    {
        string text = "fragment frag on Person { name }";
        var definition = text.Parse<GraphQLFragmentDefinition>().ShouldNotBeNull();
        definition.FragmentName.Name.Value.ShouldBe("frag");
    }

    [Fact]
    public void Should_Parse_InputFieldsDefinition()
    {
        string text = "{ name: String age: Int }";
        var definition = text.Parse<GraphQLInputFieldsDefinition>().ShouldNotBeNull();
        definition.Items.Count.ShouldBe(2);
    }

    [Fact]
    public void Should_Parse_InputObjectTypeDefinition()
    {
        string text = "input Person { name: String }";
        var definition = text.Parse<GraphQLInputObjectTypeDefinition>().ShouldNotBeNull();
        definition.Name.Value.ShouldBe("Person");
    }

    [Fact]
    public void Should_Parse_InterfaceTypeDefinition()
    {
        string text = "interface Person { name: String }";
        var definition = text.Parse<GraphQLInterfaceTypeDefinition>().ShouldNotBeNull();
        definition.Name.Value.ShouldBe("Person");
    }

    [Fact]
    public void Should_Parse_ObjectTypeDefinition()
    {
        string text = "type Person { name: String }";
        var definition = text.Parse<GraphQLObjectTypeDefinition>().ShouldNotBeNull();
        definition.Name.Value.ShouldBe("Person");
    }

    [Fact]
    public void Should_Parse_OperationDefinition()
    {
        string text = "mutation x { set(value: 1) }";
        var definition = text.Parse<GraphQLOperationDefinition>().ShouldNotBeNull();
        definition.Name.Value.ShouldBe("x");
    }

    [Fact]
    public void Should_Parse_RootOperationTypeDefinition()
    {
        string text = "query: Q";
        var definition = text.Parse<GraphQLRootOperationTypeDefinition>().ShouldNotBeNull();
        definition.Operation.ShouldBe(OperationType.Query);
    }

    [Fact]
    public void Should_Parse_ScalarTypeDefinition()
    {
        string text = "scalar JSON";
        var definition = text.Parse<GraphQLScalarTypeDefinition>().ShouldNotBeNull();
        definition.Name.Value.ShouldBe("JSON");
    }

    [Fact]
    public void Should_Parse_SchemaDefinition()
    {
        string text = "schema { query: Q subscription: S }";
        var definition = text.Parse<GraphQLSchemaDefinition>().ShouldNotBeNull();
        definition.OperationTypes.Count.ShouldBe(2);
    }

    [Fact]
    public void Should_Parse_UnionTypeDefinition()
    {
        string text = "union U = A | B";
        var definition = text.Parse<GraphQLUnionTypeDefinition>().ShouldNotBeNull();
        definition.Name.Value.ShouldBe("U");
    }

    [Fact]
    public void Should_Parse_VariableDefinition()
    {
        string text = "$id: Int";
        var definition = text.Parse<GraphQLVariableDefinition>().ShouldNotBeNull();
        definition.Variable.Name.Value.ShouldBe("id");
    }

    [Fact]
    public void Should_Parse_VariablesDefinition()
    {
        string text = "($id: Int, $amount: Float)";
        var definition = text.Parse<GraphQLVariablesDefinition>().ShouldNotBeNull();
        definition.Items.Count.ShouldBe(2);
    }

    [Fact]
    public void Should_Parse_EnumTypeExtension()
    {
        string text = "extend enum Color { YELLOW }";
        var definition = text.Parse<GraphQLEnumTypeExtension>().ShouldNotBeNull();
        definition.Name.Value.ShouldBe("Color");
    }

    [Fact]
    public void Should_Parse_InputObjectTypeExtension()
    {
        string text = "extend input Person { address: String }";
        var definition = text.Parse<GraphQLInputObjectTypeExtension>().ShouldNotBeNull();
        definition.Name.Value.ShouldBe("Person");
    }

    [Fact]
    public void Should_Parse_InterfaceTypeExtension()
    {
        string text = "extend interface Person { address: String }";
        var definition = text.Parse<GraphQLInterfaceTypeExtension>().ShouldNotBeNull();
        definition.Name.Value.ShouldBe("Person");
    }

    [Fact]
    public void Should_Parse_ObjectTypeExtension()
    {
        string text = "extend type Person { address: String }";
        var definition = text.Parse<GraphQLObjectTypeExtension>().ShouldNotBeNull();
        definition.Name.Value.ShouldBe("Person");
    }

    [Fact]
    public void Should_Parse_ScalarTypeExtension()
    {
        string text = "extend scalar JSON @my";
        var definition = text.Parse<GraphQLScalarTypeExtension>().ShouldNotBeNull();
        definition.Name.Value.ShouldBe("JSON");
    }

    [Fact]
    public void Should_Parse_SchemaExtension()
    {
        string text = "extend schema { subscription : S }";
        var definition = text.Parse<GraphQLSchemaExtension>().ShouldNotBeNull();
        definition.OperationTypes.Count.ShouldBe(1);
    }

    [Fact]
    public void Should_Parse_UnionTypeExtension()
    {
        string text = "extend union U @my @external";
        var definition = text.Parse<GraphQLUnionTypeExtension>().ShouldNotBeNull();
        definition.Name.Value.ShouldBe("U");
        definition.Directives.Items.Count.ShouldBe(2);
    }
}
