using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using DownRegex.AST;

namespace DownRegex;

public class RegexDown
{
    #region List
    private List<string>              DisorderedList { get; set; } = new List<string>();
    private List<string>              OrderList      { get; set; } = new List<string>();
    private List<string>              BlockList      { get; set; } = new List<string>();
    private List<string>              CssUsing       { get; set; } = new List<string>();
    private List<string>              JSUsing        { get; set; } = new List<string>();
    public  List<Statement>           Asts           { get; set; } = new List<Statement>();
    private Dictionary<string,string> Regexs         { get; set; } = DownApi.Regexs;
    private List<string[]>            Context        { get; set; } = new List<string[]>();
    #endregion


    #region Field
    private bool     IsBlock { get; set; }
    private string   Lang    { get; set; }
    private string[] Code    { get; set; }
    private string[] Head    { get; set; }
    private string   AboveOp { get; set; }
    #endregion

    public RegexDown(string[] code)
    {
        IsBlock = false;
        Code    = code;

        #region Regexs
        
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
        if (DisorderedList.Count > 0)
            Asts.Add(new DisorderedStatement(DisorderedList));
        else if (OrderList.Count > 0)
            Asts.Add(new OrderlyStatement(OrderList));
        else if (BlockList.Count > 0)
            Asts.Add(new BlockStatement(Lang,BlockList));
        else if (Context.Count > 0)
            Asts.Add(new TableStatement(Head,Context));
        DisorderedList = new List<string>();
        OrderList      = new List<string>();
        BlockList      = new List<string>();
        Context        = new List<string[]>();
        Head           = Array.Empty<string>();
        Lang           = String.Empty;
    }

    private List<Statement> GetTree()
    {
        Search(0);
        return Asts;
    }

    public string ToHTMLComplete()
    {
        GetTree();
        StringBuilder builder = new StringBuilder("<!DOCTYPE html>\n"+@"<html lang=""en"">"+"\n<body>"+"\n"+
                                                  @"<meta http-equiv=""Context-Type"" content=""text/html;charset=utf-8"" />");
        foreach (var css in CssUsing)
            builder.Append(css+"\n");
        foreach (var js in JSUsing)
            builder.Append(js+"\n");
        foreach (var statement in Asts)
            builder.Append(statement.ToHTML()+"\n");

        builder.Append("</body>\n</html>");
        return builder.ToString();
    }
    public string ToHTML()
    {
        GetTree();
        StringBuilder builder = new StringBuilder();
        foreach (var css in CssUsing)
            builder.Append(css+"\n");
        foreach (var js in JSUsing)
            builder.Append(js+"\n");
        foreach (var statement in Asts)
            builder.Append(statement.ToHTML()+"\n");
        return builder.ToString();
    }

    private string Search(int line)
    {

        #region end

        if (line >= Code.Length)
        {
            Init();
            return String.Empty;
        }

        #endregion
        

        var code = Code[line];


        #region BlockStatement

        if (code == "```")
        {
            IsBlock = false;
            return Search(line+1);
        }
        if (IsBlock)
        {
            BlockList.Add(code);
            return Search(line+1);
        }
        if (Regex.IsMatch(code,Regexs["BlockStatement"]))
        {
            if(AboveOp != "BlockOp")
                Init();
            AboveOp = "BlockOp";
            var a = Regex.Match(code,Regexs["BlockStatement"]);
            Lang    = a.Groups[1].Value;
            IsBlock = true;
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
            if(AboveOp != "DisOp")
                Init();
            AboveOp = "DisOp";
            var a = Regex.Match(code,Regexs["DisorderedStatement"]);
            DisorderedList.Add(a.Groups[1].Value);
            return Search(line+1);
        }
        if (Regex.IsMatch(code,Regexs["OrderlyStatement"]))
        {
            if(AboveOp != "OrderOp")
                Init();
            AboveOp = "OrderOp";
            var a = Regex.Match(code,Regexs["OrderlyStatement"]);
            OrderList.Add(a.Groups[1].Value);
            return Search(line+1);
        }
        if (Regex.IsMatch(code,Regexs["TableStatement"]))
        {
            if(AboveOp != "TableOp")
                Init();
            AboveOp = "TableOp";
            var a = Regex.Matches(code,Regexs["TableStatement"]);
            if (code[0] == '*' && code[1] == ' ')
            {
                Head = new string[a.Count];
                for (int i = 0; i < a.Count; i++)
                {
                    Match match = a[i];
                    Head[i] = 
                        i == 0 ? match.Groups[1].Value[2..^1] : match.Groups[1].Value[..^1];
                }
                return Search(line+1);
            }
            
            var l = new List<string>();
            foreach (Match match in a)
                l.Add(match.Groups[1].Value[..^1]);        
            Context.Add(l.ToArray());
            return Search(line+1);
        }

        #endregion
        
        #region init
        
        Init();
        AboveOp = "OtherOp";

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
        if (Regex.IsMatch(code,Regexs["ImageStatement"]))
        {
            var a = Regex.Match(code,Regexs["ImageStatement"]);
            Asts.Add(new ImageStatement(a.Groups[1].Value,a.Groups[2].Value,a.Groups[3].Value));
            return Search(line+1);
        }
        #endregion

        if (code != "")
            Asts.Add(new ParagraphStatement(new StringStatement(code).ToHTML()));
        return Search(line+1);
    }
    
}