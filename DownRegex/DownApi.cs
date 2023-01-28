using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using DownRegex.AST;

namespace DownRegex;

public class DownApi
{
    public static    Dictionary<string,string> Regexs { get; set; } = new Dictionary<string,string>()
                                                                      {
                                                                          {"TitleStatement",     @"\#\[([2-6])\]\s(.*)"},    // #[int(1-6)] text
                                                                          {"TitleH1Statement",   @"\#\s(.*)"},               // # text
                                                                          {"AtStatement",        @"@\[(.*?),(.*?)\]"},        // @[url,text]
                                                                          {"DisorderedStatement","\\.\\s(.*)"},              // . text
                                                                          {"OrderlyStatement",   "\\-\\s(.*)"},              // - text
                                                                          {"BlockStatement",     "```(.*)"},                 // ```lang ```
                                                                          {"AttributeStatement", "\\((.*?),(.*?)\\)"},       // (att,att_text)
                                                                          {"ImageStatement",     "\\!\\[(.*),(.*),(.*)\\]"}, // ![src,alt,title]
                                                                          {"EscapeStatement",    "//\\s(.*)"},               // // text
                                                                          {"TableStatement",     "(.*?\\|)+?"}              // context|context|context|
                                                                      };
    private readonly string          Path;
    private          List<Statement> Asts       { get; set; }
    private          string          Code       { get; set; }
    private          string[]        GetTexts() => File.ReadAllLines(Path,Encoding.UTF8);
    public DownApi(string path)
    {
        Path = path;
    }

    public string DownToHTML()
    {
        var a = new RegexDown(GetTexts());
        Asts = a.Asts;
        Code = a.ToHTMLComplete();
        return a.ToHTMLComplete();
    }
    public void ToFile() => File.WriteAllText(Path,Code);


}