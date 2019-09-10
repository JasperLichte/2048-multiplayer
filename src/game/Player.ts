import Board from "./Board.js";

export default class Player {

  private id: number;
  private board: Board;
  private score: number = 0;

  constructor(id: number) {
    this.id = id;
    this.board = new Board();
  }

  public getId = () => this.id;
  public getBoard = () => this.board;
  public getScore = () => this.score;

  public doMove(): void {}

}
