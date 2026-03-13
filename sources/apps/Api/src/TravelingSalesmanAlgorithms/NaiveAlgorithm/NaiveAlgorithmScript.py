import networkx as nx
from .. import graph_helper

def find_path(positions):
    full_path = []
    graph = graph_helper.build_graph(columns=3, rows=3, stops_in_seperated_alley=5)
    
    for i in range(0, len(positions)-1, 1):
        path = nx.shortest_path(graph, source=positions[i], target=positions[i+1])

        if i > 0:
            path = path[1:] 
        
        full_path.extend(path)

    return full_path