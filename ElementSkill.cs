using System;
using System.Diagnostics;
using System.Collections.Generic;

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
        
        public virtual GameDef.UseSkillStatus UseSkill(GameDef.Towards tw, int orignal_v, int orignal_h)
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

        public override GameDef.UseSkillStatus UseSkill(GameDef.Towards tw, int orignal_v, int orignal_h)
        {
            if (remain_use_time == 0)
                return GameDef.UseSkillStatus.RunOutTimes;

            Debug.WriteLine("Fire!!!!!!!!!!!!!! remain time {0}", remain_use_time);
            BulletManager.Add(new Bullet(orignal_v, orignal_h, 1, 10, tw));
            remain_use_time--;

            return GameDef.UseSkillStatus.Success;
        }

    }

    // 元素附属
    class Bullet
    {
        public int damage;
        public Bullet(int v, int h, int speed = 1, int flying_distance = 10, GameDef.Towards tw = GameDef.Towards.Left, int dmg = 10)
        {
            this.pos_v = v;
            this.pos_h = h;
            this.speed = speed;
            this.flying_time = 0;
            this.flying_distance = flying_distance;
            this.towards = tw;
            this.damage = dmg;
        }
        public GameDef.Towards towards;
        public int pos_v;
        public int pos_h;

        public int flying_time;

        public int flying_distance;

        public int speed;

        // 碰撞爆炸效果的实现, 普通伤害在元素中实现
        public virtual void takeEffectAfterHitObject()
        {

        }

    }

    static class BulletManager
    {
        static public GameStatusManger gsm {get; set;}
        static public List<Bullet> activeBulletList = new List<Bullet>();

        static public void Add(Bullet b)
        {
            activeBulletList.Add(b);
        }

        // 在游戏update循环中要调用
        // 在绘制流程中单独绘制
        static public void checkAllBullet()
        {

            if (activeBulletList.Count == 0)
                return;
            List<Bullet> bulletRemoveList = new List<Bullet>();

            //遍历所有飞行中的子弹, 结算碰撞和超出射程的子弹
            for (int b_idx = 0; b_idx < activeBulletList.Count; b_idx++)
            {
                var b = activeBulletList[b_idx];
                // 超出射程
                if (b.flying_time == b.flying_distance)
                {
                    bulletRemoveList.Add(b);
                    continue;
                }

                if (checkSingleBulletcollision(b))
                {
                    bulletRemoveList.Add(b);
                }
            }

            foreach (var b in bulletRemoveList)
            {
                activeBulletList.Remove(b);
            }

            bulletRemoveList = null;
        }

        // 子弹飞行逻辑
        static public bool checkSingleBulletcollision(Bullet b)
        {
            bool isHit = false;
            ElementBase beHitElement = null;
            switch (b.towards)
            {
                case GameDef.Towards.Left:
                    isHit = hitObjectLeft(b, out beHitElement);
                    break;
                case GameDef.Towards.Right:
                    isHit = hitObjectRight(b, out beHitElement);
                    break;
                case GameDef.Towards.Up:
                    isHit = hitObjectUp(b, out beHitElement);
                    break;
                case GameDef.Towards.Down:
                    isHit = hitObjectDown(b, out beHitElement);
                    break;
            }
            
            if (isHit)
            {
                //TODO monister be hit
                if (beHitElement != null)
                {
                    beHitElement.beHitByBullet(b);
                }
                return isHit;
            }

            // 继续飞行
            b.flying_time++;
            return false;
        }


        //TODO: opt this shit noodle code
        static public bool hitObjectLeft(Bullet b, out ElementBase beHitElement)
        {
            var pos_v = b.pos_v;
            var pos_h = b.pos_h;
            var distance = b.speed;
            beHitElement = null;
            for (int idx = 1; idx <= distance; idx++)
            {
                if (!gsm.stage.isValidPos(pos_v, pos_h-idx))
                {
                    return true;
                }
                
                if (gsm.stage.running_stage_map[pos_v, pos_h-idx] != GameDef.GameObj.Air)
                {
                    if (Tools.isElment(gsm.stage.running_stage_map[pos_v, pos_h-idx]))
                    {
                        beHitElement = gsm.stage.elements_map[pos_v, pos_h-idx];
                    }
                    return true;
                }

            }
            
            b.pos_h -= distance;

            return false;
        }
        
        static public bool hitObjectRight(Bullet b, out ElementBase beHitElement)
        {
            var pos_v = b.pos_v;
            var pos_h = b.pos_h;
            var distance = b.speed;
            beHitElement = null;
            for (int idx = 1; idx <= distance; idx++)
            {
                if (!gsm.stage.isValidPos(pos_v, pos_h+idx))
                {
                    return true;
                }
                
                if (gsm.stage.running_stage_map[pos_v, pos_h+idx] != GameDef.GameObj.Air)
                {
                    if (Tools.isElment(gsm.stage.running_stage_map[pos_v, pos_h+idx]))
                    {
                        beHitElement = gsm.stage.elements_map[pos_v, pos_h+idx];
                    }
                    return true;
                }
            }

            b.pos_h += distance;

            return false;
        }
        static public bool hitObjectUp(Bullet b, out ElementBase beHitElement)
        {
            var pos_v = b.pos_v;
            var pos_h = b.pos_h;
            var distance = b.speed;
            beHitElement = null;
            for (int idx = 1; idx <= distance; idx++)
            {
                if (!gsm.stage.isValidPos(pos_v-idx, pos_h))
                {
                    return true;
                }
                
                if (gsm.stage.running_stage_map[pos_v-idx, pos_h] != GameDef.GameObj.Air)
                {
                    if (Tools.isElment(gsm.stage.running_stage_map[pos_v-idx, pos_h]))
                    {
                        beHitElement = gsm.stage.elements_map[pos_v-idx, pos_h];
                    }
                    return true;
                }

            }

            b.pos_v -= distance;
            return false;
        }
        static public bool hitObjectDown(Bullet b, out ElementBase beHitElement)
        {
            var pos_v = b.pos_v;
            var pos_h = b.pos_h;
            var distance = b.speed;
            beHitElement = null;
            for (int idx = 1; idx <= distance; idx++)
            {
                if (!gsm.stage.isValidPos(pos_v+idx, pos_h))
                {
                    return true;
                }
                
                if (gsm.stage.running_stage_map[pos_v+idx, pos_h] != GameDef.GameObj.Air)
                {
                    if (Tools.isElment(gsm.stage.running_stage_map[pos_v+idx, pos_h]))
                    {
                        beHitElement = gsm.stage.elements_map[pos_v+idx, pos_h];
                    }
                    return true;
                }

            }
            b.pos_v += distance;
            return false;
        }
    }
}
