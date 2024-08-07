using Godot;
using System;

public class Effect : AnimatedSprite
{
	public override void _Ready()
	{
        Connect("animation_finished", this, "onAnimatedSpriteAnimationFinished");
		Play("Animate");
	}
	
	private void onAnimatedSpriteAnimationFinished()
	{
		QueueFree();
	}
}



