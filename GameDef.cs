using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;

namespace GameDef
{
	enum GameObj
	{
		GameObjStart = 0,
		ElementStart,
		Fire,
		Wind,
		Water,
		FlowWater,
		Wood,
		Mud,
		Glod,
		Obsidian,
		Log,
		LifeElementStart,
		
		EvilElement,
		BrightElement,
		BlackElement,
		LifeElementEnd,
		ElementEnd,


		ImmutableWall,
		Air,
		Goal,
		Player,
	}

	enum Action
	{
		left = 0,
		Right,
		Up,
		Down,

		LiftUp,
		PutDown,
		UseSkill,
		EatElement,
		RESTART,
		NOACTION
	}

    enum Tags
    {
        Fire,
        Wind,
        Mud,
        Wood,
        Water,
        Obsidian,
		Air,
        Immutable,
    }

	enum WaterType
	{
		NoFlow = 0,
		Left,
		Right,
		Up,
		Down,
		Center,

	}

	enum Towards
	{
		Left = 0,
		Right,

		Up,

		Down,
	}

	enum SkillType
	{
		NoType = 0,
		FlameBomb,
	}

	enum UseSkillStatus
	{
		Success = 0,
		RunOutTimes,
		Unknown,
	}

	enum StageType
	{
		Debug,
		Tutorials,
		ExitPoint,
	}

	class GlobalData
	{
		public readonly static Dictionary<char, GameDef.GameObj> char2GameObjDict = new Dictionary<char, GameObj>(){
			{'#', GameDef.GameObj.ImmutableWall},
			{' ', GameDef.GameObj.Air},
			{'.', GameDef.GameObj.Goal},
			{'*', GameDef.GameObj.Fire},
			{'^', GameDef.GameObj.Wood},
			{'W', GameDef.GameObj.Water},
			{'$', GameObj.Glod},
			{'&', GameObj.Obsidian},
			{'L', GameObj.Log},
			{'P', GameObj.Player},
		};

		public readonly static Dictionary<ConsoleKey, GameDef.Action> ActionMap = new Dictionary<ConsoleKey, GameDef.Action>()
		{
			{ConsoleKey.UpArrow, GameDef.Action.Up},
			{ConsoleKey.DownArrow, GameDef.Action.Down},
			{ConsoleKey.LeftArrow, GameDef.Action.left},
			{ConsoleKey.RightArrow, GameDef.Action.Right},
			{ConsoleKey.R, GameDef.Action.RESTART},
			{ConsoleKey.X, GameDef.Action.LiftUp},
			{ConsoleKey.C, GameDef.Action.PutDown},
			{ConsoleKey.Z, GameDef.Action.UseSkill},
			{ConsoleKey.V, GameDef.Action.EatElement},
		};

		public readonly static Dictionary<GameDef.GameObj, char> output = new Dictionary<GameDef.GameObj, char>()
		{
			{GameDef.GameObj.Air, ' '},
			{GameDef.GameObj.ImmutableWall, '■'},
			{GameDef.GameObj.Fire, '*'},
			{GameDef.GameObj.Wind, '@'},
			{GameDef.GameObj.Mud, '-'},
			{GameDef.GameObj.Glod, '$'},
			{GameDef.GameObj.Water, 'W'},
			{GameDef.GameObj.Wood, '^'},
			{GameDef.GameObj.Player, 'P'},
			{GameDef.GameObj.FlowWater, '~'},
			{GameDef.GameObj.Obsidian, '&'},
			{GameDef.GameObj.Log, 'L'},
			{GameDef.GameObj.Goal, '.'}
		};

		public readonly static Dictionary<GameDef.GameObj, ConsoleColor> outputForeColor = new Dictionary<GameDef.GameObj, ConsoleColor>()
		{
			{GameDef.GameObj.ImmutableWall, ConsoleColor.Gray},
			{GameDef.GameObj.Fire, ConsoleColor.Red},
			{GameDef.GameObj.Wind, ConsoleColor.DarkGray},
			{GameDef.GameObj.Mud, ConsoleColor.DarkCyan},
			{GameDef.GameObj.Glod, ConsoleColor.Yellow},
			{GameDef.GameObj.Water, ConsoleColor.Blue},
			{GameDef.GameObj.Wood, ConsoleColor.DarkGreen},
			{GameDef.GameObj.Player, ConsoleColor.Gray},
			{GameDef.GameObj.FlowWater, ConsoleColor.Gray},
			{GameDef.GameObj.Obsidian, ConsoleColor.DarkMagenta},
			{GameDef.GameObj.Log, ConsoleColor.Gray},
			{GameDef.GameObj.Goal, ConsoleColor.Magenta},
		};

		
		public readonly static Dictionary<GameDef.GameObj, ConsoleColor> outputBackColor = new Dictionary<GameDef.GameObj, ConsoleColor>()
		{
			{GameDef.GameObj.ImmutableWall, ConsoleColor.Gray},
			{GameDef.GameObj.Fire, ConsoleColor.Red},
			{GameDef.GameObj.Wind, ConsoleColor.DarkGray},
			{GameDef.GameObj.Mud, ConsoleColor.Gray},
			{GameDef.GameObj.Glod, ConsoleColor.Yellow},
			{GameDef.GameObj.Water, ConsoleColor.Blue},
			{GameDef.GameObj.Wood, ConsoleColor.Gray},
			{GameDef.GameObj.Player, ConsoleColor.Gray},
			{GameDef.GameObj.FlowWater, ConsoleColor.Cyan},
			{GameDef.GameObj.Obsidian, ConsoleColor.Gray},
			{GameDef.GameObj.Log, ConsoleColor.Gray},
		};

		public readonly static Dictionary<GameDef.Towards, GameDef.WaterType> waterTypeMapByPlayerTowards = new Dictionary<Towards, WaterType>()
		{
			{GameDef.Towards.Left, GameDef.WaterType.Left},
			{GameDef.Towards.Right, GameDef.WaterType.Right},
			{GameDef.Towards.Down, GameDef.WaterType.Down},
			{GameDef.Towards.Up, GameDef.WaterType.Up},
		};

		public readonly static Dictionary<GameDef.GameObj, SkillType> SkillMap = new Dictionary<GameObj, SkillType>()
		{
			{GameObj.Fire, GameDef.SkillType.FlameBomb}
		};

		public readonly static HashSet<GameDef.GameObj> ElementCanbeLiftUpSet = new HashSet<GameObj>()
		{
			GameObj.Fire, GameObj.Water, GameObj.Glod, GameObj.Log, GameObj.Wind, GameObj.Obsidian
		};

		public readonly static int elemDictIdxBias = 1000;

		//public readonly static int wood_fire_time = 3;
		public readonly static int wood_tolerance_time = 3;

		public readonly static int MinObsidianGenFireNeedAirNum = 3;
		public readonly static int maxLiftUpNum = 3;
	}
	
}
