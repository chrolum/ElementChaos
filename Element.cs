using System;
using System.Collections.Generic;

namespace ElementChaos
{

    abstract class ElementBase
    {
        //TODO: use tags system to implement the element mix action
        public HashSet<GameDef.Tags> tags;
        public int pos_h {get;set;}
        public int pos_v {get;set;}

        public int remain_time; // -1 no remain time
        public abstract void AutoAction();

        public abstract bool canMix(ElementBase e);
        public abstract void beMixSingle(ElementBase e);
        public abstract void beMixMult(List<ElementBase> e);

    }
    class FireElement : ElementBase
    {
        //TODOL implement
        public FireElement(int _v, int _h, int rt = -1)
        {
            this.pos_h = _h;
            this.pos_v = _v;
            this.remain_time = rt;
        }
        public override void AutoAction()
        {
            throw new NotImplementedException();
        }

        public override void beMixMult(List<ElementBase> e)
        {
            throw new NotImplementedException();
        }

        public override void beMixSingle(ElementBase e)
        {
            throw new NotImplementedException();
        }

        public override bool canMix(ElementBase e)
        {
            throw new NotImplementedException();
        }
    }

    class WoodElement : ElementBase
    {
        //TODO: implement
        public override void AutoAction()
        {
            throw new NotImplementedException();
        }

        public override void beMixMult(List<ElementBase> e)
        {
            throw new NotImplementedException();
        }

        public override void beMixSingle(ElementBase e)
        {
            throw new NotImplementedException();
        }

        public override bool canMix(ElementBase e)
        {
            throw new NotImplementedException();
        }
    }

    class WaterElement : ElementBase
    {
        //TODOL implement
        public override void AutoAction()
        {
            throw new NotImplementedException();
        }

        public override void beMixMult(List<ElementBase> e)
        {
            throw new NotImplementedException();
        }

        public override void beMixSingle(ElementBase e)
        {
            throw new NotImplementedException();
        }

        public override bool canMix(ElementBase e)
        {
            throw new NotImplementedException();
        }
    }

    class MudElement : ElementBase
    {
        //TODOL implement
        public override void AutoAction()
        {
            throw new NotImplementedException();
        }

        public override void beMixMult(List<ElementBase> e)
        {
            throw new NotImplementedException();
        }

        public override void beMixSingle(ElementBase e)
        {
            throw new NotImplementedException();
        }

        public override bool canMix(ElementBase e)
        {
            throw new NotImplementedException();
        }
    }

    class WindElement : ElementBase
    {
        //TODOL implement
        public override void AutoAction()
        {
            throw new NotImplementedException();
        }

        public override void beMixMult(List<ElementBase> e)
        {
            throw new NotImplementedException();
        }

        public override void beMixSingle(ElementBase e)
        {
            throw new NotImplementedException();
        }

        public override bool canMix(ElementBase e)
        {
            throw new NotImplementedException();
        }
    }

    class ObsidianElement : ElementBase
    {
        //TODOL implement
        public override void AutoAction()
        {
            throw new NotImplementedException();
        }

        public override void beMixMult(List<ElementBase> e)
        {
            throw new NotImplementedException();
        }

        public override void beMixSingle(ElementBase e)
        {
            throw new NotImplementedException();
        }

        public override bool canMix(ElementBase e)
        {
            return this.tags.Contains(GameDef.Tags.Immutable);
        }
    }
}
