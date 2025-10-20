using Godot;
using System;
using System.ComponentModel.DataAnnotations;

public partial class PipeController : Node2D
{

    [Export] public float minHeightFraction = 0.2f;
    [Export] public float maxHeightFraction = 0.4f;
    private int minSpacing = 50;
    private int maxSpacing = 100;

    private StaticBody2D topPipe;
    private StaticBody2D bottomPipe;
    private Area2D scorePipeArea;

    private float speed = 7.5f;

    private float baseWidth;
    private float baseHeight;

    public bool isActive = true;

    private Bird bird;


    public void Initialize(Bird bird)
    {
        this.bird = bird;
    }


    public override void _Ready()
    {
        topPipe = GetNode<StaticBody2D>("TopPipe");
        bottomPipe = GetNode<StaticBody2D>("BottomPipe");
        scorePipeArea = GetNode<Area2D>("ScoreCollision");

        var screenHeight = GetViewportRect().Size.Y;

        //this is irrelevant for score collision
        float height = (float)GD.RandRange(minHeightFraction, maxHeightFraction) * screenHeight;
        float spacing = GD.RandRange(minSpacing, maxSpacing);
        float remainingHeight = screenHeight - height + spacing;

        topPipe.Scale = new Vector2(8, height / 16f);
        bottomPipe.Scale = new Vector2(8, remainingHeight / 16f);

        topPipe.Position = new Vector2(0, 0);
        bottomPipe.Position = new Vector2(0, remainingHeight);
        scorePipeArea.Position = new Vector2(16f, 0);

        scorePipeArea.BodyExited += OnScoreAreaExited;
    }

    private void OnScoreAreaExited(Node2D body)
    {
        if (body.IsInGroup("Player"))
        {
            bird.score++;
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        if (!isActive) return;

        bool outOfBounds = false;
        foreach (Node2D pipe in GetChildren())
        {
            pipe.GlobalPosition = new Vector2(pipe.GlobalPosition.X - speed, pipe.GlobalPosition.Y);
            if (pipe.GlobalPosition.X < -200f) outOfBounds = true;
        }
        if (outOfBounds) QueueFree();
    }
}
