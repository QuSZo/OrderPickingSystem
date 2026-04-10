import networkx as nx
from .. import graph_helper

def find_path(positions):
    graph = graph_helper.build_graph(columns=3, rows=3, stops_in_seperated_alley=5)
    
    end = positions[-1]

    positions_to_calculate = positions[0:-1]
    unvisited = set(positions_to_calculate)

    current_source = positions_to_calculate[0]
    unvisited.remove(current_source)
    
    full_path = [current_source]

    while unvisited:
        calculated_target = None
        calculated_path = None
        calculated_cost = float("inf")

        for target in unvisited:
            path = nx.shortest_path(graph, source=current_source, target=target, weight="weight")
            cost = nx.path_weight(graph, path, weight="weight")

            if cost < calculated_cost:
                calculated_cost = cost
                calculated_target = target
                calculated_path = path

        full_path.extend(calculated_path[1:])

        current_source = calculated_target
        unvisited.remove(current_source)

    path_to_start = nx.shortest_path(graph, source=current_source, target=end, weight="weight")
    full_path.extend(path_to_start[1:])

    distances = []
    for i in range(len(full_path)-1):
        cost = nx.path_weight(graph, [full_path[i], full_path[i+1]], weight="weight")
        distances.append(cost)

    total_weight = nx.path_weight(graph, full_path, weight="weight")

    return full_path, total_weight, distances