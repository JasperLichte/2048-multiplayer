import Player from "./Player.js";
import Status from "./Status.js";

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

    const localPlayer = this.getLocalPlayer();
    localPlayer && localPlayer.getBoard().render();
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

  public start(): void {}

  public end(): void {}

}
