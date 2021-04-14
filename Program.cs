using System;
using System.Diagnostics;


namespace ElementChaos
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.SetWindowSize(Console.LargestWindowWidth-25, Console.LargestWindowHeight-15);
            // Console.OutputEncoding = System.Text.Encoding.UTF8;
            Game newGame = new Game();
            //select stage
            SELECTSTAGE:
            if (StageManager.currStageIdx == StageManager.aviableStageList.Count)
            {
                newGame.gameMsgUI.reset();
                newGame.gameMsgUI.publishMsg("你已通关全部关卡");
                newGame.gameMsgUI.publishMsg("To be Continue");
                newGame.gameMsgUI.publishMsg("按任意键重玩");
                StageManager.currStageIdx = 0;
            }
            newGame.selectStage();
            // stage main loop
            while(true)
            {
                if (newGame.stage.checkWin())
                {
                    //TODO Win CG
                    Debug.WriteLine("Game: stage win");
                    newGame.gameMsgUI.publishMsg("恭喜通关！按任意键进入下一关");
                    //立即绘制一次UI
                    newGame.gameMsgUI.Draw();
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
