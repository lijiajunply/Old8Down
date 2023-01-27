using System.Collections.Generic;

namespace DownRegex.AST;

public class TableStatement : Statement
{
    public List<string> Head { get; set; }

    public List<string> Context { get; set; }

    public TableStatement(List<string>? head,List<string> context)
    {
        Head    = head;
        Context = context;
    }

}