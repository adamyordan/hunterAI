from .concepts import Action, Program


class HunterProgram(Program):
    def process(self, percept):
        if percept.state['monster_visible'] is True:
            return Action('shoot')
        else:
            return None
