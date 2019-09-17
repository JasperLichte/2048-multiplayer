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
        try {
          const { data } = event;
          this.onMessage(JSON.parse(data));
        } catch(e) {}
      };
    } catch(e) {}
  }

  private onConnect = () => {
    this.socket.send(JSON.stringify({type: RequestTypes.REGISTER}));
  }

  private onDisconnect() {}

  private onMessage(data: {type: string}) {
    if (!data || !Object.entries(data).length) return;
    switch (data.type) {
      case RequestTypes.REGISTERED:
        return MessageHandler.registererd(data);
      case RequestTypes.GAME_STARTED:
        return MessageHandler.gameStarted(data);
      case RequestTypes.UPDATE:
        return MessageHandler.update(data);
      case RequestTypes.PLAYER_BOARD:
        return MessageHandler.playerBoard(data);
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
