# capitals_map.py
import sys
import os
import json
import random
import argparse
from typing import List, Dict, Optional, Tuple

DATA_FILE = "capitals_stats.json"

# Country data: (name, capital, continent, x, y) where x,y are map positions
COUNTRIES = [
    # Europe
    ("France", "Paris", "Europe", 2, 3),
    ("Germany", "Berlin", "Europe", 4, 4),
    ("Italy", "Rome", "Europe", 3, 5),
    ("Spain", "Madrid", "Europe", 1, 4),
    ("United Kingdom", "London", "Europe", 1, 3),
    ("Portugal", "Lisbon", "Europe", 0, 4),
    ("Netherlands", "Amsterdam", "Europe", 3, 3),
    ("Belgium", "Brussels", "Europe", 2, 3),
    ("Switzerland", "Bern", "Europe", 3, 4),
    ("Austria", "Vienna", "Europe", 4, 4),
    ("Greece", "Athens", "Europe", 5, 5),
    ("Turkey", "Ankara", "Europe", 6, 4),
    ("Poland", "Warsaw", "Europe", 5, 3),
    ("Sweden", "Stockholm", "Europe", 4, 1),
    ("Norway", "Oslo", "Europe", 3, 0),
    ("Denmark", "Copenhagen", "Europe", 4, 2),
    ("Finland", "Helsinki", "Europe", 5, 0),
    ("Ireland", "Dublin", "Europe", 0, 2),
    # Asia
    ("China", "Beijing", "Asia", 8, 3),
    ("Japan", "Tokyo", "Asia", 10, 2),
    ("India", "New Delhi", "Asia", 7, 5),
    ("South Korea", "Seoul", "Asia", 9, 2),
    ("Russia", "Moscow", "Asia", 6, 2),
    ("Saudi Arabia", "Riyadh", "Asia", 7, 6),
    ("Iran", "Tehran", "Asia", 6, 5),
    ("Pakistan", "Islamabad", "Asia", 7, 5),
    ("Indonesia", "Jakarta", "Asia", 9, 7),
    ("Vietnam", "Hanoi", "Asia", 8, 5),
    ("Thailand", "Bangkok", "Asia", 8, 6),
    ("Malaysia", "Kuala Lumpur", "Asia", 8, 7),
    ("Philippines", "Manila", "Asia", 9, 5),
    ("Israel", "Jerusalem", "Asia", 6, 4),
    # Africa
    ("Egypt", "Cairo", "Africa", 4, 6),
    ("South Africa", "Pretoria", "Africa", 5, 9),
    ("Nigeria", "Abuja", "Africa", 3, 7),
    ("Kenya", "Nairobi", "Africa", 4, 8),
    ("Morocco", "Rabat", "Africa", 1, 6),
    ("Ghana", "Accra", "Africa", 2, 7),
    ("Ethiopia", "Addis Ababa", "Africa", 4, 7),
    ("Tanzania", "Dodoma", "Africa", 4, 8),
    ("Algeria", "Algiers", "Africa", 2, 6),
    ("Sudan", "Khartoum", "Africa", 4, 7),
    # North America
    ("USA", "Washington", "North America", 2, 1),
    ("Canada", "Ottawa", "North America", 1, 0),
    ("Mexico", "Mexico City", "North America", 2, 3),
    ("Cuba", "Havana", "North America", 3, 3),
    ("Jamaica", "Kingston", "North America", 3, 4),
    ("Panama", "Panama City", "North America", 3, 5),
    # South America
    ("Brazil", "Brasilia", "South America", 4, 7),
    ("Argentina", "Buenos Aires", "South America", 4, 9),
    ("Chile", "Santiago", "South America", 3, 9),
    ("Peru", "Lima", "South America", 3, 8),
    ("Colombia", "Bogota", "South America", 3, 6),
    ("Venezuela", "Caracas", "South America", 3, 5),
    ("Ecuador", "Quito", "South America", 3, 7),
    # Australia/Oceania
    ("Australia", "Canberra", "Oceania", 9, 9),
    ("New Zealand", "Wellington", "Oceania", 10, 10),
]

CONTINENTS = ["Europe", "Asia", "Africa", "North America", "South America", "Oceania"]

class CapitalsGame:
    def __init__(self):
        self.countries = COUNTRIES
        self.stats = {"correct": 0, "incorrect": 0, "total": 0}
        self.load_stats()

    def load_stats(self):
        if os.path.exists(DATA_FILE):
            with open(DATA_FILE, "r") as f:
                self.stats = json.load(f)

    def save_stats(self):
        with open(DATA_FILE, "w") as f:
            json.dump(self.stats, f)

    def get_countries(self, continent: Optional[str] = None) -> List[Tuple]:
        if continent:
            return [c for c in self.countries if c[2] == continent]
        return self.countries

    def list_countries(self, continent: Optional[str] = None):
        countries = self.get_countries(continent)
        print(f"\n🌍 Countries ({len(countries)})")
        if continent:
            print(f"Continent: {continent}\n")
        else:
            print("All continents\n")
        # Group by continent
        grouped = {}
        for c in countries:
            cont = c[2]
            if cont not in grouped:
                grouped[cont] = []
            grouped[cont].append(c)
        for cont in sorted(grouped.keys()):
            print(f"  {cont}:")
            for name, capital, _, _, _ in sorted(grouped[cont]):
                print(f"    {name} – {capital}")

    def draw_map(self, continent: Optional[str] = None):
        """Draw an ASCII map of the world with country markers."""
        print("\n🗺️ World Map (countries marked with *)")
        print("  Europe          Asia")
        print(" ┌────────────┐  ┌────────────┐")
        print(" │  *  *  *    │  │  *  *  *   │")
        print(" │  *  *  *    │  │  *  *  *   │")
        print(" └────────────┘  └────────────┘")
        print("  Africa          North America")
        print(" ┌────────────┐  ┌────────────┐")
        print(" │  *  *  *    │  │  *  *  *   │")
        print(" │  *  *  *    │  │  *  *  *   │")
        print(" └────────────┘  └────────────┘")
        print("  South America   Oceania")
        print(" ┌────────────┐  ┌────────────┐")
        print(" │  *  *  *    │  │  *  *      │")
        print(" │  *  *  *    │  │  *  *      │")
        print(" └────────────┘  └────────────┘")
        print("\nLegend: * = country present")

    def get_hint(self, country, capital, level=0):
        """Get a hint about the capital."""
        if level == 0:
            return f"First letter: '{capital[0]}'"
        elif level == 1:
            return f"Number of letters: {len(capital)}"
        elif level == 2:
            return f"Continent: {country[2]}"
        return "No more hints."

    def quiz(self, continent: Optional[str] = None, reverse: bool = False):
        """Run an interactive quiz."""
        countries = self.get_countries(continent)
        if not countries:
            print("No countries found for this continent.")
            return
        # Shuffle and select up to 10
        random.shuffle(countries)
        selected = countries[:10]
        correct = 0
        total = len(selected)

        print(f"\n🧠 Capitals Quiz")
        mode = "Capital → Country" if reverse else "Country → Capital"
        print(f"Mode: {mode}")
        if continent:
            print(f"Continent: {continent}")
        print(f"Questions: {total}\n")

        for i, country_data in enumerate(selected, 1):
            name, capital, cont, x, y = country_data
            if reverse:
                question = f"Capital: {capital}"
                answer = name
            else:
                question = f"Country: {name}"
                answer = capital

            hint_level = 0
            print(f"Round {i}/{total}")
            print(question)
            while True:
                user_input = input("Your answer (or 'hint', 'skip', 'quit'): ").strip()
                if user_input.lower() == "quit":
                    print("Quitting...")
                    self.stats["total"] += correct + (i - 1 - correct)
                    self.save_stats()
                    return
                if user_input.lower() == "skip":
                    print(f"Skipped. The answer is: {answer}")
                    break
                if user_input.lower() == "hint":
                    print("💡 Hint:", self.get_hint(country_data, capital, hint_level))
                    hint_level = (hint_level + 1) % 3
                    continue
                if user_input.lower() == answer.lower():
                    print("✅ Correct!")
                    correct += 1
                    break
                else:
                    print("❌ Wrong. Try again (or type 'skip')")

            print(f"Score: {correct}/{i} ({correct/i*100:.1f}%)\n")

        self.stats["correct"] += correct
        self.stats["incorrect"] += total - correct
        self.stats["total"] += total
        self.save_stats()
        print(f"Quiz finished! Score: {correct}/{total} ({correct/total*100:.1f}%)")

    def stats(self):
        print("\n📊 Statistics")
        print(f"Correct: {self.stats['correct']}")
        print(f"Incorrect: {self.stats['incorrect']}")
        print(f"Total: {self.stats['total']}")
        if self.stats['total'] > 0:
            pct = self.stats['correct'] / self.stats['total'] * 100
            print(f"Accuracy: {pct:.1f}%")

def main():
    parser = argparse.ArgumentParser(description="Capitals Quiz - World Map")
    subparsers = parser.add_subparsers(dest="cmd", required=True)

    list_parser = subparsers.add_parser("list")
    list_parser.add_argument("--continent", choices=CONTINENTS, help="Filter by continent")

    map_parser = subparsers.add_parser("map")
    map_parser.add_argument("--continent", choices=CONTINENTS, help="Filter by continent")

    quiz_parser = subparsers.add_parser("quiz")
    quiz_parser.add_argument("--continent", choices=CONTINENTS, help="Filter by continent")
    quiz_parser.add_argument("--reverse", action="store_true", help="Capital → Country mode")

    subparsers.add_parser("stats")

    args = parser.parse_args()
    game = CapitalsGame()

    if args.cmd == "list":
        game.list_countries(args.continent)
    elif args.cmd == "map":
        game.draw_map(args.continent)
    elif args.cmd == "quiz":
        game.quiz(args.continent, args.reverse)
    elif args.cmd == "stats":
        game.stats()

if __name__ == "__main__":
    main()
