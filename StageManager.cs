using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ElementChaos
{

    //TODO use tags implement element
    class StageManager
    {
        public static int currStageIdx = 0;
        public static List<Stage> aviableStageList = new List<Stage>();

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

            var stage5 = new WaterAndFire();
            StageLoader.LoadStageCommonDataFromFile(stage5);
            stage5.InitialCommon();

            var stage6 = new GetSomeFire();
            StageLoader.LoadStageCommonDataFromFile(stage6);
            stage6.InitialCommon();

            var stage7 = new GetMoreFire();
            StageLoader.LoadStageCommonDataFromFile(stage7);
            stage7.InitialCommon();

            var stage8 = new FireLog();
            StageLoader.LoadStageCommonDataFromFile(stage8);
            stage8.InitialCommon();

            aviableStageList.Add(stage4);
            aviableStageList.Add(stage3);
            aviableStageList.Add(stage1);
            aviableStageList.Add(stage5);
            aviableStageList.Add(stage6);
            aviableStageList.Add(stage7);
            aviableStageList.Add(stage8);
            // aviableStageList.Add(stage2);

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
