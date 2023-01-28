using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using DownRegex.AST;

namespace DownRegex;

public class DownApi
{
    private readonly string          Path;
    private          List<Statement> Asts       { get; set; }
    private          string[]        GetTexts() => File.ReadAllLines(Path,Encoding.UTF8);
    public DownApi(string path)
    {
        Path = path;
    }

    public string DownToHTML()
    {
        var a = new RegexDown(GetTexts());
        Asts = a.Asts;
        return a.ToHTML();
    }
    
}