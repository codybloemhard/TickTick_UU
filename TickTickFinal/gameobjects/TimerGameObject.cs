using System;
using Microsoft.Xna.Framework;

class TimerGameObject : TextGameObject
{
    protected TimeSpan timeLeft;
    protected bool running;
    protected double multiplier;
    protected int seconds = 0;

    public TimerGameObject(int seconds, int layer = 0, string id = "")
        : base("Fonts/Hud", layer, id)
    {
        this.seconds = seconds;
        multiplier = 1;
        timeLeft = TimeSpan.FromSeconds(seconds);
        running = true;
    }

    public override void Update(GameTime gameTime)
    {
        if (!running)
        {
            return;
        }
        double totalSeconds = gameTime.ElapsedGameTime.TotalSeconds * multiplier;
        timeLeft -= TimeSpan.FromSeconds(totalSeconds);
        if (timeLeft.Ticks < 0)
        {
            return;
        }
        DateTime timeleft = new DateTime(timeLeft.Ticks);
        text = timeleft.ToString("mm:ss");
        color = Color.Yellow;
        if (timeLeft.TotalSeconds <= 10 && (int)timeLeft.TotalSeconds % 2 == 0)
        {
            color = Color.Red;
        }
    }

    public override void Reset()
    {
        base.Reset();
        timeLeft = TimeSpan.FromSeconds(seconds);
        running = true;
    }

    public bool Running
    {
        get { return running; }
        set { running = value; }
    }

    public double Multiplier
    {
        get {return multiplier; }
        set { multiplier = value; }
    }

    public bool GameOver
    {
        get { return (timeLeft.Ticks <= 0); }
    }
}