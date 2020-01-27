import sys

from caleb.algorithm import bfs
from caleb.program import GridContent
import termcolor
import time


def parse_grid(grid_str):
    grid = []
    for row in grid_str:
        gridrow = []
        for col in row:
            if col == '-':
                gridrow.append(GridContent.EMPTY)
            elif col == 'M':
                gridrow.append(GridContent.MONSTER)
            elif col == 'H':
                gridrow.append(GridContent.HUNTER)
        grid.append(gridrow)
    hunter, monsters = None, []
    for y, row in enumerate(grid):
        for x, col in enumerate(row):
            if col == GridContent.HUNTER:
                hunter = (x, y)
            elif col == GridContent.MONSTER:
                monsters.append((x, y))
    return grid, hunter, monsters


def debug_grid(grid, refresh=False, sleep=1):
    output = ''
    for row in grid:
        row_str = '  '
        for col in row:
            if col == GridContent.EMPTY:
                row_str += '-'
            elif col == GridContent.MONSTER:
                row_str += termcolor.colored('M', 'red')
            elif col == GridContent.HUNTER:
                row_str += termcolor.colored('H', 'green')
            elif col == GridContent.UNKNOWN:
                row_str += '?'
            row_str += ' '
        output += row_str + '\n'

    if refresh:
        for _ in range(len(grid)):
            sys.stdout.write("\x1b[1A\x1b[2K")  # move up cursor and delete whole line

    sys.stdout.write("\r{0}".format(output))
    sys.stdout.flush()
    time.sleep(sleep)


def set_content(grid, cell, content):
    grid[cell[1]][cell[0]] = content


def test_bfs():
    grid_str = [
        '-M------',
        '------M-',
        '--M-----',
        '--------',
        '--------',
        '--------',
        '----H---',
        '--------',
    ]

    print('\nTesting: BFS to nearest monster\n')

    grid, hunter, monsters = parse_grid(grid_str)
    debug_grid(grid)

    while True:
        if hunter in monsters:
            break
        path = bfs.bfs(hunter, monsters, grid)
        next_hunter = path[0]
        set_content(grid, hunter, GridContent.EMPTY)
        set_content(grid, next_hunter, GridContent.HUNTER)
        hunter = next_hunter
        debug_grid(grid, refresh=False)


if __name__ == '__main__':
    test_bfs()
