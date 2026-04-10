import networkx as nx
from .. import graph_helper

def find_path(positions):
    min_weight = 32

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
            path = nx.astar_path(graph, source=current_source, target=target, heuristic=lambda a, b: (abs(a[0]-b[0]) + abs(a[1]-b[1]))*min_weight, weight="weight")
            cost = nx.path_weight(graph, path, weight="weight")

            if cost < calculated_cost:
                calculated_cost = cost
                calculated_target = target
                calculated_path = path

        full_path.extend(calculated_path[1:])

        current_source = calculated_target
        unvisited.remove(current_source)

    path_to_start = nx.astar_path(graph, source=current_source, target=end, heuristic=lambda a, b: (abs(a[0]-b[0]) + abs(a[1]-b[1]))*min_weight, weight="weight")
    full_path.extend(path_to_start[1:])

    total_weight = nx.path_weight(graph, full_path, weight="weight")

    return full_path, total_weight