# capitals_map.rb
#!/usr/bin/env ruby
require 'json'
require 'optparse'
require 'date'

DATA_FILE = 'capitals_stats.json'

COUNTRIES = [
  # Europe
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
  # Asia
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
  # Africa
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
  # North America
  ["USA", "Washington", "North America", 2, 1],
  ["Canada", "Ottawa", "North America", 1, 0],
  ["Mexico", "Mexico City", "North America", 2, 3],
  ["Cuba", "Havana", "North America", 3, 3],
  ["Jamaica", "Kingston", "North America", 3, 4],
  ["Panama", "Panama City", "North America", 3, 5],
  # South America
  ["Brazil", "Brasilia", "South America", 4, 7],
  ["Argentina", "Buenos Aires", "South America", 4, 9],
  ["Chile", "Santiago", "South America", 3, 9],
  ["Peru", "Lima", "South America", 3, 8],
  ["Colombia", "Bogota", "South America", 3, 6],
  ["Venezuela", "Caracas", "South America", 3, 5],
  ["Ecuador", "Quito", "South America", 3, 7],
  # Oceania
  ["Australia", "Canberra", "Oceania", 9, 9],
  ["New Zealand", "Wellington", "Oceania", 10, 10],
]

CONTINENTS = ["Europe", "Asia", "Africa", "North America", "South America", "Oceania"]

class CapitalsGame
  attr_reader :stats

  def initialize
    @stats = { "correct" => 0, "incorrect" => 0, "total" => 0 }
    load_stats
  end

  def load_stats
    if File.exist?(DATA_FILE)
      @stats = JSON.parse(File.read(DATA_FILE)) rescue { "correct" => 0, "incorrect" => 0, "total" => 0 }
    end
  end

  def save_stats
    File.write(DATA_FILE, JSON.pretty_generate(@stats))
  end

  def get_countries(continent = nil)
    return COUNTRIES unless continent
    COUNTRIES.select { |c| c[2] == continent }
  end

  def list(continent = nil)
    countries = get_countries(continent)
    puts "\n🌍 Countries (#{countries.size})"
    if continent
      puts "Continent: #{continent}\n"
    else
      puts "All continents\n"
    end
    grouped = {}
    countries.each do |c|
      cont = c[2]
      grouped[cont] ||= []
      grouped[cont] << c
    end
    CONTINENTS.each do |cont|
      if grouped[cont]
        puts "  #{cont}:"
        grouped[cont].sort_by { |c| c[0] }.each do |c|
          puts "    #{c[0]} – #{c[1]}"
        end
      end
    end
  end

  def draw_map
    puts "\n🗺️ World Map (countries marked with *)"
    puts "  Europe          Asia"
    puts " ┌────────────┐  ┌────────────┐"
    puts " │  *  *  *    │  │  *  *  *   │"
    puts " │  *  *  *    │  │  *  *  *   │"
    puts " └────────────┘  └────────────┘"
    puts "  Africa          North America"
    puts " ┌────────────┐  ┌────────────┐"
    puts " │  *  *  *    │  │  *  *  *   │"
    puts " │  *  *  *    │  │  *  *  *   │"
    puts " └────────────┘  └────────────┘"
    puts "  South America   Oceania"
    puts " ┌────────────┐  ┌────────────┐"
    puts " │  *  *  *    │  │  *  *      │"
    puts " │  *  *  *    │  │  *  *      │"
    puts " └────────────┘  └────────────┘"
    puts "\nLegend: * = country present"
  end

  def get_hint(country, capital, level)
    case level
    when 0 then "First letter: '#{capital[0]}'"
    when 1 then "Number of letters: #{capital.length}"
    when 2 then "Continent: #{country[2]}"
    else "No more hints."
    end
  end

  def quiz(continent = nil, reverse = false)
    countries = get_countries(continent)
    if countries.empty?
      puts "No countries found."
      return
    end
    countries = countries.shuffle
    selected = countries.first(10)
    total = selected.size
    correct = 0

    puts "\n🧠 Capitals Quiz"
    mode = reverse ? "Capital → Country" : "Country → Capital"
    puts "Mode: #{mode}"
    puts "Continent: #{continent}" if continent
    puts "Questions: #{total}\n"

    selected.each_with_index do |c, i|
      name, capital, cont = c
      question, answer = reverse ? ["Capital: #{capital}", name] : ["Country: #{name}", capital]
      hint_level = 0
      puts "Round #{i+1}/#{total}"
      puts question
      loop do
        print "Your answer (or 'hint', 'skip', 'quit'): "
        input = gets.chomp
        case input.downcase
        when "quit"
          puts "Quitting..."
          @stats["total"] += correct + (i - correct)
          save_stats
          return
        when "skip"
          puts "Skipped. The answer is: #{answer}"
          break
        when "hint"
          puts "💡 Hint: #{get_hint(c, capital, hint_level)}"
          hint_level = (hint_level + 1) % 3
          next
        else
          if input.downcase == answer.downcase
            puts "✅ Correct!"
            correct += 1
            break
          else
            puts "❌ Wrong. Try again (or type 'skip')"
          end
        end
      end
      puts "Score: #{correct}/#{i+1} (#{(correct.to_f/(i+1)*100).round(1)}%)\n"
    end

    @stats["correct"] += correct
    @stats["incorrect"] += total - correct
    @stats["total"] += total
    save_stats
    puts "Quiz finished! Score: #{correct}/#{total} (#{(correct.to_f/total*100).round(1)}%)"
  end

  def stats
    puts "\n📊 Statistics"
    puts "Correct: #{@stats['correct']}"
    puts "Incorrect: #{@stats['incorrect']}"
    puts "Total: #{@stats['total']}"
    if @stats['total'] > 0
      pct = @stats['correct'].to_f / @stats['total'] * 100
      puts "Accuracy: #{pct.round(1)}%"
    end
  end
end

options = {}
OptionParser.new do |opts|
  opts.banner = "Usage: capitals_map.rb <command> [options]"
  opts.on("list", "List countries") { options[:cmd] = :list }
  opts.on("map", "Draw map") { options[:cmd] = :map }
  opts.on("quiz", "Start quiz") { options[:cmd] = :quiz }
  opts.on("stats", "Show statistics") { options[:cmd] = :stats }
  opts.on("--continent CONT", "Filter by continent") { |v| options[:continent] = v }
  opts.on("--reverse", "Capital → Country mode") { options[:reverse] = true }
end.parse!

game = CapitalsGame.new
cmd = options[:cmd] || :list

case cmd
when :list
  game.list(options[:continent])
when :map
  game.draw_map
when :quiz
  game.quiz(options[:continent], options[:reverse])
when :stats
  game.stats
else
  puts "Unknown command. Use list, map, quiz, stats."
end
