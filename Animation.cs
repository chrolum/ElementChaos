using System;

namespace ElementChaos
{
    /*
    * all cg, win, falied, element effects manager
    */

    class Animation
    {
        public static void DrawWinAni()
        {
            //TODO
        }

        public static void DrawFailedAni()
        {
            //TODO
        }

    }

    abstract class ElementAnimationBase
    {
        public int orignal_v;
        public int orignal_x;

        abstract public void draw();
    }

    class ExplosionAnimation : ElementAnimationBase
    {
        //TODO
        public override void draw()
        {
            throw new NotImplementedException();
        }
    }

    class CombustionAnimation : ElementAnimationBase
    {
        //TODO
        public override void draw()
        {
            throw new NotImplementedException();
        }
    }

}
