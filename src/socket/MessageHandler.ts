import Globals from "../Globals.js";
import Game from "../game/Game.js";
import { $ } from "../helpers/DomHelper.js";

export default class MessageHandler {

  public static registererd(data: {}) {
    // @ts-ignore
    const {gameID, localPlayerID} = data;
    Globals.game = new Game(gameID, localPlayerID);
  }

  public static gameStarted(data: {}) {
    $('#welcome-card').remove();
    Globals.game.start();
  }

  public static update(data: {}) {
    // @ts-ignore
    const { players, gameStatus } = data;
    Globals.game.setPlayerIds(players.map(player => player.id));
    Globals.game.setStatus(gameStatus);
  }

  public static playerBoard(data: {}) {}

  public static unregistered(data: {}) {}

  public static gameEnded(data: {}) {}

  public static error(data: {}) {}

  // outgoing
  public static send(type: string, data: {} = {}) {
    const socket: WebSocket = Globals.connection.getSocket();
    data['type'] = type;
    data['playerID'] = Globals.game.getLocalPlayerId();
    socket.send(JSON.stringify(data));
  }

}
