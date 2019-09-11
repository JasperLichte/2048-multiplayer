import Tile from "./Tile.js";
import HtmlHelper from "../helpers/HtmlHelper.js";
import { $ } from "../helpers/DomHelper.js";

export default class Board {

  private id: number;
  private readonly width = 4;
  private readonly height = 4;
  private tiles: Tile[][] = [];

  constructor(id: number) {
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

  public update(): void {
    const $board: HTMLElement = $(`#board-${this.id}`);
    if (!$board) return;

    this.tiles.forEach((row: Tile[], y) => {
      const $row = $board.querySelector(`.row:nth-of-type(${y + 1})`);
      if (!$row) return;
      row.forEach((tile: Tile, x) => {
        const $tile = $row.querySelector(`.tile:nth-of-type(${x + 1})`);
        if (!$tile) return;
        const value = tile.getValue();
        $tile.innerHTML = value.toString();
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

  public render() {
    const $board = HtmlHelper.div(
      '',
      {
        'class': 'board',
        'id': `board-${this.id}`,
      }
    );
    
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

    $('#app').appendChild($board);
  }

  public static shiftToRight(tiles: Tile[][]): [Tile[][], boolean] {
    return [null, false];
  }

  public getTiles = () => this.tiles;
  public setTiles = (tiles: Tile[][]) => this.tiles = tiles;

}
