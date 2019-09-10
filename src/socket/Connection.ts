import config from '../config/config.js';

export default class Connection {

  private socket: WebSocket;

  constructor() {
    this.socket = new WebSocket(config['SERVER_BASE_PATH']());
  }

  public getSocket() {
    return this.socket;
  }

}
