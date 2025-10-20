using Godot;
using System;

public partial class Startup : Node2D
{

    public override void _Ready()
    {
        GD.Randomize();
    }

}
