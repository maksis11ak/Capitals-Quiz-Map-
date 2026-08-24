// CapitalsMap.java
import java.io.*;
import java.nio.file.*;
import java.util.*;
import java.util.stream.*;
import com.google.gson.*;

class Country {
    String name;
    String capital;
    String continent;
    int x, y;
    Country() {}
    Country(String name, String capital, String continent, int x, int y) {
        this.name = name; this.capital = capital; this.continent = continent;
        this.x = x; this.y = y;
    }
}

class Stats {
    int correct = 0;
    int incorrect = 0;
    int total = 0;
}

public class CapitalsMap {
    private static final List<Country> COUNTRIES = Arrays.asList(
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
    );

    private static final List<String> CONTINENTS = Arrays.asList("Europe", "Asia", "Africa", "North America", "South America", "Oceania");
    private static final String DATA_FILE = "capitals_stats.json";
    private static final Gson gson = new GsonBuilder().setPrettyPrinting().create();

    private Stats stats = new Stats();

    public CapitalsMap() { loadStats(); }

    private void loadStats() {
        try {
            Path path = Paths.get(DATA_FILE);
            if (Files.exists(path)) {
                String json = new String(Files.readAllBytes(path));
                stats = gson.fromJson(json, Stats.class);
            }
        } catch (Exception e) {}
    }

    private void saveStats() {
        try {
            Files.write(Paths.get(DATA_FILE), gson.toJson(stats).getBytes());
        } catch (Exception e) {}
    }

    private List<Country> getCountries(String continent) {
        if (continent == null) return COUNTRIES;
        return COUNTRIES.stream().filter(c -> c.continent.equals(continent)).collect(Collectors.toList());
    }

    private void list(String continent) {
        List<Country> countries = getCountries(continent);
        System.out.printf("\n🌍 Countries (%d)%n", countries.size());
        if (continent != null) System.out.printf("Continent: %s%n%n", continent);
        else System.out.println("All continents\n");
        Map<String, List<Country>> grouped = new LinkedHashMap<>();
        for (Country c : countries) {
            grouped.computeIfAbsent(c.continent, k -> new ArrayList<>()).add(c);
        }
        for (String cont : CONTINENTS) {
            if (grouped.containsKey(cont)) {
                System.out.printf("  %s:%n", cont);
                grouped.get(cont).stream().sorted((a,b) -> a.name.compareTo(b.name)).forEach(
                    c -> System.out.printf("    %s – %s%n", c.name, c.capital)
                );
            }
        }
    }

    private void drawMap() {
        System.out.println("\n🗺️ World Map (countries marked with *)");
        System.out.println("  Europe          Asia");
        System.out.println(" ┌────────────┐  ┌────────────┐");
        System.out.println(" │  *  *  *    │  │  *  *  *   │");
        System.out.println(" │  *  *  *    │  │  *  *  *   │");
        System.out.println(" └────────────┘  └────────────┘");
        System.out.println("  Africa          North America");
        System.out.println(" ┌────────────┐  ┌────────────┐");
        System.out.println(" │  *  *  *    │  │  *  *  *   │");
        System.out.println(" │  *  *  *    │  │  *  *  *   │");
        System.out.println(" └────────────┘  └────────────┘");
        System.out.println("  South America   Oceania");
        System.out.println(" ┌────────────┐  ┌────────────┐");
        System.out.println(" │  *  *  *    │  │  *  *      │");
        System.out.println(" │  *  *  *    │  │  *  *      │");
        System.out.println(" └────────────┘  └────────────┘");
        System.out.println("\nLegend: * = country present");
    }

    private String getHint(Country c, String capital, int level) {
        switch (level) {
            case 0: return "First letter: '" + capital.charAt(0) + "'";
            case 1: return "Number of letters: " + capital.length();
            case 2: return "Continent: " + c.continent;
            default: return "No more hints.";
        }
    }

    private void quiz(String continent, boolean reverse) throws Exception {
        List<Country> countries = getCountries(continent);
        if (countries.isEmpty()) {
            System.out.println("No countries found.");
            return;
        }
        Collections.shuffle(countries);
        List<Country> selected = countries.subList(0, Math.min(10, countries.size()));
        int total = selected.size();
        int correct = 0;

        System.out.printf("\n🧠 Capitals Quiz%n");
        String mode = reverse ? "Capital → Country" : "Country → Capital";
        System.out.printf("Mode: %s%n", mode);
        if (continent != null) System.out.printf("Continent: %s%n", continent);
        System.out.printf("Questions: %d%n%n", total);

        BufferedReader reader = new BufferedReader(new InputStreamReader(System.in));

        for (int i = 0; i < selected.size(); i++) {
            Country c = selected.get(i);
            String question = reverse ? "Capital: " + c.capital : "Country: " + c.name;
            String answer = reverse ? c.name : c.capital;
            int hintLevel = 0;
            System.out.printf("Round %d/%d%n", i+1, total);
            System.out.println(question);
            while (true) {
                System.out.print("Your answer (or 'hint', 'skip', 'quit'): ");
                String input = reader.readLine().trim();
                switch (input.toLowerCase()) {
                    case "quit":
                        System.out.println("Quitting...");
                        stats.total += correct + (i - correct);
                        saveStats();
                        return;
                    case "skip":
                        System.out.printf("Skipped. The answer is: %s%n", answer);
                        break;
                    case "hint":
                        System.out.printf("💡 Hint: %s%n", getHint(c, c.capital, hintLevel));
                        hintLevel = (hintLevel + 1) % 3;
                        continue;
                    default:
                        if (input.equalsIgnoreCase(answer)) {
                            System.out.println("✅ Correct!");
                            correct++;
                            break;
                        } else {
                            System.out.println("❌ Wrong. Try again (or type 'skip')");
                            continue;
                        }
                }
                break;
            }
            System.out.printf("Score: %d/%d (%.1f%%)%n%n", correct, i+1, (double)correct/(i+1)*100);
        }

        stats.correct += correct;
        stats.incorrect += total - correct;
        stats.total += total;
        saveStats();
        System.out.printf("Quiz finished! Score: %d/%d (%.1f%%)%n", correct, total, (double)correct/total*100);
    }

    private void showStats() {
        System.out.println("\n📊 Statistics");
        System.out.printf("Correct: %d%n", stats.correct);
        System.out.printf("Incorrect: %d%n", stats.incorrect);
        System.out.printf("Total: %d%n", stats.total);
        if (stats.total > 0) {
            double pct = (double)stats.correct / stats.total * 100;
            System.out.printf("Accuracy: %.1f%%%n", pct);
        }
    }

    public static void main(String[] args) throws Exception {
        if (args.length < 1) {
            System.out.println("Usage: CapitalsMap <command> [options]");
            return;
        }
        CapitalsMap app = new CapitalsMap();
        Map<String, String> params = new HashMap<>();
        for (int i=1; i<args.length; i++) {
            if (args[i].startsWith("--") && i+1 < args.length) {
                params.put(args[i].substring(2), args[++i]);
            }
        }
        String cmd = args[0];
        switch (cmd) {
            case "list":
                app.list(params.get("continent"));
                break;
            case "map":
                app.drawMap();
                break;
            case "quiz":
                app.quiz(params.get("continent"), params.containsKey("reverse"));
                break;
            case "stats":
                app.showStats();
                break;
            default:
                System.out.println("Unknown command. Use list, map, quiz, stats.");
        }
    }
}
