from flask import Flask, jsonify, request
from flask_cors import CORS


class Remote:
    class Controller:
        @staticmethod
        def ping():
            return 'pong'

        @staticmethod
        def init(remote):
            remote.init()
            return jsonify({'ok': True})

        @staticmethod
        def step(remote):
            state = request.get_json(force=True)
            response = remote.step(state)
            return jsonify(response)

    def __init__(self, init_function):
        self.init_function = init_function
        self.agents = None
        self.environment = None
        self.time = 0

    def init(self):
        self.environment, self.agents = self.init_function()
        self.time = 0

    def step(self, state):
        self.environment.update(state)
        for agent in self.agents:
            agent.step(self.environment)
        action = self.environment.get_remote_action()
        action_serializable = action.__dict__ if action is not None else None
        self.time += 1
        return {'action': action_serializable}

    def app(self):
        app = Flask(__name__)
        CORS(app)
        app.add_url_rule('/', 'ping', lambda: Remote.Controller.ping())
        app.add_url_rule('/init', 'init', lambda: Remote.Controller.init(self), methods=['PUT'])
        app.add_url_rule('/step', 'step', lambda: Remote.Controller.step(self), methods=['PUT'])
        return app
