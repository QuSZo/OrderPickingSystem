import networkx as nx
from .. import graph_helper

def find_path(positions):
    graph = graph_helper.build_graph(columns=3, rows=3, stops_in_seperated_alley=5)

    positions = positions[:-1]    
    full_path = nx.approximation.traveling_salesman_problem(graph, nodes=positions, weight="weight")

    total_weight = nx.path_weight(graph, full_path, weight="weight")

    return full_path, total_weight