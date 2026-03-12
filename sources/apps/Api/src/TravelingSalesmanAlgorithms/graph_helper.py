import networkx as nx

# Budowanie grafu
def build_graph(parallelWays, perpendicularWays, stopsOnSeparatedRoad):
    #numberOfStops = parallelWays * (perpendicularWays - 1) * stopsOnSeparatedRoad
    stopsOnParallelWay = perpendicularWays + (perpendicularWays - 1) * stopsOnSeparatedRoad
    stepsToCross = stopsOnSeparatedRoad + 1
    graph = nx.Graph()

    # połączenia kolumnowe
    for column in range(0, parallelWays, 1):
        for node in range(0, stopsOnParallelWay-1, 1):
            firstNode = node + (column*stopsOnParallelWay)
            secondNode = node + (column*stopsOnParallelWay) + 1
            if ((firstNode - column) % stepsToCross == 0):
                graph.add_edge(firstNode, secondNode, weight = 2)
            elif((secondNode - column) % stepsToCross == 0):
                graph.add_edge(firstNode, secondNode, weight = 2)
            else:
                graph.add_edge(firstNode, secondNode, weight = 1)

    # połączenia wierszowe
    for column in range(1, parallelWays, 1):
        for index_in_row in range(0, stopsOnParallelWay, stepsToCross):
            correctNodeNumber = index_in_row + column*stopsOnParallelWay
            nodeOnLeft = index_in_row + (column-1)*stopsOnParallelWay
            graph.add_edge(nodeOnLeft, correctNodeNumber, weight = 3)

    # punkt startowy robota
    nodes = len(graph)
    graph.add_edge(nodes-1, nodes, weight=2)

    return graph, stopsOnParallelWay