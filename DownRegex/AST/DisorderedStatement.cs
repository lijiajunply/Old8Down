using System.Collections.Generic;
using System.Text;

namespace DownRegex.AST;

public class DisorderedStatement : Statement
{
    public List<string> Lists { get; set; }

    public DisorderedStatement(List<string> list)
    {
        Lists = list;
    }

    public override string ToHTML()
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("<ul>\n");
        foreach (var VARIABLE in Lists)
        {
            stringBuilder.Append($"     <li>{VARIABLE}</li>\n");
        }
        stringBuilder.Append("</ul>");
        return stringBuilder.ToString();
    }
}