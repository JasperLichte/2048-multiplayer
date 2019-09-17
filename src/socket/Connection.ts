import config from '../config/config.js';
import RequestTypes from './RequestTypes.js';
import MessageHandler from './MessageHandler.js';

export default class Connection {

  private socket: WebSocket;

  constructor() {
    try {
      this.socket = new WebSocket(config['WEBSOCKET_URL']());

      this.socket.onopen = this.onConnect;
      this.socket.onclose = this.onDisconnect;
      this.socket.onmessage = event => {
        console.log(event);
        try {
          const { data } = event;
          this.onMessage(data.type, data.data);
        } catch(e) {}
      };
    } catch(e) {}
  }

  private onConnect() {
    console.log('connected');
  }

  private onDisconnect() {
    console.log('disconnected');
  }

  private onMessage(type: string, data: {}) {
    if (!data || !Object.entries(data).length) return;
    switch (type) {
      case RequestTypes.REGISTERED:
        return MessageHandler.registererd(data);
      case RequestTypes.ROUND_STARTED:
        return MessageHandler.roundStarted(data);
      case RequestTypes.UPDATE:
        return MessageHandler.update(data);
      case RequestTypes.PLAYER_BOARD:
        return MessageHandler.playerBoard(data);
      case RequestTypes.ROUND_ENDED:
        return MessageHandler.roundEnded(data);
      case RequestTypes.UNREGISTERED:
        return MessageHandler.unregistered(data);
      case RequestTypes.GAME_ENDED:
        return MessageHandler.gameEnded(data);
      case RequestTypes.ERROR:
        return MessageHandler.error(data);
    }
  }

  public getSocket() {
    return this.socket;
  }

}
