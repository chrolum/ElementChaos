using System;

namespace ElementChaos
{
    class Program
    {
        static void Main(string[] args)
        {
            Game newGame = new Game(30, 40);
            while(true)
            {
                if (newGame.hasWin())
                {
                    
                }
                else if (newGame.hasFailed())
                {

                }
                var action = newGame.GetUserAction();
                newGame.Update(action);
                newGame.draw();
            }
        }
    }
}
