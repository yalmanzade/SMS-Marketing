static class Error
{
    private static string path = "Logs/";
    private static string filename = "log.txt";
    public static DateTime timeStamp;
    public static string system = "";
    public static string errorCode = "";
    public static string message = "";
    public static void InitializeError(string currentSystem, string currentCode, string currentMessage)
    {
        timeStamp = DateTime.Now;
        system = currentSystem;
        message = currentMessage;
        errorCode = currentCode;
    }
    public static void LogError()
    {
        try
        {
            using (StreamWriter reader = new StreamWriter(path + filename, true))
            {
                reader.WriteLine(timeStamp + " - " + errorCode + " - " + message);
                Console.WriteLine("Error: {0}", message);
            }
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine("Could not find log.txt");
        }
        catch (DirectoryNotFoundException)
        {
            Console.WriteLine("Could not find directory: {0} ", path + filename);
        }
        catch (IOException)
        {
            Console.WriteLine("Could not write to file.");
        }
    }

}