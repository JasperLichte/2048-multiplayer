import Tile from "./Tile.js";
import HtmlHelper from "../helpers/HtmlHelper.js";
import { $ } from "../helpers/DomHelper.js";
import Direction from "./Direction.js";

export default class Board {

  private id: number;
  private readonly width = 4;
  private readonly height = 4;
  private tiles: Tile[][] = [];
  private isLocalPlayer: boolean = false;

  constructor(id: number, isLocalPlayer: boolean) {
    this.id = id,
    this.init();
  }

  private init() {
    for (let y = 0; y < this.height; y++) {
      this.tiles[y] = [];
      for (let x = 0; x < this.width; x++) {
        this.tiles[y][x] = new Tile(0);
      }
    }
  }

  public getTiles = () => this.tiles;
  public setTiles = (tiles: Tile[][]) => this.tiles = tiles;

  public render($target: HTMLElement) {
    const $wrapper = HtmlHelper.div(
      '',
      {'id': `board-${this.id} ${this.isLocalPlayer ? 'local' : ''}`}
    );
    const $score = HtmlHelper.h2('0', {'class': 'score'});
    const $board = HtmlHelper.div('', {'class': 'board'});
    
    for (const row of this.tiles) {
      const $row = HtmlHelper.div('', {'class': 'row'});
      for (const tile of row) {
        const value: number = tile.getValue();
        $row.appendChild(HtmlHelper.span(
          value.toString(),
          {
            'class': 'tile',
            'data-value': `v_${value.toString()}`,
          }
        ));
      }
      $board.appendChild($row);
    }

    $wrapper.appendChild($score);
    $wrapper.appendChild($board);
    $target.appendChild($wrapper);
  }

  public update(score: number): void {
    const $wrapper: HTMLElement = $(`#board-${this.id}`); 
    const $board: HTMLElement = $wrapper.querySelector('.board');
    const $score: HTMLElement = $wrapper.querySelector('.score');

    if (!$board || !$score) return;

    $score.textContent = `${score}`;

    this.tiles.forEach((row: Tile[], y) => {
      const $row = $board.querySelector(`.row:nth-of-type(${y + 1})`);
      if (!$row) return;
      row.forEach((tile: Tile, x) => {
        const $tile = $row.querySelector(`.tile:nth-of-type(${x + 1})`);
        if (!$tile) return;
        const value = tile.getValue();
        $tile.textContent = value.toString();
        $tile.setAttribute('data-value', `v_${value.toString()}`);
      });
    });
  }

  public randomlyInsertNewTile() {
    const options: Array<{x: number, y: number}> = [];
    for (let y = 0; y < this.tiles.length; y++) {
      for (let x = 0; x < this.tiles[y].length; x++) {
        if (!this.tiles[y][x].getValue()) {
          options.push({x, y});
        }
      }
    }
    if (!options.length) {
      return;
    }
    const spot = options[Math.floor(Math.random() * options.length)];
    this.tiles[spot.y][spot.x] = new Tile(Math.random() > 0.5 ? 2 : 4);
  }

  public static shift(direction: Direction) {
    return (tiles: Tile[][]): Tile[][] => {
      switch (direction) {
        case Direction.Up:
          return Board.shiftToTop(tiles);
        case Direction.Right:
          return Board.shiftToRight(tiles);
        case Direction.Down:
          return Board.shiftToBottom(tiles);
        case Direction.Left:
          return Board.shiftToLeft(tiles);
      }
      return [];
    };
  }

  private static shiftToRight(tiles: Tile[][]): Tile[][]{
    const newTiles: Tile[][] = [];
    for (const row of tiles) {
      const newRow: Tile[] = (row => {
        const length = row.length;
        row = row.filter(tile => !!tile.getValue());
        const emptyTiles: Tile[] = Array(length - row.length).fill(new Tile(0)); 
        return [...emptyTiles, ...row];
      })(row);
      newTiles.push(newRow);
    }
    return newTiles;
  }

  private static shiftToLeft(tiles: Tile[][]): Tile[][] {
    const newTiles: Tile[][] = [];
    for (const row of tiles) {
      const newRow: Tile[] = (row => {
        const length = row.length;
        row = row.filter(tile => !!tile.getValue());
        const emptyTiles: Tile[] = Array(length - row.length).fill(new Tile(0)); 
        return [...emptyTiles, ...row];
      })(row.reverse());
      newTiles.push(newRow.reverse());
    }
    return newTiles;
  }

  private static shiftToTop(tiles: Tile[][]): Tile[][] {
    tiles = Board.rotate(tiles);
    tiles = Board.shiftToLeft(tiles);
    tiles = Board.unrotate(tiles);
    return tiles;
  }

  private static shiftToBottom(tiles: Tile[][]): Tile[][] {
    tiles = Board.rotate(tiles);
    tiles = Board.shiftToRight(tiles);
    tiles = Board.unrotate(tiles);
    return tiles;
  }

  private static rotate(tiles: Tile[][]): Tile[][] {
    return tiles[0].map((col, i) => tiles.map(row => row[i]));
  }

  private static unrotate(tiles: Tile[][]): Tile[][] {
    tiles = Board.rotate(tiles);
    tiles = Board.rotate(tiles);
    tiles = Board.rotate(tiles);
    return tiles;
  }

  public static combine(direction: Direction) {
    return (tiles: Tile[][]): [Tile[][], number] => {
      switch (direction) {
        case Direction.Up:
          return Board.combineToTop(tiles);
        case Direction.Right:
          return Board.combineToRight(tiles);
        case Direction.Down:
          return Board.combineToBottom(tiles);
        case Direction.Left:
          return Board.combineToLeft(tiles);
      }
      return [[], 0];
    };
  }

  private static combineToTop(tiles: Tile[][]): [Tile[][], number] {
    let points = 0;
    tiles = Board.rotate(tiles);
    [tiles, points] = Board.combineToLeft(tiles);
    tiles = Board.unrotate(tiles);
    return [tiles, points];
  }

  private static combineToRight(tiles: Tile[][]): [Tile[][], number] {
    const newTiles = [];
    let points = 0;
    for (const row of tiles) {
      for (let i = row.length - 1; i > 0; i--) {
        if (row[i].getValue() === row[i - 1].getValue()) {
          const newValue = row[i].getValue() + row[i - 1].getValue();
          points += newValue;
          row[i] = new Tile(newValue);
          row[i - 1] = new Tile(0);
        }
      }
      newTiles.push(row);
    }
    return [newTiles, points];
  }

  private static combineToBottom(tiles: Tile[][]): [Tile[][], number] {
    let points = 0;
    tiles = Board.rotate(tiles);
    [tiles, points] = Board.combineToRight(tiles);
    tiles = Board.unrotate(tiles);
    return [tiles, points];
  }

  private static combineToLeft(tiles: Tile[][]): [Tile[][], number] {
    const newTiles = [];
    let points = 0;
    for (let row of tiles) {
      row = row.reverse();
      for (let i = row.length - 1; i > 0; i--) {
        if (row[i].getValue() === row[i - 1].getValue()) {
          const newValue = row[i].getValue() + row[i - 1].getValue();
          row[i] = new Tile(newValue);
          points += newValue;
          row[i - 1] = new Tile(0);
        }
      }
      newTiles.push(row.reverse());
    }
    return [newTiles, points];
  }

  public isEqualTo(other: Tile[][]): boolean {
    for (let y = 0; y < other.length; y++) {
      for (let x = 0; y < other[y].length; x++) {
        if (other[y][x] !== this.tiles[y][x]) {
          return false;
        }
      }
    }
    return true;
  }

}
