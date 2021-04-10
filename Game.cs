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

		ConsoleCanvas canvas;
		Stage stage;
		GameStatusManger gsm;
		private Player player;
		private int SenceWidth, SenceHeigth;
		private int gamePoint = 0;

		private bool isDeath = false;
		private bool isWin = false;

		public bool hasFailed() {return this.isDeath;}
		public bool hasWin() {return this.isWin;}

		// the draw buffer should be assign from canvas
		char[,] draw_buffer;
		ConsoleColor[,] color_buffer;
		public int getPoint() {return this.gamePoint;}


		public Game()
		{
			this.stage = StageLoader.LoadStageFromFile();
			SenceWidth = stage.h_size;
			SenceHeigth = stage.v_size;
			player = new Player();
			this.gsm = GameStatusManger.GetInstance();
			// gsm.Inital();
			this.gsm.stage = this.stage;

			// TODO: adopt the new draw system
			// new draw buffer
			canvas = new ConsoleCanvas(SenceWidth + 2, SenceHeigth+ 2);
			draw_buffer = canvas.GetBuffer();
			color_buffer = canvas.GetColorBuffer();
			
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

			//
			gsm.stage.AutoSettleElement();
			// move first, then check the new head position eating
			System.Threading.Thread.Sleep(100);
		}

		public void Restart()
		{
			//TODO
		}
		public void draw()
		{
			//TODO: 更换Canvas绘制系统
			// new canvas system
			canvas.ClearBuffer_DoubleBuffer();
			// draw area
			DrawMap();
			DrawPlayer();
			DrawAnimation();
			DrawUI();
			canvas.Refresh_DoubleBuffer();
		}

		// draw method
		public void DrawMap()
		{
			for (int r = 0; r < this.SenceHeigth; r++)
			{
				for (int c = 0; c < this.SenceWidth; c++)
				{
					var obj = stage.running_stage_map[r, c];
					if (!GameDef.GlobalData.output.ContainsKey(obj))
					{
						obj = GameDef.GameObj.Air;
					}
					draw_buffer[r,c] = GameDef.GlobalData.output[obj];
				}
			}
		}

		public void DrawPlayer()
		{
			draw_buffer[player.pos_v, player.pos_h] = GameDef.GlobalData.output[GameDef.GameObj.Player];
		}

		//TODO: UI组件？
		public void DrawUI()
		{

		}

		//TODO: draw animation
		public void DrawAnimation()
		{

		}

		private void AddPoint(GameDef.GameObj obj)
		{
			//TODO
		}

	}

	
}
