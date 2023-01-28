using System.Text;
using System.Text.RegularExpressions;

namespace DownRegex.AST;

public class StringStatement : Statement
{
    private string Context { get; set; }

    public List<Statement> Children { get; set; } = new List<Statement>();

    public StringStatement(string context)
    {
        Context = context;
    }

    public override string ToHTML()
    {
        GetChildren();
        if (Children.Count == 0)
            return Context;
        StringBuilder builder = new StringBuilder();
        foreach (var statement in Children)
            builder.Append(statement.ToHTML());
        return builder.ToString();
    }

    public List<Statement> GetChildren()
    {
        BuildChildren();
        if (Children.Count != 0){
            foreach (var child in Children)
                if (child is StringStatement statement)
                    statement.BuildChildren();
        }

        return Children;
    }
    private void BuildChildren()
    {
        var             Regexs = DownApi.Regexs;
        List<Statement> a      = new List<Statement>();
        if (Regex.IsMatch(Context,Regexs["AttributeStatement"]))
        {
            var regex   = new Regex(Regexs["AttributeStatement"]);
            var matches = regex.Matches(Context);
            var s       = regex.Replace(Context,"\n\r\n");
            var strings = s.Split("\n").Where(x => x != "").ToArray();
            int i       = 0;
            foreach (var s1 in strings)
            {
                if (s1 == "\r")
                {
                    var _ = new AttributeStatement(matches[i].Groups[1].Value,matches[i].Groups[2].Value);
                    a.Add(_);
                    i++;
                }
                else
                    a.Add(new StringStatement(s1));
            }
            Children = a.Count == 0 ? new List<Statement>() : a;
            return;
        }
        if (Regex.IsMatch(Context,Regexs["AtStatement"]))
        {
            var regex   = new Regex(Regexs["AtStatement"]);
            var matches = regex.Matches(Context);
            var s       = regex.Replace(Context,"\n\r\n");
            var strings = s.Split("\n").Where(x => x != "").ToArray();
            int i       = 0;
            foreach (var s1 in strings)
            {
                if (s1 == "\r")
                {
                    var _ = new AtStatement(matches[i].Groups[1].Value,matches[i].Groups[2].Value);
                    a.Add(_);
                    i++;
                }
                else
                    a.Add(new StringStatement(s1));
            }
            Children = a.Count == 0 ? new List<Statement>() : a;
        }
    }
}