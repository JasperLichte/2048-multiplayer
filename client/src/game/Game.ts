import Player from "./Player.js";
import Status from "./Status.js";
import Board from "./Board.js";
import { $ } from "../helpers/DomHelper.js";
import HtmlHelper from "../helpers/HtmlHelper.js";
import Tile from "./Tile.js";
import MessageHandler from "../socket/MessageHandler.js";
import RequestTypes from "../socket/RequestTypes.js";

export default class Game {

  private id: number;
  private playerIds: number[] = [];
  private players: Player[] = [];
  private status: Status;
  private localPlayerId: number;
  private localePlayerIsAdmin: boolean;
  private remainingTime: number;
  private otherPlayers: Player[] = [];

  constructor(id: number, localPlayerId: number, isAdmin: boolean) {
    this.id = id;
    this.localPlayerId = localPlayerId;
    this.localePlayerIsAdmin = isAdmin;
  }

  public setPlayerIds = (ids: number[]) => {
    const idsToAdd = ids.filter(id => !this.playerIds.includes(id));
    const idsToRemove = ids.filter(id => this.playerIds.includes(id));

    idsToAdd.forEach(id => this.players.push(
      new Player(
        id,
        (id === this.localPlayerId),
        (id === this.localPlayerId ? this.localePlayerIsAdmin : false)
      )
    ));

    this.players.filter(player => !idsToRemove.includes(player.getId()));
      
    this.playerIds = ids;
  }

  public getStatus = () => this.status;
  public setStatus = (status: Status) => this.status = status;
  public setRemainingTime = (remainingTime: number) => this.remainingTime = remainingTime;
  public getLocalPlayerId = () => this.localPlayerId;
  public getPlayers = () => this.players;
  public setOtherPlayers = (players: Player[]) => this.otherPlayers = players;
  public getOtherPlayers = () => this.otherPlayers;

  public getLocalPlayer(): Player {
    for(const player of this.players) {
      if (player.getId() === this.localPlayerId) {
        return player;
      }
    }
    
    return null;
  }

  public start(): void {
    this.setStatus(Status.RUNNING);

    const localPlayer = this.getLocalPlayer();
    if (!localPlayer) return;
    const board: Board = localPlayer.getBoard();
    board.randomlyInsertNewTile();
    board.randomlyInsertNewTile();
    board.render($('#local-player-board-wrapper'));
    localPlayer.listenForInputs();
    MessageHandler.send(
      RequestTypes.DO_PLAYER_UPDATE,
      {
        newScore: localPlayer.getScore(),
        board: {tiles: localPlayer.getBoard().getTiles()}
      }
    );
  }

  public update() {
    const minutes = Math.floor(this.remainingTime / 60000);
    const seconds = ((this.remainingTime % 60000) / 1000).toFixed(0);
    const str = `${minutes}:${~~seconds < 10 ? '0' : ''}${seconds}`;

    if (!$('#remaining-time')) {
      HtmlHelper.span(str, {id: 'remaining-time'}, $('#game'));
    } else {
      $('#remaining-time').innerText = str;
    }

    if (this.status === Status.RUNNING) {
      this.renderOtherPlayerBoards();
    }
  }

  public end(): void {}

  private renderOtherPlayerBoards(): void {
    for (const player of this.getOtherPlayers()) {
      if (!$(`#game #board-${player.getId()}`)) {
        this.$createPlayerBoard(player);
      }
      this.$updatePlayerBoard(player);
    }
  }

  private $createPlayerBoard(player: Player) {
    const $board = document.createElement('div');

    $board.setAttribute('id', `board-${player.getId()}`);
    $board.setAttribute('class', 'remote');

    const $score = document.createElement('h2');
    $score.innerText = player.getScore().toString();
    $score.setAttribute('class', 'score');
    $board.appendChild($score);

    const $tiles = document.createElement('div');
    for(let y = 0; y < player.getBoard().getTiles().length; y++) {
      const row = player.getBoard().getTiles()[y];
      const $row = document.createElement('div');
      $row.setAttribute('class', 'row');
      for (let x = 0; x < row.length; x++) {
        const cell: Tile = row[x];
        const $cell = document.createElement('span');
        $cell.setAttribute('class', 'tile');
        $cell.setAttribute('data-value', `v_${cell.getValue()}`);
        $cell.innerHTML = cell.getValue().toString();
        $row.appendChild($cell);
      }
      $tiles.appendChild($row);
    }
    $tiles.setAttribute('class', 'board');

    $board.appendChild($tiles);

    $('#remote-player-board-wrapper').appendChild($board);
  }

  private $updatePlayerBoard(player: Player) {
    let $board = $(`#board-${player.getId()}`);

    $board.querySelector('.score').innerHTML = player.getScore().toString();

    $board = $board.querySelector('.board');

    for (let y = 0; y < player.getBoard().getTiles().length; y++) {
      const row = player.getBoard().getTiles()[y];
      const $row = $board.querySelector(`.row:nth-of-type(${y + 1})`);
      for (let x = 0; x < row.length; x++) {
        const cell = row[x];
        const $cell = $row.querySelector(`.tile:nth-of-type(${x + 1})`);
        $cell.innerHTML = cell.getValue().toString();
        $cell.setAttribute('data-value', `v_${cell.getValue()}`);
      }
    }
  }

  public $renderScoreboard() {
    const $rows: HTMLElement[] = [HtmlHelper.tr([
      HtmlHelper.th('Platz'),
      HtmlHelper.th('Name'),
      HtmlHelper.th('Score'),
    ])];

    this.players
    .sort((a, b) => b.getScore() - a.getScore())
    .forEach((p, i) => $rows.push(HtmlHelper.tr([
      HtmlHelper.td((i + 1).toString()),
      HtmlHelper.td(p.getName()),
      HtmlHelper.td(p.getScore().toString()),
    ])));

    HtmlHelper.div(
      HtmlHelper.div(HtmlHelper.table($rows), {class: 'dialog'}),
      {id: 'score-board-wrapper'},
      $('#game')
    );
  }

}
