using System;
using System.Timers;
using server.model;
using server.model.enums;
using server.model.interfaces;
using server.model.responses;

namespace server
{
    class GameHandler
    {
        private Game game;
        //https://stackoverflow.com/questions/2278525/system-timers-timer-how-to-get-the-time-remaining-until-elapse
        private Timer timer;
        private DateTime startTime;
        private Config config;

        private static GameHandler gameHandler = new GameHandler();
        public event EventHandler TimerElapsed;
        protected virtual void OnTimerElapsed(EventArgs e)
        {
            EventHandler handler = TimerElapsed;
            handler?.Invoke(this, e);
        }

        private GameHandler()
        {
            this.game = new Game();
            this.config = Config.loadConfig();
            this.timer=new Timer(config.roundDuration);
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
                this.game = new Game();
                return registerPlayer();
            }
            return new ErrorResponse("Game is not open for registration!");
        }

        private IResponse registerPlayer()
        {
            Console.WriteLine($"Game is open for registration...registering player");
            Player player = new Player();
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
            OnTimerElapsed(EventArgs.Empty);
            Console.WriteLine("Stopped Timer");
        }

        internal void unregisterPlayer(long playerID)
        {
            Player player = game.players.Find(x =>
                 x.id == playerID
            );
            Console.WriteLine($"Removing player {player.id} from the game");
            if (player.isAdmin)
            {
                Console.WriteLine("Player is admin, removing all players");
                BroadcastHandler broadcastHandler = BroadcastHandler.getBroadcastHandler();
                broadcastHandler.sendResponseToAll(new GameClosedResponse());
                game.close();
                timer.Stop();
                return;
            }
            game.players.Remove(player);
        }

        internal void updatePlayer(long playerID, long newScore, Board board)
        {
            if (this.game.status== Status.FINISHED)
            {
                return;
            }
            Player player = game.players.Find(x =>
                 x.id == playerID
            );
            player.score = newScore;
            player.board = board;
        }
        internal IResponse getPlayerBoard(long playerID){
            

            return null;
        }
    }
}