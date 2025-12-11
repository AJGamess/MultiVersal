using Godot;
using System;

[GlobalClass]
public partial class HurtBox : Area2D
{
    Signal hurt;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		CollisionLayer = 0;
		CollisionMask = 2;

    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}
}
