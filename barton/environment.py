from .concepts import RemoteEnvironment


class HunterRemoteEnvironment(RemoteEnvironment):
    def __init__(self):
        super().__init__()
        self.monster_visible = False

    def update_state(self, state):
        if 'monster_visible' in state:
            self.monster_visible = state['monster_visible']
