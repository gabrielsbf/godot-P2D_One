using Godot;
using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Reflection;

public partial class protagonist : CharacterBody2D
{
	public const float Speed = 150.0f;
	public const float JumpVelocity = -400.0f;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();
	
	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;
		var animatedSprite2d = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		// Add the gravity.
		animatedSprite2d.SpriteFrames.SetAnimationLoop("Jump", false);
		if (!IsOnFloor())
		{
			animatedSprite2d.Animation = "Jump";
			velocity.Y += gravity * (float)delta;
			if (velocity.Y < 0)
				animatedSprite2d.SetFrameAndProgress(0, 0);
			if (velocity.Y > 0 && velocity.Y <= 40)
				animatedSprite2d.SetFrameAndProgress(1,0);
			if (velocity.Y > 40)
				animatedSprite2d.SetFrameAndProgress(2,0);
		}
		
		// Handle Jump.
		if (Input.IsActionJustPressed("jump") && IsOnFloor())
		{
			velocity.Y = JumpVelocity;
		}

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 direction = Input.GetVector("move_left", "move_right", "jump", "move_down");
		if (direction != Vector2.Zero)
		{
			velocity.X = direction.X * Speed;
			animatedSprite2d.FlipH = velocity.X < 0;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
		}
		if(velocity.X != 0 && IsOnFloor())
		{
			animatedSprite2d.Animation = "Run";
		}

		else if (velocity.X == 0 && IsOnFloor())
		{
			animatedSprite2d.Animation = "Idle";
			
		}
		if(IsOnFloor())
			animatedSprite2d.Play();
		Velocity = velocity;
		MoveAndSlide();
	}
}
