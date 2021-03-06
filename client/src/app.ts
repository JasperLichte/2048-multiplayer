import { $ } from "./helpers/DomHelper.js";
import MessageHandler from "./socket/MessageHandler.js";
import RequestTypes from "./socket/RequestTypes.js";
import Config from "./game/Config.js";

(() => {
  console.info(`Version: ${Config.APP_VERSION}`);

  const $welcomeCard = $('#welcome-card');
  const $name: HTMLInputElement = $welcomeCard.querySelector('#name');
  const $start: HTMLButtonElement = $welcomeCard.querySelector('#start'); 
  $start && $start.addEventListener('click', () => {
    const name: string = $name.value;
    if (!name) return;
    MessageHandler.send(RequestTypes.REGISTER_PLAYER, {name});
    MessageHandler.send(RequestTypes.GAME_START);
    $('#spinner').classList.remove('hidden');
  });

  window.addEventListener('beforeunload', () => {
    MessageHandler.send(RequestTypes.UNREGISTER);
  });

})();
