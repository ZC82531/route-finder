// dijkstra.cpp
#include <iostream>
#include <fstream>
#include <sstream>
#include <vector>
#include <map>
#include <climits>
#include <set>
#include <cstring>
#include <string>

// All edges are directed, so distance to both routes must be specified, and any mentioned blocks in terminal will be for the order specified.

extern "C" {
    void FindShortestPath(const char* start, const char* end, char* path, int* distance) {
        // Reading graph data from temp.txt
        std::ifstream file("temp.txt");
        std::map<std::string, std::map<std::string, int> > graph;
        std::string line;

        while (std::getline(file, line)) {
            std::istringstream iss(line);
            std::string city1, city2;
            int dist;

            // Manually parse the quoted strings
            if (std::getline(iss, city1, '"') && std::getline(iss, city1, '"') &&
                std::getline(iss, city2, '"') && std::getline(iss, city2, '"')) {
                
                iss >> dist;

                // Only add the forward edge for directed graph
                graph[city1][city2] = dist;
            }
        }

        // Dijkstra's algorithm
        std::map<std::string, int> dist_map;
        std::map<std::string, std::string> prev_map;
        std::map<std::string, std::string> edge_map; // To store edge weights
        std::set<std::pair<int, std::string> > pq;

        for (const auto& node : graph) {
            dist_map[node.first] = INT_MAX;
            prev_map[node.first] = "";
        }

        dist_map[start] = 0;
        pq.insert(std::make_pair(0, start));

        while (!pq.empty()) {
            std::string u = pq.begin()->second;
            int current_dist = pq.begin()->first;
            pq.erase(pq.begin());

            if (u == end) break;

            for (const auto& neighbor : graph[u]) {
                std::string v = neighbor.first;
                int weight = neighbor.second;
                int new_dist = current_dist + weight;

                if (new_dist < dist_map[v]) {
                    pq.erase(std::make_pair(dist_map[v], v));
                    dist_map[v] = new_dist;
                    prev_map[v] = u;
                    edge_map[v] = std::to_string(weight); // Record the weight of the edge
                    pq.insert(std::make_pair(new_dist, v));
                }
            }
        }

        // Check if the destination is reachable
        if (dist_map[end] == INT_MAX) {
            // Route is not possible
            strncpy(path, "Route is not possible", 1024);
            *distance = -1; // Set distance to -1 to indicate no path found
            return;
        }

        // Reconstruct the path
        std::vector<std::string> path_vec;
        std::vector<std::string> weights;
        for (std::string at = end; !at.empty(); at = prev_map[at]) {
            path_vec.push_back(at);
            if (!prev_map[at].empty()) {
                weights.push_back(edge_map[at]); // Collect weights in reverse order
            }
        }
        std::reverse(path_vec.begin(), path_vec.end());
        std::reverse(weights.begin(), weights.end());

        // Format the path with weights
        std::ostringstream oss;
        for (size_t i = 0; i < path_vec.size(); ++i) {
            if (i > 0) {
                oss << "--(" << weights[i-1] << ")-->";
            }
            oss << path_vec[i];
        }

        std::string result = oss.str();
        strncpy(path, result.c_str(), 1024);

        *distance = dist_map[end];
    }
}
