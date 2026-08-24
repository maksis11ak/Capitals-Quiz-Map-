// capitals_map.cpp
#include <iostream>
#include <fstream>
#include <string>
#include <vector>
#include <map>
#include <algorithm>
#include <random>
#include <cctype>
#include <nlohmann/json.hpp>
#include <getopt.h>

using namespace std;
using json = nlohmann::json;

struct Country {
    string name, capital, continent;
    int x, y;
};

vector<Country> COUNTRIES = {
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
    {"New Zealand", "Wellington", "Oceania", 10, 10}
};

vector<string> CONTINENTS = {"Europe", "Asia", "Africa", "North America", "South America", "Oceania"};
const string DATA_FILE = "capitals_stats.json";

struct Stats {
    int correct = 0;
    int incorrect = 0;
    int total = 0;
};

Stats loadStats() {
    Stats s;
    ifstream f(DATA_FILE);
    if (!f.is_open()) return s;
    json j;
    f >> j;
    s.correct = j["correct"];
    s.incorrect = j["incorrect"];
    s.total = j["total"];
    return s;
}

void saveStats(const Stats& s) {
    json j = {{"correct", s.correct}, {"incorrect", s.incorrect}, {"total", s.total}};
    ofstream f(DATA_FILE);
    f << setw(2) << j << endl;
}

vector<Country> getCountries(const string& continent) {
    if (continent.empty()) return COUNTRIES;
    vector<Country> result;
    for (auto& c : COUNTRIES) {
        if (c.continent == continent) result.push_back(c);
    }
    return result;
}

void listCountries(const string& continent) {
    auto countries = getCountries(continent);
    cout << "\n🌍 Countries (" << countries.size() << ")\n";
    if (!continent.empty()) cout << "Continent: " << continent << "\n\n";
    else cout << "All continents\n\n";
    map<string, vector<Country>> grouped;
    for (auto& c : countries) grouped[c.continent].push_back(c);
    for (auto& cont : CONTINENTS) {
        if (grouped.count(cont)) {
            cout << "  " << cont << ":\n";
            sort(grouped[cont].begin(), grouped[cont].end(),
                 [](const Country& a, const Country& b) { return a.name < b.name; });
            for (auto& c : grouped[cont]) {
                cout << "    " << c.name << " – " << c.capital << "\n";
            }
        }
    }
}

void drawMap() {
    cout << "\n🗺️ World Map (countries marked with *)\n";
    cout << "  Europe          Asia\n";
    cout << " ┌────────────┐  ┌────────────┐\n";
    cout << " │  *  *  *    │  │  *  *  *   │\n";
    cout << " │  *  *  *    │  │  *  *  *   │\n";
    cout << " └────────────┘  └────────────┘\n";
    cout << "  Africa          North America\n";
    cout << " ┌────────────┐  ┌────────────┐\n";
    cout << " │  *  *  *    │  │  *  *  *   │\n";
    cout << " │  *  *  *    │  │  *  *  *   │\n";
    cout << " └────────────┘  └────────────┘\n";
    cout << "  South America   Oceania\n";
    cout << " ┌────────────┐  ┌────────────┐\n";
    cout << " │  *  *  *    │  │  *  *      │\n";
    cout << " │  *  *  *    │  │  *  *      │\n";
    cout << " └────────────┘  └────────────┘\n";
    cout << "\nLegend: * = country present\n";
}

string getHint(const Country& c, const string& capital, int level) {
    switch (level) {
        case 0: return "First letter: '" + string(1, capital[0]) + "'";
        case 1: return "Number of letters: " + to_string(capital.size());
        case 2: return "Continent: " + c.continent;
        default: return "No more hints.";
    }
}

void quiz(const string& continent, bool reverse) {
    auto countries = getCountries(continent);
    if (countries.empty()) {
        cout << "No countries found.\n";
        return;
    }
    random_device rd;
    mt19937 g(rd());
    shuffle(countries.begin(), countries.end(), g);
    if (countries.size() > 10) countries.resize(10);
    int total = countries.size();
    int correct = 0;
    Stats stats = loadStats();

    cout << "\n🧠 Capitals Quiz\n";
    string mode = reverse ? "Capital → Country" : "Country → Capital";
    cout << "Mode: " << mode << "\n";
    if (!continent.empty()) cout << "Continent: " << continent << "\n";
    cout << "Questions: " << total << "\n\n";

    for (size_t i = 0; i < countries.size(); i++) {
        auto& c = countries[i];
        string question = reverse ? "Capital: " + c.capital : "Country: " + c.name;
        string answer = reverse ? c.name : c.capital;
        int hintLevel = 0;
        cout << "Round " << i+1 << "/" << total << "\n";
        cout << question << "\n";
        while (true) {
            cout << "Your answer (or 'hint', 'skip', 'quit'): ";
            string input;
            getline(cin, input);
            string cmd = input;
            transform(cmd.begin(), cmd.end(), cmd.begin(), ::tolower);
            if (cmd == "quit") {
                cout << "Quitting...\n";
                stats.total += correct + (int)(i - correct);
                saveStats(stats);
                return;
            } else if (cmd == "skip") {
                cout << "Skipped. The answer is: " << answer << "\n";
                break;
            } else if (cmd == "hint") {
                cout << "💡 Hint: " << getHint(c, c.capital, hintLevel) << "\n";
                hintLevel = (hintLevel + 1) % 3;
                continue;
            } else {
                string ans = input;
                transform(ans.begin(), ans.end(), ans.begin(), ::tolower);
                string ansLower = answer;
                transform(ansLower.begin(), ansLower.end(), ansLower.begin(), ::tolower);
                if (ans == ansLower) {
                    cout << "✅ Correct!\n";
                    correct++;
                    break;
                } else {
                    cout << "❌ Wrong. Try again (or type 'skip')\n";
                }
            }
        }
        cout << "Score: " << correct << "/" << i+1 << " (" << (correct/(double)(i+1)*100) << "%)\n\n";
    }

    stats.correct += correct;
    stats.incorrect += total - correct;
    stats.total += total;
    saveStats(stats);
    cout << "Quiz finished! Score: " << correct << "/" << total << " (" << (correct/(double)total*100) << "%)\n";
}

void showStats() {
    Stats stats = loadStats();
    cout << "\n📊 Statistics\n";
    cout << "Correct: " << stats.correct << "\n";
    cout << "Incorrect: " << stats.incorrect << "\n";
    cout << "Total: " << stats.total << "\n";
    if (stats.total > 0) {
        double pct = (double)stats.correct / stats.total * 100;
        cout << "Accuracy: " << pct << "%\n";
    }
}

int main(int argc, char* argv[]) {
    if (argc < 2) {
        cerr << "Usage: capitals_map <command> [options]\n";
        return 1;
    }
    string cmd = argv[1];
    string continent;
    bool reverse = false;

    for (int i=2; i<argc; i++) {
        string arg = argv[i];
        if (arg == "--continent" && i+1 < argc) continent = argv[++i];
        if (arg == "--reverse") reverse = true;
    }

    if (cmd == "list") {
        listCountries(continent);
    } else if (cmd == "map") {
        drawMap();
    } else if (cmd == "quiz") {
        quiz(continent, reverse);
    } else if (cmd == "stats") {
        showStats();
    } else {
        cerr << "Unknown command. Use list, map, quiz, stats.\n";
        return 1;
    }
    return 0;
}
