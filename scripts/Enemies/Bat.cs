using Godot;
using System;

public class Bat : KinematicBody2D
{
	[Export]
	int MAX_SPEED = 50;
	[Export]
	int ACCELERATION = 500;
	[Export]
	int FRICTION = 500;

	private const float KNOCKBACK_FORCE = 120;
	private Vector2 velocity = Vector2.Zero;
	private Vector2 knockback = Vector2.Zero;

	private AnimatedSprite sprite;
	private Stats stats;
	private SwordHitbox swordHitbox;
	private PlayerDetectionZone zone;
	private Hurtbox hurtbox;
	private AnimationPlayer blinkAnimationPlayer;

	CreateEffectRepository _effect;

	private enum STATE { 
		IDLE,
		WANDER,
		CHASE
	};
	STATE state = STATE.CHASE;

	public override void _Ready()
	{
		sprite = (AnimatedSprite)GetNode("AnimatedSprite");
		stats = (Stats)GetNode("Stats");
		zone = (PlayerDetectionZone)GetNode("PlayerDetectionZone");
		swordHitbox = (SwordHitbox)GetNode("/root/Game/YSort/Player/HitboxPivot/SwordHitbox");
		hurtbox = (Hurtbox)GetNode("HurtBox");
		_effect = new CreateEffectRepository();
		blinkAnimationPlayer = (AnimationPlayer)GetNode("BlinkAnimationPlayer");
	}
	public override void _PhysicsProcess(float delta)
	{
		knockback = knockback.MoveToward(Vector2.Zero, delta * 200);
		knockback = MoveAndSlide(knockback);

		switch (state)
		{
			case STATE.IDLE:
				{
					velocity = velocity.MoveToward(Vector2.Zero, delta * FRICTION);
					seekPlayer();
					break;
				}
			case STATE.WANDER:
				{ 
					break;
				}
			case STATE.CHASE:
				{
					var player = zone.player as Player;
					if(player != null)
					{
						var distanceToPlayerLocation = (player.GlobalPosition - GlobalPosition).Normalized();
                        velocity = velocity.MoveToward(distanceToPlayerLocation * MAX_SPEED , delta * ACCELERATION);
                    }
					else
					{
						velocity = velocity.MoveToward(Vector2.Zero, delta * ACCELERATION);
						state = STATE.IDLE;
					}
                    sprite.FlipH = velocity.x < 0;
                    break;
				}

		}
		
		velocity = MoveAndSlide(velocity);
	}

	private void seekPlayer()
	{
		if(zone.canSeePlayer())
		{
			state = STATE.CHASE;
		}
	}
	private void onHurtBoxAreaEntered(Area2D area)
	{
		var pivot = area.GetParent();
		var player = (Player)pivot.GetParent();

        knockback = player.rollVector * KNOCKBACK_FORCE; // player.rollVector: Get the direction player is facing and make it knockback direction.
        stats.Health -= swordHitbox.damage;
		hurtbox.createHitEffect();
		hurtbox.startInvincibility((float)0.4);
	}

	private void onStatsNoHealth()
	{
		createDeathEffectInterface(_effect);
        QueueFree();
	}

	private void onHurtBoxInvincibleStarted()
	{
		blinkAnimationPlayer.Play("Start");
	}

	private void onHurtBoxInvincibleEnded()
	{
		blinkAnimationPlayer.Play("Stop");
	}

    private void createDeathEffectInterface(IDeathEffect effect)
	{
		// This is how to inject Interfaces and use it
		effect.CreateEffect("res://scenes/Effects/EnemyDeathEffect.tscn", GetParent(), GlobalPosition);
	}
}
