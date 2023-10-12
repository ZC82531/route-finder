using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Text;
using System.Runtime.InteropServices;

public class GraphFilter
{
    // Import the FindShortestPath function from the C++ library.
    [DllImport("libgraph.dylib", CallingConvention = CallingConvention.Cdecl)]
    private static extern void FindShortestPath(string start, string end, StringBuilder path, out int distance);

    public static void FilterGraph()
    {
        // Display all routes from data.txt
        Console.WriteLine("Here are all the routes:");

        // Regex pattern to match the expected route format in the input file.
        var regex = new Regex(@"^""(.+?)""\s*""(.+?)""\s*(\d+)$", RegexOptions.Compiled);

        // Read routes from the data.txt file.
        using (var reader = new StreamReader("data.txt"))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                var match = regex.Match(line); // Check if the line matches the route pattern.
                if (match.Success)
                {
                    // Extract city names and distance from the regex match.
                    string city1 = match.Groups[1].Value.Trim();
                    string city2 = match.Groups[2].Value.Trim();
                    int parsedDistance;

                    // Try to parse the distance; display it if successful.
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

        // Prompt user to input blocked routes.
        var blockedEdges = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        Console.WriteLine("Enter blocked routes in the format \"A\" \"B\", one per line. Enter -1 to finish:");

        string inputLine;
        while ((inputLine = Console.ReadLine()) != "-1")
        {
            var formattedKey = FormatKey(inputLine); // Format input into a consistent key.
            blockedEdges.Add(formattedKey); // Add the blocked route to the set.
        }

        // Write the filtered routes to a temporary file.
        string tempFilePath = "temp.txt";

        using (var writer = new StreamWriter(tempFilePath, false)) // Overwrite any existing contents.
        {
            using (var reader = new StreamReader("data.txt"))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var match = regex.Match(line); // Check each line for the route pattern.
                    if (match.Success)
                    {
                        // Extract city names and distance from the match.
                        string city1 = match.Groups[1].Value.Trim();
                        string city2 = match.Groups[2].Value.Trim();
                        int parsedDistance;

                        // Try to parse the distance; write to temp file if valid.
                        if (int.TryParse(match.Groups[3].Value, out parsedDistance))
                        {
                            // Format the key for comparison.
                            string key = FormatKey($"\"{city1}\" \"{city2}\"");
                            // Write the route to temp file if it's not blocked.
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

        // Prompt the user for source and destination cities.
        Console.WriteLine("Enter the source city:");
        string source = Console.ReadLine().Trim('"');

        Console.WriteLine("Enter the destination city:");
        string destination = Console.ReadLine().Trim('"');

        // Find the shortest path using the imported C++ function.
        var path = new StringBuilder(1024);
        int shortestDistance;
        FindShortestPath(source, destination, path, out shortestDistance);

        // Display the shortest path and distance.
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

    // Format the input into a key for comparison.
    private static string FormatKey(string input)
    {
        var result = new StringBuilder();
        foreach (var c in input)
        {
            if (c == '"')
            {
                result.Append('|'); // Replace quotes with pipe symbols.
            }
            else if (char.IsLetterOrDigit(c))
            {
                result.Append(c); // Append letters and digits.
            }
        }
        return result.ToString();
    }
}

class Program
{
    static void Main(string[] args)
    {
        // Run the graph filtering process.
        GraphFilter.FilterGraph();

        // Clear the contents of the temp file after use.
        File.WriteAllText("temp.txt", string.Empty);
    }
}
