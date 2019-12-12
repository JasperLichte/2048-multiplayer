import Player from "../game/Player.js";
import HtmlHelper from "./HtmlHelper.js";
import { $ } from './DomHelper.js';
import Globals from "../Globals.js";

export default class RenderHelper {
  public static connectedPlayers(players: Player[]) {
    players = players.filter(p => p.getId() !== Globals.game.getLocalPlayerId());
    const registeredPlayers = players.filter(p => !!p.getName());
    const connectedPlayers = players.filter(p => !p.getName()).length;

    $('#connected-players') && $('#connected-players').remove();
    HtmlHelper.div(
      [
        HtmlHelper.ul(
          registeredPlayers.map(p => HtmlHelper.li(
            p.getName() + (p.isAdmin ? ' [Admin]' : '')
          ))
        ),
        HtmlHelper.p(
          (connectedPlayers > 0 ? `${connectedPlayers.toString()} more connected` : ''),
          {class: connectedPlayers == 0 ? 'hidden' : ''}
        )
      ],
      {id: 'connected-players'},
      $('#welcome-card')
    );
  }
}
