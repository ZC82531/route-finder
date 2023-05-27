# Graph Shortest Path Finder

## Overview

This C++ program allows you to input routes between roads or cities with their respective distance. It uses Dijkstra’s algorithm to find the shortest path between two places. However, the program can also handle blocked routes by considering cities as inaccessible.

This is useful for applications such as truck route optimization, where certain infrastructure might be temporarily unavailable. This is also useful for airlines to display flight routes when there are weather shutdowns.

## Features

- **Find Shortest Path:** Computes the shortest path between a given source and a destination with specific edges and weight.
- **Handle Blocked Access Points:** Accounts for cities/road point that are blocked and cannot be used as part of the path.

## Prerequisites

- **g++ Compiler:** Ensure you have `g++` installed on your system.

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
2. Compile the project using g++:
```bash
g++ -std=c++11 -o graph_project Graph.cpp main.cpp
```
## Execution
1. Run the compiled program:
```bash
./graph_project
```
2. The program will prompt you to enter start city, end city, then blocked cities and will then calculate and display the shortest path, taking into account the blocked routes.

## Sample Output
Example output of the program:
```
Enter the start city: New York
Enter the end city: Los Angeles
Enter blocked nodes (type 'done' when finished):
done
Shortest path from New York to Los Angeles is:
New York --(800)--> Chicago --(1500)--> Houston --(1200)--> Phoenix --(500)--> Los Angeles
Total distance: 4000
```

## Sample Input Files
Enter the number of cities for first number (5), second number is number of paths (7).
```
5 7
"New York" "Los Angeles" 4500
"New York" "Chicago" 800
"Chicago" "Los Angeles" 4000
"Chicago" "Houston" 1500
"Los Angeles" "Houston" 2300
"Houston" "Phoenix" 1200
"Phoenix" "Los Angeles" 500
```


