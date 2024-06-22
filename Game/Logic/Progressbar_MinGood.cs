using System;
using System.Threading;
using Godot;

namespace PollsAndPillars.Logic;

public partial class Progressbar_MinGood : ProgressBar
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        WendeFarbcodeAn(Value);
        ValueChanged += WendeFarbcodeAn;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }

    private void WendeFarbcodeAn(double value)
    {
        var r = (float)(value * 0.01);
        var g = (float)Math.Abs(value * 0.01 - 1);
        var b = 0.0f;
        var sbf = new StyleBoxFlat()
        {
            CornerRadiusBottomLeft = 5, CornerRadiusBottomRight = 5, CornerRadiusTopLeft = 5, CornerRadiusTopRight = 5,
            BorderWidthBottom = 2, BorderWidthLeft = 2, BorderWidthRight = 2, BorderWidthTop = 2,
            BorderColor = new Color("#cccccc00")
        };
        sbf.BgColor = new Color(r, g, b);
        AddThemeStyleboxOverride("fill", sbf);
    }
}