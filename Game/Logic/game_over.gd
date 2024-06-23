extends Node


# Called when the node enters the scene tree for the first time.
func on_restart_button_pressed():
	get_tree().change_scene_to_file("res://Scenes/PollsAndPillars.tscn");
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func on_quit_button_pressed(delta):
	get_tree().quit();
	pass
