using System;
using System.Collections.Generic;
using System.Diagnostics;

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
        
        public string name = "defalut";
        // TODO: 将结算剩余存在时间的逻辑移动到这里
        public virtual void AutoAction()
        {

        }

        public virtual void AfterPutDown()
        {

        }

        public virtual void BeLiftedUp()
        {}

        public ElementBase(int v, int h, int rt = -1)
        {
            this.pos_h = h;
            this.pos_v = v;
            this.remain_time = rt;
            this.gsm = GameStatusManger.GetInstance();
        }
        // 每种元素重写该函数来实现不同的元素交互效果
        public virtual void checkNearElement() {}

        // tool method

        //check
        /*
        *     X
        *   X o X
        *     X
        */
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
        public FireElement(int v, int h, int rt = -1) : base(v, h, rt){
            this.name = "Fire";
            this.type = GameDef.GameObj.Fire;
        }
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
            this.name = "Wood";
            this.fire_tolerance_time = Tools.rand.Next(3,10);
            this.type = GameDef.GameObj.Wood;
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
                gsm.stage.GenerateNewElement(GameDef.GameObj.Fire, pos_v, pos_h, Tools.rand.Next(5,17));
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
        public GameDef.WaterType water_type;
        public int flow_stop_distance;
        public int curr_flow_distance = 1; // 0表示在水源代替成流动水

        // 该list用于水源被移除后，流动水消失的逻辑
        public List<FlowWaterElement> childrenFlowWaterList;
        public WaterElement(int v, int h, int rt = -1, 
            GameDef.WaterType wt = GameDef.WaterType.NoFlow, int fd = 4) : base(v, h, rt)
        {
            this.water_type = wt;
            this.flow_stop_distance = fd;
            this.name = "Water";
            childrenFlowWaterList = new List<FlowWaterElement>();
            this.type = GameDef.GameObj.Water;
        }
        public override void AutoAction()
        {
            if (this.water_type == GameDef.WaterType.NoFlow
             || curr_flow_distance == flow_stop_distance)
            {
                return;
            }

            //TODO: 暂时设置为水源被拾取后，全部流动水会瞬间消失
            GenerateFlowWater();
            // gsm.stage.GenerateNewElement(GameDef.GameObj.FlowWater, pos_v++, pos_h);
        }

        private bool GenerateFlowWater()
        {
            int dv = 0, dh = 0;
            switch (this.water_type)
            {
                case GameDef.WaterType.Left:
                    dh -= curr_flow_distance;
                    break;
                case GameDef.WaterType.Right:
                    dh += curr_flow_distance;
                    break;
                case GameDef.WaterType.Down:
                    dv += curr_flow_distance;
                    break;
                case GameDef.WaterType.Up:
                    dv -= curr_flow_distance;
                    break;
            }


            // if (!gsm.stage.isValidPos(pos_v + dv, pos_h + dh))
            //     return false;
            if (gsm.stage.GenerateNewElement(GameDef.GameObj.FlowWater, pos_v + dv, pos_h + dh))
            {
            
                this.childrenFlowWaterList.Add((FlowWaterElement)gsm.stage.elements_map[pos_v + dv, pos_h + dh]);
                curr_flow_distance++;
                Debug.WriteLine("[WaterE] Generating flow water at ({0}, {1})", pos_v + dv, pos_h + dh);
                return true;
            }

            return false;
            
        }

        
        public override void AfterPutDown()
        {
            //水源被重新放置后, 改变水流方向
            // 流动水的生成由AutoAction实现
            this.water_type = GameDef.GlobalData.waterTypeMapByPlayerTowards[gsm.player.towards];    
        }

        public override void BeLiftedUp()
        {
            for (int i = 0; i < childrenFlowWaterList.Count; i++)
            {
                var e = childrenFlowWaterList[i];
                gsm.stage.RemoveElement(e.pos_v, e.pos_h);
                Debug.WriteLine("[WaterE] remove flow water at ({0}, {1})", e.pos_v, e.pos_h);
            } 
            gsm.stage.RemoveElement(this.pos_v, this.pos_h);

            // 重设元素所有的状态
            this.childrenFlowWaterList.Clear();
            this.curr_flow_distance = 1;
        }
    }

    class MudElement : ElementBase
    {
        //TODO implement
        public MudElement(int v, int h, int rt = -1) : base(v, h, rt)
        {
            this.name = "Mud";
            this.type = GameDef.GameObj.Mud;
        }
        public override void AutoAction()
        {
            throw new NotImplementedException();
        }

    }

    class WindElement : ElementBase
    {
        //TODO implement
        public WindElement(int v, int h, int rt = -1) : base(v, h, rt)
        {
            this.name = "Wind";
            this.type = GameDef.GameObj.Wind;
        }
        public override void AutoAction()
        {
            throw new NotImplementedException();
        }

    }

    class ObsidianElement : ElementBase
    {
        //TODOL implement
        public ObsidianElement(int v, int h, int rt = -1) : base(v, h, rt)
        {
            this.name = "Obsidian";
            this.type = GameDef.GameObj.Obsidian;
        }
        public override void AutoAction()
        {
            throw new NotImplementedException();
        }

    }

    class FlowWaterElement : ElementBase
    {
        //TODO implement
        public FlowWaterElement(int v, int h, int rt = -1) : base(v, h, rt)
        {
            this.name = "FlowWater";
            this.type = GameDef.GameObj.FlowWater;
        }
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
