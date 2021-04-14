using System;
using System.Collections;
using System.Security.Cryptography;

namespace ElementChaos
{

    //TODO use tags implement element
    class Tools
    {
        public readonly static int PackOffset = 1000;
        public static Random rand = new Random();
        public static bool isElment(GameDef.GameObj o)
        {
            return o > GameDef.GameObj.ElementStart && o < GameDef.GameObj.ElementEnd;
        }

        public static int PackCoords(int v, int h)
        {
            return v * PackOffset + h;
        }

        public static int UnPackCoords_V(int packNum)
        {
            return packNum / PackOffset;
        }

        public static int UnPackCoords_H(int packNum)
        {
            return packNum % PackOffset;
        }

    }
}
