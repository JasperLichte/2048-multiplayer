import config from '../config/config.js';
import RequestTypes from './RequestTypes.js';
import MessageHandler from './MessageHandler.js';
import { $ } from '../helpers/DomHelper.js';

export default class Connection {

  private static connection = new Connection();

  private socket: WebSocket;

  private constructor() {
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

  public static getInstance = () => Connection.connection;

  private onConnect = () => {
    $('#spinner').classList.add('hidden');
    MessageHandler.send(RequestTypes.REGISTER);
  }

  private onDisconnect() {
    $('#spinner').classList.remove('hidden');
  }

  private onMessage(data: {type: string}) {
    if (!data || !Object.entries(data).length) return;
    console.log(`<<< ${data.type}`, data);
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
      case RequestTypes.GAME_CLOSED:
        return MessageHandler.gameClosed(data);
      case RequestTypes.ERROR:
        return MessageHandler.error(data);
    }
  }

  public getSocket() {
    return this.socket;
  }

}
