using Godot;
using System;

public partial class Spawner : Node2D
{
    [Export]
    private Bird bird;
    [Export]
    private PackedScene pipeScene;
    [Export]
    public float spawnDelay = 1.5f;
    [Export]
    public Vector2 spawnOffsett = Vector2.Zero;

    private float timer = 0f;


    public override void _Ready()
    {
        Node2D pipeInstance = pipeScene.Instantiate<PipeController>();
        GetParent().AddChild(pipeInstance);
    }

    public override void _PhysicsProcess(double delta)
    {
        if (pipeScene == null) return;

        timer += (float)delta;

        if (timer >= spawnDelay)
        {
            timer = 0f;
            Spawn();
        }
    }

    private void Spawn()
    {
        var instance = pipeScene.Instantiate<PipeController>();
        instance.Initialize(bird);

        instance.GlobalPosition = GlobalPosition + spawnOffsett;
        AddChild(instance);

        if (bird.isDead)
        {
            foreach (PipeController item in GetChildren())
            {
                item.isActive = false;
            }
        }
    }

}
