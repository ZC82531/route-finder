#ifndef GRAPH_H
#define GRAPH_H

#include <string>
#include <map>
#include <vector>

using namespace std;

class Graph {
public:
    void addEdge(const string& city1, const string& city2, int distance);
    map<string, int> getNeighbors(const string& city) const;

private:
    map<string, map<string, int>> adjList;  // Adjacency list to store the graph
};

#endif
