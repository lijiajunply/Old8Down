using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace DownRegex;

public static class APIs
{
    public static string[] GetTexts(string path) => File.ReadAllLines(path, Encoding.UTF8);

    public static string DownToHTML(string path)
    {
        var a = new RegexDown(GetTexts(path));
        return a.ToHTML();
    }
}