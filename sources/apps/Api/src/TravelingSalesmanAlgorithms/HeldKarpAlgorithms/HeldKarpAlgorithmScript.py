from itertools import combinations
import networkx as nx
from .. import graph_helper

def find_path(positions):
    graph = graph_helper.build_graph(columns=3, rows=3, stops_in_seperated_alley=5)
    positions = positions[:-1]

    n = len(positions)
    dist_matrix = [[0] * n for _ in range(n)]

    for i in range(len(positions)):
        for j in range(len(positions)):
            if i == j:
                continue
            path = nx.shortest_path(graph, source=positions[i], target=positions[j], weight="weight")
            cost = nx.path_weight(graph, path, weight="weight")
            dist_matrix[i][j] = cost

    cost, tour = held_karp(dist_matrix)

    start = positions[tour[0]]
    full_path = [start]
    
    for i in range(0, len(tour)-1, 1):
        path = nx.shortest_path(graph, source=positions[tour[i]], target=positions[tour[i+1]], weight="weight")
        
        full_path.extend(path[1:])

    distances = []
    for i in range(len(full_path)-1):
        cost = nx.path_weight(graph, [full_path[i], full_path[i+1]], weight="weight")
        distances.append(cost)

    total_weight = nx.path_weight(graph, full_path, weight="weight")

    return full_path, total_weight, distances

def held_karp(dist):
    n = len(dist)
    N = 1 << n
    INF = float('inf')

    dp = [[INF] * n for _ in range(N)]
    parent = [[None] * n for _ in range(N)]

    dp[1][0] = 0

    for mask in range(1, N):
        if not (mask & 1):
            continue
        for j in range(1, n):
            if not (mask & (1 << j)):
                continue
            prev_mask = mask ^ (1 << j)
            for k in range(n):
                if prev_mask & (1 << k):
                    cost = dp[prev_mask][k] + dist[k][j]
                    if cost < dp[mask][j]:
                        dp[mask][j] = cost
                        parent[mask][j] = k

    full_mask = (1 << n) - 1
    min_cost = INF
    last = None
    for j in range(1, n):
        cost = dp[full_mask][j] + dist[j][0]
        if cost < min_cost:
            min_cost = cost
            last = j

    path = []
    mask = full_mask
    curr = last
    while curr is not None:
        path.append(curr)
        prev = parent[mask][curr]
        mask ^= (1 << curr)
        curr = prev
    path.append(0)
    path.reverse()
    path.append(0)

    return min_cost, path