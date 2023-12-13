using Godot;
using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Reflection;

public partial class protagonist : CharacterBody2D
{
	private const float Speed = 150.0f;
	private const float JumpVelocity = -400.0f;
	private const float Acceleration = 5f;
	private const float Friction = 10f;
	private float atk_timer = 0.5f;
	private float atk_timer_reset = 0.5f;
	private bool is_atack = false;
	

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();
	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;
		Vector2 direction = Input.GetVector("move_left", "move_right", "jump", "move_down");

		var animatedSprite2d = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		var collisionshape2d = GetNode<CollisionShape2D>("Area2D/CollisionShape2D");
		animatedSprite2d.SpriteFrames.SetAnimationLoop("Jump", false);
		animatedSprite2d.SpriteFrames.SetAnimationLoop("Atack_1", false);
		
		//DINAMICS and PHYSICS
		if (!IsOnFloor())
			velocity.Y += gravity * (float)delta;
	
		if (Input.IsActionJustPressed("jump") && IsOnFloor())
		{
			velocity.Y = JumpVelocity;
		}
		if (Input.IsActionJustPressed("atack"))
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Friction);
			is_atack = true;
			collisionshape2d.Disabled = false;

			atk_timer = atk_timer_reset;

		}
		if (direction != Vector2.Zero)
		{
			velocity.X = Mathf.MoveToward(velocity.X, direction.X * Speed, Acceleration);
			animatedSprite2d.FlipH = velocity.X < 0;
		}
		else
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Friction);

		Velocity = velocity;
		MoveAndSlide();

		//ANIMATION
		if(!is_atack)
		{
			if(IsOnFloor())
			{
				if(velocity.X != 0)
					animatedSprite2d.Animation = "Run";

				if (velocity.X == 0)
					animatedSprite2d.Animation = "Idle";

				animatedSprite2d.Play();
			}
			else
			{
				animatedSprite2d.Animation = "Jump";
				if (velocity.Y < 0)
					animatedSprite2d.SetFrameAndProgress(0, 0);
				if (velocity.Y > 0 && velocity.Y <= 40)
					animatedSprite2d.SetFrameAndProgress(1,0);
				if (velocity.Y > 40)
					animatedSprite2d.SetFrameAndProgress(2,0);
			}
		}
		else if (is_atack)
		{
			animatedSprite2d.Animation = "Atack_1";
			animatedSprite2d.Play();
			atk_timer -= (float)delta;
			if(atk_timer < 0)
				is_atack = false;
		}

	}
	private void _on_area_2d_body_entered(Node2D body)
{
	// Replace with function body.
}

}


