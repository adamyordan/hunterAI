from barton.concepts import RemoteEnvironment


class GeneralRemoteEnvironment(RemoteEnvironment):
    def __init__(self):
        super().__init__()
        self.state = {}

    def update_state(self, state):
        self.state = state
