using Godot;
using System;

public class CreateEffectRepository : Node, IDeathEffect
{
    public void CreateEffect(string scene, Node parent, Vector2 position)
    {
        var effect = GD.Load<PackedScene>(scene).Instance() as AnimatedSprite;
        effect.GlobalPosition = position;
        parent.AddChild(effect);
    }
}
