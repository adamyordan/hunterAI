from .remote import Remote
from .architecture import HunterRemoteArchitecture
from .environment import HunterRemoteEnvironment
from alexander.program import HunterProgram
from alexander.concepts import Agent

if __name__ == '__main__':
    def init_function():
        environment = HunterRemoteEnvironment()
        program = HunterProgram()
        architecture = HunterRemoteArchitecture()
        agent = Agent(program, architecture)
        return environment, [agent]

    remote = Remote(init_function)
    remote.app().run()
