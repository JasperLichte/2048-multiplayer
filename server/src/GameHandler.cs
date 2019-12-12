using System;
using System.Threading.Tasks;
using System.Timers;
using log4net;
using server.model;
using server.model.enums;
using server.model.interfaces;
using server.model.responses;

namespace server
{
    class GameHandler
    {
        public Game game;
        //https://stackoverflow.com/questions/2278525/system-timers-timer-how-to-get-the-time-remaining-until-elapse
        private Timer timer;
        private DateTime startTime;
        private Config config;

        private IDBQueryHelper dataStorage;

        private static GameHandler gameHandler = new GameHandler();
        public event EventHandler TimerElapsed;
        private static readonly ILog log = LogManager.GetLogger(typeof(GameHandler));
        protected virtual void OnTimerElapsed(EventArgs e)
        {
            EventHandler handler = TimerElapsed;
            handler?.Invoke(this, e);
        }

        private GameHandler()
        {
            this.config = Config.loadConfig();
            this.timer = new Timer(config.roundDuration);
            this.dataStorage = new DBQueryDummy();
            Game lastgame = dataStorage.loadLastGameData();
            this.game = new Game(lastgame == null ? 1 : lastgame.id + 1);
        }
        internal static GameHandler getHandler()
        {
            return gameHandler;
        }
        internal IResponse registerNewPlayer()
        {
            if (game.allowedToRegister())
            {
                return registerPlayer();
            }
            if (game.status == Status.FINISHED)
            {
                this.game = new Game(this.game.id + 1);
                return registerPlayer();
            }
            return new ErrorResponse("Game is not open for registration!");
        }

        private IResponse registerPlayer()
        {
           // Console.WriteLine($"Game is open for registration...registering player");
            long playerID = 0;
            if (game.lastPlayerID == 0)
            {
                Player lastPlayer = dataStorage.loadLastPlayerData();
                playerID = lastPlayer.id + 1;
            }
            else
            {
                playerID = game.lastPlayerID + 1;
            }
            Player player = new Player(config, playerID);
            if (game.registerPlayer(player))
            {
                return new RegisterResponse(player, game.id, config);
            }
            return new ErrorResponse("Player already registered");
        }
        internal IResponse getUpdate()
        {
            if (config.roundDuration - (long)(DateTime.Now - startTime).TotalMilliseconds < 0)
            {
                return new UpdateResponse(game.players, config.roundDuration, game.status);
            }
            else
            {
                return new UpdateResponse(game.players, config.roundDuration - (long)(DateTime.Now - startTime).TotalMilliseconds, game.status);
            }
        }

        internal void registerPlayerName(long playerID, string name)
        {
            Player player = game.players.Find(x =>
                 x.id == playerID
            );
            player.name = name;
        }

        internal Boolean startGame(long playerID)
        {

            Player player = game.players.Find(x =>
                x.id == playerID
           );
            if (!player.isAdmin)
            {
                return false;
            }
            game.status = Status.RUNNING;
            timer = new Timer(config.roundDuration);
            timer.Elapsed += stopGame;
            timer.AutoReset = false;
            timer.Start();
            startTime = DateTime.Now;
            Console.WriteLine("Started Timer");
            return true;
        }

        internal void stopGame(Object source, ElapsedEventArgs e)
        {
            game.close();
            Task.Run(() =>
            {
                this.dataStorage.storeData(this.game);
            });
            OnTimerElapsed(EventArgs.Empty);
            Console.WriteLine("Stopped Timer");
        }

        internal void unregisterPlayer(long playerID)
        {
            Player player = game.players.Find(x =>
                 x.id == playerID
            );

            if (player==null)
            {
                Console.WriteLine($"No player with ID {playerID} found");
                return;
            }
            Console.WriteLine($"Removing player {player.id} from the game");
            if (player.isAdmin)
            {
                Console.WriteLine("Player is admin, removing all players");
                BroadcastHandler broadcastHandler = BroadcastHandler.getBroadcastHandler();
                broadcastHandler.sendResponseToAll(new GameClosedResponse());
                game.close();
                timer.Stop();
            }
            game.players.Remove(player);
        }

        internal void updatePlayer(long playerID, long newScore, Board board)
        {
            if (this.game.status == Status.FINISHED)
            {
                return;
            }
            Player player = game.players.Find(x =>
                 x.id == playerID
            );
            player.score = newScore;
            player.board = board;
        }
        internal IResponse getPlayerBoard(long playerID)
        {


            return null;
        }
    }
}