class Program
{

    static List<Log> logs = new List<Log>();
    static void Main(string[] args)
    {
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
