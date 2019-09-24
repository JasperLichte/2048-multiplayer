import Globals from "../Globals.js";
import Game from "../game/Game.js";
import { $ } from "../helpers/DomHelper.js";
import Config from "../game/Config.js";
import HtmlHelper from "../helpers/HtmlHelper.js";
import RequestTypes from "./RequestTypes.js";

export default class MessageHandler {

  public static registererd(data: {}) {
    // @ts-ignore
    const { gameID, localPlayerID, isAdmin, config } = data;
    const { boardSize, maxUsers, roundDuration } = config;

    boardSize && (Config.BOARD_SIZE = boardSize);
    maxUsers && (Config.MAX_USERS = maxUsers);
    roundDuration && (Config.ROUND_DURATION = roundDuration);

    Globals.game = new Game(gameID, localPlayerID, isAdmin);
    Globals.game.setRemainingTime(Config.ROUND_DURATION);

    if (isAdmin) {
      HtmlHelper.span(
        'Admin',
        {id: 'admin-badge'},
        $('#welcome-card')
      );
    }
  }

  public static gameStarted(data: {}) {
    $('#welcome-card').remove();
    $('#spinner').classList.add('hidden');
    Globals.game.start();
    MessageHandler.initRequestUpdateTimer();
  }

  public static update(data: {}) {
    // @ts-ignore
    const { players, gameStatus, remainingTime } = data;
    Globals.game.setPlayerIds(players.map(player => player.id));
    Globals.game.setStatus(gameStatus);
    Globals.game.setRemainingTime(remainingTime);

    Globals.game.update();
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

  private static initRequestUpdateTimer() {
    setInterval(() => MessageHandler.send(RequestTypes.GET_UPDATE), 500);
  }

}
