using DownRegex;

var a    = new DownApi("/home/luckyfish/RiderProjects/Old8Down/DownRegex/CSS/Ex.od");
var html = a.DownToHTML();
Console.WriteLine(html);
a.ToFile();