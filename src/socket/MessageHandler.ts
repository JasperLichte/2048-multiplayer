import Globals from "../Globals.js";
import Game from "../game/Game.js";
import Status from "../game/Status.js";

export default class MessageHandler {

  public static registererd(data: {}) {
    // @ts-ignore
    const {gameID, localPlayerID} = data;
    Globals.game = new Game(gameID, localPlayerID);
  }

  public static gameStarted(data: {}) {}

  public static update(data: {}) {
    // @ts-ignore
    const { players, gameStatus } = data;
    Globals.game.setPlayerIds(players.map(player => player.id));
    Globals.game.setStatus(gameStatus);
    if (Globals.game.getStatus() !== Status.RUNNING) {
      Globals.game.start();
    }
  }

  public static playerBoard(data: {}) {}
  
  public static unregistered(data: {}) {}

  public static gameEnded(data: {}) {}

  public static error(data: {}) {}

}
