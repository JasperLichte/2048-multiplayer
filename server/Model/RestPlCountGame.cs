namespace server.model
{
    public class RestPlayerCountGame
    {
        // These Variables are displayed in the browser
        public string name {get; set;}
        public long gameCount {get; set;}

        //Constructor, if the Method has the same Name like the class
        public RestPlayerCountGame(string name, int count)
        {
            this.name = name;
            this.gameCount = count;
        }

    }
}