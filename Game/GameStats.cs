using Godot;
using System;
using System.Diagnostics.Tracing;

[GlobalClass]
public partial class GameStats : Resource
{
    [Export]
    public double Score = 0;
    [Export]
    public double SekundenProWahlperiode = 60;
}
