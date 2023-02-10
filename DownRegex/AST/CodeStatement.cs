namespace DownRegex.AST;

public class CodeStatement : Statement
{
    private string Lang { get;  set; }
    private string Code { get; set; }

    public CodeStatement(List<string> codes,string lang = "js")
    {
        foreach (var code in codes)
            Code += code+"\n";
        Lang = lang;
    }

    public override string ToHTML()
    {
        if (Lang == "js")
            return $"<script>\n{Code}</script>";
        return String.Empty;
    }
}