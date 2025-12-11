using Godot;
using System;

public partial class EnemyController : CharacterBody2D
{
	public int health = 100;
    public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;
        // Add the gravity.
        if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
		}
		Velocity = velocity;
		MoveAndSlide();
    }
}