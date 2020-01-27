from alexander.concepts import Action, Program
from caleb.algorithm import bfs
from enum import Enum


class GridContent(Enum):
    UNKNOWN = -1
    EMPTY = 0
    MONSTER = 1
    HUNTER = 2


class Actions(Enum):
    MOVE_FORWARD = Action('move_forward')
    SHOOT = Action('shoot')
    ROTATE_LEFT = Action('rotate_left')
    ROTATE_RIGHT = Action('rotate_right')


class HunterProgram(Program):
    def __init__(self):
        self.initiated = False
        self.grid_size = None
        self.grid = None
        self.hunter_position = None
        self.hunter_rotation = None
        self.monster_count = 0
        self.hunter_projectile_range = 1

    def process(self, percept):
        self.update_state_with_percept(percept)
        action = self.choose_action()
        self.update_state_with_action(action)
        return action

    def update_state_with_percept(self, percept):
        if not self.initiated:
            self.grid_size = percept.state['MapSize']
            self.grid = [[GridContent.UNKNOWN for _ in range(self.grid_size['x'])] for _ in range(self.grid_size['y'])]
            self.initiated = True

        self.hunter_position = tuple(percept.state['HunterPosition'])
        self.hunter_rotation = percept.state['HunterRotation']
        self.monster_count = percept.state['MonsterCount']
        self.hunter_projectile_range = percept.state['HunterProjectileDistance']
        for vision in percept.state['Vision']:
            x, y = vision['Position']
            self.grid[y][x] = GridContent(vision['Content'])

    def get_unknown_cells(self):
        unknown_cells = []
        for y, row in enumerate(self.grid):
            for x, col in enumerate(row):
                if col == GridContent.UNKNOWN:
                    unknown_cells.append((x, y))
        return unknown_cells

    def action_move_towards(self, cells):
        path_to_target = bfs.bfs(self.hunter_position, cells, self.grid)
        if path_to_target:
            next_cell = path_to_target[0]
            rotate_action = self.calc_rotate_action(next_cell)
            return rotate_action or Actions.MOVE_FORWARD.value

    def choose_action(self):
        if self.monster_count > 0:
            unknown_cells = self.get_unknown_cells()
            if not unknown_cells:
                return None
            return self.action_move_towards(unknown_cells)

    def update_state_with_action(self, action):
        pass

    def calc_rotate_action(self, cell):
        if self.hunter_position[0] == cell[0] and self.hunter_position[1] == cell[1]:
            return None
        elif self.hunter_position[0] == cell[0]:
            if self.hunter_position[1] > cell[1]:
                target_direction = 0
            else:
                target_direction = 180
        elif self.hunter_position[1] == cell[1]:
            if self.hunter_position[0] > cell[0]:
                target_direction = 270
            else:
                target_direction = 90
        else:
            return None

        if (self.hunter_rotation - target_direction) % 360 == 90:
            return Actions.ROTATE_LEFT.value
        elif (self.hunter_rotation - target_direction) % 360 == 180:
            return Actions.ROTATE_LEFT.value
        elif (self.hunter_rotation - target_direction) % 360 == 270:
            return Actions.ROTATE_RIGHT.value
        else:
            return None
