using System;
using System.Diagnostics;
using System.Collections.Generic;


namespace ElementChaos
{
    class SampleUI
    {
        public ConsoleCanvas canvas;
        public char[,] draw_buff;
        public ConsoleColor[,] color_buff;

        public int msgNum = 0;
        public int maxMsgNum = 5;
        public List<string> MsgList;

        public SampleUI(int w, int h, char _empty = ' ', int anchor_v = 0, int anchor_h = 0)
        {
            this.canvas = new ConsoleCanvas(w, h, _empty, anchor_v, anchor_h);
            draw_buff = canvas.GetBuffer();
            color_buff = canvas.GetColorBuffer();
            this.MsgList = new List<string>();
        }

        public void publishMsg(string msg, ConsoleColor color = ConsoleColor.Gray)
        {
            if (MsgList.Count == maxMsgNum)
            {
                Debug.WriteLine("clear msglist");
                msgNum = 1;
                MsgList.Clear();
            }
            MsgList.Add(msg);
        }

        private void drawMsg()
        {
            int r_idx = 0;
            foreach (var msg in MsgList)
            {

                if (msg.Length >= canvas.Width)
                {
                    Debug.WriteLine("Too long string");
                    continue;
                }
                int c_idx = 0;
                foreach (var c in msg)
                {
                    draw_buff[r_idx, c_idx++] = c;
                    //color_buff[msgNum, c_idx++] = color;
                }
                r_idx++;
            }
        }

        public void Draw()
        {
            canvas.ClearBuffer_DoubleBuffer();
            //Msg Draw
            drawMsg();



            canvas.Refresh_DoubleBuffer();
        }

        public void reset()
        {
            this.MsgList.Clear();
        }

    }

    class GameMsgUI : SampleUI
    {
        public GameMsgUI(int w, int h, char _empty = ' ', int anchor_v = 0, int anchor_h = 0) : base(w, h, _empty, anchor_v, anchor_h)
        {
            MessageManager.Instance.publishGameMsg += publishMsg;
        }
    }

    class PlayerStatusMsgUI : SampleUI
    {
        public PlayerStatusMsgUI(int w, int h, char _empty = ' ', int anchor_v = 0, int anchor_h = 0) : base(w, h, _empty, anchor_v, anchor_h)
        {
            MessageManager.Instance.publishPlayerStatusMsg += publishMsg;
            this.maxMsgNum = 30;
        }
    }

    class MessageManager
    {
        private static MessageManager _instance;
        public static MessageManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new MessageManager();
                }
                return _instance;
            }
        }


        public delegate void DelegateGameMsg(string msg, ConsoleColor color = ConsoleColor.Gray);

        public delegate void DelegatePlayerStatusMsg(string msg, ConsoleColor color= ConsoleColor.Gray);

        public DelegateGameMsg publishGameMsg;

        public DelegatePlayerStatusMsg publishPlayerStatusMsg;
    }
}
