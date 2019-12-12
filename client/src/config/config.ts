export default {
  SERVER_PORT: 5000,
  SERVER_BASE_PATH: function() {return `192.168.188.23:${this.SERVER_PORT}/server`},
  WEBSOCKET_URL: function() {return `ws://${this.SERVER_BASE_PATH()}`},
  DEBUG_OUTPUTS: {
    IN_SOCKET: false,
    OUT_SOCKET: false,
  }
};
// 7286293135070148
