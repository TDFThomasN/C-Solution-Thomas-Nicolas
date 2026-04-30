using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Xml.Linq;

string projectRoot  = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", ".."));
string inputFolder  = Path.Combine(projectRoot, "InputData");
string outputFolder = Path.Combine(projectRoot, "OutputData");

Directory.CreateDirectory(outputFolder);

string chosenFile = Directory.GetFiles(inputFolder, "*.json").First();
Console.WriteLine($"Fichier sélectionné : {Path.GetFileName(chosenFile)}");

string rawJson    = File.ReadAllText(chosenFile);
JToken rootToken  = JToken.Parse(rawJson);
JArray jsonArray  = rootToken is JArray arr ? arr : (JArray)((JObject)rootToken).Properties().First(p => p.Value.Type == JTokenType.Array).Value;

List<Dictionary<string, object?>> records = jsonArray
    .Select(token => JsonConvert.DeserializeObject<Dictionary<string, object?>>(token.ToString())!)
    .ToList();

List<string> allFields = records
    .SelectMany(r => r.Keys)
    .Distinct()
    .OrderBy(f => f)
    .ToList();

Console.WriteLine("Champs dispo :");
for (int i = 0; i < allFields.Count; i++)
    Console.WriteLine($"  {i + 1}. {allFields[i]}");

Console.Write("Num des champs à exclure ou Entrée pour skip cette étape : ");
string? input = Console.ReadLine();

List<string> excluded = string.IsNullOrWhiteSpace(input)
    ? new List<string>()
    : input.Split(',')
        .Select(p => p.Trim())
        .Where(p => int.TryParse(p, out _))
        .Select(p => int.Parse(p))
        .Where(i => i >= 1 && i <= allFields.Count)
        .Select(i => allFields[i - 1])
        .ToList();

List<string> fieldsToKeep = allFields.Where(f => !excluded.Contains(f)).ToList();

List<Dictionary<string, object?>> filtered = records
    .Select(r => r.Where(p => fieldsToKeep.Contains(p.Key)).ToDictionary(p => p.Key, p => p.Value))
    .ToList();

Console.WriteLine("--- Aperçu des entrées retenues ---");
Console.WriteLine(string.Join(" | ", fieldsToKeep));
foreach (var record in filtered.Take(5))
    Console.WriteLine(string.Join(" | ", fieldsToKeep.Select(f => record.TryGetValue(f, out var v) ? v?.ToString() : "")));

Console.Write("Exporter ? (O/N) : ");
if (Console.ReadLine()?.Trim().ToUpper() != "O") return;

XDocument xml = new XDocument(
    new XDeclaration("1.0", "utf-8", null),
    new XElement("Records",
        filtered.Select(r =>
            new XElement("Record",
                fieldsToKeep.Where(f => r.ContainsKey(f))
                            .Select(f => new XElement(f, r[f]?.ToString() ?? ""))
            )
        )
    )
);

string outputPath = Path.Combine(outputFolder, Path.GetFileNameWithoutExtension(chosenFile) + ".xml");
xml.Save(outputPath);
Console.WriteLine($"Fichier généré : {outputPath}");
