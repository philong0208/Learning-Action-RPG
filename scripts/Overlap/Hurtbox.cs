using Godot;
using System;

public class Hurtbox : Area2D
{
    [Export] bool showHitFX = true;
    CreateEffectRepository _effect;

    public override void _Ready()
    {
        _effect = new CreateEffectRepository();
    }

    private void onHurtBoxAreaEntered(Area2D area)
    {
        if (showHitFX)
        {
            createHitEffectInterface(_effect);
        }
    }

    private void createHitEffectInterface(IDeathEffect effect)
    {
        // This is how to inject Interfaces and use it
        effect.CreateEffect("res://scenes/Effects/HitEffect.tscn", GetTree().CurrentScene, GlobalPosition);
    }
}
