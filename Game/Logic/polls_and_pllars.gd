extends Node2D

var tile_map = TileMap;
var popup_menu = PopupMenu;
const ground_layer = 2;
var popup_is_showing = false;
var mouse_in_popup = false;
var mouse_position_successful_click = Vector2i();
var atlas_coords_factory = Vector2i(2,5);
var atlas_coords_carbon_capture = Vector2i(1,5);

signal ui_aktualisieren_event_handler(wohlstand, arbeitslosigkeit, klimabelastung);


# Called when the node enters the scene tree for the first time.
func _ready():
	tile_map = get_node("TileMap")
	popup_menu = get_node("PopupMenu");


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	if Input.is_action_just_pressed("click"):
		var mouse_position = get_global_mouse_position();
		var tile_mouse_position = tile_map.local_to_map(mouse_position);
		var atlas_coords_mouse = tile_map.get_cell_atlas_coords(1,tile_mouse_position);
		if atlas_coords_mouse == Vector2i(6,1) && !(popup_is_showing && mouse_in_popup):
			var absolute_mouse_position = get_viewport().get_mouse_position();
			popup_menu.popup(Rect2i(absolute_mouse_position.x, absolute_mouse_position.y, popup_menu.size.x, popup_menu.size.y));
			popup_is_showing = true;
			mouse_position_successful_click = tile_mouse_position;

func on_popup_menu_id_pressed(id):
	var source_id = 0;
	var atlas_coords_target_tile;
	if id == 1:
		atlas_coords_target_tile = atlas_coords_carbon_capture;
	else:
		atlas_coords_target_tile = atlas_coords_factory;
	tile_map.set_cell(ground_layer, mouse_position_successful_click, source_id, atlas_coords_target_tile);
	
	if atlas_coords_target_tile == atlas_coords_carbon_capture:
		ui_aktualisieren_event_handler.emit(12,15,-10);
	else:
		ui_aktualisieren_event_handler.emit(-12,15,-10);
		
	popup_is_showing = false;

func on_popup_menu_mouse_entered():
	mouse_in_popup = true;

func on_popup_menu_mouse_exited():
	mouse_in_popup = false;
