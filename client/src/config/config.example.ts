export default {
  SERVER_PORT: 5000,
  SERVER_BASE_PATH: function() {return `localhost:${this.SERVER_PORT}/server`},
  WEBSOCKET_URL: function() {return `ws://${this.SERVER_BASE_PATH()}`},
  DEBUG_OUTPUTS: {
    IN_SOCKET: true,
    OUT_SOCKET: true,
  }
};
