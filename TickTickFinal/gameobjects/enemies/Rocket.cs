using Microsoft.Xna.Framework;
using System.Collections.Generic;

class Rocket : AnimatedGameObject
{
    protected double spawnTime;
    protected Vector2 startPosition;
    protected Player player = null;

    public Rocket(bool moveToLeft, Vector2 startPosition)
    {
        LoadAnimation("Sprites/Rocket/spr_rocket@3", "default", true, 0.2f);
        PlayAnimation("default");
        Mirror = moveToLeft;
        this.startPosition = startPosition;
        Reset();
    }

    public override void Reset()
    {
        visible = false;
        position = startPosition;
        velocity = Vector2.Zero;
        spawnTime = GameEnvironment.Random.NextDouble() * 5;
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        if (spawnTime > 0)
        {
            spawnTime -= gameTime.ElapsedGameTime.TotalSeconds;
            return;
        }
        visible = true;
        velocity.X = 600;
        if (Mirror)
            this.velocity.X *= -1;
        CheckPlayerCollision();
        //kijk of de rocket buiten het level zit
        Rectangle screenBox = new Rectangle(0, 0, (int)Camera.WorldSize.X, (int)Camera.WorldSize.Y);
        if (!screenBox.Intersects(this.BoundingBox))
            Reset();
    }
    
    public void CheckPlayerCollision()
    {
        if(player == null)
            player = GameWorld.Find("player") as Player;
        if (CollidesWith(player) && visible)
        {
            if(player.Velocity.Y > 0 && player.IsAlive)
            {
                Reset();
                player.Jump(1000);
            }
            else player.Die(false);
        }
        //check collision met alle bullets
        List<GameObject> bullets = player.Bullets.Children;
        for(int i = 0; i < bullets.Count; i++)
            if (CollidesWith(bullets[i] as SpriteGameObject))
            {
                Reset();
                (bullets[i] as Bullet).Reset();
            }
    }
}