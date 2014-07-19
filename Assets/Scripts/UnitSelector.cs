using UnityEngine;
using System.Collections;

public class UnitSelector : MonoBehaviour
{

	private Transform CurrentTile;
	public Unit CurrentUnit;
	public Building CurrentBuilding;

	private Game Game;

	private float Anim_Rotate_Speed = 360;
	private float Anim_Rotate_Gap = 1;
	private float Anim_Rotate_Time = 0;
	private bool Anim_Rotating = false;

	// Use this for initialization
	void Start ()
	{
		Game = GameObject.Find("Game").GetComponent<Game>();
	}

	// Update is called once per frame
	void Update ()
	{
		// Animate
		if (Anim_Rotating)
		{

			if (transform.rotation.eulerAngles.y + (Anim_Rotate_Speed * Time.deltaTime) >= 90)
			{
				transform.rotation = Quaternion.AngleAxis(0, new Vector3(0, 1, 0));
				Anim_Rotating = false;
			}
			else
				transform.Rotate(new Vector3(0, 1, 0), Anim_Rotate_Speed * Time.deltaTime);
		}
		else
		{
			Anim_Rotate_Time += Time.deltaTime;
			if (Anim_Rotate_Time >= Anim_Rotate_Gap)
			{
				Anim_Rotate_Time = 0;
				Anim_Rotating = true;
			}
		}

		// On Right-MouseDown
		if (Input.GetMouseButtonDown(1))
		{
			Game.HUD.CancelOutOfMenu();
			if (Game.HUD.InMenu())
				return;
			
			if (CurrentUnit != null && !CurrentUnit.IsMoving())
			{
				if (CurrentUnit.IsWaitingForActionAccept())
					CurrentUnit.UndoAction();
				else if (CurrentUnit.IsWaitingForMoveAccept())
					CurrentUnit.UndoMove();
				else
					UnselectCurrentUnit();
			}
			else if (CurrentBuilding != null)
				UnselectCurrentBuilding();
		}
	}

	// Selecting Units
	public void SelectUnit(Unit unit)
	{
		// Select Target?
		if (CurrentUnit != null && CurrentUnit.IsWaitingForActionAccept() && CurrentUnit.InAttackRangeList(unit))
		{
			if (CurrentUnit.GetAttackTarget() == unit)
			{
				CurrentUnit.AcceptAttack();
				UnselectCurrentUnit();
			}
			else
				CurrentUnit.SetAttackTarget(unit);

			return;
		}

		// Select Friendly Unit
		if (Game.CurrentTeam != unit.Team || (CurrentUnit != null && CurrentUnit != unit) || unit.HasMoved())
			return;

		UnselectCurrentBuilding();
		UnselectCurrentUnit();
		CurrentUnit = unit;

		SelectTile(CurrentUnit.TilePosition());

		CurrentUnit.Select();
	}
	private void UnselectUnit(Unit unit)
	{
		unit.Unselect();

		CurrentUnit = null;
	}
	public void UnselectCurrentUnit()
	{
		if (CurrentUnit == null)
			return;
		UnselectUnit(CurrentUnit);
	}

	// Selecting Buildings
	public void SelectBuilding(Building building)
	{
		// Select Friendly Building
		if (Game.CurrentTeam != building.Team || Game.Selector.CurrentUnit != null)
			return;

		UnselectCurrentBuilding();
		CurrentBuilding = building;

		SelectTile(CurrentBuilding.TilePosition());

		CurrentBuilding.Select();
	}
	private void UnselectBuilding(Building building)
	{
		if (Game.CurrentTeam != building.Team)
			return;
		
		building.Unselect();
		
		CurrentBuilding = null;
	}
	public void UnselectCurrentBuilding()
	{
		if (CurrentBuilding == null)
			return;
		UnselectBuilding(CurrentBuilding);
	}

	// Selecting Tiles
	public void SelectTile(Transform tile)
	{
		UnselectCurrentTile();
		CurrentTile = tile;

		tile.gameObject.GetComponent<Tile>().Select();



		transform.position = new Vector3(tile.transform.position.x, tile.transform.position.y + 1, tile.transform.position.z);

		// Update Unit Path
		if (CurrentUnit != null && !CurrentUnit.IsMoving() && !CurrentUnit.IsWaitingForMoveAccept())
			CurrentUnit.CalculatePathTo(tile.position, false);
	}
	public void SelectTile(Point tilePosition)
	{
		SelectTile(Game.Level.GetTile(tilePosition).transform);
	}

	private void UnselectTile(Transform tile)
	{
		tile.gameObject.GetComponent<Tile>().UnTint();

		CurrentTile = null;
	}
	public void UnselectCurrentTile()
	{
		if (CurrentTile == null)
			return;
		UnselectTile(CurrentTile);
	}

}
