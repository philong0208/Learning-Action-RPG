using Godot;
using System;

public class PlayerDetectionZone : Area2D
{
    public Node player = null;
    public override void _Ready()
    {
        
    }

    public bool canSeePlayer()
    {
        return player != null;
    }

    public void onPlayerDetectionZoneBodyEntered(Node node)
    {
        player = node;
    }
    public void onPlayerDetectionZoneBodyExited(Node node)
    {
        player = null;
    }
}
