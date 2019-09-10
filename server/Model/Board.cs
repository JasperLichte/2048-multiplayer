using System.Collections.Generic;

namespace server.model
{
    class Board
    {

        int width;
        int height;
        public List<List<Tile>> tiles { get; set; }
    }
}