export default class Tile {

  private value: number = 0;

  constructor(initialValue: number = 0) {
    this.value = initialValue;
  }

  public getValue = () => this.value;

}
