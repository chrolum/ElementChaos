
using System;
using System.Collections.Generic;
using System.Text;

namespace ElementChaos
{
    class Player
	{

		GameStatusManger gsm;
		public int pos_v {get;set;}
		public int pos_h {get;set;}

		public List<ElementBase> liftUpELemList;
		public Player(int _h = 7, int _v = 7)
		{
			this.pos_v = _v;
			this.pos_h = _h;
			this.liftUpELemList = new List<ElementBase>();
			this.gsm = GameStatusManger.GetInstance();
		}

		/*
		* Update snake linked list position record
		* No vaild move check!
		* No tail adding logic
		*/
		public void move(GameDef.Action d)
		{
			int dh = 0, dv = 0;


			switch (d)
			{
				case GameDef.Action.left:
					dh = -1;
					break;
				case GameDef.Action.Right:
					dh = 1;
					break;
				case GameDef.Action.Up:
					dv = -1;
					break;
				case GameDef.Action.Down:
					dv = 1;
					break;
				default:
					dv = 0;
					dh = 0;
					break;
			}

			if (!gsm.canMoveTo(this.pos_v + dv, this.pos_h + dh))
				return;
			
			this.pos_v += dv;
			this.pos_h += dh;
		}
	
		public void LiftUpElement()
		{
			//TODO
			var gsm = GameStatusManger.GetInstance();

			if (!Tools.isElment(gsm.stage.running_stage_map[pos_v, pos_h]))
			{
				return;
			}

			this.liftUpELemList.Add(gsm.stage.elements_map[pos_v, pos_h]);
			gsm.stage.running_stage_map[pos_v, pos_h] = GameDef.GameObj.Air;
		}

	}
}
