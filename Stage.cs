using System;

using System.Collections.Generic;
using System.Text;

namespace ElementChaos
{
    class StageLoader
    {
        public static Stage LoadStageFromFile(string filePath = @"../../../stage1")
        {
            
            Stage stage = new Stage();
            System.IO.StreamReader file = new System.IO.StreamReader(filePath);
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

            while ((line = file.ReadLine()) != null)
            {

                foreach (char c in line)
                {

                    GameDef.GameObj obj = GameDef.GameObj.Air;
                    if (GameDef.GlobalData.char2GameObjDict.ContainsKey(c))
                    {
                        obj = GameDef.GlobalData.char2GameObjDict[c];
                    }
                    stage.orignal_stage_map[v_idx, h_idx] = obj;
                    stage.running_stage_map[v_idx, h_idx] = obj;
                    h_idx++;
                }
                h_idx = 0;
                v_idx++;
            }

            stage.Initial();

            return stage;
        }

        //test
        // static void Main(string[] args)
        // {
        //     var s = StageLoader.LoadStageFromFile();
        //     s.test_print();
        // }
    }

    class Stage
    {
        // for restrat
        public GameDef.GameObj[,] orignal_stage_map;

        // public player
        public GameDef.GameObj[,] running_stage_map;
		public ElementBase[,] elements_map;
		public List<ElementBase> activateElement;


        public int v_size {get;set;}
        public int h_size {get;set;}

        public void Initial()
        {
            //根据关卡文件生成元素图
            for (int r = 0; r < v_size; r++)
            {
                for (int c = 0; c < h_size; c++)
                {
                    if (!Tools.isElment(running_stage_map[r, c]))
                        continue;
                    var e = ElementFactory.CreateElment(running_stage_map[r, c], r, c);
                    elements_map[r, c] = e;
                    activateElement.Add(e);
                }
            }
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
                return false;

            this.running_stage_map[v, h] = e;
            this.elements_map[v, h] = ElementFactory.CreateElment(e, v, h, rt);
            this.activateElement.Add(this.elements_map[v, h]);
            return true;
        }

        public bool isValidPos(int v, int h)
        {
            return (v >= 0 && h >= 0 && v < this.v_size - 1 && h < this.h_size - 1);
        }
    }
}