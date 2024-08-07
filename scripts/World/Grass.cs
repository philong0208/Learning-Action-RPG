using Godot;
using System;

public class Grass : Node2D
{

	//public static PackedScene _GrassEffect = (PackedScene)GD.Load("res://scenes/Effects/GrassEffect.tscn");
	//public void createGrassEffect()
	//{
	//	//PackedScene _GrassEffect = (PackedScene)ResourceLoader.Load("res://scenes/Effects/GrassEffect.tscn");
	//	Node2D grassEffect = (Node2D)_GrassEffect.Instance(); // Add intance of a scene. In this case, adding GrassEffec.tscn as a child of Grass
	//	Node world = GetTree().CurrentScene;
	//	world.AddChild(grassEffect);
	//	grassEffect.GlobalPosition = GlobalPosition; // Apply grassEffect to all Grass.tscn
	//}

	CreateEffectRepository _effect;

    public override void _Ready()
    {
        _effect = new CreateEffectRepository();
    }

    private void onHurtBoxAreaEntered(object area)
	{
		createGrassEffectInterface(_effect);
		QueueFree();
	}

    private void createGrassEffectInterface(IDeathEffect effect)
    {
        // This is how to inject Interfaces and use it
        effect.CreateEffect("res://scenes/Effects/GrassEffect.tscn", GetParent(), GlobalPosition);
    }

}



