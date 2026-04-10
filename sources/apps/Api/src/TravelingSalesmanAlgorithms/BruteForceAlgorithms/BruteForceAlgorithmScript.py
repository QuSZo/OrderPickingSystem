import networkx as nx
import itertools
from .. import graph_helper

def find_path(positions):
    graph = graph_helper.build_graph(columns=3, rows=3, stops_in_seperated_alley=5)

    start = positions[0]
    end = positions[-1]
    middle = positions[1:-1]

    best_order = None
    best_cost = float('inf')

    for perm in itertools.permutations(middle):
        full_perm = [start] + list(perm) + [end]

        cost = 0
        for i in range(len(full_perm) - 1):
            cost += nx.shortest_path_length(graph, full_perm[i], full_perm[i+1])
        
        if cost < best_cost:
            best_cost = cost
            best_order = full_perm

    full_path = []

    for i in range(len(best_order) - 1):
        path = nx.shortest_path(graph, best_order[i], best_order[i+1], weight="weight")

        if i > 0:
            path = path[1:]

        full_path.extend(path)

    distances = []
    for i in range(len(full_path)-1):
        cost = nx.path_weight(graph, [full_path[i], full_path[i+1]], weight="weight")
        distances.append(cost)

    total_weight = sum(graph[u][v]["weight"] for u, v in zip(full_path, full_path[1:]))

    return full_path, total_weight, distances