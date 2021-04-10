using System;

namespace ElementChaos
{

    //TODO use tags implement element
    class Tools
    {
        public static Random rand = new Random();
        public static bool isElment(GameDef.GameObj o)
        {
            return o > GameDef.GameObj.ElementStart && o < GameDef.GameObj.ElementEnd;
        }
    }
}
