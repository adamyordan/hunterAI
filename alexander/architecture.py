from .concepts import Architecture, Percept
import logging


class CliArchitecture(Architecture):
    class Environment:
        def __init__(self):
            self.monster_visible = False

    def __init__(self, program, environment):
        super().__init__(program)
        self.environment = environment

    def perceive(self):
        percept = Percept({'monster_visible': self.environment.monster_visible})
        action = self.program.process(percept)
        if action:
            self.act(action)

    def act(self, action):
        if action.id == 'shoot':
            logging.debug('Pew! Shooting monster')
            self.environment.monster_visible = False
