class Program
{
    static void Main(string[] args)
    {
        DateOnly today = DateOnly.FromDateTime(DateTime.Now);
        List<Event> events = new List<Event>();
        var stats = new Dictionary<DateOnly, int>();

        while (true)
        {
            Console.WriteLine("Zadejte příkaz: EVENT;název;datum (YYYY-MM-DD), LIST, STATS nebo END");
            string? input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("Prázdný vstup. Zkuste to znovu.");
                continue;
            }

            if (input == "END")
                return;
            else if (input == "LIST")
            {
                if (events.Count == 0)
                {
                    Console.WriteLine("Žádné události nejsou uloženy.");
                    continue;
                }
                Console.WriteLine("Seznam událostí:");
                foreach (Event ev in events)
                {
                    Console.WriteLine(ev.Print(today));
                }
            }
            else if (input == "STATS")
            {
                if (events.Count == 0)
                {
                    Console.WriteLine("Žádné události nejsou uloženy.");
                    continue;
                }
                foreach (var pair in stats)
                {
                    Console.WriteLine($"Date: {pair.Key.ToString("yyyy-MM-dd")}: events: {pair.Value}");
                }
            }
            else if (input.StartsWith("EVENT;"))
            {
                string[] parts = input.Split(';'); // nit: pomocna metoda na parsovani
                if (parts.Length != 3)
                {
                    Console.WriteLine("Špatný formát příkazu EVENT. Správný formát: EVENT;název;datum (YYYY-MM-DD)");
                    continue;
                }
                string name = parts[1].Trim();
                string dateStr = parts[2].Trim();
                DateOnly date;
                if (!DateOnly.TryParse(dateStr, out date))
                {
                    Console.WriteLine("Špatný formát data. Správný formát: YYYY-MM-DD");
                    continue;
                }
                AddEvent(new Event(name, date), stats, events);
                Console.WriteLine($"Událost '{name}' s datem {date.ToString("yyyy-MM-dd")} byla přidána.");
            }
            else
            {
                Console.WriteLine("Neplatný vstup. Zkuste to znovu.");
            }
        }
    }

    static void AddEvent(Event ev, Dictionary<DateOnly, int> stats, List<Event> events)
    {
        events.Add(ev);

        if (stats.ContainsKey(ev.Date))
            stats[ev.Date]++;
        else
            stats[ev.Date] = 1;
    }
}

public class Event // Separatni soubor
{
    public string Name { get; set; }
    public DateOnly Date { get; set; }

    public Event(string name, DateOnly date)
    {
        Name = name;
        Date = date;
    }

    public string Print(DateOnly today)
    {
        int days = Date.DayNumber - today.DayNumber;
        if (days > 0)
        {
            return $"Event {Name} with date {Date.ToString("yyyy-MM-dd")} will happen in {days} days";
        }
        else if (days < 0)
        {
            return $"Event {Name} with date {Date.ToString("yyyy-MM-dd")} happened {Math.Abs(days)} days ago";
        }
        else
        {
            return $"Event {Name} with date {Date.ToString("yyyy-MM-dd")} is today!";
        }
    }
}
