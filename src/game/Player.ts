import Board from "./Board.js";
import Tile from "./Tile.js";

export default class Player {

  private id: number;
  private board: Board;
  private score: number = 0;

  constructor(id: number) {
    this.id = id;
    this.board = new Board(this.id);
  }

  public getId = () => this.id;
  public getBoard = () => this.board;
  public getScore = () => this.score;

  public listenForInputs() {
    window.addEventListener('keydown', e => {
      const direction = ((key: string): string => {
        switch (e.key) {
          case 'ArrowUp': return 'UP';
          case 'ArrowRight': return 'RIGHT';
          case 'ArrowDown': return 'DOWN';
          case 'ArrowLeft': return 'LEFT';
        }
        return '';
      })(e.key);

      direction && this.doMove(direction);
    });
  }

  private doMove(direction: string): void {
    console.log(direction);
    let boardhasChanged = false;
    let newBoard: Tile[][];
    switch (direction) {
      case 'RIGHT':
        [newBoard, boardhasChanged] = Board.shiftToRight(this.board.getTiles());
        break;
    }

    if (boardhasChanged) {
      //this.board.setTiles(newBoard.getTiles());
      this.board.randomlyInsertNewTile();
      this.board.update();
    }
  }

}
