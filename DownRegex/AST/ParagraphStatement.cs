using System;
using System.Collections.Generic;
using System.Text;

namespace DownRegex.AST;

public class ParagraphStatement : Statement
{
    private string          Paragraph { get; set; }
    private StringStatement Statement { get; set; }
    public ParagraphStatement(string          paragraph) => Paragraph = paragraph;
    public ParagraphStatement(StringStatement statement) => Statement = statement;
    public override string ToHTML()
    {
        if (Paragraph != null)
            return $"<p>{Paragraph}</p>";
        if (Statement != null)
            return Statement.ToHTML();
        return String.Empty;
    }
}