using System.Collections.Generic;

namespace server.model
{
    public class Board
    {

        int width;
        int height;
        public List<List<Tile>> tiles { get; set; }
    }
}