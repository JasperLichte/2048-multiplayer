import Player from "./Player.js";
import Status from "./Status.js";
import Board from "./Board.js";

export default class Game {

  private id: number;
  private players: Player[] = [];
  private status: Status;
  public localPlayerId: number;

  constructor(
    id: number,
    playerIds: number[],
    localPlayerId: number,
    status: Status
  ) {
    this.id = id;
    this.localPlayerId = localPlayerId;
    this.status = status;

    playerIds.forEach(id => {
      this.players.push(new Player(id));
    });
  }

  public setStatus(status: Status) {
    this.status = status;
  };

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
    if (!localPlayer) {
      return;
    }
    const board: Board = localPlayer.getBoard();
    board.randomlyInsertNewTile();
    board.randomlyInsertNewTile();
    board.render();
    localPlayer.listenForInputs();
  }

  public end(): void {}

}
