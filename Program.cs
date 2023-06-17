using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Text;
using System.Runtime.InteropServices;

public class GraphFilter
{
    [DllImport("libgraph.dylib", CallingConvention = CallingConvention.Cdecl)]
    private static extern void FindShortestPath(string start, string end, StringBuilder path, out int distance);

    public static void FilterGraph()
    {
        // Display all routes from data.txt
        Console.WriteLine("Here are all the routes:");

        var regex = new Regex(@"^""(.+?)""\s*""(.+?)""\s*(\d+)$", RegexOptions.Compiled);

        using (var reader = new StreamReader("data.txt"))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                var match = regex.Match(line);
                if (match.Success)
                {
                    string city1 = match.Groups[1].Value.Trim();
                    string city2 = match.Groups[2].Value.Trim();
                    int parsedDistance;

                    if (int.TryParse(match.Groups[3].Value, out parsedDistance))
                    {
                        Console.WriteLine($"{city1} --({parsedDistance})--> {city2}");
                    }
                    else
                    {
                        Console.WriteLine("Invalid distance format in file.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid line format in file.");
                }
            }
        }

        // Prompt user for blocked routes
        var blockedEdges = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        Console.WriteLine("Enter blocked routes in the format \"A\" \"B\", one per line. Enter -1 to finish:");

        string inputLine;
        while ((inputLine = Console.ReadLine()) != "-1")
        {
            var formattedKey = FormatKey(inputLine);
            blockedEdges.Add(formattedKey);
        }

        // Write filtered data to temp file
        string tempFilePath = "temp.txt";

        using (var writer = new StreamWriter(tempFilePath, false)) // Overwrite old contents
        {
            using (var reader = new StreamReader("data.txt"))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var match = regex.Match(line);
                    if (match.Success)
                    {
                        string city1 = match.Groups[1].Value.Trim();
                        string city2 = match.Groups[2].Value.Trim();
                        int parsedDistance;

                        if (int.TryParse(match.Groups[3].Value, out parsedDistance))
                        {
                            // Format the key as "|City1||City2|"
                            string key = FormatKey($"\"{city1}\" \"{city2}\"");
                            // Write the line to temp file only if the key is not in the blocked set
                            if (!blockedEdges.Contains(key))
                            {
                                writer.WriteLine($"\"{city1}\" \"{city2}\" {parsedDistance}");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid distance format in file.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid line format in file.");
                    }
                }
            }
        }

        // Prompt user for source and destination
        Console.WriteLine("Enter the source city:");
        string source = Console.ReadLine().Trim('"');

        Console.WriteLine("Enter the destination city:");
        string destination = Console.ReadLine().Trim('"');

        // Find the shortest path using the C++ library
        var path = new StringBuilder(1024);
        int shortestDistance;
        FindShortestPath(source, destination, path, out shortestDistance);

        Console.WriteLine($"Shortest path from {source} to {destination}:");
        if (shortestDistance == -1 || shortestDistance == 0)
        {
            Console.WriteLine("Route is not possible");
        }
        else
        {
            Console.WriteLine(path.ToString());
            Console.WriteLine($"Distance: {shortestDistance}");
        }
    }

    private static string FormatKey(string input)
    {
        var result = new StringBuilder();
        foreach (var c in input)
        {
            if (c == '"')
            {
                result.Append('|');
            }
            else if (char.IsLetterOrDigit(c))
            {
                result.Append(c);
            }
        }
        return result.ToString();
    }
}

class Program
{
    static void Main(string[] args)
    {
        GraphFilter.FilterGraph();

        File.WriteAllText("temp.txt", string.Empty);
    }
}
