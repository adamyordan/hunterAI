from alexander.concepts import Architecture, Environment


class RemoteArchitecture(Architecture):
    def perceive(self, environment):
        raise NotImplementedError()

    def act(self, environment, action):
        raise NotImplementedError()


class RemoteEnvironment(Environment):
    def __init__(self):
        self.remote_action = None

    def update(self, state):
        self.remote_action = None
        self.update_state(state)

    def set_remote_action(self, action):
        self.remote_action = action

    def get_remote_action(self):
        return self.remote_action

    def update_state(self, state):
        raise NotImplementedError()
