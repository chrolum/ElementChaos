using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ElementChaos
{

    //TODO use tags implement element
    class StageManager
    {
        public static int currStageIdx = 0;
        static List<Stage> aviableStageList = new List<Stage>();

        public static void LoadAllStage()
        {
            var stage1 = new FireAndWoodTutorialStage();
            StageLoader.LoadStageCommonDataFromFile(stage1);
            stage1.InitialCustom();

            aviableStageList.Add(stage1);
        }

        public static Stage GetNextStage()
        {
            if (currStageIdx >= aviableStageList.Count)
            {
                Debug.WriteLine("Fatal: no avibale stage");
                return null;
            }
            return aviableStageList[currStageIdx++];
        }

        public static Stage GetStageByIdx(int idx)
        {
            if (idx >= aviableStageList.Count)
                return null;
            return aviableStageList[idx];
        }
    }
}
