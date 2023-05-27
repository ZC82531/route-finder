#include <iostream>
#include <string>
#include <set>
#include <map>
#include <vector>
#include <queue>
#include <utility>
#include <limits>
#include <sstream>
#include <fstream>
#include <regex>
#include "Graph.h"

using namespace std;

// Helper function to trim quotes from strings
string trimQuotes(const string& str) {
    if (str.length() >= 2 && str.front() == '"' && str.back() == '"') {
        return str.substr(1, str.length() - 2);
    }
    return str;
}

void readGraphFromFile(Graph& graph, const string& filename) {
    ifstream file(filename);
    if (!file.is_open()) {
        cerr << "Error opening file: " << filename << endl;
        return;
    }

    int numCities, numEdges;
    file >> numCities >> numEdges;
    file.ignore();  // Ignore newline after numbers

    string line;
    while (getline(file, line)) {
        istringstream iss(line);
        string city1, city2;
        int distance;

        // Use standard string literal for regex
        regex regexPattern("\"([^\"]*)\"\\s+\"([^\"]*)\"\\s+(\\d+)");
        smatch match;
        if (regex_search(line, match, regexPattern)) {
            city1 = trimQuotes(match[1].str());
            city2 = trimQuotes(match[2].str());
            distance = stoi(match[3].str());

            graph.addEdge(city1, city2, distance);
        }
    }
}

pair<vector<string>, int> dijkstra(const Graph& graph, const string& start, const string& end, set<string>& blockedNodes) {
    map<string, int> dist;
    map<string, string> prev;
    map<string, int> edgeWeights;  // To store weights of edges in the path
    priority_queue<pair<int, string>, vector<pair<int, string>>, greater<pair<int, string>>> pq;

    // Initialize distances and priority queue
    dist[start] = 0;
    pq.emplace(0, start);
    for (const auto& pair : graph.getNeighbors(start)) {
        dist[pair.first] = numeric_limits<int>::max();
    }

    while (!pq.empty()) {
        auto top = pq.top();
        int currentDist = top.first;
        string u = top.second;
        pq.pop();

        if (currentDist > dist[u]) continue;

        if (u == end) break;

        for (const auto& pair : graph.getNeighbors(u)) {
            string v = pair.first;
            int weight = pair.second;

            if (blockedNodes.count(v) > 0 || blockedNodes.count(u) > 0) continue;

            int newDist = currentDist + weight;
            if (dist.find(v) == dist.end() || newDist < dist[v]) {
                dist[v] = newDist;
                prev[v] = u;
                edgeWeights[v] = weight;  // Store edge weight
                pq.emplace(newDist, v);
            }
        }
    }

    vector<string> path;
    int totalDistance = 0;
    for (string at = end; !at.empty(); at = prev[at]) {
        path.push_back(at);
        if (prev.find(at) != prev.end()) {
            string from = prev[at];
            if (edgeWeights.find(at) != edgeWeights.end()) {
                totalDistance += edgeWeights[at];
            }
        }
    }
    reverse(path.begin(), path.end());
    return {path, totalDistance};
}

int main() {
    Graph graph;
    readGraphFromFile(graph, "data.txt");

    string start, end;
    cout << "Enter the start city: ";
    getline(cin, start);
    cout << "Enter the end city: ";
    getline(cin, end);

    // Get blocked nodes
    set<string> blockedNodes;
    cout << "Enter blocked nodes (type 'done' when finished):" << endl;
    string blockedNode;
    while (getline(cin, blockedNode) && blockedNode != "done") {
        blockedNodes.insert(blockedNode);
    }

    auto result = dijkstra(graph, start, end, blockedNodes);
    vector<string> path = result.first;
    int totalDistance = result.second;

    if (path.empty() || path[0] != start) {
        cout << "No route found from " << start << " to " << end << endl;
    } else {
        cout << "Shortest path from " << start << " to " << end << " is:" << endl;
        for (size_t i = 0; i < path.size() - 1; ++i) {
            cout << path[i] << " --(" << graph.getNeighbors(path[i]).at(path[i + 1]) << ")--> ";
        }
        cout << path.back() << endl;
        cout << "Total distance: " << totalDistance << endl;
    }

    return 0;
}
