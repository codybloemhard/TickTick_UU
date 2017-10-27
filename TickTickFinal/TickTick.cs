using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using System;
using System.Runtime.InteropServices;

class TickTick : GameEnvironment
{
    /*
    Pressing the [X] button on the monogame window
    does not always close the process, but if you close the
    console the process is killed too so this is needed.
    Get console to show up:
    source: https://stackoverflow.com/questions/4362111/how-do-i-show-a-console-output-window-in-a-forms-application
    */
    [DllImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    static extern bool AllocConsole();

    [STAThread]
    static void Main()
    {
        AllocConsole();
        TickTick game = new TickTick();
        game.Run();
    }

    public TickTick()
    {
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }
    
    protected override void LoadContent()
    {
        base.LoadContent();
        screen = new Point(1440, 825);
        windowSize = new Point(1024, 586);
        FullScreen = false;
        
        gameStateManager.AddGameState("titleMenu", new TitleMenuState());
        gameStateManager.AddGameState("helpState", new HelpState());
        gameStateManager.AddGameState("playingState", new PlayingState(Content));
        gameStateManager.AddGameState("levelMenu", new LevelMenuState());
        gameStateManager.AddGameState("gameOverState", new GameOverState());
        gameStateManager.AddGameState("levelFinishedState", new LevelFinishedState());
        gameStateManager.SwitchTo("titleMenu");

        MediaPlayer.Volume = 0.0f;
        AssetManager.PlayMusic("Sounds/snd_music");
    }
}