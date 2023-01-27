namespace DownRegex.AST;

public class StringStatement : Statement
{
    private string Context { get; set; }

    public StringStatement(string context) => Context = context;

    public override string ToHTML() => Context;
}