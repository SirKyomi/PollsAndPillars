extends ProgressBar


# Called when the node enters the scene tree for the first time.
func _ready():
	wende_farbcode_an(value);
	pass # Replace with function body.

func wende_farbcode_an(_value):
	var r = abs(_value * 0.01 - 1);
	var g = _value * 0.01
	var b = 0.0
	var sbf = StyleBoxFlat.new();
	sbf.corner_radius_bottom_left=5;
	sbf.corner_radius_bottom_right=5;
	sbf.corner_radius_top_left=5;
	sbf.corner_radius_top_right=5;
	sbf.border_width_bottom = 2;
	sbf.border_width_left = 2;
	sbf.border_width_right = 2;
	sbf.border_width_top = 2;
	sbf.border_color = Color('#cccccc00');
	sbf.bg_color = Color(r,g,b);
	add_theme_stylebox_override('fill',sbf);
