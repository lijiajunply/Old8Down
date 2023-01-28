namespace DownRegex.AST;

public class AttributeStatement : Statement
{
    private string                     TheAttribute    { get; set; }
    private string                     AttributeString { get; set; }
    private Dictionary<string, string> AttributeSet    { get; set; } = new ();

    public AttributeStatement(string theAttribute,string attributeString)
    {
        AttributeString = attributeString;
        TheAttribute = theAttribute;
        AttributeSet.Add("Bold","b");
        AttributeSet.Add("Italic","i");
        AttributeSet.Add("Delete","d");
        AttributeSet.Add("Escape","e");
    }

    public override string ToHTML()
    {
        if (TheAttribute == AttributeSet["Bold"])
        {
            return $"<strong>{AttributeString}</strong>";
        }
        if (TheAttribute == AttributeSet["Italic"])
        {
            return $"<em>{AttributeString}</em>";
        }
        if (TheAttribute == AttributeSet["Delete"])
        {
            return $"<s>{AttributeString}</s>";
        }
        if (TheAttribute == AttributeSet["Escape"])
        {
            return AttributeString;
        }
        return String.Empty;
    }
}