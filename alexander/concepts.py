class Environment:
    pass


class Action:
    def __init__(self, id):
        self.id = id


class Percept:
    def __init__(self, state):
        self.state = state


class Architecture:
    def perceive(self, environment):
        raise NotImplementedError()

    def act(self, environment, action):
        raise NotImplementedError()


class Program:
    def process(self, percept):
        raise NotImplementedError()


class Agent:
    def __init__(self, program, architecture):
        self.program = program
        self.architecture = architecture

    def step(self, environment):
        percept = self.architecture.perceive(environment)
        action = self.program.process(percept)
        if action:
            self.architecture.act(environment, action)
