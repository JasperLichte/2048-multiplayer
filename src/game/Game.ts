import Player from "./Player.js";
import Status from "./Status.js";
import Board from "./Board.js";
import { $ } from "../helpers/DomHelper.js";

export default class Game {

  private id: number;
  private playerIds: number[] = [];
  private players: Player[] = [];
  private status: Status;
  public localPlayerId: number;

  constructor(id: number, localPlayerId: number) {
    this.id = id;
    this.localPlayerId = localPlayerId;
  }

  public setPlayerIds = (ids: number[]) => {
    ids
      .filter(id => !this.playerIds.includes(id))
      .forEach(id => this.players.push(new Player(id, id === this.localPlayerId)));
      
    this.playerIds = ids;
  }

  public getStatus = () => this.status;

  public setStatus = (status: Status) => this.status = status;

  private getLocalPlayer(): Player {
    for(const player of this.players) {
      if (player.getId() === this.localPlayerId) {
        return player;
      }
    }
    
    return null;
  }

  public start(): void {
    const localPlayer = this.getLocalPlayer();
    if (!localPlayer) return;
    const board: Board = localPlayer.getBoard();
    board.randomlyInsertNewTile();
    board.randomlyInsertNewTile();
    board.render($('#app'));
    localPlayer.listenForInputs();
  }

  public end(): void {}

}
