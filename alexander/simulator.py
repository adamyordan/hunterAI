from .concepts import Agent
from .architecture import HunterArchitecture
from .program import HunterProgram
from .environment import HunterEnvironment
import logging
import os
import time


class Simulator:
    def __init__(self, environment, agents):
        self.environment = environment
        self.agents = agents
        self.time = 0

    def step(self):
        for agent in self.agents:
            agent.step(self.environment)
        self.time += 1

    def debug(self):
        logging.debug('[time:%d] monster_visible is %s', self.time, self.environment.monster_visible)

    @staticmethod
    def instantiate():
        environment = HunterEnvironment()

        program = HunterProgram()
        architecture = HunterArchitecture()
        agent = Agent(program, architecture)

        return Simulator(environment, [agent])


if __name__ == '__main__':
    logging.basicConfig(level=os.environ.get('LOGLEVEL', 'DEBUG'))

    simulator = Simulator.instantiate()
    simulator.debug()

    time.sleep(1)

    logging.debug('stepping...')
    simulator.step()
    simulator.debug()

    time.sleep(1)

    logging.debug('set monster_visible to True')
    simulator.environment.monster_visible = True
    simulator.debug()

    time.sleep(1)
    logging.debug('stepping...')
    simulator.step()
    simulator.debug()
