using Godot;
using System;

public class Stats : Node
{
    [Export]
    public int maxHealth = 1;
    public int health;
    public int Health
    {
        get
        {
            return this.health;
        }
        set
        {
            this.health = value;
            if (this.health <= 0)
            {
                EmitSignal("noHealth");
            }
        }

    }


    public override void _Ready()
    {
        health = maxHealth;
    }

    [Signal]
    public delegate void noHealth();
}