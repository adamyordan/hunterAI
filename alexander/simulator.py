from .architecture import CliArchitecture
from .program import HunterProgram
import logging
import os
import time


class Simulator:
    def __init__(self, architecture):
        self.architecture = architecture
        self.time = 0

    def step(self):
        self.architecture.perceive()
        self.time += 1

    def debug(self):
        logging.debug('monster_visible is %s', self.architecture.environment.monster_visible)

    @staticmethod
    def instantiate():
        program = HunterProgram()
        environment = CliArchitecture.Environment()
        architecture = CliArchitecture(program, environment)
        return Simulator(architecture)


if __name__ == '__main__':
    logging.basicConfig(level=os.environ.get("LOGLEVEL", "INFO"))

    simulator = Simulator.instantiate()
    simulator.debug()

    time.sleep(1)

    logging.debug('stepping...')
    simulator.step()
    simulator.debug()

    time.sleep(1)

    logging.debug('set monster_visible to True')
    simulator.architecture.environment.monster_visible = True
    simulator.debug()

    time.sleep(1)
    logging.debug('stepping...')
    simulator.step()
    simulator.debug()
