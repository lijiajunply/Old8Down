using System;
using System.Collections.Generic;
using System.Text;

namespace DownRegex.AST;

public class ParagraphStatement : Statement
{
    private string          Paragraph  { get; set; }
    private List<Statement> Statements { get; set; }
    public ParagraphStatement(string          paragraph) => Paragraph = paragraph;
    public ParagraphStatement(List<Statement> statements) => Statements = statements;
    public override string ToHTML()
    {
        if (Paragraph != null)
        {
            return $"<p>{Paragraph}</p>";
        }
        if(Statements.Count!= 0)
        {
            StringBuilder builder = new StringBuilder("<p>");
            foreach (var statement in Statements)
                builder.Append(statement.ToHTML());
            builder.Append("</p>");
            return builder.ToString();
        }
        return String.Empty;
    }
}