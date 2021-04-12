
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace ElementChaos
{
    class Player
	{

		GameStatusManger gsm;
		public int pos_v {get;set;}
		public int pos_h {get;set;}

		public Stack<ElementBase> liftUpELemStack;

		public GameDef.Towards towards;

		public ElementSkillBase equipSkill = null;
		public Player(int _h = 7, int _v = 7)
		{
			this.pos_v = _v;
			this.pos_h = _h;
			this.liftUpELemStack = new Stack<ElementBase>();
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

			GameDef.Towards towards = 0;;
			switch (d)
			{
				case GameDef.Action.left:
					dh = -1;
					towards = GameDef.Towards.Left;
					break;
				case GameDef.Action.Right:
					dh = 1;
					towards = GameDef.Towards.Right;
					break;
				case GameDef.Action.Up:
					dv = -1;
					towards = GameDef.Towards.Up;
					break;
				case GameDef.Action.Down:
					dv = 1;
					towards = GameDef.Towards.Down;
					break;
				default:
					dv = 0;
					dh = 0;
					break;
			}

			this.towards = towards;
			if (!gsm.canMoveTo(this.pos_v + dv, this.pos_h + dh))
				return;
			
			this.pos_v += dv;
			this.pos_h += dh;
		}
	
		public void LiftUpElement()
		{
			//TODO
			int elementPos_v, elementPos_h;
			if (!GetNewPosByPlayerTowards(out elementPos_v, out elementPos_h))
			{
				return;
			}

			var e_tpye = gsm.stage.running_stage_map[elementPos_v, elementPos_h];

			if (!Tools.isElment(e_tpye) 
				|| liftUpELemStack.Count >= GameDef.GlobalData.maxLiftUpNum
				|| e_tpye == GameDef.GameObj.FlowWater)
			{
				Debug.WriteLine("Lift at ({0}, {1}) Failed, bag num {2}, bag max size {3}", 
					elementPos_v, elementPos_h,
					liftUpELemStack.Count, GameDef.GlobalData.maxLiftUpNum);
				return;
			}

			var e = gsm.stage.elements_map[elementPos_v, elementPos_h];
			this.liftUpELemStack.Push(e);
			gsm.stage.RemoveElement(elementPos_v, elementPos_h);
			e.BeLiftedUp();
			
			Debug.WriteLine("[Player] Lift up element at ({0}, {1})", elementPos_v, elementPos_h);
		}

		public void PushDownElement()
		{
			if (liftUpELemStack.Count == 0)
			{
				Debug.WriteLine("[Player] Emtyp lifted-up element bag, push down nothing");
				return;
			}
			var e = liftUpELemStack.Peek();
			
			int new_v, new_h;
			GetNewPosByPlayerTowards(out new_v, out new_h);


			if (!gsm.stage.PushExistElementAt(e, new_v, new_h))			
			{
				Debug.WriteLine("[Player] push down {0} element failed at ({1}, {2})", e.name, new_v, new_h);
				return;
			}

			liftUpELemStack.Pop();
			e.AfterPutDown();
		}

		private bool GetNewPosByPlayerTowards(out int new_v, out int new_h)
		{
			int dv = 0, dh = 0;
			
			switch (this.towards)
			{
				case GameDef.Towards.Left:
					dh -= 1;
					break;
				case GameDef.Towards.Right:
					dh += 1;
					break;
				case GameDef.Towards.Down:
					dv += 1;
					break;
				case GameDef.Towards.Up:
					dv -= 1;
					break;
			}

			new_v = this.pos_v + dv;
			new_h = this.pos_h + dh;
	
			return true;
		}
	
		public bool EatElement()
		{
			if (this.liftUpELemStack.Count == 0)
			{
				Debug.WriteLine("[Player] no element can be eaten");
				return false;
			}

			var e = liftUpELemStack.Peek();
			if (!e.canBeEaten())
			{
				Debug.WriteLine("[Player] element {0} can not be eatean", e.name);
				return false;
			}

			// 吃同一种元素，剩余使用次数叠加
			if (this.equipSkill != null && this.equipSkill.type == GameDef.GlobalData.SkillMap[e.type])
			{
				this.equipSkill.remain_use_time += e.remain_time;
			}
			else
			{
				this.equipSkill = new FlameBomb(e.remain_time);
			}

			liftUpELemStack.Pop();

			return true;
		}

		public bool UseSkill()
		{
			if (equipSkill == null)
				return false;

			if (equipSkill.UseSkill(this.towards) == GameDef.UseSkillStatus.RunOutTimes)
			{
				equipSkill = null;
				return true;
			}
			return true;
		}
	}
}
