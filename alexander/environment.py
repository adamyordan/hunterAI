from .concepts import Environment


class HunterEnvironment(Environment):
    def __init__(self):
        self.monster_visible = False
