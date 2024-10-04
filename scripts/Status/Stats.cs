using Godot;
using System;

public class Stats : Node
{
    [Export]
    public int maxHealth = 1;
    public int health;

    public int Health
    {
        get => health;
        set => setHealth(value);
    }

    public int MaxHealth
    {
        get => maxHealth;
        set => setMaxHealth(value);
    }

    public override void _Ready()
    {
        this.health = maxHealth;
    }

    public void setHealth(int value)
    {
        this.health = value;
        EmitSignal("healthChanged", Health);
        if (this.health <= 0)
        {
            EmitSignal("noHealth");
        }
    }

    public void setMaxHealth(int value)
    {
        maxHealth = value;
        this.Health = Mathf.Min(Health, MaxHealth);
        EmitSignal("maxHealthChanged", MaxHealth);
    }

    [Signal]
    public delegate void noHealth();

    [Signal]
    public delegate void healthChanged(int value);

    [Signal]
    public delegate void maxHealthChanged(int value);
}