using System;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace ElementChaos
{
    class FireAndWoodTutorialStage : Stage
    {

        public int GlodEatTimes = 0;
        public FireAndWoodTutorialStage()
        {
            this.filePath = "../../../stage/Tutorials/FireAndWood";
            this.desc = new List<string>()
            {
                "教学关卡:干柴烈火",
                " ",
                "关卡目标:到达出口",
                " ",
                "[元素介绍]",
                "* ---- 火元素,可以引燃木元素,可被拾取",
                " ",
                "^ ---- 木元素,不可被拾",
                " ",
                ". ---- 出口",
                " ",
                "[操作指南]",
                " ",
                "【C】 放下元素, 需要人物前方有空地",
                
            };

        }

        public override void InitialCustom()
        {
            
        }

        public override bool checkFailed()
        {
            return this.player.Hp <= 0;
        }

        public override bool checkWin()
        {
            
            var keyLists = goalDict.Keys.ToList();
            var p = keyLists[0];

            var v = Tools.UnPackCoords_V(p);
            var h = Tools.UnPackCoords_H(p);
            return v == player.pos_v && h == player.pos_h;
        }
    }

    class LiftUpAndPushDownStage : Stage
    {
        public LiftUpAndPushDownStage()
        {
            this.filePath = "../../../stage/Tutorials/LiftUAndPushDown";
            this.desc = new List<string>()
            {
                "教学关卡:淘金热",
                " ",
                "关卡目标:拾取所有的金元素, 并放到指定位置",
                " ",
                " $ ---- 金元素",
                " ",
                " . ---- 金元素放置点",
                " ",
                "新操作:放置元素【C 键】",
                " ",
                "将背包第一个元素放置在人物朝向的前一格",
                " ",
                "人物前一格有遮挡物将无法放置"

            };

        }

        public override bool checkWin()
        {
            var keyLists = goalDict.Keys;
            foreach (var p in keyLists)
            {
                var v = Tools.UnPackCoords_V(p);
                var h = Tools.UnPackCoords_H(p);
                if (this.running_stage_map[v, h] != GameDef.GameObj.Glod)
                    return false;
            }
            return true;
        }
    }

    class LiftUpStage : Stage
    {

        public LiftUpStage()
        {
            this.filePath = "../../../stage/Tutorials/LiftUp";
            this.desc = new List<string>()
            {
                "教学关卡: 元素漫步",
                " ",
                "关卡目标: 拾取所有的金元素",
                " ",
                "方向键控制人物移动",
                " ",
                "直接走过某些元素,即可自动吸取",
                " ",
                "当元素背包满后[上限3个元素],你无法再吸取元素",
            };
            
        }

        public override bool checkWin()
        {
            return this.player.liftUpELemStack.Count == 3;
        }
    }

    class WaterAndFire : Stage
    {

        public WaterAndFire()
        {
            this.filePath = "../../../stage/Tutorials/WaterAndFire";
            this.desc = new List<string>()
            {
                "教学关卡: 炎炎消防队",
                " ",
                "关卡目标: 熄灭所有火元素",
                " ",
                "【元素指南】",
                " ",
                "W ---- 水元素,被放置后会往人物朝向生成三格流动水",
                " ",
                "* ---- 火元素,可以被流动水浇灭",
                " ",
                "【操作指南】",
                " ",
                "【C】 放下元素, 需要人物前方有空地",
            };
            
        }

        public override bool checkWin()
        {
            foreach (var e in activateElement)
            {
                if (e.type == GameDef.GameObj.Fire)
                    return false;
            }

            return true;
        }
    }

    class GetSomeFire : Stage
    {

        public int GlodEatTimes = 0;
        public GetSomeFire()
        {
            this.filePath = "../../../stage/Tutorials/GetSomeFire";
            this.desc = new List<string>()
            {
                "教学关卡:点石成火",
                " ",
                "关卡目标:到达出口",
                "",
                "关卡描述: 想放把火但是没打火机？ 试试黑曜石和金元素吧！",
                " ",
                "[元素介绍]",
                "$ ---- 金元素",
                " ",
                "& ---- 黑曜石, 周围存在金元素,且有2格空地时，会随机产生火花",
                " ",
                ". ---- 出口",
                " ",
                "[操作指南]",
                " ",
                "【C】 放下元素, 需要人物前方有空地",
                
            };
        }

        public override bool checkWin()
        {
            
            var keyLists = goalDict.Keys.ToList();
            var p = keyLists[0];

            var v = Tools.UnPackCoords_V(p);
            var h = Tools.UnPackCoords_H(p);
            return v == player.pos_v && h == player.pos_h;
        }
    }

    class GetMoreFire : Stage
    {
        public GetMoreFire()
        {
            this.filePath = "../../../stage/Tutorials/MoreFire";
            this.desc = new List<string>()
            {
                "教学关卡:更多的火",
                " ",
                "关卡目标:烧掉所有木元素",
                "",
                "关卡描述: 用火花点燃原木，然后烧掉树林",
                " ",
                "[元素介绍]",
                "$ ---- 金元素",
                " ",
                "& ---- 黑曜石, 周围存在金元素,且有充足的空气(3格空地)时，会随机产生火花",
                " ",
                "L ---- 原木，可被火花点燃，燃烧持续时间更长",
                ". ---- 出口",
                " ",
                "[操作指南]",
                " ",
                "【C】 放下元素, 需要人物前方有空地",
                
            };
        }

        public override bool checkWin()
        {
            
            // var keyLists = goalDict.Keys.ToList();
            // var p = keyLists[0];

            // var v = Tools.UnPackCoords_V(p);
            // var h = Tools.UnPackCoords_H(p);
            // return v == player.pos_v && h == player.pos_h;
            foreach (var e in activateElement)
            {
                //TODO: 临时剔除原木的判断
                if (e.type == GameDef.GameObj.Wood && this.running_stage_map[e.pos_v, e.pos_h] != GameDef.GameObj.Log)
                {
                    return false;
                }
            }

            return true;
        }
    }

    class FireLog : Stage
    {
        public FireLog()
        {
            this.filePath = "../../../stage/Tutorials/FireLog";
            this.desc = new List<string>()
            {
                "教学关卡:更多的火",
                " ",
                "关卡目标:到达出口",
                "",
                "关卡描述: 想办法吃掉火元素，获得元素能力，将森林烧掉",
                " ",
                "[元素介绍]",
                "L ---- 原木，更难被点燃，但燃烧效果更长",
                " ",
                "~ ---- 流动水，不可拾取",
                " ",
                "^ ---- 木元素，可被点燃",
                " ",
                "[操作指南]",
                " ",
                "新操作【V】将最新拾取的元素转换为元素能力",
                "新操作【Z】使用元素能力",
                " ",
                "[能力指南]",
                " ",
                "火焰弹: 由非永久火元素转化而来，按V向前发生一颗火焰弹",
                " ",
                "碰到物体火迸裂出火花", 
                " ",
                "可穿过流动水",
            };
        }

        public override bool checkWin()
        {
            
            // var keyLists = goalDict.Keys.ToList();
            // var p = keyLists[0];

            // var v = Tools.UnPackCoords_V(p);
            // var h = Tools.UnPackCoords_H(p);
            // return v == player.pos_v && h == player.pos_h;
            foreach (var e in activateElement)
            {
                //TODO: 临时剔除原木的判断
                if (e.type == GameDef.GameObj.Wood && this.running_stage_map[e.pos_v, e.pos_h] != GameDef.GameObj.Log)
                {
                    return false;
                }
            }

            return true;
        }
    }
}