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

            var stage2 = new Stage();
            StageLoader.LoadStageCommonDataFromFile(stage2);
            stage2.InitialCommon();

            var stage3 = new LiftUpAndPushDownStage();
            StageLoader.LoadStageCommonDataFromFile(stage3);
            stage3.InitialCommon();

            var stage4 = new LiftUpStage();
            StageLoader.LoadStageCommonDataFromFile(stage4);
            stage4.InitialCommon();

            aviableStageList.Add(stage4);
            aviableStageList.Add(stage3);
            aviableStageList.Add(stage2);
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
