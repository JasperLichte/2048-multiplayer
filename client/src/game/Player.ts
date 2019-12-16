import Board from "./Board.js";
import Tile from "./Tile.js";
import Direction from "./Direction.js";
import MessageHandler from "../socket/MessageHandler.js";
import RequestTypes from "../socket/RequestTypes.js";
import Globals from "../Globals.js";
import Status from "./Status.js";

export default class Player {

  private id: number;
  private board: Board;
  private score: number = 0;
  public isLocalPlayer: boolean = false;
  public isAdmin: boolean = false;
  private name: string = '';

  constructor(id: number, isLocalPlayer: boolean, isAdmin: boolean) {
    this.id = id;
    this.isLocalPlayer = isLocalPlayer;
    this.isAdmin = isAdmin;
    this.board = new Board(this.id, this.isLocalPlayer);
  }

  public getId = () => this.id;
  public getBoard = () => this.board;
  public getScore = () => this.score;
  public setScore = (score: number) => this.score = score;
  private addScore = (score: number) => this.score += score;
  public setTiles = (tiles: Tile[][]) => this.board.setTiles(tiles);
  public getName = () => this.name;
  public setName = (name: string) => this.name = name;

  public listenForInputs() {
    if (!this.isLocalPlayer) return;

    window.addEventListener('keydown', e => {
      if (Globals.game.getStatus() !== Status.RUNNING) {
        return;
      }
      
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
    if (this.board.isFull()) {
      return;
    }

    const shift = Board.shift(direction);

    let pointsEarned = 0;
    let newTiles = shift(this.board.getTiles());
    [ newTiles, pointsEarned ] = Board.combine(direction)(newTiles);
    newTiles = shift(newTiles);

    if (!this.board.isEqualTo(newTiles)) {
      this.board.setTiles(newTiles);
      this.addScore(pointsEarned);
      this.board.randomlyInsertNewTile();
      this.board.update(this.getScore());
      
      MessageHandler.send(
        RequestTypes.DO_PLAYER_UPDATE,
        {
          newScore: this.getScore(),
          board: {tiles: this.board.getTiles()}
        }
      );
    }
  }

}
