namespace DownRegex.AST;

public class EscapeStatement : Statement
{
    public EscapeStatement(string text) => Text = text;
    public string Text { get; set; }
    public override string ToHTML() => $"<p>{Text}</p>";
}