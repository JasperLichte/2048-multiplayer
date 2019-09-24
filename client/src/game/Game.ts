import Player from "./Player.js";
import Status from "./Status.js";
import Board from "./Board.js";
import { $ } from "../helpers/DomHelper.js";

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
    board.render($('#app'));
    localPlayer.listenForInputs();
  }

  public update() {
    console.log(this.remainingTime);
  }

  public end(): void {}

}
