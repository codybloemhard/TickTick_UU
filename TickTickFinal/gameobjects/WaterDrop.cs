using System;
using Microsoft.Xna.Framework;

class WaterDrop : SpriteGameObject
{
    protected float bounce;

    public WaterDrop(int layer=0, string id="") : base("Sprites/spr_water", layer, id) 
    {
    }

    public override void Update(GameTime gameTime)
    {
        double t = gameTime.TotalGameTime.TotalSeconds * 3.0f + Position.X;
        bounce = (float)Math.Sin(t) * 0.2f;
        position.Y += bounce;
        Player player = GameWorld.Find("player") as Player;
        if (visible && CollidesWith(player))
        {
            visible = false;
            GameEnvironment.AssetManager.PlaySound("Sounds/snd_watercollected");
        }
    }
}
