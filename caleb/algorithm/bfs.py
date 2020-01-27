def neighbors(current, grid):
    size_x = len(grid[0])
    size_y = len(grid)

    res = []
    if current[0] - 1 >= 0:
        res.append((current[0] - 1, current[1]))
    if current[0] + 1 < size_x:
        res.append((current[0] + 1, current[1]))
    if current[1] - 1 >= 0:
        res.append((current[0], current[1] - 1))
    if current[1] + 1 < size_y:
        res.append((current[0], current[1] + 1))
    return res


def bfs(start, goals, grid):
    """
    Using BFS to get path to nearest goal.
    :param start:
    :param goals:
    :param grid:
    :return:
    """
    size_x = len(grid[0])
    size_y = len(grid)

    visited = [[False for _ in range(size_x)] for _ in range(size_y)]
    parent = [[None for _ in range(size_x)] for _ in range(size_y)]
    queue = [start]
    visited[start[1]][start[0]] = True

    while queue:
        current = queue.pop(0)
        if current in goals:
            path = []
            while parent[current[1]][current[0]]:
                path.append(current)
                current = parent[current[1]][current[0]]
            return path[::-1]
        for neighbor in neighbors(current, grid):
            if not visited[neighbor[1]][neighbor[0]]:
                queue.append(neighbor)
                parent[neighbor[1]][neighbor[0]] = current
                visited[neighbor[1]][neighbor[0]] = True

    raise ValueError('No Path Found')
