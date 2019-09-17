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
        private Timer timer;

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
        }
        public static GameHandler getHandler()
        {
            return gameHandler;
        }
        public IResponse registerNewPlayer()
        {
            if (game.enterInGame())
            {
                Console.WriteLine($"Game is open for registration...registering player");
                Player player = new Player();
                if (game.registerPlayer(player))
                {
                    return new RegisterResponse(player, game.id);
                }
            }
            //mia logik wenn spiel beendet ist/neues spiel
            return new ErrorResponse();
        }
        public IResponse getUpdate()
        {
            return new UpdateResponse(game.players, 10L, game.status);
        }
        public Boolean startGame()
        {
            game.status = Status.RUNNING;
            //timer = new Timer(120000);
            timer = new Timer(5000);
            timer.Elapsed += stopGame;
            timer.AutoReset=false;
            timer.Start();
            Console.WriteLine("Started Timer");
            return true;
        }

        public void stopGame(Object source, ElapsedEventArgs e)
        {
            game.status = Status.FINISHED;
            OnTimerElapsed(EventArgs.Empty);
            Console.WriteLine("Stopped Timer");
        }
    }
}