using Godot;
using System;

public partial class FightingGameCamera : Camera2D
{
    // variables that can be adjusted in the editor
	[Export]
	public float zoomOffset = 0.3f;
	[Export]
	bool debugMode = false;
    [Export]
    CharacterBody2D player;
    [Export]
    CharacterBody2D opponent;

    // variables here for fighting game camera behavior
    Rect2 viewportRect;
	Rect2 cameraRect;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        //Get the initial viewport rectangle
        viewportRect = GetViewportRect();
        // Enable or disable processing based on whether there are child nodes
        SetProcess(GetChildCount() > 0);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        if (player == null || opponent == null)
        {
            GD.PushError("Camera Error: Player or Enemy is NOT assigned.");
            return;
        }

        Vector2 p1 = player.GlobalPosition;
        Vector2 p2 = opponent.GlobalPosition;

        // TEMP debug:
        GD.Print("P1: ", p1, "  P2: ", p2);

        // Create a small rect at player
        cameraRect = new Rect2(p1 - new Vector2(1, 1), new Vector2(2, 2));

        // Expand to enemy position
        cameraRect = cameraRect.Expand(p2);

        // Calculate center and zoom
        Vector2 targetCenter = CalculateCenter(cameraRect);
        Vector2 targetZoom = CalculateZoom(cameraRect, viewportRect.Size);

        // Move camera
        GlobalPosition = GlobalPosition.Lerp(targetCenter, 5f * (float)delta);
        Zoom = Zoom.Lerp(targetZoom, 5f * (float)delta);

        //If debug mode is enabled, draw the rectangles
        _Draw();
    }
    //Calculate the center point of a rectangle
    private Vector2 CalculateCenter(Rect2 rect)
    {
        //Return the center point of the rectangle
        return new Vector2(rect.Position.X + rect.Size.X / 2, rect.Position.Y + rect.Size.Y / 2);
    }
    //Calculate the center point of a rectangle given the viewport size
    private Vector2 CalculateZoom(Rect2 rect, Vector2 viewport_size)
    {
        //Calculate the maximum zoom level needed to fit the rectangle within the viewport
        //This is needed to prevent aspect ratio distortion
        float maxZoom = Math.Max(rect.Size.X / viewport_size.X, rect.Size.Y / viewport_size.Y);
        //Return the zoom level with the zoom offset applied
        return new Vector2(maxZoom + zoomOffset, maxZoom + zoomOffset);
    }
    
    public override void _Draw()
    {
        if (debugMode)
        {
            //Draw the camera rectangle in white
            DrawRect(cameraRect, Colors.White, true);
            //Draw a Circle at the center of the camera rectangle in red
            DrawCircle(CalculateCenter(cameraRect), 5, Colors.Red);
        }
    }
}
