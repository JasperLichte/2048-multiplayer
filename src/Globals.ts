import Game from "./game/Game.js";
import Connection from "./socket/Connection.js";
import Status from "./game/Status.js";

export default class Globals {

  private static socket: WebSocket;
  private static game: Game;

  public static init(
    gameId: number,
    playerIds: number[],
    localId: number,
    status: Status
  ) {
    Globals.socket = (new Connection()).getSocket();
    Globals.game = new Game(gameId, playerIds, localId, status);
  }

  public static getSocket = () => Globals.socket;
  public static getGame = () => Globals.game;

}
