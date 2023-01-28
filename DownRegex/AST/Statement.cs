using System;
using System.Text.RegularExpressions;

namespace DownRegex.AST;

public class Statement
{
    public virtual string ToHTML() => String.Empty;
}