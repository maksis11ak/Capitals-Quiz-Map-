// capitals_map.js
#!/usr/bin/env node
const fs = require('fs');
const readline = require('readline');
const { program } = require('commander');

const DATA_FILE = 'capitals_stats.json';
const CONTINENTS = ['Europe', 'Asia', 'Africa', 'North America', 'South America', 'Oceania'];

const COUNTRIES = [
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

class CapitalsGame {
    constructor() {
        this.stats = { correct: 0, incorrect: 0, total: 0 };
        this.loadStats();
        this.rl = readline.createInterface({
            input: process.stdin,
            output: process.stdout
        });
    }

    loadStats() {
        if (fs.existsSync(DATA_FILE)) {
            try {
                this.stats = JSON.parse(fs.readFileSync(DATA_FILE));
            } catch (e) {}
        }
    }

    saveStats() {
        fs.writeFileSync(DATA_FILE, JSON.stringify(this.stats, null, 2));
    }

    getCountries(continent) {
        return continent ?
            COUNTRIES.filter(c => c[2] === continent) :
            COUNTRIES;
    }

    list(continent) {
        const countries = this.getCountries(continent);
        console.log(`\n🌍 Countries (${countries.length})`);
        if (continent) console.log(`Continent: ${continent}\n`);
        else console.log('All continents\n');
        const grouped = {};
        for (const c of countries) {
            const cont = c[2];
            if (!grouped[cont]) grouped[cont] = [];
            grouped[cont].push(c);
        }
        for (const cont of CONTINENTS) {
            if (grouped[cont]) {
                console.log(`  ${cont}:`);
                for (const c of grouped[cont].sort((a,b) => a[0].localeCompare(b[0]))) {
                    console.log(`    ${c[0]} – ${c[1]}`);
                }
            }
        }
    }

    drawMap() {
        console.log('\n🗺️ World Map (countries marked with *)');
        console.log('  Europe          Asia');
        console.log(' ┌────────────┐  ┌────────────┐');
        console.log(' │  *  *  *    │  │  *  *  *   │');
        console.log(' │  *  *  *    │  │  *  *  *   │');
        console.log(' └────────────┘  └────────────┘');
        console.log('  Africa          North America');
        console.log(' ┌────────────┐  ┌────────────┐');
        console.log(' │  *  *  *    │  │  *  *  *   │');
        console.log(' │  *  *  *    │  │  *  *  *   │');
        console.log(' └────────────┘  └────────────┘');
        console.log('  South America   Oceania');
        console.log(' ┌────────────┐  ┌────────────┐');
        console.log(' │  *  *  *    │  │  *  *      │');
        console.log(' │  *  *  *    │  │  *  *      │');
        console.log(' └────────────┘  └────────────┘');
        console.log('\nLegend: * = country present');
    }

    getHint(country, capital, level) {
        switch (level) {
            case 0: return `First letter: '${capital[0]}'`;
            case 1: return `Number of letters: ${capital.length}`;
            case 2: return `Continent: ${country[2]}`;
            default: return 'No more hints.';
        }
    }

    question(query) {
        return new Promise(resolve => {
            this.rl.question(query, answer => resolve(answer));
        });
    }

    async quiz(continent, reverse) {
        let countries = this.getCountries(continent);
        if (countries.length === 0) {
            console.log('No countries found.');
            return;
        }
        // Shuffle
        for (let i = countries.length - 1; i > 0; i--) {
            const j = Math.floor(Math.random() * (i + 1));
            [countries[i], countries[j]] = [countries[j], countries[i]];
        }
        const selected = countries.slice(0, 10);
        const total = selected.length;
        let correct = 0;

        console.log(`\n🧠 Capitals Quiz`);
        const mode = reverse ? 'Capital → Country' : 'Country → Capital';
        console.log(`Mode: ${mode}`);
        if (continent) console.log(`Continent: ${continent}`);
        console.log(`Questions: ${total}\n`);

        for (let i = 0; i < selected.length; i++) {
            const c = selected[i];
            const [name, capital, cont] = c;
            let question, answer;
            if (reverse) {
                question = `Capital: ${capital}`;
                answer = name;
            } else {
                question = `Country: ${name}`;
                answer = capital;
            }
            let hintLevel = 0;
            console.log(`Round ${i+1}/${total}`);
            console.log(question);
            while (true) {
                const input = await this.question('Your answer (or "hint", "skip", "quit"): ');
                const cmd = input.trim().toLowerCase();
                if (cmd === 'quit') {
                    console.log('Quitting...');
                    this.stats.total += correct + (i - correct);
                    this.saveStats();
                    this.rl.close();
                    return;
                }
                if (cmd === 'skip') {
                    console.log(`Skipped. The answer is: ${answer}`);
                    break;
                }
                if (cmd === 'hint') {
                    console.log(`💡 Hint: ${this.getHint(c, capital, hintLevel)}`);
                    hintLevel = (hintLevel + 1) % 3;
                    continue;
                }
                if (cmd === answer.toLowerCase()) {
                    console.log('✅ Correct!');
                    correct++;
                    break;
                } else {
                    console.log('❌ Wrong. Try again (or type "skip")');
                }
            }
            console.log(`Score: ${correct}/${i+1} (${(correct/(i+1)*100).toFixed(1)}%)\n`);
        }

        this.stats.correct += correct;
        this.stats.incorrect += total - correct;
        this.stats.total += total;
        this.saveStats();
        console.log(`Quiz finished! Score: ${correct}/${total} (${(correct/total*100).toFixed(1)}%)`);
        this.rl.close();
    }

    stats() {
        console.log('\n📊 Statistics');
        console.log(`Correct: ${this.stats.correct}`);
        console.log(`Incorrect: ${this.stats.incorrect}`);
        console.log(`Total: ${this.stats.total}`);
        if (this.stats.total > 0) {
            const pct = this.stats.correct / this.stats.total * 100;
            console.log(`Accuracy: ${pct.toFixed(1)}%`);
        }
    }
}

program
    .command('list')
    .option('--continent <continent>', 'Filter by continent')
    .action((options) => {
        const game = new CapitalsGame();
        game.list(options.continent);
    });

program
    .command('map')
    .option('--continent <continent>', 'Filter by continent')
    .action(() => {
        const game = new CapitalsGame();
        game.drawMap();
    });

program
    .command('quiz')
    .option('--continent <continent>', 'Filter by continent')
    .option('--reverse', 'Capital → Country mode')
    .action(async (options) => {
        const game = new CapitalsGame();
        await game.quiz(options.continent, options.reverse);
    });

program
    .command('stats')
    .action(() => {
        const game = new CapitalsGame();
        game.stats();
    });

program.parse(process.argv);
