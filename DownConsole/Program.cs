using System;
using System.IO;
using System.Text.RegularExpressions;
using DownRegex;


var a = APIs.GetTexts("/home/luckyfish/RiderProjects/Old8Down/DownRegex/CSS/Ex.od");
foreach (var variable in a)
    Console.WriteLine(variable);
var html = APIs.DownToHTML("/home/luckyfish/RiderProjects/Old8Down/DownRegex/CSS/Ex.od");
Console.WriteLine(html);
File.WriteAllText("/home/luckyfish/RiderProjects/Old8Down/DownRegex/CSS/Ex.html",html);