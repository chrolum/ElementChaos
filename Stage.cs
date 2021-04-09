using System;

using System.Collections.Generic;
using System.Text;

namespace ElementChaos
{
    class StageLoader
    {
        public static Stage LoadStageFromFile(string fileName = "stage1")
        {
            
            Stage stage = new Stage();
            System.IO.StreamReader file = new System.IO.StreamReader(fileName);
            string line;
            //get map size
            line = file.ReadLine();
            string[] size = line.Split(" ");

            int map_v_size = Int32.Parse(size[0]);
            int map_h_size = Int32.Parse(size[1]);

            stage.Initial(map_v_size, map_h_size);

            int v_idx = 0;
            int h_idx = 0;
            while ((line = file.ReadLine()) != null)
            {
                GameDef.GameObj obj = GameDef.GameObj.Air;

                foreach (char c in line)
                {
                    if (GameDef.GlobalData.char2GameObjDict.ContainsKey(c))
                    {
                        obj = GameDef.GlobalData.char2GameObjDict[c];
                    }
                    stage.orignal_stage_map[v_idx, h_idx] = obj;
                    stage.running_stage_map[v_idx, h_idx] = obj;
                    h_idx++;
                }
                v_idx++;
            }

            return stage;
        }

        //test
        static void Main(string[] args)
        {
            var s = StageLoader.LoadStageFromFile();
            s.test_print();
        }
    }

    class Stage
    {
        // for restrat
        public GameDef.GameObj[,] orignal_stage_map;

        // public player
        public GameDef.GameObj[,] running_stage_map;
		public ElementBase[,] elements_map;
		public List<ElementBase> activateElement;


        int v_size;
        int h_size;

        public void Initial(int v_size, int h_size)
        {
            this.orignal_stage_map = new GameDef.GameObj[v_size, h_size];
            this.running_stage_map = new GameDef.GameObj[v_size, h_size];
            this.v_size = v_size;
            this.h_size = h_size;
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

        public bool GenerateElement(GameDef.GameObj e, int v, int h)
        {
            if (!Tools.isElment(e) || !isValidPos(v, h))
                return false;

            this.running_stage_map[v, h] = e;
            this.elements_map[v, h] = ElementFactory.CreateElment(e, v, h);
            this.activateElement.Add(this.elements_map[v, h]);
            return true;
        }

        public bool isValidPos(int v, int h)
        {
            return (v >= 0 && h >= 0 && v < this.v_size - 1 && v < this.h_size - 1);
        }
    }
}