using Godot;
using System;

public partial class MainMenu : Node2D
{
	private void OnStartButtonPressed(){
		GetTree().ChangeSceneToFile("res://Scenes/PollsAndPillars.tscn");
	}

	private void OnQuitButtonPressed(){
		GetTree().Quit();
	}
}
