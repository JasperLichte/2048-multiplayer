using System.Collections.Generic;

namespace server.model
{
    public class Board
    {
        public List<List<Tile>> tiles { get; set; }

        internal Board()
        {
        }

        internal Board fill(Config config)
        {
            tiles = new List<List<Tile>>();

            for (int i = 0; i < config.boardSize; i++)
            {
                List<Tile> a = new List<Tile>();
                for (int j = 0; j < config.boardSize; j++)
                {
                    a.Add(new Tile());
                }
                tiles.Add(a);
            }
            return this;
        }
    }
}