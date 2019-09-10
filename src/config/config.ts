export default {
  SERVER_PORT: '5000',
  SERVER_BASE_PATH: () => `localhost:${this.SERVER_PORT}/server`,
  WEBSOCKET_URL: () => `ws://${this.SERVER_BASE_PATH()}`,
};
