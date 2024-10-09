using Godot;
using System;

public class Hurtbox : Area2D
{
    private CollisionShape2D hurtboxCollision;

    private bool _invincible = false;
    private bool invincible { 
        get { return _invincible; }
        set 
        {
            if (_invincible != value)
            {
                _invincible = value;
                if (_invincible)
                {
                    EmitSignal("invincibleStarted");
                }
                else
                {
                    EmitSignal("invincibleEnded");
                }
            }
        } 
    }

    Timer timer;

    CreateEffectRepository _effect;

    public override void _Ready()
    {
        timer = (Timer)GetNode("InvincibleTimer");
        hurtboxCollision = (CollisionShape2D)GetNode("HurtboxCollisionShape2D");
        _effect = new CreateEffectRepository();
    }

    public void createHitEffect()
    {
        createHitEffectInterface(_effect);
    }

    public void startInvincibility(float duration)
    {
        this.invincible = true;
        timer.Start(duration);
    }

    private void onInvincibleTimerTimeout()
    {
        this.invincible = false;
    }

    private void onHurtBoxInvincibleEnded()
    {
        //Monitoring = true;
        hurtboxCollision.Disabled = false;
    }

    private void onHurtBoxInvincibleStarted()
    {
        hurtboxCollision.SetDeferred("disabled", true);
    }

    private void createHitEffectInterface(IDeathEffect effect)
    {
        // This is how to inject Interfaces and use it
        effect.CreateEffect("res://scenes/Effects/HitEffect.tscn", GetTree().CurrentScene, GlobalPosition);
    }

    [Signal]
    public delegate void invincibleStarted();
    [Signal]
    public delegate void invincibleEnded();
}
