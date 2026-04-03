import networkx as nx
from .. import graph_helper

def find_path(positions):
    full_path = []
    graph = graph_helper.build_graph(columns=3, rows=3, stops_in_seperated_alley=5)
    
    for i in range(0, len(positions)-1, 1):
        path = nx.shortest_path(graph, source=positions[i], target=positions[i+1], weight="weight")

        if i > 0:
            path = path[1:] 
        
        full_path.extend(path)

    total_weight = sum(graph[u][v]["weight"] for u, v in zip(full_path, full_path[1:]))

    return full_path, total_weight