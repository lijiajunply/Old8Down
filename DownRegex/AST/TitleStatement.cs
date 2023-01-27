namespace DownRegex.AST;

public class TitleStatement : Statement
{
    public int Count { get; set; }
    public string Text { get; set; }

    public TitleStatement(int count, string text)
    {
        Text = text;
        Count = count;
    }

    public override string ToHTML() => $"<h{Count}>{Text}</h{Count}>";
}