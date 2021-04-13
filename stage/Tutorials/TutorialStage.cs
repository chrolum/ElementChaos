using System;

using System.Collections.Generic;
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
                "关卡目标: 吃掉三个金元素",
                " ",
                "方向键控制人物行动",
                "X ---- 拾取当前朝向的元素",
                "C ---- 放下元素",
                "V ---- 吃掉元素",
                "Z ---- 使用元素技能,部分元素吃掉后有元素技能",
                " ",
                "* ---- 火元素,可以引燃木元素,可被拾取",
                "^ ---- 木元素,不可被拾",
                "$ ---- 金元素,吃掉可获得分数",
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
            // 检查还有没有活跃的金元素
            foreach (var e in activateElement)
            {
                if (e.type == GameDef.GameObj.Glod)
                    return false;
            }
            return true;
        }
    }

    class LiftUpAndPushDownStage : Stage
    {
        public LiftUpAndPushDownStage()
        {
            this.filePath = "../../../stage/Tutorials/LiftUAndPushDown";
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
}