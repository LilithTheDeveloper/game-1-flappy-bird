using Godot;

public partial class Bird : CharacterBody2D
{

	[Export]
	public Label gameOverLabel;

	[Export]
	public Label scoreLabel;

	[Export]
	public int JumpForce { get; set; } = 400;
	public float Gravity = 800;
	public float Tilt = 35f;
	public float TiltGrowth = 10f;



	//clamps
	private float maxAcceleration = 800f;

	private float maxY = 600f;
	private float minY = 0f;

	public bool isDead = false;

	public int score = 0;

	public void GetInput(double delta)
	{
		var screenHeight = GetViewportRect().Size.Y;

		maxY = screenHeight;

		//this is gravity  i think
		Velocity = new Vector2(Velocity.X, Velocity.Y + Gravity * (float)delta);

		float ClampedVelocityY = Mathf.Clamp(Velocity.Y, -maxAcceleration, maxAcceleration);
		Velocity = new Vector2(Velocity.X, ClampedVelocityY);

		if (Input.IsActionJustPressed("jump") && !isDead)
		{
			Velocity = new Vector2(Velocity.X, -JumpForce);
		}

		if (Velocity.Y >= 0) RotationDegrees += TiltGrowth;
		else RotationDegrees -= TiltGrowth;

		float rotationClamp = Mathf.Clamp(RotationDegrees, -Tilt, Tilt);
		RotationDegrees = rotationClamp;
	}

	public override void _Process(double delta)
	{
		scoreLabel.Text = score.ToString();
	}


	public override void _PhysicsProcess(double delta)
	{
		GetInput(delta);
		KinematicCollision2D collision = MoveAndCollide(Velocity * (float)delta);
		Position = new Vector2(Position.X, Mathf.Clamp(Position.Y, minY, maxY));

		//die if fall to ground
		if (Position.Y >= maxY)
		{
			isDead = true;
		}
		//Die if collide with wall
		else if (collision != null && collision.GetNormal().X != 0)
		{
			isDead = true;
		}
		if (isDead) gameOverLabel.Text = "Game Over";
	}


}
