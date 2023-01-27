using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using DownRegex.AST;

namespace DownRegex;

public class RegexDown
{
    #region List
    public List<string>              DisorderedList { get; set; } = new ();
    public List<string>              OrderList      { get; set; } = new ();
    public List<string>              BlockList      { get; set; } = new ();
    public List<string>              CssUsing       { get; set; } = new ();
    public List<string>              JSUsing        { get; set; } = new ();
    public List<Statement>           Asts           { get; set; } = new ();
    public Dictionary<string,string> Regexs         { get; set; } = new ();
    #endregion


    #region Field
    public bool     isBlock { get; set; }
    public string   Lang    { get; set; }
    public string[] Code    { get; set; }
    #endregion

    public RegexDown(string[] code)
    {
        isBlock = false;
        Code    = code;

        #region Regexs

        Regexs.Add("TitleStatement",     @"\#\[([2-6])\]\s(.*)");    // #[int(1-6)] (text)
        Regexs.Add("TitleH1Statement",   @"\#\s(.*)");               // # (text)
        Regexs.Add("AtStatement",        @"@\[(.*),(.*)\]");         // @(url,text)
        Regexs.Add("DisorderedStatement","\\.\\s(.*)");              // . text
        Regexs.Add("OrderlyStatement",   "\\-\\s(.*)");              // - text
        Regexs.Add("BlockStatement",     "```(.*)");                 // ```lang ```
        Regexs.Add("AttributeStatement", "\\((.*?),(.*?)\\)"); // (att,att_text)
        Regexs.Add("ImageStatement",     "\\!\\[(.*),(.*),(.*)\\]"); // ![src,alt,title]
        Regexs.Add("EscapeStatement",    "//\\s(.*)");               // // text

        #endregion

        #region Css&JS

        CssUsing.Add(@"<link rel=""stylesheet"" href=""prism.css"">");
        JSUsing.Add(@"<script src = ""https://cdn.jsdelivr.net/npm/jquery@3.2/dist/jquery.min.js""></script>"+"\n"+
                        @"<script src =""https://cdn.jsdelivr.net/npm/semantic-ui@2.5.0/dist/semantic.min.js""></script>"
                        +"\n"+
                        @"<script src =""prism.js""></script>");
        #endregion

    }

    private void Init()
    {
        DisorderedList = new List<string>();
        OrderList      = new List<string>();
        BlockList      = new List<string>();
        Lang           = String.Empty;
    }

    private List<Statement> GetTree()
    {
        Search(0);
        return Asts;
    }

    public string ToHTML()
    {
        GetTree();
        StringBuilder builder = new StringBuilder("<!DOCTYPE html>\n"+@"<html lang=""en"">"+"\n<body>"+"\n"+
                                                  @"<meta http-equiv=""Context-Type"" content=""text/html;charset=utf-8"" />");
        foreach (var css in CssUsing)
        {
            builder.Append(css+"\n");
        }
        foreach (var js in JSUsing)
        {
            builder.Append(js+"\n");
        }
        foreach (var statement in Asts)
        {
            builder.Append(statement.ToHTML()+"\n");
        }

        builder.Append("</body>\n</html>");
        return builder.ToString();
    }

    public string Search(int line)
    {
        if (line >= Code.Length)
        {
            if (DisorderedList.Count > 0)
            {
                Asts.Add(new DisorderedStatement(DisorderedList));
                return String.Empty;
            }
            if (OrderList.Count > 0)
            {
                Asts.Add(new OrderlyStatement(OrderList));
                return String.Empty;
            }
            if (BlockList.Count > 0)
            {
                Asts.Add(new BlockStatement(Lang,BlockList));
                return String.Empty;
            }
            Init();
            return String.Empty;
        }

        string code = Code[line];


        #region BlockStatement

        if (code == "```")
        {
            isBlock = false;
            return Search(line+1);
        }
        if (isBlock)
        {
            BlockList.Add(code);
            return Search(line+1);
        }
        if (Regex.IsMatch(code,Regexs["BlockStatement"]))
        {
            var a = Regex.Match(code,Regexs["BlockStatement"]);
            Lang    = a.Groups[1].Value;
            isBlock = true;
            return Search(line+1);
        }

        #endregion


        
        #region Escape

        if (Regex.IsMatch(code,Regexs["EscapeStatement"]))
        {
            var a = Regex.Match(code,Regexs["EscapeStatement"]);
            Asts.Add(new EscapeStatement(a.Groups[1].Value));
            return Search(line+1);
        }

        #endregion


        #region List

        if (Regex.IsMatch(code,Regexs["DisorderedStatement"]))
        {
            var a = Regex.Match(code,Regexs["DisorderedStatement"]);
            DisorderedList.Add(a.Groups[1].Value);
            return Search(line+1);
        }
        if (Regex.IsMatch(code,Regexs["OrderlyStatement"]))
        {
            var a = Regex.Match(code,Regexs["OrderlyStatement"]);
            OrderList.Add(a.Groups[1].Value);
            return Search(line+1);
        }

        #endregion


        #region init

        List<string> aaa = new List<string>();
        if (DisorderedList.Count > 0)
        {
            aaa = DisorderedList;
            Asts.Add(new DisorderedStatement(aaa));
        }
        if (OrderList.Count > 0)
        {
            aaa = OrderList;
            Asts.Add(new OrderlyStatement(aaa));

        }
        if (BlockList.Count > 0)
        {
            aaa = BlockList;
            Asts.Add(new BlockStatement(Lang,aaa));
        }
        Init();

        #endregion


        #region other

        if (Regex.IsMatch(code,Regexs["TitleStatement"]))
        {
            var a = Regex.Match(code,Regexs["TitleStatement"]);
            Asts.Add(new TitleStatement(Int32.Parse(a.Groups[1].Value),a.Groups[2].Value));
            return Search(line+1);
        }
        if (Regex.IsMatch(code,Regexs["TitleH1Statement"]))
        {
            var a = Regex.Match(code,Regexs["TitleH1Statement"]);
            Asts.Add(new TitleStatement(1,a.Groups[1].Value));
            return Search(line+1);
        }
        if (Regex.IsMatch(code,Regexs["AtStatement"]))
        {
            var a = Regex.Match(code,Regexs["AtStatement"]);
            Asts.Add(new AtStatement(a.Groups[1].Value,a.Groups[2].Value));
            return Search(line+1);
        }
        if (Regex.IsMatch(code,Regexs["AttributeStatement"]))
        {
            List<Statement> statements = new List<Statement>();
            
            var re = new Regex(Regexs["AttributeStatement"]);
            var p  = re.Replace(code,"\n").Split("\n");
            var a  = re.Matches(code);
            for (int i = 0; i < p.Length; i++)
            {
                statements.Add(new StringStatement(p[i]));
                if(i < a.Count)
                    statements.Add(new AttributeStatement(a[i].Groups[1].Value,a[i].Groups[2].Value));
            }
            Asts.Add(new ParagraphStatement(statements));
            return Search(line+1);
        }
        if (Regex.IsMatch(code,Regexs["ImageStatement"]))
        {
            var a = Regex.Match(code,Regexs["ImageStatement"]);
            Asts.Add(new ImageStatement(a.Groups[1].Value,a.Groups[2].Value,a.Groups[3].Value));
            return Search(line+1);
        }

        #endregion


        if (code != "")
            Asts.Add(new ParagraphStatement(code));

        return Search(line+1);
    }
}