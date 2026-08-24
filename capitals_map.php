# capitals_map.php
#!/usr/bin/env php
<?php

define('DATA_FILE', 'capitals_stats.json');

$COUNTRIES = [
    // Europe
    ["France", "Paris", "Europe", 2, 3],
    ["Germany", "Berlin", "Europe", 4, 4],
    ["Italy", "Rome", "Europe", 3, 5],
    ["Spain", "Madrid", "Europe", 1, 4],
    ["United Kingdom", "London", "Europe", 1, 3],
    ["Portugal", "Lisbon", "Europe", 0, 4],
    ["Netherlands", "Amsterdam", "Europe", 3, 3],
    ["Belgium", "Brussels", "Europe", 2, 3],
    ["Switzerland", "Bern", "Europe", 3, 4],
    ["Austria", "Vienna", "Europe", 4, 4],
    ["Greece", "Athens", "Europe", 5, 5],
    ["Turkey", "Ankara", "Europe", 6, 4],
    ["Poland", "Warsaw", "Europe", 5, 3],
    ["Sweden", "Stockholm", "Europe", 4, 1],
    ["Norway", "Oslo", "Europe", 3, 0],
    ["Denmark", "Copenhagen", "Europe", 4, 2],
    ["Finland", "Helsinki", "Europe", 5, 0],
    ["Ireland", "Dublin", "Europe", 0, 2],
    // Asia
    ["China", "Beijing", "Asia", 8, 3],
    ["Japan", "Tokyo", "Asia", 10, 2],
    ["India", "New Delhi", "Asia", 7, 5],
    ["South Korea", "Seoul", "Asia", 9, 2],
    ["Russia", "Moscow", "Asia", 6, 2],
    ["Saudi Arabia", "Riyadh", "Asia", 7, 6],
    ["Iran", "Tehran", "Asia", 6, 5],
    ["Pakistan", "Islamabad", "Asia", 7, 5],
    ["Indonesia", "Jakarta", "Asia", 9, 7],
    ["Vietnam", "Hanoi", "Asia", 8, 5],
    ["Thailand", "Bangkok", "Asia", 8, 6],
    ["Malaysia", "Kuala Lumpur", "Asia", 8, 7],
    ["Philippines", "Manila", "Asia", 9, 5],
    ["Israel", "Jerusalem", "Asia", 6, 4],
    // Africa
    ["Egypt", "Cairo", "Africa", 4, 6],
    ["South Africa", "Pretoria", "Africa", 5, 9],
    ["Nigeria", "Abuja", "Africa", 3, 7],
    ["Kenya", "Nairobi", "Africa", 4, 8],
    ["Morocco", "Rabat", "Africa", 1, 6],
    ["Ghana", "Accra", "Africa", 2, 7],
    ["Ethiopia", "Addis Ababa", "Africa", 4, 7],
    ["Tanzania", "Dodoma", "Africa", 4, 8],
    ["Algeria", "Algiers", "Africa", 2, 6],
    ["Sudan", "Khartoum", "Africa", 4, 7],
    // North America
    ["USA", "Washington", "North America", 2, 1],
    ["Canada", "Ottawa", "North America", 1, 0],
    ["Mexico", "Mexico City", "North America", 2, 3],
    ["Cuba", "Havana", "North America", 3, 3],
    ["Jamaica", "Kingston", "North America", 3, 4],
    ["Panama", "Panama City", "North America", 3, 5],
    // South America
    ["Brazil", "Brasilia", "South America", 4, 7],
    ["Argentina", "Buenos Aires", "South America", 4, 9],
    ["Chile", "Santiago", "South America", 3, 9],
    ["Peru", "Lima", "South America", 3, 8],
    ["Colombia", "Bogota", "South America", 3, 6],
    ["Venezuela", "Caracas", "South America", 3, 5],
    ["Ecuador", "Quito", "South America", 3, 7],
    // Oceania
    ["Australia", "Canberra", "Oceania", 9, 9],
    ["New Zealand", "Wellington", "Oceania", 10, 10],
];

$CONTINENTS = ["Europe", "Asia", "Africa", "North America", "South America", "Oceania"];

function loadStats() {
    if (file_exists(DATA_FILE)) {
        return json_decode(file_get_contents(DATA_FILE), true) ?: ["correct" => 0, "incorrect" => 0, "total" => 0];
    }
    return ["correct" => 0, "incorrect" => 0, "total" => 0];
}

function saveStats($stats) {
    file_put_contents(DATA_FILE, json_encode($stats, JSON_PRETTY_PRINT));
}

function getCountries($continent = null) {
    global $COUNTRIES;
    if ($continent === null) return $COUNTRIES;
    return array_filter($COUNTRIES, function($c) use ($continent) {
        return $c[2] == $continent;
    });
}

function listCountries($continent = null) {
    $countries = getCountries($continent);
    echo "\n🌍 Countries (" . count($countries) . ")\n";
    if ($continent) echo "Continent: $continent\n\n";
    else echo "All continents\n\n";
    $grouped = [];
    foreach ($countries as $c) {
        $cont = $c[2];
        if (!isset($grouped[$cont])) $grouped[$cont] = [];
        $grouped[$cont][] = $c;
    }
    global $CONTINENTS;
    foreach ($CONTINENTS as $cont) {
        if (isset($grouped[$cont])) {
            echo "  $cont:\n";
            usort($grouped[$cont], function($a, $b) { return strcmp($a[0], $b[0]); });
            foreach ($grouped[$cont] as $c) {
                echo "    {$c[0]} – {$c[1]}\n";
            }
        }
    }
}

function drawMap() {
    echo "\n🗺️ World Map (countries marked with *)\n";
    echo "  Europe          Asia\n";
    echo " ┌────────────┐  ┌────────────┐\n";
    echo " │  *  *  *    │  │  *  *  *   │\n";
    echo " │  *  *  *    │  │  *  *  *   │\n";
    echo " └────────────┘  └────────────┘\n";
    echo "  Africa          North America\n";
    echo " ┌────────────┐  ┌────────────┐\n";
    echo " │  *  *  *    │  │  *  *  *   │\n";
    echo " │  *  *  *    │  │  *  *  *   │\n";
    echo " └────────────┘  └────────────┘\n";
    echo "  South America   Oceania\n";
    echo " ┌────────────┐  ┌────────────┐\n";
    echo " │  *  *  *    │  │  *  *      │\n";
    echo " │  *  *  *    │  │  *  *      │\n";
    echo " └────────────┘  └────────────┘\n";
    echo "\nLegend: * = country present\n";
}

function getHint($country, $capital, $level) {
    switch ($level) {
        case 0: return "First letter: '{$capital[0]}'";
        case 1: return "Number of letters: " . strlen($capital);
        case 2: return "Continent: {$country[2]}";
        default: return "No more hints.";
    }
}

function quiz($continent = null, $reverse = false) {
    $countries = getCountries($continent);
    if (empty($countries)) {
        echo "No countries found.\n";
        return;
    }
    shuffle($countries);
    $selected = array_slice($countries, 0, 10);
    $total = count($selected);
    $correct = 0;
    $stats = loadStats();

    echo "\n🧠 Capitals Quiz\n";
    $mode = $reverse ? "Capital → Country" : "Country → Capital";
    echo "Mode: $mode\n";
    if ($continent) echo "Continent: $continent\n";
    echo "Questions: $total\n\n";

    foreach ($selected as $i => $c) {
        list($name, $capital, $cont) = $c;
        $question = $reverse ? "Capital: $capital" : "Country: $name";
        $answer = $reverse ? $name : $capital;
        $hintLevel = 0;
        echo "Round " . ($i+1) . "/$total\n";
        echo "$question\n";
        while (true) {
            echo "Your answer (or 'hint', 'skip', 'quit'): ";
            $input = trim(fgets(STDIN));
            switch (strtolower($input)) {
                case "quit":
                    echo "Quitting...\n";
                    $stats["total"] += $correct + ($i - $correct);
                    saveStats($stats);
                    return;
                case "skip":
                    echo "Skipped. The answer is: $answer\n";
                    break 2;
                case "hint":
                    echo "💡 Hint: " . getHint($c, $capital, $hintLevel) . "\n";
                    $hintLevel = ($hintLevel + 1) % 3;
                    break;
                default:
                    if (strtolower($input) == strtolower($answer)) {
                        echo "✅ Correct!\n";
                        $correct++;
                        break 2;
                    } else {
                        echo "❌ Wrong. Try again (or type 'skip')\n";
                    }
            }
        }
        echo "Score: $correct/" . ($i+1) . " (" . ($correct/($i+1)*100) . "%)\n\n";
    }

    $stats["correct"] += $correct;
    $stats["incorrect"] += $total - $correct;
    $stats["total"] += $total;
    saveStats($stats);
    echo "Quiz finished! Score: $correct/$total (" . ($correct/$total*100) . "%)\n";
}

function showStats() {
    $stats = loadStats();
    echo "\n📊 Statistics\n";
    echo "Correct: {$stats['correct']}\n";
    echo "Incorrect: {$stats['incorrect']}\n";
    echo "Total: {$stats['total']}\n";
    if ($stats['total'] > 0) {
        $pct = $stats['correct'] / $stats['total'] * 100;
        echo "Accuracy: " . round($pct, 1) . "%\n";
    }
}

if ($argc < 2) {
    die("Usage: php capitals_map.php <command> [options]\n");
}
$cmd = $argv[1];
$continent = null;
$reverse = false;

for ($i=2; $i<$argc; $i++) {
    if ($argv[$i] == '--continent' && isset($argv[$i+1])) {
        $continent = $argv[++$i];
    }
    if ($argv[$i] == '--reverse') $reverse = true;
}

switch ($cmd) {
    case 'list':
        listCountries($continent);
        break;
    case 'map':
        drawMap();
        break;
    case 'quiz':
        quiz($continent, $reverse);
        break;
    case 'stats':
        showStats();
        break;
    default:
        echo "Unknown command. Use list, map, quiz, stats.\n";
}
?>
