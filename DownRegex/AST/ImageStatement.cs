namespace DownRegex.AST;

public class ImageStatement : Statement
{
    public string FilePath { get; set; }
    public string Name { get; set; }
    public string Title { get; set; }

    public ImageStatement(string filePath, string name,string title)
    {
        FilePath = filePath;
        Name = name;
        Title = title;
    }

    public override string ToHTML() => $"<div><img src={FilePath} alt={Name} title={Title}></div>";
}