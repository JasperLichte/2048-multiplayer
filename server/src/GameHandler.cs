using System;
using server.model;
using server.model.responses;

namespace server
{
    class GameHandler
    {
        private Game game;
        private static GameHandler gameHandler = new GameHandler();


        private GameHandler(){
            this.game=new Game();
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
    }
}