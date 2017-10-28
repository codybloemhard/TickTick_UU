using Microsoft.Xna.Framework;
using System;

partial class Level : GameObjectList
{
    protected bool locked, solved;
    protected Button quitButton;

    public Level(int levelIndex)
    {
        Add(new GameObjectList(1, "waterdrops"));
        Add(new GameObjectList(2, "enemies"));
        LoadTiles("Content/Levels/" + levelIndex + ".txt");
        // load the backgrounds
        GameObjectList backgrounds = new GameObjectList(0, "backgrounds");
        //zorg dat de lucht altijd in beeld is
        int xline = 0;
        while(xline < WorldSize.X)
        {
            SpriteGameObject backgroundSky = new SpriteGameObject("Backgrounds/spr_sky");
            backgroundSky.Position = new Vector2(xline, WorldSize.Y - backgroundSky.Height);
            backgrounds.Add(backgroundSky);
            xline += backgroundSky.Width;
        }
        
        // add a few random mountains
        for (int i = 0; i < 5; i++)
        {
            SpriteGameObject mountain = new SpriteGameObject("Backgrounds/spr_mountain_" + (GameEnvironment.Random.Next(2) + 1), 1);
            mountain.Position = new Vector2((float)GameEnvironment.Random.NextDouble() * WorldSize.X - (mountain.Width / 2), 
                WorldSize.Y - mountain.Height);
            backgrounds.Add(mountain);
        }

        Clouds clouds = new Clouds(2);
        backgrounds.Add(clouds);
        Add(backgrounds);
        
        SpriteGameObject timerBackground = new SpriteGameObject("Sprites/spr_timer", 100);
        timerBackground.Position = new Vector2(10, 10);
        timerBackground.UI = true;
        Add(timerBackground);

        quitButton = new Button("Sprites/spr_button_quit", 100);
        quitButton.Position = new Vector2(GameEnvironment.Screen.X - quitButton.Width - 10, 10);
        quitButton.UI = true;
        Add(quitButton);
        
        TimerGameObject timer = new TimerGameObject(LevelTimeSeconds, 101, "timer");
        timer.Position = new Vector2(25, 30);
        Add(timer);
    }

    public bool Completed
    {
        get
        {
            SpriteGameObject exitObj = Find("exit") as SpriteGameObject;
            Player player = Find("player") as Player;
            if (!exitObj.CollidesWith(player))
            {
                return false;
            }
            GameObjectList waterdrops = Find("waterdrops") as GameObjectList;
            foreach (GameObject d in waterdrops.Children)
            {
                if (d.Visible)
                {
                    return false;
                }
            }
            return true;
        }
    }

    public bool GameOver
    {
        get
        {
            TimerGameObject timer = Find("timer") as TimerGameObject;
            Player player = Find("player") as Player;
            return !player.IsAlive || timer.GameOver;
        }
    }

    public bool Locked
    {
        get { return locked; }
        set { locked = value; }
    }

    public bool Solved
    {
        get { return solved; }
        set { solved = value; }
    }
}