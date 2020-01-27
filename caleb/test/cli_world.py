import json
import sys
import time

import requests
import termcolor

from caleb.draft.program2 import GridContent, Actions


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


def set_content(grid, cell, content):
    grid[cell[1]][cell[0]] = content


class CliWorld:

    def __init__(self, grid_str, hunter_face):
        self.grid, self.hunter, self.monsters = parse_grid(grid_str)
        self.hunter_face = hunter_face
        self.vision = []
        self.seen = {self.hunter}
        self.dead_monster = None

    def run(self):
        self.update()
        self.debug(sleep=1)
        self.init()
        while True:
            self.update()
            state = self.get_state()
            step_response = self.step(state)
            self.actuate(step_response['action'])
            self.update()
            self.debug(refresh=True, sleep=0.3)

    def init(self):
        requests.put('http://localhost:5000/init')

    def step(self, state):
        r = requests.put('http://localhost:5000/step', data=json.dumps(state))
        return r.json()

    def update(self):
        self.vision = []
        vision_range = 3
        hx, hy = self.hunter
        if self.hunter_face == 180:
            for i in range(vision_range):
                self.vision.append((hx, hy + i))
                for j in range(1, i+1):
                    self.vision.append((hx - j, hy + i))
                    self.vision.append((hx + j, hy + i))
        elif self.hunter_face == 0:
            for i in range(vision_range):
                self.vision.append((hx, hy - i))
                for j in range(1, i+1):
                    self.vision.append((hx - j, hy - i))
                    self.vision.append((hx + j, hy - i))
        elif self.hunter_face == 270:
            for i in range(vision_range):
                self.vision.append((hx - i, hy))
                for j in range(1, i+1):
                    self.vision.append((hx - i, hy - j))
                    self.vision.append((hx - i, hy + j))
        elif self.hunter_face == 90:
            for i in range(vision_range):
                self.vision.append((hx + i, hy))
                for j in range(1, i+1):
                    self.vision.append((hx + i, hy - j))
                    self.vision.append((hx + i, hy + j))
        for c in self.vision:
            self.seen.add(c)

    def get_state(self):
        map_size_x, map_size_y = len(self.grid[0]), len(self.grid)
        visionContent = []
        for x, y in self.vision:
            try:
                visionContent.append({
                    'Position': (x, y),
                    'Content': self.grid[y][x].value
                })
            except:
                pass
        state = {
            'Vision': visionContent,
            'MonsterCount': len(self.monsters),
            'MapSize': {'x': map_size_x, 'y': map_size_y},
            'HunterPosition': self.hunter,
            'HunterRotation': self.hunter_face,
            'HunterProjectileDistance': 3
        }
        return state

    def actuate(self, action):
        self.dead_monster = None
        if action is None: return
        hx, hy = self.hunter
        if action['id'] == Actions.MOVE_FORWARD.value.id:
            if self.hunter_face == 0:
                hnext = (hx, hy - 1)
            elif self.hunter_face == 90:
                hnext = (hx + 1, hy)
            elif self.hunter_face == 180:
                hnext = (hx, hy + 1)
            else:
                hnext = (hx - 1, hy)

            set_content(self.grid, self.hunter, GridContent.EMPTY)
            set_content(self.grid, hnext, GridContent.HUNTER)
            self.hunter = hnext

        elif action['id'] == Actions.ROTATE_RIGHT.value.id:
            self.hunter_face = (self.hunter_face + 90) % 360

        elif action['id'] == Actions.ROTATE_LEFT.value.id:
            self.hunter_face = (self.hunter_face - 90) % 360

        elif action['id'] == Actions.SHOOT.value.id:
            hx, hy = self.hunter
            for i in range(1, 4):
                if self.hunter_face == 0:
                    curr = (hx, hy - i)
                elif self.hunter_face == 90:
                    curr = (hx + i, hy)
                elif self.hunter_face == 180:
                    curr = (hx, hy + i)
                else:
                    curr = (hx - i, hy)
                if self.grid[curr[1]][curr[0]] == GridContent.MONSTER:
                    set_content(self.grid, curr, GridContent.EMPTY)
                    self.monsters.remove(curr)
                    self.dead_monster = curr
                    break

    def debug(self, refresh=False, sleep=1):
        output = ''
        row_str = '      '
        for i in range(len(self.grid[0])):
            row_str += str(i) + ' '
        output += row_str + '\n'

        for y, row in enumerate(self.grid):
            row_str = '  ' + str(y) + '   '
            for x, col in enumerate(row):
                if col == GridContent.EMPTY:
                    if self.dead_monster and x == self.dead_monster[0] and y == self.dead_monster[1]:
                        row_str += termcolor.colored('x', 'red')
                    elif (x,y) in self.vision:
                        row_str += termcolor.colored('-', 'yellow')
                    elif (x,y) in self.seen:
                        row_str += termcolor.colored('-', 'white')
                    else:
                        row_str += termcolor.colored('-', 'grey')
                elif col == GridContent.MONSTER:
                    row_str += termcolor.colored('M', 'red')
                elif col == GridContent.HUNTER:
                    row_str += termcolor.colored('H', 'green')
                elif col == GridContent.UNKNOWN:
                    row_str += '?'
                row_str += ' '
            output += row_str + '\n'

        if refresh:
            for _ in range(len(self.grid) + 1):
                sys.stdout.write("\x1b[1A\x1b[2K")  # move up cursor and delete whole line

        sys.stdout.write("\r{0}".format(output))
        sys.stdout.flush()
        time.sleep(sleep)


if __name__ == '__main__':
    grid_str = [
        '-M-------',
        '------M--',
        '--M------',
        '---------',
        '---------',
        '---------',
        '----H----',
        '---------',
        '---------',
    ]

    # grid_str = [
    #     '-M-------',
    #     '------M--',
    #     '---------',
    #     '---------',
    #     '---------',
    #     '---------',
    #     '----H----',
    #     '-----M---',
    #     '---------',
    # ]

    print('\nTesting: Hunter Agent\n')
    world = CliWorld(grid_str, hunter_face=0)
    world.run()
