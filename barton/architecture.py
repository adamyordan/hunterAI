from alexander.concepts import Percept
from .concepts import Architecture
import logging


class HunterRemoteArchitecture(Architecture):
    def perceive(self, environment):
        return Percept({'monster_visible': environment.monster_visible})

    def act(self, environment, action):
        if action.id == 'shoot':
            logging.debug('Pew! Shooting monster')
        environment.set_remote_action(action)
