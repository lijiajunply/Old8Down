using System.Reflection;

namespace DownRegex.AST;

public class AtStatement : Statement
{
    private string AtUri   { get; set; }
    private string Context { get; set; }

    public AtStatement(string atUri, string context)
    {
        Context = context;
        AtUri = atUri;
    }

    public override string ToHTML() => $"<a href={AtUri} target="+"opentype"+$">{Context}</a>";
}