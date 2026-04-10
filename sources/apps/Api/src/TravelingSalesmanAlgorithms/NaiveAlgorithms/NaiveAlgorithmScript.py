import networkx as nx
from .. import graph_helper

def find_path(positions):
    graph = graph_helper.build_graph(columns=3, rows=3, stops_in_seperated_alley=5)

    start = positions[0]
    full_path = [start]
    
    for i in range(0, len(positions)-1, 1):
        path = nx.shortest_path(graph, source=positions[i], target=positions[i+1], weight="weight")
        
        full_path.extend(path[1:])

    distances = []
    for i in range(len(full_path)-1):
        cost = nx.path_weight(graph, [full_path[i], full_path[i+1]], weight="weight")
        distances.append(cost)

    total_weight = nx.path_weight(graph, full_path, weight="weight")

    return full_path, total_weight, distances