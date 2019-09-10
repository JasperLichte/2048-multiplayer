import config from '../config/config.js';

export default class Connection {

  private socket: WebSocket;

  constructor() {
    try {
      this.socket = new WebSocket(config['SERVER_BASE_PATH']());
    } catch(e) {}
  }

  public getSocket() {
    return this.socket;
  }

}