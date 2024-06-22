using Godot;

namespace PollsAndPillars.Logic;

public partial class PollsAndPillars : Node2D
{
	TileMap tileMap;
	PopupMenu popupMenu;
	int groundLayer = 2;
	bool popupIsShowing = false;
	bool mouseIsInPopup = false;
	Vector2I mousePositionSuccessFullClick;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		tileMap = GetNode<TileMap>("TileMap");
		popupMenu = GetNode<PopupMenu>("PopupMenu");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("click"))
		{
			var mousePosition = GetGlobalMousePosition();
			var tileMousePosition = tileMap.LocalToMap(mousePosition);
			var atlasCoordsMouse = tileMap.GetCellAtlasCoords(1, tileMousePosition);
			
			if (atlasCoordsMouse == new Vector2I(6, 1) && !(popupIsShowing && mouseIsInPopup)) {
				//GD.Print("Buildable Tile found and accessible");
				var absoluteMousePosition = GetViewport().GetMousePosition();
				popupMenu.Popup(new Rect2I((int)absoluteMousePosition.X, (int)absoluteMousePosition.Y, popupMenu.Size.X, popupMenu.Size.Y));
				popupIsShowing = true;
				mousePositionSuccessFullClick = tileMousePosition;
			}
		}
	}

	private void OnPopupMenuIdPressed(int id) {
		var sourceID = 1;
		var atlasCoordsTargetTile = id == 1 ? new Vector2I(2, 4) : new Vector2I(1, 4);
		tileMap.SetCell(groundLayer, mousePositionSuccessFullClick, sourceID, atlasCoordsTargetTile);
		
		if (atlasCoordsTargetTile == new Vector2I(2, 4)){
			//klimafreundlich
			//stats anpassen
		} else {
			//klimaschädlich
			//stats anpassen
		}

		popupIsShowing = false;
	}

	private void OnPopupMenuMouseEntered() {
		mouseIsInPopup = popupIsShowing;
	}

	private void OnPopupMenuMouseExited() {
		mouseIsInPopup = false;
	}
}