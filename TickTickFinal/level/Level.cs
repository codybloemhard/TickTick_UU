﻿using Microsoft.Xna.Framework;
using System;

partial class Level : GameObjectList
{
    protected bool locked, solved;
    protected Button quitButton;

    public Level(int levelIndex)
    {
        Add(new GameObjectList(1, "waterdrops"));
        Add(new GameObjectList(2, "enemies"));
        bullets = new GameObjectList(3, "bullets");
        Add(bullets);

        LoadTiles("Content/Levels/" + levelIndex + ".txt");
        // load the backgrounds
        GameObjectList backgrounds = new GameObjectList(0, "backgrounds");
        
        SpriteGameObject backgroundSky = new SpriteGameObject("Backgrounds/spr_sky", 0, "sky");
        backgroundSky.Position = new Vector2(0, WorldSize.Y - backgroundSky.Height);
        backgrounds.Add(backgroundSky);
        
        // add a few random mountains
        for (int i = 0; i < 30; i++)
        {
            SpriteGameObject mountain = new SpriteGameObject("Backgrounds/spr_mountain_1", (int)GameEnvironment.Random.Next(3));
            mountain.Position = new Vector2((float)GameEnvironment.Random.NextDouble() * WorldSize.X, 
                WorldSize.Y - mountain.Height);
            mountain.paralax = true;
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