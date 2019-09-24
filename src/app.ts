import Connection from "./socket/Connection.js";
import { $ } from "./helpers/DomHelper.js";
import Globals from "./Globals.js";
import MessageHandler from "./socket/MessageHandler.js";
import RequestTypes from "./socket/RequestTypes.js";

Globals.connection = new Connection();

(() => {
  const $welcomeCard = $('#welcome-card');
  const $name: HTMLInputElement = $welcomeCard.querySelector('#name');
  const $start: HTMLButtonElement = $welcomeCard.querySelector('#start'); 
  $start && $start.addEventListener('click', () => {
    const name: string = $name.value;
    if (!name) return;
    MessageHandler.send(RequestTypes.GAME_START);
    $('#spinner').classList.remove('hidden');
  });
})();
