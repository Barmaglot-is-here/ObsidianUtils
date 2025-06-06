using static Utils.Command;

string tag = GetString("Enter tag");

bool repeat = true;

while (repeat)
{
    string directory = GetString("Enter dirrectory");

    foreach (string file in Directory.EnumerateFiles(directory))
    {
        string text = File.ReadAllText(file);

        text = $"#{tag}\n\n" + text;

        File.WriteAllText(file, text);
    }

    Console.WriteLine("Done");

    while (true)
    {
        var answer = GetKey("Repeat? [Y/N]");

        Console.WriteLine();

        if (answer.Key == ConsoleKey.N)
        {
            repeat = false;

            break;
        }
        else if (answer.Key != ConsoleKey.Y)
            Console.WriteLine("Wrong key\n");
        else
            break;
    }
}