class Action:
    def __init__(self, id):
        self.id = id


class Percept:
    def __init__(self, state):
        self.state = state


class Architecture:
    def __init__(self, program):
        self.program = program

    def perceive(self):
        raise NotImplementedError()

    def act(self, action):
        raise NotImplementedError()


class Program:
    def process(self, percept):
        raise NotImplementedError()
