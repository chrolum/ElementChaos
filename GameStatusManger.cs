using System;
using System.Collections.Generic;
using System.Text;

namespace ElementChaos
{
	class GameStatusManger
	{

		private GameStatusManger() { }

		private static GameStatusManger _instance;

		public static GameStatusManger GetInstance()
		{
			if (_instance == null)
			{
				_instance = new GameStatusManger();
			}

			return _instance;
		}

		public void Inital(int _v, int _h)
		{
			// this.running_map = new GameDef.GameObj[_v, _h];
			// this.elements_map = new ElementBase[_v, _h];
		}

		public int getElemPosMapIdx(int v, int h)
		{
			return v * GameDef.GlobalData.elemDictIdxBias + h;
		}
        // map 

		public Stage stage;
        //visiable control
		public Player player;

        //
		public bool canMoveTo(int v, int h)
		{
			if (stage.isValidPos(v, h) && 
				(stage.running_stage_map[v, h] == GameDef.GameObj.Air || stage.running_stage_map[v, h] == GameDef.GameObj.Goal))
				return true;

			return false;
		}

	}
}
