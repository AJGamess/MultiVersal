using Godot;
using System;

[GlobalClass]
public partial class Hitbox : Area2D
{
    PlayerController playerController;
	HurtBox HurtBox;
    private int damage;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        // Initialize variables
        playerController = GetParent<PlayerController>();
		HurtBox = playerController.GetNode<HurtBox>("HurtBox");
        // Set damage value
        damage = playerController.meleeDamage;
        // Set collision layer and mask
        CollisionLayer = 2;
		CollisionMask = 0;
        // Connect area entered signal
        AreaEntered += Hitbox_AreaEntered;
    }
    // Area entered method
    private void Hitbox_AreaEntered(Area2D area)
    {
        //check if the area is a HurtBox and not the player's own HurtBox
        if (area is HurtBox hitHurtBox && area != HurtBox)
        {
            // Apply damage to the entity that owns the HurtBox
            var owner = hitHurtBox.GetParent<Node>();
            if (owner is PlayerController enemyController)
            {
                enemyController.health -= damage;
                GD.Print("Hit! Enemy Health: " + enemyController.health);
            }
        }
    }
    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
	}
}
