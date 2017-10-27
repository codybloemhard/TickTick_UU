using Microsoft.Xna.Framework;

class VisibilityTimer : GameObject
{
    protected GameObject target;
    protected float timeLeft;
    protected float totalTime;

    public VisibilityTimer(GameObject target, int layer=0, string id = "")
        : base(layer, id)
    {
        totalTime = 5;
        timeLeft = 0;
        this.target = target;
    }

    public override void Update(GameTime gameTime)
    {
        timeLeft -= (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (timeLeft <= 0)
        {
            target.Visible = false;
        }
    }

    public void StartVisible()
    {
        timeLeft = totalTime;
        target.Visible = true;
    }
}
