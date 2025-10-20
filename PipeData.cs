using Godot;
using System;

public partial class PipeData : Area2D
{

    [Export]
    public float height;

    [Export]
    public bool isTop = false;

    public void Initalize(float height, Vector2 pos)
    {
        this.height = height;
        this.Position = Position;
        Scale = new Vector2(1, height / 100f);

    }

    public override void _Ready()
    {
    }
}
