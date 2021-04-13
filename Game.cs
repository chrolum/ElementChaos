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

		ConsoleCanvas stagecanvas;
		ConsoleCanvas gameMsgCanvas;
		ConsoleCanvas playerStatusUICanvas;
		public Stage stage;
		GameStatusManger gsm;
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

		public void selectStage()
		{
			//TODO
			this.stage = StageManager.GetStageByIdx(0);
			this.stage.reload();
			this.gsm.stage = this.stage;
		}
		public Game(int scence_h_size = 55, int scence_v_size = 35)
		{
			SenceWidth = scence_h_size;
			SenceHeigth = scence_v_size;
			StageManager.LoadAllStage();
			this.stage = StageManager.GetStageByIdx(0);
			this.gsm = GameStatusManger.GetInstance();
			// gsm.Inital();
			this.gsm.stage = this.stage;

			// TODO: adopt the new draw system
			// new draw buffer
			stagecanvas = new ConsoleCanvas(SenceWidth, SenceHeigth, anchor_h:2, anchor_v:2);
			gameMsgCanvas = new ConsoleCanvas(SenceWidth, 10, anchor_v: SenceHeigth + 2, anchor_h: 2);
			playerStatusUICanvas = new ConsoleCanvas(25, SenceHeigth, anchor_v: 2, anchor_h: 2*(SenceWidth+1) + 2);


			draw_buffer = stagecanvas.GetBuffer();
			color_buffer = stagecanvas.GetColorBuffer();
			stagecanvas.drawEdge();
			gameMsgCanvas.drawEdge();
			playerStatusUICanvas.drawEdge();

			BulletManager.gsm = this.gsm;
			
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
					stage.player.move(action);
					break;
				case GameDef.Action.RESTART:
					Restart();
					return;
				case GameDef.Action.LiftUp:
					stage.player.LiftUpElement();
					break;
				case GameDef.Action.PutDown:
					stage.player.PushDownElement();
					break;
				case GameDef.Action.UseSkill:
					stage.player.UseSkill();
					break;
				case GameDef.Action.EatElement:
					stage.player.EatElement();
					break;
				default:
					break;
			}

			//
			gsm.stage.AutoSettleElement();
			// move first, then check the new head position eating
			BulletManager.checkAllBullet();
			System.Threading.Thread.Sleep(50);
		}

		public void Restart()
		{
			this.stage.reload();
		}
		public void draw()
		{
			//TODO: 更换Canvas绘制系统
			// new canvas system
			stagecanvas.ClearBuffer_DoubleBuffer();
			gameMsgCanvas.ClearBuffer_DoubleBuffer();
			playerStatusUICanvas.ClearBuffer_DoubleBuffer();
			// draw area
			DrawMap();
			DrawAnimation();
			DrawUI();
			DrawPlayer();
			stagecanvas.Refresh_DoubleBuffer();
			gameMsgCanvas.Refresh_DoubleBuffer();
			playerStatusUICanvas.Refresh_DoubleBuffer();
		}

		// draw method
		public void DrawMap()
		{
			for (int r = 0; r < this.stage.v_size; r++)
			{
				for (int c = 0; c < this.stage.h_size; c++)
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
			draw_buffer[stage.player.pos_v, stage.player.pos_h] = GameDef.GlobalData.output[GameDef.GameObj.Player];
		}

		//TODO: UI组件？
		public void DrawUI()
		{

		}

		//TODO: draw animation
		public void DrawAnimation()
		{
			//Bullet
			foreach (var b in BulletManager.activeBulletList)
			{
				draw_buffer[b.pos_v, b.pos_h] = 'o';
			}
		}

		private void AddPoint(GameDef.GameObj obj)
		{
			//TODO
		}

	}

	
}
