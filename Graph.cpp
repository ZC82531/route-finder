#include "Graph.h"

void Graph::addEdge(const string& city1, const string& city2, int distance) {
    adjList[city1][city2] = distance;
}

map<string, int> Graph::getNeighbors(const string& city) const {
    auto it = adjList.find(city);
    if (it != adjList.end()) {
        return it->second;
    }
    return {}; // Return empty map if city is not found
}
