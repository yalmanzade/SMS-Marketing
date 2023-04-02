static class Error
{
    private static string Path = "Logs/";
    private static string Filename = "log.txt";
    public static DateTime TimeStamp;
    public static string CurrentSystem = "";
    public static string ErrorCode = "";
    public static string message = "";

    public static void InitializeError(string currentSystem, string currentCode, string currentMessage)
    {
        TimeStamp = DateTime.Now;
        CurrentSystem = currentSystem;
        message = currentMessage;
        ErrorCode = currentCode;
    }

    public static void LogError()
    {
        try
        {
            using (StreamWriter reader = new StreamWriter(Path + Filename, true))
            {
                //reader.WriteLine(TimeStamp + " - " + ErrorCode + " - " + message);
                reader.WriteLine($"{TimeStamp} - Error Code: {ErrorCode} - System: {CurrentSystem} - Message: {message}");
                Console.WriteLine("Error: {0}", message);
            }
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine("Could not find log.txt");
        }
        catch (DirectoryNotFoundException)
        {
            Console.WriteLine("Could not find directory: {0} ", Path + Filename);
        }
        catch (IOException)
        {
            Console.WriteLine("Could not write to file.");
        }
    }

}