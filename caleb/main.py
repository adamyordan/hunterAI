from alexander.concepts import Agent
from barton.remote import Remote
from .architecture import GeneralRemoteArchitecture
from .environment import GeneralRemoteEnvironment
from caleb.program import HunterProgram

if __name__ == '__main__':
    def init_function():
        environment = GeneralRemoteEnvironment()
        program = HunterProgram()
        architecture = GeneralRemoteArchitecture()
        agent = Agent(program, architecture)
        return environment, [agent]

    remote = Remote(init_function)
    remote.app().run(debug=True)
