using System.Reflection;

namespace DownRegex.AST;

public class AtStatement : Statement
{
    public string AtUri { get; set; }
    public string Context { get; set; }

    public AtStatement(string atUri, string context)
    {
        Context = context;
        AtUri = atUri;
    }

    public override string ToHTML() => $"<a href={AtUri} target="+"opentype"+$">{Context}</a>";
}