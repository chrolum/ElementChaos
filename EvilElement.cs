using System;

namespace ElementChaos
{
    class EvilElement : ElementBase
    {
        public int Hp {get;set;}
        public EvilElement(int v, int h, int rt = -1, string name = "Evil", int Hp = 60) : base(v, h, rt)
        {
            this.name = name;
            this.type = GameDef.GameObj.Mud;
            this.Hp = Hp;
        }

        public override void beHitByBullet(Bullet b)
        {
            this.Hp -= b.damage;

            // remian 设置为0， 交由元素管理器进行销毁
            this.remain_time = 0;
        }
    }
}
