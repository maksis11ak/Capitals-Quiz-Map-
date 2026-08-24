// CapitalsMap.cs
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

class Country
{
    [JsonPropertyName("name")] public string Name { get; set; }
    [JsonPropertyName("capital")] public string Capital { get; set; }
    [JsonPropertyName("continent")] public string Continent { get; set; }
    [JsonPropertyName("x")] public int X { get; set; }
    [JsonPropertyName("y")] public int Y { get; set; }

    public Country() { }
    public Country(string name, string capital, string continent, int x, int y)
    {
        Name = name; Capital = capital; Continent = continent; X = x; Y = y;
    }
}

class Stats
{
    [JsonPropertyName("correct")] public int Correct { get; set; }
    [JsonPropertyName("incorrect")] public int Incorrect { get; set; }
    [JsonPropertyName("total")] public int Total { get; set; }
}

class CapitalsMap
{
    private static readonly List<Country> Countries = new List<Country>
    {
        new Country("France", "Paris", "Europe", 2, 3),
        new Country("Germany", "Berlin", "Europe", 4, 4),
        new Country("Italy", "Rome", "Europe", 3, 5),
        new Country("Spain", "Madrid", "Europe", 1, 4),
        new Country("United Kingdom", "London", "Europe", 1, 3),
        new Country("Portugal", "Lisbon", "Europe", 0, 4),
        new Country("Netherlands", "Amsterdam", "Europe", 3, 3),
        new Country("Belgium", "Brussels", "Europe", 2, 3),
        new Country("Switzerland", "Bern", "Europe", 3, 4),
        new Country("Austria", "Vienna", "Europe", 4, 4),
        new Country("Greece", "Athens", "Europe", 5, 5),
        new Country("Turkey", "Ankara", "Europe", 6, 4),
        new Country("Poland", "Warsaw", "Europe", 5, 3),
        new Country("Sweden", "Stockholm", "Europe", 4, 1),
        new Country("Norway", "Oslo", "Europe", 3, 0),
        new Country("Denmark", "Copenhagen", "Europe", 4, 2),
        new Country("Finland", "Helsinki", "Europe", 5, 0),
        new Country("Ireland", "Dublin", "Europe", 0, 2),
        new Country("China", "Beijing", "Asia", 8, 3),
        new Country("Japan", "Tokyo", "Asia", 10, 2),
        new Country("India", "New Delhi", "Asia", 7, 5),
        new Country("South Korea", "Seoul", "Asia", 9, 2),
        new Country("Russia", "Moscow", "Asia", 6, 2),
        new Country("Saudi Arabia", "Riyadh", "Asia", 7, 6),
        new Country("Iran", "Tehran", "Asia", 6, 5),
        new Country("Pakistan", "Islamabad", "Asia", 7, 5),
        new Country("Indonesia", "Jakarta", "Asia", 9, 7),
        new Country("Vietnam", "Hanoi", "Asia", 8, 5),
        new Country("Thailand", "Bangkok", "Asia", 8, 6),
        new Country("Malaysia", "Kuala Lumpur", "Asia", 8, 7),
        new Country("Philippines", "Manila", "Asia", 9, 5),
        new Country("Israel", "Jerusalem", "Asia", 6, 4),
        new Country("Egypt", "Cairo", "Africa", 4, 6),
        new Country("South Africa", "Pretoria", "Africa", 5, 9),
        new Country("Nigeria", "Abuja", "Africa", 3, 7),
        new Country("Kenya", "Nairobi", "Africa", 4, 8),
        new Country("Morocco", "Rabat", "Africa", 1, 6),
        new Country("Ghana", "Accra", "Africa", 2, 7),
        new Country("Ethiopia", "Addis Ababa", "Africa", 4, 7),
        new Country("Tanzania", "Dodoma", "Africa", 4, 8),
        new Country("Algeria", "Algiers", "Africa", 2, 6),
        new Country("Sudan", "Khartoum", "Africa", 4, 7),
        new Country("USA", "Washington", "North America", 2, 1),
        new Country("Canada", "Ottawa", "North America", 1, 0),
        new Country("Mexico", "Mexico City", "North America", 2, 3),
        new Country("Cuba", "Havana", "North America", 3, 3),
        new Country("Jamaica", "Kingston", "North America", 3, 4),
        new Country("Panama", "Panama City", "North America", 3, 5),
        new Country("Brazil", "Brasilia", "South America", 4, 7),
        new Country("Argentina", "Buenos Aires", "South America", 4, 9),
        new Country("Chile", "Santiago", "South America", 3, 9),
        new Country("Peru", "Lima", "South America", 3, 8),
        new Country("Colombia", "Bogota", "South America", 3, 6),
        new Country("Venezuela", "Caracas", "South America", 3, 5),
        new Country("Ecuador", "Quito", "South America", 3, 7),
        new Country("Australia", "Canberra", "Oceania", 9, 9),
        new Country("New Zealand", "Wellington", "Oceania", 10, 10)
    };

    private static readonly List<string> Continents = new List<string> { "Europe", "Asia", "Africa", "North America", "South America", "Oceania" };
    private const string DataFile = "capitals_stats.json";
    private Stats stats = new Stats();

    public CapitalsMap() => LoadStats();

    private void LoadStats()
    {
        if (!File.Exists(DataFile)) return;
        string json = File.ReadAllText(DataFile);
        stats = JsonSerializer.Deserialize<Stats>(json) ?? new Stats();
    }

    private void SaveStats()
    {
        string json = JsonSerializer.Serialize(stats, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(DataFile, json);
    }

    private List<Country> GetCountries(string continent)
    {
        if (string.IsNullOrEmpty(continent)) return Countries;
        return Countries.Where(c => c.Continent == continent).ToList();
    }

    private void List(string continent)
    {
        var countries = GetCountries(continent);
        Console.WriteLine($"\n🌍 Countries ({countries.Count})");
        if (!string.IsNullOrEmpty(continent)) Console.WriteLine($"Continent: {continent}\n");
        else Console.WriteLine("All continents\n");
        var grouped = countries.GroupBy(c => c.Continent).ToDictionary(g => g.Key, g => g.ToList());
        foreach (var cont in Continents)
        {
            if (grouped.TryGetValue(cont, out var list))
            {
                Console.WriteLine($"  {cont}:");
                foreach (var c in list.OrderBy(c => c.Name))
                    Console.WriteLine($"    {c.Name} – {c.Capital}");
            }
        }
    }

    private void DrawMap()
    {
        Console.WriteLine("\n🗺️ World Map (countries marked with *)");
        Console.WriteLine("  Europe          Asia");
        Console.WriteLine(" ┌────────────┐  ┌────────────┐");
        Console.WriteLine(" │  *  *  *    │  │  *  *  *   │");
        Console.WriteLine(" │  *  *  *    │  │  *  *  *   │");
        Console.WriteLine(" └────────────┘  └────────────┘");
        Console.WriteLine("  Africa          North America");
        Console.WriteLine(" ┌────────────┐  ┌────────────┐");
        Console.WriteLine(" │  *  *  *    │  │  *  *  *   │");
        Console.WriteLine(" │  *  *  *    │  │  *  *  *   │");
        Console.WriteLine(" └────────────┘  └────────────┘");
        Console.WriteLine("  South America   Oceania");
        Console.WriteLine(" ┌────────────┐  ┌────────────┐");
        Console.WriteLine(" │  *  *  *    │  │  *  *      │");
        Console.WriteLine(" │  *  *  *    │  │  *  *      │");
        Console.WriteLine(" └────────────┘  └────────────┘");
        Console.WriteLine("\nLegend: * = country present");
    }

    private string GetHint(Country c, string capital, int level)
    {
        switch (level)
        {
            case 0: return $"First letter: '{capital[0]}'";
            case 1: return $"Number of letters: {capital.Length}";
            case 2: return $"Continent: {c.Continent}";
            default: return "No more hints.";
        }
    }

    private void Quiz(string continent, bool reverse)
    {
        var countries = GetCountries(continent);
        if (!countries.Any())
        {
            Console.WriteLine("No countries found.");
            return;
        }
        var rnd = new Random();
        countries = countries.OrderBy(x => rnd.Next()).ToList();
        var selected = countries.Take(10).ToList();
        int total = selected.Count, correct = 0;

        Console.WriteLine($"\n🧠 Capitals Quiz");
        string mode = reverse ? "Capital → Country" : "Country → Capital";
        Console.WriteLine($"Mode: {mode}");
        if (!string.IsNullOrEmpty(continent)) Console.WriteLine($"Continent: {continent}");
        Console.WriteLine($"Questions: {total}\n");

        for (int i = 0; i < selected.Count; i++)
        {
            var c = selected[i];
            string question = reverse ? $"Capital: {c.Capital}" : $"Country: {c.Name}";
            string answer = reverse ? c.Name : c.Capital;
            int hintLevel = 0;
            Console.WriteLine($"Round {i+1}/{total}");
            Console.WriteLine(question);
            while (true)
            {
                Console.Write("Your answer (or 'hint', 'skip', 'quit'): ");
                string input = Console.ReadLine()?.Trim() ?? "";
                switch (input.ToLower())
                {
                    case "quit":
                        Console.WriteLine("Quitting...");
                        stats.Total += correct + (i - correct);
                        SaveStats();
                        return;
                    case "skip":
                        Console.WriteLine($"Skipped. The answer is: {answer}");
                        break;
                    case "hint":
                        Console.WriteLine($"💡 Hint: {GetHint(c, c.Capital, hintLevel)}");
                        hintLevel = (hintLevel + 1) % 3;
                        continue;
                    default:
                        if (input.Equals(answer, StringComparison.OrdinalIgnoreCase))
                        {
                            Console.WriteLine("✅ Correct!");
                            correct++;
                            break;
                        }
                        else
                        {
                            Console.WriteLine("❌ Wrong. Try again (or type 'skip')");
                            continue;
                        }
                }
                break;
            }
            Console.WriteLine($"Score: {correct}/{i+1} ({(double)correct/(i+1)*100:F1}%)\n");
        }

        stats.Correct += correct;
        stats.Incorrect += total - correct;
        stats.Total += total;
        SaveStats();
        Console.WriteLine($"Quiz finished! Score: {correct}/{total} ({(double)correct/total*100:F1}%)");
    }

    private void ShowStats()
    {
        Console.WriteLine("\n📊 Statistics");
        Console.WriteLine($"Correct: {stats.Correct}");
        Console.WriteLine($"Incorrect: {stats.Incorrect}");
        Console.WriteLine($"Total: {stats.Total}");
        if (stats.Total > 0)
        {
            double pct = (double)stats.Correct / stats.Total * 100;
            Console.WriteLine($"Accuracy: {pct:F1}%");
        }
    }

    static void Main(string[] args)
    {
        if (args.Length < 1)
        {
            Console.WriteLine("Usage: CapitalsMap <command> [options]");
            return;
        }
        var app = new CapitalsMap();
        var cmd = args[0];
        var parsed = ParseArgs(args);
        switch (cmd)
        {
            case "list":
                app.List(parsed.GetValueOrDefault("continent"));
                break;
            case "map":
                app.DrawMap();
                break;
            case "quiz":
                app.Quiz(parsed.GetValueOrDefault("continent"), parsed.ContainsKey("reverse"));
                break;
            case "stats":
                app.ShowStats();
                break;
            default:
                Console.WriteLine("Unknown command. Use list, map, quiz, stats.");
                break;
        }
    }

    static Dictionary<string, string> ParseArgs(string[] args)
    {
        var dict = new Dictionary<string, string>();
        for (int i = 1; i < args.Length; i++)
        {
            if (args[i].StartsWith("--") && i + 1 < args.Length)
                dict[args[i].Substring(2)] = args[++i];
        }
        return dict;
    }
}
