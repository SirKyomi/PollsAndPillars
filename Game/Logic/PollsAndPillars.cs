using Godot;
using System;

public partial class PollsAndPillars : Node2D
{
	TileMap tileMap;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		tileMap = GetNode<TileMap>("TileMap");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("click"))
		{
			GD.Print("click");
			var mousePosition = GetGlobalMousePosition();
			GD.Print("MousePosition " + mousePosition);

			var tileMousePosition = tileMap.LocalToMap(mousePosition);
			GD.Print("TileMousePosition " + tileMousePosition);

			var x = tileMap.GetCellAtlasCoords(1, tileMousePosition);
			GD.Print(x);
			if (x == new Vector2I(6, 1)){
				GD.Print("Buildable Tile found");
			}
		}
	}
}
