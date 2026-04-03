import networkx as nx

def build_graph(columns, rows, stops_in_seperated_alley):
    stops_in_column = rows + (rows - 1) * stops_in_seperated_alley
    stepsToCross = stops_in_seperated_alley + 1
    graph = nx.Graph()

    for x in range(0, columns, 1):
        for y in range(0, stops_in_column-1, 1):
            first_node_in_current_column = (x * stops_in_column)
            first_node_id = y + first_node_in_current_column
            second_node_id = y + first_node_in_current_column + 1

            distance_between_nodes = calculate_distance(first_node_id, second_node_id, x, stepsToCross)
            graph.add_edge((x,y), (x,y+1), weight = distance_between_nodes)

    for x in range(1, columns, 1):
        for y in range(0, stops_in_column, stepsToCross):
            graph.add_edge((x-1,y), (x,y), weight = 132)

    # graph.add_edge((0,0), (0,-1), weight=2)

    return graph

def calculate_distance(first_node_id, second_node_id, column, stepsToCross):
    if ((first_node_id - column) % stepsToCross == 0):
        return 52
    elif((second_node_id - column) % stepsToCross == 0):
        return 52
    else:
        return 32