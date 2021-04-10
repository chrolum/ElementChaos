using System;
using System.Collections.Generic;

namespace ElementChaos
{

    class ElementBase
    {
        //TODO: use tags system to implement the element mix action
        public HashSet<GameDef.Tags> tags;

        public GameStatusManger gsm;
        public GameDef.GameObj type;
        public int pos_h {get;set;}
        public int pos_v {get;set;}

        public bool isNew = true; // the new element will not auto action

        public int remain_time; // -1 no remain time
        public int tmp_cnt; // for test;
        public virtual void AutoAction()
        {

        }

        public ElementBase(int v, int h, int rt = -1)
        {
            this.pos_h = h;
            this.pos_v = v;
            this.remain_time = rt;
            this.gsm = GameStatusManger.GetInstance();
        }
        public virtual void checkNearElement() {}

        // tool method

        //check
        protected bool isNear(GameDef.GameObj elementType)
        {
            var map = gsm.stage.running_stage_map;
            var stage = gsm.stage;

            //up
            if (stage.isValidPos(pos_v+1, pos_h) && map[pos_v+1, pos_h] == elementType)
                return true;
            //down
            if (stage.isValidPos(pos_v-1, pos_h) && map[pos_v-1, pos_h] == elementType)
                return true;

            //right
            if (stage.isValidPos(pos_v, pos_h+1) && map[pos_v, pos_h+1] == elementType)
                return true;
            //left
            if (stage.isValidPos(pos_v, pos_h-1) && map[pos_v, pos_h-1] == elementType)
                return true;

            return false;
        }

    }
    class FireElement : ElementBase
    {
        //TODOL implement
        public FireElement(int v, int h, int rt = -1) : base(v, h, rt){}
        public override void AutoAction()
        {
            if (remain_time == -1)
                return;
            //燃烧逻辑
            if (remain_time == 0)
            {
                gsm.stage.RemoveElement(pos_v, pos_h);
                return;
            }
            remain_time--;
        }

        public override void checkNearElement()
        {
            base.checkNearElement();
        }

    }

    class WoodElement : ElementBase
    {
        //TODO: implement
        public WoodElement(int v, int h, int rt = -1) : base(v, h, rt)
        {
            this.fire_tolerance_time = Tools.rand.Next(3,10);
        }
        public int nearFireTime = 0;
        public int fire_tolerance_time;
        public override void AutoAction()
        {
            if (remain_time == -1)
                return;

			//    //燃烧逻辑
			//    if (remain_time != -1)
			//    {
			//        remain_time--;
			//    }
			//    else if (remain_time == 0)
			//    {
			//        gsm.stage.RemoveElement(pos_v, pos_h);
			//    }
		}

		public override void checkNearElement()
        {
            //被火元素点燃的逻辑, 检查相接四格内有没有火元素
            //remian_time由统一的管理器调用AutoAction继续结算

            //普通情况
            if (!isNear(GameDef.GameObj.Fire) && nearFireTime == 0)
                return;

            if (isNear(GameDef.GameObj.Fire) && nearFireTime == this.fire_tolerance_time) //开始燃烧
            {
                //remain_time = GameDef.GlobalData.wood_fire_time;
                gsm.stage.RemoveElement(pos_v, pos_h);
                gsm.stage.GenerateElement(GameDef.GameObj.Fire, pos_v, pos_h, Tools.rand.Next(5,17));
            }
            else if (isNear(GameDef.GameObj.Fire) && nearFireTime != this.fire_tolerance_time)
            {
                nearFireTime++;
            }
            else if (!isNear(GameDef.GameObj.Fire))//远离火源后，逐渐恢复耐受次数
            {
                nearFireTime--;
            }
        }
    }

    class WaterElement : ElementBase
    {
        //TODO implement
        public WaterElement(int v, int h, int rt = -1) : base(v, h, rt){}
        public override void AutoAction()
        {
            var gsm = GameStatusManger.GetInstance();
            gsm.stage.GenerateElement(GameDef.GameObj.FlowWater, pos_v++, pos_h);
        }

    }

    class MudElement : ElementBase
    {
        //TODO implement
        public MudElement(int v, int h, int rt = -1) : base(v, h, rt){}
        public override void AutoAction()
        {
            throw new NotImplementedException();
        }

    }

    class WindElement : ElementBase
    {
        //TODO implement
        public WindElement(int v, int h, int rt = -1) : base(v, h, rt){}
        public override void AutoAction()
        {
            throw new NotImplementedException();
        }

    }

    class ObsidianElement : ElementBase
    {
        //TODOL implement
        public ObsidianElement(int v, int h, int rt = -1) : base(v, h, rt){}
        public override void AutoAction()
        {
            throw new NotImplementedException();
        }

    }

    class FlowWaterElement : ElementBase
    {
        //TODO implement
        public FlowWaterElement(int v, int h, int rt = -1) : base(v, h, rt){}
        public override void AutoAction()
        {
            
        }

    }

    class ElementFactory
    {
        public static ElementBase CreateElment(GameDef.GameObj e, int v, int h, int rt = -1)
        {
            switch (e)
            {
                case GameDef.GameObj.Fire:
                    return new FireElement(v, h, rt);
                case GameDef.GameObj.Water:
                    return new WaterElement(v, h, rt);
                case GameDef.GameObj.FlowWater:
                    return new FlowWaterElement(v, h, rt);
                case GameDef.GameObj.Wood:
                    return new WoodElement(v, h, rt);
                //TODO
            }

            return new FireElement(0, 0);
        }
    }
}
