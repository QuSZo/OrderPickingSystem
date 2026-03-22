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
        path = nx.shortest_path(graph, best_order[i], best_order[i+1])

        if i > 0:
            path = path[1:]

        full_path.extend(path)

    return full_path