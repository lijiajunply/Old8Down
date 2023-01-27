using System.Collections.Generic;
using System.Text;

namespace DownRegex.AST;

public class OrderlyStatement : Statement
{
    public List<string> Lists { get; set; }

    public OrderlyStatement(List<string> list)
    {
        Lists = list;
    }

    public override string ToHTML()
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("<ol>\n");
        foreach (var VARIABLE in Lists)
        {
            stringBuilder.Append($"     <li>{VARIABLE}</li>\n");
        }
        stringBuilder.Append("</ol>");
        return stringBuilder.ToString();
    }
}