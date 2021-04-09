using System;
using System.Collections.Generic;
using System.Text;

/*
 * orignal postion start from left-up coner
 * 
 */

namespace ElementChaos
{
	
	class Game
	{
		// store scence data

		Stage stage;
		GameStatusManger gsm;
		private int SenceWidth, SenceHeigth;
		private Player player;
		private int gamePoint = 0;

		private bool isDeath = false;
		private bool isWin = false;

		public bool hasFailed() {return this.isDeath;}
		public bool hasWin() {return this.isWin;}

		GameDef.GameObj[,] running_map;
		char[,] draw_map;
		public int getPoint() {return this.gamePoint;}


		public Game(int heigth, int width, int stage_no = 1)
		{
			SenceWidth = width;
			SenceHeigth = heigth;
			player = new Player();
			this.stage = StageLoader.LoadStageFromFile();
			this.gsm = GameStatusManger.GetInstance();
			// gsm.Inital();
			this.gsm.stage = this.stage;

			this.draw_map = new char[heigth, width];
			
		}
		public GameDef.Action GetUserAction()
		{
			if (Console.KeyAvailable)  
    		{  
        		ConsoleKeyInfo key = Console.ReadKey(true);  
				var k = key.Key;
				if (!GameDef.GlobalData.ActionMap.ContainsKey(k))
					return GameDef.Action.NOACTION;
				return GameDef.GlobalData.ActionMap[k];
			}
			return GameDef.Action.NOACTION;
		}

		public void Update(GameDef.Action action)
		{
			switch (action)
			{
				case GameDef.Action.left:
				case GameDef.Action.Right:
				case GameDef.Action.Down:
				case GameDef.Action.Up:
					player.move(action);
					break;
				case GameDef.Action.RESTART:
					Restart();
					return;
				case GameDef.Action.LiftUp:
					player.LiftUpElement();
					break;
				default:
					break;
			}
			// move first, then check the new head position eating
			System.Threading.Thread.Sleep(150);
		}

		public void Restart()
		{
			//TODO
		}
		public void draw()
		{
			//TODO: 更换Canvas绘制系统
			//TODO: UI组件？
			//draw fix item
			// Console.Clear();

			// print map first
			for (int r = 0; r < this.SenceHeigth; r++)
			{
				for (int c = 0; c < this.SenceWidth; c++)
				{
					draw_map[r,c] = GameDef.GlobalData.output[stage.running_stage_map[r, c]];
				}
			}

			// print player

			//print Animation

			Console.SetCursorPosition(0, 0);
			for (int r = 0; r < this.SenceHeigth; r++)
			{
				for (int c = 0; c < this.SenceWidth; c++)
				{
					Console.SetCursorPosition(c, r);
					//TODO: 用stringbuilder拼接成完整的字符串后再打印
					Console.Write("{0}", draw_map[r, c]);
				}
				Console.WriteLine();
			}

			//TODO: UI System
		}

		

	

		private void AddPoint(GameDef.GameObj obj)
		{
			//TODO
		}

	}

	
}
