using System;
using System.Collections;

class Program
{

    static void Main(string[] args)
    {
        // File path input
        Console.WriteLine("Please enter the path to the file you want to parse:" +
                 "\n" + "(Press Enter to use the default sample_log.txt file)");
        string logFilePath = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(logFilePath))
        {
            logFilePath = "examples/sample_log.txt";
        }

        // Check if the file exists
        if (!File.Exists(logFilePath))
        {
            Console.WriteLine($"Error: The file '{logFilePath}' does not exist.");
            return;
        }
        List<Log> logs = new List<Log>();

        logs = ParseLogs(logFilePath);

        Console.WriteLine("Finished parsing the log file.");

        string choice = PrintTerminal();

        // Main loop
        while (choice != "4")
        {
            if (choice == "1")
            {
                DisplayAllLogs(logs);
            }

            if (choice == "2")
            {
                DisplayEventOccurrences(logs);
            }

            if (choice == "3")
            {
                Console.WriteLine("TODO");
            }
            choice = PrintTerminal();
        }

        Console.WriteLine("Exiting the program. Goodbye!");
        return;
    }

    static List<Log> ParseLogs(string logFilePath)
    {
        List<Log> logs = new List<Log>();
        string[] lines = File.ReadAllLines(logFilePath);

        foreach (string line in lines)
        {
            int openBracketIndex = line.IndexOf('[');
            int closeBracketIndex = line.IndexOf(']');

            string timestampStr = line.Substring(openBracketIndex + 1, closeBracketIndex - openBracketIndex - 1);
            DateTime timestamp = DateTime.Parse(timestampStr);

            string next = line.Substring(closeBracketIndex + 2);
            int spaceIndex = next.IndexOf(' ');

            // Event type and message
            string eventType = next.Substring(0, spaceIndex);
            string message = next.Substring(spaceIndex + 1);

            logs.Add(new Log(timestamp, eventType, message));
        }
        return logs;
    }

    static string PrintTerminal()
    {
        while (true)
        {
            Console.WriteLine("Please select an option:");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("1 - See all logs");
            Console.WriteLine("2 - See all event occurrences");
            Console.WriteLine("3 - See most frequent messages");
            Console.WriteLine("4 - Quit");
            Console.ResetColor();
            Console.Write("Enter your choice: ");


            string choice = Console.ReadLine().Trim();

            if (choice == "1" || choice == "2" || choice == "3" || choice == "4")
            {
                return choice;
            }
            else
            {
                Console.WriteLine("Invalid input.\n");
            }
        }
    }

    static void DisplayAllLogs(List<Log> logs)
    {
        // Header
        Console.WriteLine("{0,-22} {1,-15} {2}", "Timestamp", "Event Type", "Message");
        Console.WriteLine(new string('-', 60));

        foreach (var log in logs)
        {
            switch (log.Event_Type)
            {
                case "ERROR":
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case "WARNING":
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
            }

            // Display log entry
            Console.WriteLine("{0,-22} {1,-15} {2}",
                log.Timestamp.ToString("yyyy-MM-dd HH:mm:ss"),
                log.Event_Type,
                log.Message);

            // Reset color
            Console.ResetColor();
        }
        Console.WriteLine("Press Any Key to Continue...");
        Console.ReadKey();
    }

    static void DisplayEventOccurrences(List<Log> logs)
    {
        Dictionary<string, List<Log>> eventTypeLogs = new Dictionary<string, List<Log>>();

        foreach (var log in logs)
        {
            var eventType = log.Event_Type;
            if (!eventTypeLogs.ContainsKey(eventType))
            {
                eventTypeLogs[eventType] = new List<Log>();
            }
            eventTypeLogs[eventType].Add(log);
        }

        // Header
        Console.WriteLine("{0,-8} {1,-15} {2}", "Number", "Event Type", "Occurrences");
        Console.WriteLine(new string('-', 30));

        foreach (var i in eventTypeLogs)
        {
            Console.WriteLine("{0,-8} {1,-15} {2}",
                eventTypeLogs.Keys.ToList().IndexOf(i.Key) + 1,
                i.Key,
                i.Value.Count);
        }

        // Index of event types
        List<string> eventTypes = eventTypeLogs.Keys.ToList();

        // See al errors of that type
        Console.WriteLine("To see all errors of that type, select that number from the menu.");
        Console.Write("Select your choice or select any other key to return to the main menu: ");
        string eventChoice = Console.ReadLine().Trim();

        int eventChoiceNumber;
        if (Int32.TryParse(eventChoice, out eventChoiceNumber))
        {
            eventChoiceNumber -= 1;
            if ((eventTypes.Count > eventChoiceNumber) && (eventChoiceNumber >= 0))
            {
                DisplayAllLogs(eventTypeLogs[eventTypes[eventChoiceNumber]]);
            }
        }
    }

}

class Log
{
    public DateTime Timestamp { get; }
    public string Event_Type { get;}
    public string Message { get; }

    public Log(DateTime timestamp, string eventType, string message)
    {
        Timestamp = timestamp;
        Event_Type = eventType;
        Message = message;
    }
}
