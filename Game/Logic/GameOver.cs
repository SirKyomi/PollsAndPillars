using Godot;
using System;

public partial class GameOver : Node2D
{
    [Export]
	public GameStats gameStats;

// Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
		double zeitInSekunden = gameStats.Score;
		double sekundenProWahlperiode = gameStats.SekundenProWahlperiode;

		var sekundenProJahr = sekundenProWahlperiode / 4;
		var sekundenProMonat = sekundenProJahr / 12;
		var sekundenProTag = sekundenProJahr / 365;

		var jahre = zeitInSekunden / sekundenProJahr;
		var restVonJahr = zeitInSekunden % sekundenProJahr;

		var monate = restVonJahr / sekundenProMonat;
		var restVonMonat = zeitInSekunden % sekundenProMonat;

		var tage = restVonMonat / sekundenProTag;
		var scoreJahr = GetNode<Label>("%ScoreJahreValue");

		scoreJahr.Text = ((int)jahre).ToString();
		var scoreMonat = GetNode<Label>("%ScoreMonateValue");
		scoreMonat.Text = ((int)monate).ToString();
		var scoreTag = GetNode<Label>("%ScoreTageValue");
		scoreTag.Text = ((int)tage).ToString();
    }

	private void OnRestartButtonPressed(){
		GetTree().ChangeSceneToFile("res://Scenes/PollsAndPillars.tscn");
	}

	private void OnQuitButtonPressed(){
		GetTree().Quit();
	}
}
