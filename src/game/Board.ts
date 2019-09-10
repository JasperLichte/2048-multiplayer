import Tile from "./Tile.js";
import HtmlHelper from "../helpers/HtmlHelper.js";
import { $ } from "../helpers/DomHelper.js";

export default class Board {

  private readonly width = 4;
  private readonly height = 4;
  private tiles: Tile[][] = [];

  constructor() {
    this.init();
  }

  private init() {
    for (let y = 0; y < this.height; y++) {
      this.tiles[y] = [];
      for (let x = 0; x < this.width; x++) {
        this.tiles[y][x] = new Tile();
      }
    }
  }

  public update(): void {}

  public render() {
    const $board = HtmlHelper.div(
      '',
      {
        'class': 'board',
        'id': 'local-player-board',
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

}
