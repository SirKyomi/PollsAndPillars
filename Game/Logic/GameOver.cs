using Godot;
using System;

public partial class GameOver : Node2D
{
	private void OnRestartButtonPressed(){
		GetTree().ChangeSceneToFile("res://Scenes/PollsAndPillars.tscn");
	}

	private void OnQuitButtonPressed(){
		GetTree().Quit();
	}
}
