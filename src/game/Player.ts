import Board from "./Board.js";
import Tile from "./Tile.js";
import Direction from "./Direction.js";

export default class Player {

  private id: number;
  private board: Board;
  private score: number = 0;
  private isLocalPlayer: boolean = false;

  constructor(id: number, isLocalPlayer: boolean) {
    this.id = id;
    this.isLocalPlayer = isLocalPlayer;
    this.board = new Board(this.id, this.isLocalPlayer);
  }

  public getId = () => this.id;
  public getBoard = () => this.board;
  public getScore = () => this.score;
  private addScore = (score: number) => this.score += score;

  public listenForInputs() {
    window.addEventListener('keydown', e => {
      const direction = ((key: string): Direction => {
        switch (key) {
          case 'ArrowUp': return Direction.Up;
          case 'ArrowRight': return Direction.Right;
          case 'ArrowDown': return Direction.Down;
          case 'ArrowLeft': return Direction.Left;
        }
        return null;
      })(e.key);

      direction && this.doMove(direction);
    });
  }

  private doMove(direction: Direction): void {
    const shift = Board.shift(direction);

    let pointsEarned = 0;
    let newTiles: Tile[][] = shift(this.board.getTiles());
    [ newTiles, pointsEarned ] = Board.combine(direction)(newTiles);
    newTiles = shift(newTiles);

    if (!this.board.isEqualTo(newTiles)) {
      this.board.setTiles(newTiles);
      this.addScore(pointsEarned);
      this.board.randomlyInsertNewTile();
      this.board.update(this.getScore());
    }
  }

}
