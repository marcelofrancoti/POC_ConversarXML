using System.Text.Json;
using System.Xml.Linq;
using Microsoft.VisualBasic.FileIO; 

class Program
{
    static void Main(string[] args)
    {
        var csvPath = "Files/exemplo.csv";
        var xmlPath = "Files/exemplo.xml";

        Console.WriteLine("CSV para JSON:");
        Console.WriteLine(ConvertCsvToJson(csvPath));
        Console.WriteLine("\n-----------------------\n");

        Console.WriteLine("XML para JSON:");
        Console.WriteLine(ConvertXmlToJson(xmlPath));
    }

    static string ConvertCsvToJson(string filePath)
    {
        using var parser = new TextFieldParser(filePath);
        parser.TextFieldType = FieldType.Delimited;
        parser.SetDelimiters(",");

        var headers = parser.ReadFields();
        var rows = new System.Collections.Generic.List<Dictionary<string, string>>();

        while (!parser.EndOfData)
        {
            var fields = parser.ReadFields();
            var row = headers.Zip(fields, (h, f) => new { h, f })
                             .ToDictionary(x => x.h, x => x.f);
            rows.Add(row);
        }

        return JsonSerializer.Serialize(rows, new JsonSerializerOptions { WriteIndented = true });
    }

    static string ConvertXmlToJson(string filePath)
    {
        var doc = XDocument.Load(filePath);
        var root = doc.Root;

        var data = root.Elements()
                       .Select(e => e.Elements()
                                     .ToDictionary(x => x.Name.LocalName, x => x.Value))
                       .ToList();

        return JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
    }
}
