using Godot;
using System;

public partial class PlayerController : CharacterBody2D
{
    // Movement fields
    public const float Speed = 150.0f;
    public const float JumpVelocity = -500.0f;

    // Animation fields
    AnimationPlayer animationPlayer;

    // Combo fields
    private int comboStep = 0;
    private float comboTimer = 0f;
    private const float comboMaxTime = 1f; // time allowed to press next attack

    // Fighting game fields
    public int health = 100;
    public int meleeDamage = 5;

    // Called when the node enters the scene tree for the first time
    public override void _Ready()
    {
        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
    }

    // Called every physics frame. 'delta' is the elapsed time since the previous frame
    public override void _PhysicsProcess(double delta)
    {
        // Handle COMBO INPUT
        HandleCombo((float)delta);

        // Movement AFTER attack logic
        HandleMovement((float)delta);
    }
    // Handle combo input and timing
    private void HandleCombo(float delta)
    {
        // Reduce combo timer
        if (comboTimer > 0)
            comboTimer -= delta;
        else
            comboStep = 0; // reset if timer expired

        if (Input.IsActionJustPressed("Light Attack"))
        {
            comboStep++;

            switch (comboStep)
            {
                case 1:
                    animationPlayer.Play("LightAttack1");
                    comboTimer = comboMaxTime;
                    break;

                case 2:
                    animationPlayer.Play("LightAttack2");
                    comboTimer = comboMaxTime;
                    break;

                case 3:
                    meleeDamage *= 2; // increase damage for heavy attack
                    animationPlayer.Play("HeavyAttack");
                    comboStep = 0; // reset after heavy
                    comboTimer = 0;
                    meleeDamage /= 2; // reset damage
                    break;

                default:
                    comboStep = 1;
                    animationPlayer.Play("Light Attack 1");
                    comboTimer = comboMaxTime;
                    break;
            }
        }
        else if (Input.IsActionJustPressed("Heavy Attack"))
        {
            // Heavy attack resets combo
            meleeDamage *= 2; // increase damage for heavy attack
            animationPlayer.Play("HeavyAttack");
            comboStep = 0;
            comboTimer = 0;
            meleeDamage /= 2; // reset damage
        }
    }
    // Handle player movement and animations
    private void HandleMovement(float delta)
    {
        Vector2 velocity = Velocity;

        if (!IsOnFloor())
            velocity += (GetGravity() / 1.5f) * delta;

        if (Input.IsActionJustPressed("Jump") && IsOnFloor())
            velocity.Y = JumpVelocity;

        Vector2 direction = Input.GetVector("Move Left", "Move Right", "ui_up", "ui_down");

        if (direction != Vector2.Zero)
        {
            velocity.X = direction.X * Speed;

            if (direction.X < 0)
                animationPlayer.Play("Float Backwards");
            else if (direction.X > 0)
                animationPlayer.Play("Float Forwards");
            else
                animationPlayer.Play("Idle");
        }
        else
        {
            velocity.X = Mathf.MoveToward(velocity.X, 0, Speed);
        }

        Velocity = velocity;
        MoveAndSlide();
    }
}
