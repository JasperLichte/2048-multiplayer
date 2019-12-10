import Globals from "../Globals.js";
import Game from "../game/Game.js";
import { $ } from "../helpers/DomHelper.js";
import Config from "../game/Config.js";
import HtmlHelper from "../helpers/HtmlHelper.js";
import RequestTypes from "./RequestTypes.js";
import Status from "../game/Status.js";
import Connection from "./Connection.js";
import Player from "../game/Player.js";
import Tile from "../game/Tile.js";
import config from "../config/config.js";

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
    Globals.game.setOtherPlayers(players
    .filter(p => p.id !== Globals.game.getLocalPlayerId())
    .map(p => {
      const player = new Player(p.id, p.id === Globals.game.getLocalPlayerId, p.isAdmin);
      player.setScore(p.score);
      player.setTiles(
        p.board.tiles.map(r => r.map(c => new Tile(c.value)))
      );
      return player;
    }));

    Globals.game.getOtherPlayers().forEach(player => player.setScore(
      players.find(p => p.id === player.getId()).score
    ));

    Globals.game.getPlayers().forEach(player => {
      const p = players.find(p => p.id === player.getId());
      player.setName(p.name);
      if (!player.isLocalPlayer) {
        player.setScore(p.score);
      }
    });

    Globals.game.setStatus(gameStatus);
    Globals.game.setRemainingTime(remainingTime);

    Globals.game.update();
  }

  public static unregistered(data: {}) {}

  public static gameEnded(data: {}) {
    Globals.game.setStatus(Status.FINISHED);
    Globals.game.setRemainingTime(0);
    Globals.game.$renderScoreboard();

    MessageHandler.send(RequestTypes.GET_UPDATE);
  }

  public static gameClosed(data: {}) {
    MessageHandler.send(RequestTypes.UNREGISTER);
    window.location.reload();
  }

  public static error(data: {}) {}

  // outgoing
  public static send(type: string, data: {} = {}) {
    if (config.DEBUG_OUTPUTS.OUT_SOCKET) {
      console.log(`>>> ${type}`, data);
    }

    const socket: WebSocket = Connection.getInstance().getSocket();
    data['type'] = type;
    if (Globals.game) {
      data['playerID'] = Globals.game.getLocalPlayerId();
    }
    socket.send(JSON.stringify(data));
  }

  private static initRequestUpdateTimer() {
    setInterval(() => {
      if (Globals.game.getStatus() !== Status.RUNNING) return;
      
      MessageHandler.send(RequestTypes.GET_UPDATE);
    }, 500)
  }

}
