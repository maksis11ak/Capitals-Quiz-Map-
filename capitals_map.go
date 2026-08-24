// capitals_map.go
package main

import (
	"bufio"
	"encoding/json"
	"flag"
	"fmt"
	"math/rand"
	"os"
	"strings"
	"time"
)

type Country struct {
	Name      string `json:"name"`
	Capital   string `json:"capital"`
	Continent string `json:"continent"`
	X         int    `json:"x"`
	Y         int    `json:"y"`
}

type Stats struct {
	Correct   int `json:"correct"`
	Incorrect int `json:"incorrect"`
	Total     int `json:"total"`
}

var countries = []Country{
	{"France", "Paris", "Europe", 2, 3},
	{"Germany", "Berlin", "Europe", 4, 4},
	{"Italy", "Rome", "Europe", 3, 5},
	{"Spain", "Madrid", "Europe", 1, 4},
	{"United Kingdom", "London", "Europe", 1, 3},
	{"Portugal", "Lisbon", "Europe", 0, 4},
	{"Netherlands", "Amsterdam", "Europe", 3, 3},
	{"Belgium", "Brussels", "Europe", 2, 3},
	{"Switzerland", "Bern", "Europe", 3, 4},
	{"Austria", "Vienna", "Europe", 4, 4},
	{"Greece", "Athens", "Europe", 5, 5},
	{"Turkey", "Ankara", "Europe", 6, 4},
	{"Poland", "Warsaw", "Europe", 5, 3},
	{"Sweden", "Stockholm", "Europe", 4, 1},
	{"Norway", "Oslo", "Europe", 3, 0},
	{"Denmark", "Copenhagen", "Europe", 4, 2},
	{"Finland", "Helsinki", "Europe", 5, 0},
	{"Ireland", "Dublin", "Europe", 0, 2},
	{"China", "Beijing", "Asia", 8, 3},
	{"Japan", "Tokyo", "Asia", 10, 2},
	{"India", "New Delhi", "Asia", 7, 5},
	{"South Korea", "Seoul", "Asia", 9, 2},
	{"Russia", "Moscow", "Asia", 6, 2},
	{"Saudi Arabia", "Riyadh", "Asia", 7, 6},
	{"Iran", "Tehran", "Asia", 6, 5},
	{"Pakistan", "Islamabad", "Asia", 7, 5},
	{"Indonesia", "Jakarta", "Asia", 9, 7},
	{"Vietnam", "Hanoi", "Asia", 8, 5},
	{"Thailand", "Bangkok", "Asia", 8, 6},
	{"Malaysia", "Kuala Lumpur", "Asia", 8, 7},
	{"Philippines", "Manila", "Asia", 9, 5},
	{"Israel", "Jerusalem", "Asia", 6, 4},
	{"Egypt", "Cairo", "Africa", 4, 6},
	{"South Africa", "Pretoria", "Africa", 5, 9},
	{"Nigeria", "Abuja", "Africa", 3, 7},
	{"Kenya", "Nairobi", "Africa", 4, 8},
	{"Morocco", "Rabat", "Africa", 1, 6},
	{"Ghana", "Accra", "Africa", 2, 7},
	{"Ethiopia", "Addis Ababa", "Africa", 4, 7},
	{"Tanzania", "Dodoma", "Africa", 4, 8},
	{"Algeria", "Algiers", "Africa", 2, 6},
	{"Sudan", "Khartoum", "Africa", 4, 7},
	{"USA", "Washington", "North America", 2, 1},
	{"Canada", "Ottawa", "North America", 1, 0},
	{"Mexico", "Mexico City", "North America", 2, 3},
	{"Cuba", "Havana", "North America", 3, 3},
	{"Jamaica", "Kingston", "North America", 3, 4},
	{"Panama", "Panama City", "North America", 3, 5},
	{"Brazil", "Brasilia", "South America", 4, 7},
	{"Argentina", "Buenos Aires", "South America", 4, 9},
	{"Chile", "Santiago", "South America", 3, 9},
	{"Peru", "Lima", "South America", 3, 8},
	{"Colombia", "Bogota", "South America", 3, 6},
	{"Venezuela", "Caracas", "South America", 3, 5},
	{"Ecuador", "Quito", "South America", 3, 7},
	{"Australia", "Canberra", "Oceania", 9, 9},
	{"New Zealand", "Wellington", "Oceania", 10, 10},
}

var continents = []string{"Europe", "Asia", "Africa", "North America", "South America", "Oceania"}

var statsFile = "capitals_stats.json"

func loadStats() Stats {
	var stats Stats
	data, err := os.ReadFile(statsFile)
	if err != nil {
		return stats
	}
	json.Unmarshal(data, &stats)
	return stats
}

func saveStats(stats Stats) {
	data, _ := json.MarshalIndent(stats, "", "  ")
	os.WriteFile(statsFile, data, 0644)
}

func filterByContinent(continent string) []Country {
	if continent == "" {
		return countries
	}
	var filtered []Country
	for _, c := range countries {
		if c.Continent == continent {
			filtered = append(filtered, c)
		}
	}
	return filtered
}

func listCountries(continent string) {
	selected := filterByContinent(continent)
	fmt.Printf("\n🌍 Countries (%d)\n", len(selected))
	if continent != "" {
		fmt.Printf("Continent: %s\n\n", continent)
	} else {
		fmt.Println("All continents\n")
	}
	grouped := make(map[string][]Country)
	for _, c := range selected {
		grouped[c.Continent] = append(grouped[c.Continent], c)
	}
	for _, cont := range continents {
		if list, ok := grouped[cont]; ok {
			fmt.Printf("  %s:\n", cont)
			for _, c := range list {
				fmt.Printf("    %s – %s\n", c.Name, c.Capital)
			}
		}
	}
}

func drawMap(continent string) {
	fmt.Println("\n🗺️ World Map (countries marked with *)")
	fmt.Println("  Europe          Asia")
	fmt.Println(" ┌────────────┐  ┌────────────┐")
	fmt.Println(" │  *  *  *    │  │  *  *  *   │")
	fmt.Println(" │  *  *  *    │  │  *  *  *   │")
	fmt.Println(" └────────────┘  └────────────┘")
	fmt.Println("  Africa          North America")
	fmt.Println(" ┌────────────┐  ┌────────────┐")
	fmt.Println(" │  *  *  *    │  │  *  *  *   │")
	fmt.Println(" │  *  *  *    │  │  *  *  *   │")
	fmt.Println(" └────────────┘  └────────────┘")
	fmt.Println("  South America   Oceania")
	fmt.Println(" ┌────────────┐  ┌────────────┐")
	fmt.Println(" │  *  *  *    │  │  *  *      │")
	fmt.Println(" │  *  *  *    │  │  *  *      │")
	fmt.Println(" └────────────┘  └────────────┘")
	fmt.Println("\nLegend: * = country present")
}

func getHint(c Country, capital string, level int) string {
	switch level {
	case 0:
		return "First letter: '" + string(capital[0]) + "'"
	case 1:
		return "Number of letters: " + fmt.Sprintf("%d", len(capital))
	case 2:
		return "Continent: " + c.Continent
	default:
		return "No more hints."
	}
}

func quiz(continent string, reverse bool) {
	selected := filterByContinent(continent)
	if len(selected) == 0 {
		fmt.Println("No countries found for this continent.")
		return
	}
	rand.Shuffle(len(selected), func(i, j int) { selected[i], selected[j] = selected[j], selected[i] })
	if len(selected) > 10 {
		selected = selected[:10]
	}
	total := len(selected)
	correct := 0
	stats := loadStats()

	fmt.Printf("\n🧠 Capitals Quiz\n")
	mode := "Capital → Country"
	if !reverse {
		mode = "Country → Capital"
	}
	fmt.Printf("Mode: %s\n", mode)
	if continent != "" {
		fmt.Printf("Continent: %s\n", continent)
	}
	fmt.Printf("Questions: %d\n\n", total)

	reader := bufio.NewReader(os.Stdin)

	for i, c := range selected {
		var question, answer string
		if reverse {
			question = "Capital: " + c.Capital
			answer = c.Name
		} else {
			question = "Country: " + c.Name
			answer = c.Capital
		}
		hintLevel := 0
		fmt.Printf("Round %d/%d\n", i+1, total)
		fmt.Println(question)
		for {
			fmt.Print("Your answer (or 'hint', 'skip', 'quit'): ")
			input, _ := reader.ReadString('\n')
			input = strings.TrimSpace(input)
			switch strings.ToLower(input) {
			case "quit":
				fmt.Println("Quitting...")
				stats.Total += correct + (i - correct)
				saveStats(stats)
				return
			case "skip":
				fmt.Printf("Skipped. The answer is: %s\n", answer)
				break
			case "hint":
				fmt.Printf("💡 Hint: %s\n", getHint(c, c.Capital, hintLevel))
				hintLevel = (hintLevel + 1) % 3
				continue
			default:
				if strings.EqualFold(input, answer) {
					fmt.Println("✅ Correct!")
					correct++
					break
				} else {
					fmt.Println("❌ Wrong. Try again (or type 'skip')")
					continue
				}
			}
			break
		}
		fmt.Printf("Score: %d/%d (%.1f%%)\n\n", correct, i+1, float64(correct)/float64(i+1)*100)
	}

	stats.Correct += correct
	stats.Incorrect += total - correct
	stats.Total += total
	saveStats(stats)
	fmt.Printf("Quiz finished! Score: %d/%d (%.1f%%)\n", correct, total, float64(correct)/float64(total)*100)
}

func showStats() {
	stats := loadStats()
	fmt.Println("\n📊 Statistics")
	fmt.Printf("Correct: %d\n", stats.Correct)
	fmt.Printf("Incorrect: %d\n", stats.Incorrect)
	fmt.Printf("Total: %d\n", stats.Total)
	if stats.Total > 0 {
		pct := float64(stats.Correct) / float64(stats.Total) * 100
		fmt.Printf("Accuracy: %.1f%%\n", pct)
	}
}

func main() {
	if len(os.Args) < 2 {
		fmt.Println("Usage: capitals_map <command> [options]")
		return
	}
	cmd := os.Args[1]
	rand.Seed(time.Now().UnixNano())

	switch cmd {
	case "list":
		listCmd := flag.NewFlagSet("list", flag.ExitOnError)
		continent := listCmd.String("continent", "", "Filter by continent")
		listCmd.Parse(os.Args[2:])
		listCountries(*continent)

	case "map":
		mapCmd := flag.NewFlagSet("map", flag.ExitOnError)
		continent := mapCmd.String("continent", "", "Filter by continent")
		mapCmd.Parse(os.Args[2:])
		drawMap(*continent)

	case "quiz":
		quizCmd := flag.NewFlagSet("quiz", flag.ExitOnError)
		continent := quizCmd.String("continent", "", "Filter by continent")
		reverse := quizCmd.Bool("reverse", false, "Capital → Country mode")
		quizCmd.Parse(os.Args[2:])
		quiz(*continent, *reverse)

	case "stats":
		showStats()

	default:
		fmt.Println("Unknown command. Use list, map, quiz, stats.")
	}
}
