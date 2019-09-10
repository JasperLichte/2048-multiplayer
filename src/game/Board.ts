import Tile from "./Tile.js";

export default class Board {

  private readonly width = 4;
  private readonly height = 4;
  private tiles: Tile[][] = [];

  constructor() {
    this.init();
    console.log(this);
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

}
