using System;
using System.Diagnostics;

namespace ElementChaos
{
    class Program
    {
        static void Main(string[] args)
        {
            Game newGame = new Game();
            //select stage
            SELECTSTAGE:
            newGame.selectStage();
            // stage main loop
            while(true)
            {
                if (newGame.stage.checkWin())
                {
                    //TODO Win CG
                    Debug.WriteLine("Game: stage win");
                    Console.ReadKey();
                    goto SELECTSTAGE;
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
