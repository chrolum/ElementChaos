using System;
using System.Diagnostics;

namespace ElementChaos
{
    class ElementSkillBase
    {
        public int remain_use_time;
        public string name = "defalut";

        public GameDef.SkillType type = GameDef.SkillType.NoType;

        public ElementSkillBase(int rmt = 1)
        {
            this.remain_use_time = rmt;
        }
        
        public virtual GameDef.UseSkillStatus UseSkill(GameDef.Towards tw)
        {
            return GameDef.UseSkillStatus.Success;
        }
    }

    class FlameBomb : ElementSkillBase
    {
        public FlameBomb(int rmt = 1) : base (rmt)
        {
            this.name = "FlameBomb";
            this.remain_use_time = rmt;
        }

        public override GameDef.UseSkillStatus UseSkill(GameDef.Towards tw)        
        {
            if (remain_use_time == 0)
                return GameDef.UseSkillStatus.RunOutTimes;

            Debug.WriteLine("Fire!!!!!!!!!!!!!! remain time {0}", remain_use_time);
            remain_use_time--;

            return GameDef.UseSkillStatus.Success;
        }

    }
}
