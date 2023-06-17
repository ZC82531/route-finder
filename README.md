# Graph Shortest Path Finder

## Overview

This C++ program allows you to input routes between roads or cities with their respective distance. The program is designed to handle restricted roads at certain times.

This is useful for applications such as truck route optimization, where certain infrastructure might be temporarily unavailable. This is also useful for airlines to display flight routes when there are weather shutdowns.

## Features

- **Find Shortest Path:** Computes the shortest path between a given source and a destination with specific edges and weight.
- **Handle Blocked Access Points:** Accounts for cities/road point that are blocked and cannot be used as part of the path.

## Prerequisites
- **.NET SDK:** https://dotnet.microsoft.com/en-us/download 

- **g++ Compiler:** This is for MacOS and Linux Operating Systems
- **MinGW Compiler:** This is applicable for Windows operating systems

## Getting Started

### Cloning the Repository

To clone the repository, use the following command:

```bash
git clone <repository>
```

## Compilation
1. Navigate into the cloned repository directory:
```bash
cd route-finder
```
2. Compile the shared library using g++:
#### For MacOS:
```bash
g++ -shared -std=c++11 -o libgraph.dylib dijkstra.cpp
```
#### For Windows:
```bash
g++ -shared -std=c++11 -o libgraph.dll dijkstra.cpp
```
#### For Linux:
```bash
g++ -shared -fPIC -std=c++11 -o libgraph.so dijkstra.cpp
```
##### For Windows & Linux, also replace the DllImport in Program.cs from "libgraph.dylib" to "libgraph.so"/"libgraph.dll".
## Execution
1. Bulid the program:
```bash
dotnet build
```

2. Run the program and wait for prompts
```bash
dotnet run
```
3. The program will prompt you to enter blocked paths between two points in quotes. Example:
"New York" "Los Angeles"
4. Each entry should be two cities in quotes with a single space and enter, then enter -1 when finished
5. The program will then prompt you to enter the starting and ending city, you can put these without quotes (this is case sensitive).

## Sample Output
Example output of the program:
```
Here are all the routes:
New York --(800)--> Chicago
Chicago --(4000)--> Los Angeles
Chicago --(1500)--> Houston
Los Angeles --(2300)--> Houston
Houston --(1200)--> Phoenix
Phoenix --(333)--> Los Angeles
New York --(4500)--> Los Angeles
Enter blocked routes in the format "A" "B", one per line. Enter -1 to finish:
"New York" "Los Angeles"
"Phoenix" "Los Angeles"
-1
Enter the source city:
New York
Enter the destination city:
Los Angeles
Shortest path from New York to Los Angeles:
New York--(800)-->Chicago--(4000)-->Los Angeles
Distance: 4800
```

## Sample Input Files
```
"New York" "Los Angeles" 4500
"New York" "Chicago" 800
"Chicago" "Los Angeles" 4000
"Chicago" "Houston" 1500
"Los Angeles" "Houston" 2300
"Houston" "Phoenix" 1200
"Phoenix" "Los Angeles" 500
```


