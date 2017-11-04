using System;
using Microsoft.Xna.Framework;

class Bullet : SpriteGameObject
{
    private bool shooting = false;
    private bool facing;
    private Vector2 vel;
    private uint framesAlive;

    public Bullet() : base("Sprites/spr_water")
    {
        Reset();
    }

    public override void Reset()
    {
        visible = false;
        position = Vector2.Zero;
        velocity = Vector2.Zero;
        shooting = false;
        vel = Vector2.Zero;
    }

    public void Spawn(bool facing, Vector2 pos, Random r)
    {
        visible = true;
        this.facing = facing;
        position = pos;
        shooting = true;
        vel = new Vector2(1, (float)(r.NextDouble() - 0.5f) * 0.4f);
        vel.Normalize();
        framesAlive = 0;
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        if (position.X < 0 || position.X > Camera.WorldSize.X
            || position.Y < 0 || position.Y > Camera.WorldSize.Y)
            framesAlive = 601;
        framesAlive++;
        if (shooting && facing != true)
            this.Velocity = vel * 600;
        if (shooting && facing)
            this.Velocity = vel * -600;
    }

    public uint FramesAlive { get { return framesAlive; } }
}