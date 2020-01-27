from alexander.concepts import Percept, Architecture


class GeneralRemoteArchitecture(Architecture):
    def perceive(self, environment):
        return Percept(environment.state)

    def act(self, environment, action):
        environment.set_remote_action(action)
