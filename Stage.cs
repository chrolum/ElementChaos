using System;

using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Linq;

namespace ElementChaos
{
    class StageLoader
    {

        // 只设置关卡的元素地图还有玩家，其他特殊条件在各自的关卡类中实现
        public static void LoadStageCommonDataFromFile(Stage stage)
        {
            
            System.IO.StreamReader file = new System.IO.StreamReader(stage.filePath);
            string line;
            //get map size
            line = file.ReadLine();
            string[] size = line.Split(" ");

            int map_v_size = Int32.Parse(size[0]);
            int map_h_size = Int32.Parse(size[1]);

            int v_idx = 0;
            int h_idx = 0;
            stage.h_size = map_h_size;
            stage.v_size = map_v_size;

            stage.running_stage_map = new GameDef.GameObj[map_v_size + 1, map_h_size + 1];
            stage.orignal_stage_map = new GameDef.GameObj[map_v_size + 1, map_h_size + 1];
            stage.elements_map = new ElementBase[map_v_size + 1, map_h_size + 1];
            stage.activateElement = new List<ElementBase>();
            stage.goalDict = new Dictionary<int, bool>();

            while ((line = file.ReadLine()) != null)
            {

                foreach (char c in line)
                {

                    GameDef.GameObj obj = GameDef.GameObj.Air;
                    if (GameDef.GlobalData.char2GameObjDict.ContainsKey(c))
                    {
                        obj = GameDef.GlobalData.char2GameObjDict[c];
                    }

                    //player 和 goal不往地图上写
                    if (obj == GameDef.GameObj.Player)
                    {
                        stage.player = new Player(h_idx, v_idx);
                    }
                    else if (obj == GameDef.GameObj.Goal)
                    {
                        stage.goalDict[Tools.PackCoords(v:v_idx, h:h_idx)] = false;
                    }
                    else
                    {
                        stage.running_stage_map[v_idx, h_idx] = obj;
                    }

                    stage.orignal_stage_map[v_idx, h_idx] = obj;
                    h_idx++;
                }
                h_idx = 0;
                v_idx++;
            }

            stage.InitialCommon();
        }

        //test
        // static void Main(string[] args)
        // {
        //     var s = StageLoader.LoadStageFromFile();
        //     s.test_print();
        // }

        public static void LoadStage(Stage stage)
        {
            StageLoader.LoadStageCommonDataFromFile(stage);
            stage.InitialCustom();
        }
    }

    class Stage
    {
        // for restrat
        public string filePath = "../../../stage/DebugStage";
        public GameDef.StageType type = GameDef.StageType.Debug;
        public GameDef.GameObj[,] orignal_stage_map;

        public Dictionary<int, bool> goalDict;
        public Player player;
        public List<string> desc;

        public GameDef.GameObj[,] running_stage_map;
		public ElementBase[,] elements_map;
		public List<ElementBase> activateElement;

        public int v_size {get;set;}
        public int h_size {get;set;}

        public int gamePoint = 0;

        public void InitialCommon()
        {
            //根据关卡文件生成元素图
            activateElement.Clear();
            clearElementMap();
            resetRunningStage();
            resetGoalDict();
            for (int r = 0; r < v_size; r++)
            {
                for (int c = 0; c < h_size; c++)
                {
                    if (!Tools.isElment(orignal_stage_map[r, c]))
                        continue;
                    var e = ElementFactory.CreateElment(orignal_stage_map[r, c], r, c);
                    elements_map[r, c] = e;
                    activateElement.Add(e);
                }
            }
            gamePoint = 0;
        }

        public void clearElementMap()
        {
            for (int r = 0; r < v_size; r++)
            {
                for (int c = 0; c < h_size; c++)
                {
                    elements_map[r, c] = null;
                }
            }
        }

        public void resetRunningStage()
        {
            // TODO 顺便重置玩家, 后期改掉;
            for (int r = 0; r < v_size; r++)
            {
                for (int c = 0; c < h_size; c++)
                {
                    if (orignal_stage_map[r, c] == GameDef.GameObj.Player)
                    {
                        this.player = new Player(c, r);
                        running_stage_map[r, c] = GameDef.GameObj.Air;
                    }
                    else if (orignal_stage_map[r, c] == GameDef.GameObj.Goal)
                    {
                        running_stage_map[r, c] = GameDef.GameObj.Air;
                    }
                    else
                    {
                        running_stage_map[r, c] = orignal_stage_map[r, c];
                    }
                }
            }
        }

        public void resetGoalDict()
        {
            if (this.goalDict.Count == 0)
                return;
            
            var keyList = goalDict.Keys.ToList();
            foreach (var k in keyList)
            {
                goalDict[k] = false;
            }
        }

        public virtual void InitialCustom()
        {

        }

        public void reload()
        {
            InitialCommon();
            InitialCustom();
        }

        public void test_print()
        {
            StringBuilder sb = new StringBuilder();
            for (int r = 0; r < h_size; r++)
            {
                for (int c = 0; c < v_size; c++)
                {
                    sb.Append(GameDef.GlobalData.output[running_stage_map[r, c]]);
                }
                sb.Append("\n");
            }

            Console.WriteLine(sb.ToString());
        }

        public bool RemoveElement(int v, int h)
        {
            if (!isValidPos(v, h))
                return false;

            this.running_stage_map[v, h] = GameDef.GameObj.Air;
            if (elements_map[v, h] != null)
            {
                this.activateElement.Remove(this.elements_map[v, h]);
                Debug.WriteLine("Remove {0} element at ({1}, {2})", this.elements_map[v, h].name, v, h);
            }
            this.elements_map[v, h] = null;

            return true;
        }
        // element auto action logic
        public void AutoSettleElement()
        {
            for (int r = 0; r < v_size; r++)
            {
                for (int c = 0; c < h_size; c++)
                {
                    var e = elements_map[r, c];
                    if (e == null)
                    {
                        continue;
                    }

                    //新生成的元素不参与结算
                    if (e.isNew)
                    {
                        e.isNew = false;
                    }
                    else
                    {
                        // 先逐个元素计算持续时间，新元素生成等逻辑
                        e.AutoAction();
                        // 每个元素检查周围存在的元素，进行元素规则结算
                        e.checkNearElement();
                    }
                }
            }
        }

        // tool method area
        public bool GenerateNewElement(GameDef.GameObj e, int v, int h, int rt = -1)
        {
            

            if (!Tools.isElment(e) || !isValidPos(v, h))
            {
                Debug.WriteLine("[Stage] invaild element or pos!");
                return false;
            }

            // 只能覆盖空气或者火
            if (!isAirAt(v, h) && !isFire(v, h))
                return false;

            // 先清理原位置的
            this.RemoveElement(v, h);

            this.running_stage_map[v, h] = e;
            this.elements_map[v, h] = ElementFactory.CreateElment(e, v, h, rt);
            this.activateElement.Add(this.elements_map[v, h]);
            Debug.WriteLine("[Stage] generate new {0} element at ({1}, {2})", this.elements_map[v, h].name, v, h);
            return true;
        }

        public bool PushExistElementAt(ElementBase element, int v, int h)
        {
            if (element == null || !isValidPos(v, h) || !isAirAt(v, h))
                return false;

            element.pos_v = v;
            element.pos_h = h;
            this.running_stage_map[v, h] = element.type;
            this.elements_map[v, h] = element;
            // Modify: 元素被举起后，只有被吃了才失效
            // this.activateElement.Add(element);
            Debug.WriteLine("[Stage] Push Down {0} elment at ({1}, {2})", element.name, v, h);
            return true;
        }

        public bool isValidPos(int v, int h)
        {
            return (v >= 0 && h >= 0 && v < this.v_size - 1 && h < this.h_size - 1);
        }

        public bool isSpecifyGameObjAt(GameDef.GameObj obj, int v, int h)
        {
            if (!isValidPos(v, h))
                return false;

            return running_stage_map[v, h] == obj;
        }

        public bool isAirAt(int v, int h)
        {
            return isSpecifyGameObjAt(GameDef.GameObj.Air, v ,h);
        }

        public bool isFire(int v, int h)
        {
            return isSpecifyGameObjAt(GameDef.GameObj.Fire, v, h);
        }

        public virtual bool checkWin()
        {
            return false;
        }

        public virtual bool checkFailed()
        {
            return false;
        }

    }
}