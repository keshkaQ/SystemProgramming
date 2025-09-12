class Program
{
    static void Main(string[] args)
    {
        string fileName = args[0];
        string targetWord = args[1];
        int counter = 0;
        try
        {
            using (StreamReader sr = new StreamReader(fileName))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    string[] words = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    foreach (string word in words)
                    {
                        if (word.Equals(targetWord, StringComparison.OrdinalIgnoreCase))
                            counter++;
                    }
                }
            }
        }
        catch (Exception)
        {
            Console.WriteLine("0");
            return;
        }
        Console.WriteLine(counter);
    }
}