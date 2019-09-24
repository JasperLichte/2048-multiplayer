import Player from "./Player.js";
import Status from "./Status.js";
import Board from "./Board.js";
import { $ } from "../helpers/DomHelper.js";
import HtmlHelper from "../helpers/HtmlHelper.js";

export default class Game {

  private id: number;
  private playerIds: number[] = [];
  private players: Player[] = [];
  private status: Status;
  private localPlayerId: number;
  private localePlayerIsAdmin: boolean;
  private remainingTime: number;

  constructor(id: number, localPlayerId: number, isAdmin: boolean) {
    this.id = id;
    this.localPlayerId = localPlayerId;
    this.localePlayerIsAdmin = isAdmin;
  }

  public setPlayerIds = (ids: number[]) => {
    const idsToAdd = ids.filter(id => !this.playerIds.includes(id));
    const idsToRemove = ids.filter(id => this.playerIds.includes(id));

    idsToAdd.forEach(id => this.players.push(
      new Player(
        id,
        (id === this.localPlayerId),
        (id === this.localPlayerId ? this.localePlayerIsAdmin : false)
      )
    ));

    this.players.filter(player => !idsToRemove.includes(player.getId()));
      
    this.playerIds = ids;
  }

  public getStatus = () => this.status;

  public setStatus = (status: Status) => this.status = status;

  public setRemainingTime = (remainingTime: number) => this.remainingTime = remainingTime;

  public getLocalPlayerId = () => this.localPlayerId;

  public getLocalPlayer(): Player {
    for(const player of this.players) {
      if (player.getId() === this.localPlayerId) {
        return player;
      }
    }
    
    return null;
  }

  public start(): void {
    this.setStatus(Status.RUNNING);

    const localPlayer = this.getLocalPlayer();
    if (!localPlayer) return;
    const board: Board = localPlayer.getBoard();
    board.randomlyInsertNewTile();
    board.randomlyInsertNewTile();
    board.render($('#game'));
    localPlayer.listenForInputs();
  }

  public update() {
    const minutes = Math.floor(this.remainingTime / 60000);
    const seconds = ((this.remainingTime % 60000) / 1000).toFixed(0);
    const str = `${minutes}:${~~seconds < 10 ? '0' : ''}${seconds}`;

    if (!$('#remaining-time')) {
      HtmlHelper.span(str, {id: 'remaining-time'}, $('#game'));
    } else {
      $('#remaining-time').innerText = str;
    }
  }

  public end(): void {}

}
