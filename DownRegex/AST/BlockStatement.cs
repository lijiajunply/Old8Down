using System.Collections.Generic;
using System.Text;

namespace DownRegex.AST;

public class BlockStatement : Statement
{
    public bool         isLang      { get; set; } = false;
    public string       Lang        { get; set; }
    public List<string> BlockString { get; set; }

    public BlockStatement(string lang, List<string> blockString)
    {
        isLang = true;
        Lang = lang;
        BlockString = blockString;
    }
    public override string ToHTML()
    {
        StringBuilder a = new StringBuilder();
        BlockString.ForEach(x=>a.Append(x+"\n"));
        return @"<pre><code class=""language-"+Lang+@""">"+a.ToString()+@"</code></pre>";
    }
}