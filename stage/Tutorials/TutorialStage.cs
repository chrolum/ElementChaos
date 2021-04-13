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
}