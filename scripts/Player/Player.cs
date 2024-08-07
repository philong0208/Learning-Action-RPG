using Godot;
using System;

public class Player : KinematicBody2D
{
	private enum STATE
	{
		MOVE,
		ROLL,
		ATTACK
	};

	const float MAX_SPEED = 80;
	const float ACCELERATION = 500;
	const float FRICTION = 500;
	const float ROLL_SPEED = 125;

	public Vector2 velocity = Vector2.Zero;
	public Vector2 rollVector = Vector2.Down;
	STATE state = STATE.MOVE;

	private AnimationPlayer animationPlayer = null;
	private AnimationTree animationTree = null;
	private AnimationNodeStateMachinePlayback animationState;


	public override void _Ready()
	{
		animationPlayer = (AnimationPlayer)GetNode("AnimationPlayer");
		animationTree = (AnimationTree)GetNode("AnimationTree");
		animationState = (AnimationNodeStateMachinePlayback)animationTree.Get("parameters/playback");
		animationTree.Active = true;
	}
	public override void _PhysicsProcess(float delta)
	{
		switch (state)
		{
			case STATE.MOVE:
				{
					moveState(delta);
					break;
				}
			case STATE.ROLL:
				{
					rollState(delta);
					break;
				}
			case STATE.ATTACK:
				{
					attackState(delta);
					break;
				}
			default:
				{
					moveState(delta);
					break;
				}
		}
		
	}   
	
	public void moveState(float delta)
	{
		var inputVector = Vector2.Zero;

		inputVector.x = Input.GetActionStrength("ui_right") - Input.GetActionStrength("ui_left");
		inputVector.y = Input.GetActionStrength("ui_down") - Input.GetActionStrength("ui_up");
		inputVector = inputVector.Normalized();

		if (inputVector != Vector2.Zero)
		{
			rollVector = inputVector;
			animationTree.Set("parameters/Idle/blend_position", velocity);
			animationTree.Set("parameters/Run/blend_position", velocity);
			animationTree.Set("parameters/Attack/blend_position", velocity);
			animationTree.Set("parameters/Roll/blend_position", velocity);
			animationState.Travel("Run");
			velocity = velocity.MoveToward(inputVector * MAX_SPEED, delta * ACCELERATION);
		}
		else
		{
			animationState.Travel("Idle");
			velocity = velocity.MoveToward(Vector2.Zero, delta * FRICTION);
		}

		move();

		if (Input.IsActionJustPressed("roll"))
		{
			state = STATE.ROLL;
		}

		if (Input.IsActionJustPressed("attack"))
		{
			state = STATE.ATTACK;
		}
	}

	public void rollState(float dela)
	{
		velocity = rollVector * ROLL_SPEED;
		animationState.Travel("Roll");
		move();
	}

	public void attackState(float delta)
	{
		velocity = Vector2.Zero;
		animationState.Travel("Attack");
	}

	public void move()
	{
		velocity = MoveAndSlide(velocity); // Do not use MoveAndSlide() outside of _PhysicProcess(delta)
	}

	public void rollAnimationFinish()
	{
		velocity = velocity / 2;
		state = STATE.MOVE;
	}

	public void attackAnimationFinish()
	{
		state = STATE.MOVE;
	}
}
