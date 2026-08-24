🗺️ Capitals Quiz (Map) — Multi‑Language Interactive Geography Trainer
8 languages, one immersive learning experience – explore world capitals on an ASCII map, quiz yourself, and master geography – right from your terminal.

✨ Features
🌍 Interactive ASCII map – visual continent map with country placements

📋 List all countries – see countries with their capitals and continents

🎯 Quiz modes – country→capital or capital→country

💡 Hints – reveal first letter, number of letters, or continent

📊 Score tracking – persistent statistics saved locally

🌐 Continent filter – focus on one continent at a time

⏱️ Timed mode – challenge yourself with a timer (optional)

🧰 Supported Languages & Files
Language	File	Dependencies
Python	capitals_map.py	none (stdlib)
Go	capitals_map.go	none (stdlib)
JavaScript (Node)	capitals_map.js	commander (optional)
Ruby	capitals_map.rb	json, date
PHP	capitals_map.php	none (extensions)
Java	CapitalsMap.java	Java 8+
C#	CapitalsMap.cs	.NET Core 3.1+
C++	capitals_map.cpp	nlohmann/json
🚀 Quick Start
All implementations follow the same CLI pattern:

bash
# Show all countries with capitals
<command> list

# Show ASCII map
<command> map

# Start a quiz (default: country→capital, all continents)
<command> quiz

# Quiz with specific continent
<command> quiz --continent Europe

# Quiz in reverse mode (capital→country)
<command> quiz --reverse

# Show statistics
<command> stats
Commands:

list [--continent CONT] – list countries and capitals

map [--continent CONT] – show ASCII map

quiz [--continent CONT] [--reverse] – interactive quiz

stats – show statistics

📸 Example Output
text
🗺️ Capitals Quiz – World Map

    EUROPE            ASIA
   ┌────────┐      ┌──────────┐
   │  FR*   │      │   *JP    │
   │  *DE   │      │  CN*     │
   └────────┘      └──────────┘
    SOUTH AMERICA     AFRICA
   ┌──────────┐    ┌──────────┐
   │  BR*     │    │   *EG    │
   │  *AR     │    │  ZA*     │
   └──────────┘    └──────────┘

Quiz Mode: Country → Capital
Round 1/10
Country: France
Your answer (or 'hint', 'skip', 'quit'): Paris
✅ Correct!
Score: 1/1 (100.0%)
📁 Repository Structure
text
.
├── README.md
├── python/
│   └── capitals_map.py
├── go/
│   └── capitals_map.go
├── javascript/
│   └── capitals_map.js
├── ruby/
│   └── capitals_map.rb
├── php/
│   └── capitals_map.php
├── java/
│   └── CapitalsMap.java
├── csharp/
│   └── CapitalsMap.cs
└── cpp/
│   └── capitals_map.cpp
