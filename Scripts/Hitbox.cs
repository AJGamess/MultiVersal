using Godot;
using System;

[GlobalClass]
public partial class Hitbox : Area2D
{
    PlayerController playerController;
	private int damage;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		playerController = GetParent<PlayerController>();
		damage = playerController.meleeDamage;
        CollisionLayer = 2;
		CollisionMask = 0;
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
