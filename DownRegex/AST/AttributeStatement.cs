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
        AttributeSet.Add("Delete","~");
    }

    public override string ToHTML()
    {
        string att = "";
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
        return String.Empty;
    }
}