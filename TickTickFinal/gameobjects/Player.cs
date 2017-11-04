using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

partial class Player : AnimatedGameObject
{
    protected Vector2 startPosition;
    protected bool isOnTheGround;
    protected float previousYPosition;
    protected bool isAlive;
    protected bool exploded;
    protected bool finished;
    protected bool walkingOnIce, walkingOnHot;

    protected bool left;
    protected GameObjectList blist;
    protected Random r;
    protected TimerGameObject timer;
    protected SpriteGameObject sky;

    public Player(Vector2 start) : base(2, "player")
    {
        LoadAnimation("Sprites/Player/spr_idle", "idle", true); 
        LoadAnimation("Sprites/Player/spr_run@13", "run", true, 0.05f);
        LoadAnimation("Sprites/Player/spr_jump@14", "jump", false, 0.05f); 
        LoadAnimation("Sprites/Player/spr_celebrate@14", "celebrate", false, 0.05f);
        LoadAnimation("Sprites/Player/spr_die@5", "die", false);
        LoadAnimation("Sprites/Player/spr_explode@5x5", "explode", false, 0.04f);

        r = new Random();

        startPosition = start;
        Reset();
    }
    
    public override void Reset()
    {
        position = startPosition;
        velocity = Vector2.Zero;
        isOnTheGround = true;
        isAlive = true;
        exploded = false;
        finished = false;
        walkingOnIce = false;
        walkingOnHot = false;
        PlayAnimation("idle");
        previousYPosition = BoundingBox.Bottom;
    }

    public override void HandleInput(InputHelper inputHelper)
    {
        float walkingSpeed = 400;
        if (walkingOnIce)
            walkingSpeed *= 1.5f;
        if (!isAlive)
            return;
        if (inputHelper.IsKeyDown(Keys.Left))
        {
            velocity.X = -walkingSpeed;
            left = true;
        }
        else if (inputHelper.IsKeyDown(Keys.Right))
        {
            velocity.X = walkingSpeed;
            left = false;
        }
        else if (!walkingOnIce && isOnTheGround)
            velocity.X = 0.0f;
        if (velocity.X != 0.0f)
            Mirror = velocity.X < 0;
        if (inputHelper.KeyPressed(Keys.Up) && isOnTheGround)
            Jump();
        //shoot
        if (inputHelper.KeyPressed(Keys.Space))
            Shoot();
    }
    //zet dereferentie naar alle bullets
    public void SetBullets(GameObjectList b)
    {
        blist = b;
    }
    //haal alle bullets op
    public GameObjectList Bullets { get { return blist; } }
    //verwijder bullets als ze te oud zijn
    public void HandleBullets()
    {
        for(int i = 0; i < blist.Children.Count; i++)
        {
            if ((blist.Children[i] as Bullet).FramesAlive > 600)
                blist.Remove(blist.Children[i]);
        }
    }
    //maak de bullets en stop ze in de lijst
    public void Shoot()
    {
        if(timer == null)
            timer = GameWorld.Find("timer") as TimerGameObject;
        timer.DecreaseSeconds(1);
        for (int i = 0; i < 16; i++)
        {
            Bullet b = new Bullet();
            b.Spawn(left, position - (Vector2.UnitY * Height / 2), r);
            blist.Add(b);
        }
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        //volg speler
        Camera.FollowPlayer(position);
        HandleBullets();
        //laat de lucht nooit uit het scherm gaan
        if(sky == null)
            sky = GameWorld.Find("sky") as SpriteGameObject;
        sky.Position = Camera.TopLeft - new Vector2(100, 100);
        if (!finished && isAlive)
        {
            if (isOnTheGround)
            {
                if (velocity.X == 0)
                {
                    PlayAnimation("idle");
                }
                else
                {
                    PlayAnimation("run");
                }
            }
            else if (velocity.Y < 0)
            {
                PlayAnimation("jump");
            }

            if(timer == null)
                timer = GameWorld.Find("timer") as TimerGameObject;
            if (walkingOnHot)
            {
                timer.Multiplier = 2;
            }
            else if (walkingOnIce)
            {
                timer.Multiplier = 0.5;
            }
            else
            {
                timer.Multiplier = 1;
            }

            TileField tiles = GameWorld.Find("tiles") as TileField;
            if (BoundingBox.Top >= tiles.Rows * tiles.CellHeight)
            {
                Die(true);
            }
        }

        DoPhysics();
    }

    public void Explode()
    {
        if (!isAlive || finished)
        {
            return;
        }
        isAlive = false;
        exploded = true;
        velocity = Vector2.Zero;
        position.Y += 15;
        PlayAnimation("explode");
    }

    public void Die(bool falling)
    {
        if (!isAlive || finished)
        {
            return;
        }
        isAlive = false;
        velocity.X = 0.0f;
        if (falling)
        {
            GameEnvironment.AssetManager.PlaySound("Sounds/snd_player_fall");
        }
        else
        {
            velocity.Y = -900;
            GameEnvironment.AssetManager.PlaySound("Sounds/snd_player_die");
        }
        PlayAnimation("die");
    }

    public bool IsAlive
    {
        get { return isAlive; }
    }

    public bool Finished
    {
        get { return finished; }
    }

    public void LevelFinished()
    {
        finished = true;
        velocity.X = 0.0f;
        PlayAnimation("celebrate");
        GameEnvironment.AssetManager.PlaySound("Sounds/snd_player_won");
    }
}