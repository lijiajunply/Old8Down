using System.Collections.Generic;
using System.Text;

namespace DownRegex.AST;

public class TableStatement : Statement
{
    private string[] Head { get; set; }

    private List<string[]> Context { get; set; } = new List<string[]>();

    public TableStatement(string[] head,List<string[]> context)
    {
        Head    = head;
        Context = context;
    }

    public override string ToHTML()
    {
        StringBuilder builder = new StringBuilder(@"<table class=""tabletable-bordered"">"+"\n");
        if (Head.Length != 0)
        {
            builder.Append("<thead><tr>\n");
            foreach (var s in Head)
                builder.Append($" <th>{s}</th> ");
            builder.Append("\n</tr></thead>");
        }
        if (Context.Count!=0)
        {
            builder.Append("\n<tbody>\n");
            foreach (var v in Context)
            {
                builder.Append("<tr>");
                foreach (var variable in v)
                    builder.Append($" <td>{variable}</td> ");
                builder.Append("</tr>\n");
            }
            builder.Append("</tbody>");
        }
        builder.Append("\n</table>");
        return builder.ToString();
    }
}