using UnityEngine;
using System.Collections;

public class Building : MonoBehaviour
{

	protected Game Game;
	private bool Selected = false;
	public Unit UnitOnTop;
	
	private const int HitPointsDefault = 20;
	private int HitPoints = HitPointsDefault;

	private Color AlphaOffset = new Color(0, 0, 0, 0);

	public int Team = 0;
	
	public int Type = 0;

	public const int CITY 	= 1;
	public const int HQ 	= 2;
	public const int BASE 	= 3;

	void Start ()
	{
		Game = GameObject.Find("Game").GetComponent<Game>();

		SetTeam(Team, true);
	}
	public void Reset()
	{
		Selected = false;
		UnitOnTop = null;
	}

	void OnMouseDown()
	{
		if (Game.HUD.ActionPopup.Visible)
			return;
		
		// On Left-MouseDown
		if (Game.Selector.CurrentUnit != null && (UnitOnTop == null || UnitOnTop == Game.Selector.CurrentUnit) && !Game.Selector.CurrentUnit.IsWaitingForMoveAccept())
			Game.Selector.CurrentUnit.MoveToTile(TilePosition());
		else if (!Selected)
			Game.Selector.SelectBuilding(this);
	}
	void OnMouseEnter()
	{
		if (Game.HUD.ActionPopup.Visible)
			return;
		
		Game.Selector.SelectTile(TilePosition());

		Game.HUD.SetTileInfo(GetTypeName(), Team, GetHitPoints());
	}

	public void Select()
	{
		Selected = true;

		if (Type == BASE)
		{
			Game.HUD.BuyMenu.SetBuilding(this);
			Game.HUD.BuyMenu.Show(false, transform.position);
		}
	}
	public void Unselect()
	{
		Selected = false;

		if (Type == BASE)
			Game.HUD.BuyMenu.Hide();
	}

	public void OnUnitEnter(Unit unit)
	{
		UnitOnTop = unit;
		unit.BuildingOn = this;

		AlphaOffset = new Color(0, 0, 0, 0.2f);
		SetTeam();

		gameObject.GetComponent<Collider>().enabled = false;
	}
	public void OnUnitLeave()
	{
		if (UnitOnTop != null)
			UnitOnTop.BuildingOn = null;
		UnitOnTop = null;

		AlphaOffset = new Color(0, 0, 0, 0);
		SetTeam();

		gameObject.GetComponent<Collider>().enabled = true;
	}

	public void Capture(int hitPoints, int team)
	{
		if (HitPoints - hitPoints <= 0)
		{
			int previousTeam = Team;

			SetTeam(team);
			HitPoints = GetHitPointsMax();

			if (previousTeam != 0)
				Game.CheckWinLoseConditions();
		}
		else
			HitPoints -= hitPoints;
	}
	public void Heal(int hitPoints)
	{
		if (HitPoints + hitPoints > GetHitPointsMax())
			HitPoints = GetHitPointsMax();
		else
			HitPoints += hitPoints;
	}
	public int GetHitPoints() { return HitPoints; }
	public int GetHitPointsMax() { return HitPointsDefault; }

	public void SetTeam(int team = -1, bool init = false)
	{
		if (team == -1)
			team = Team;

		if (Team != team || init)
		{
			if (Team != 0 && Game.Teams[Team - 1].Buildings.IndexOf(this) != -1)
				Game.Teams[Team - 1].Buildings.Remove(this);
			
			Team = team;
			
			if (Team != 0 && Game.Teams[Team - 1].Buildings.IndexOf(this) == -1)
				Game.Teams[Team - 1].Buildings.Add(this);
		}

		// Set Colour
		GetComponentInChildren<TeamColour>().SetTeam(team, AlphaOffset);

//		switch (Team)
//		{
//			case 1:
//			{
//				for (int i = 0; i < transform.childCount; i++)
//					transform.GetChild(i).renderer.material.SetColor("_Color", Color.red - AlphaOffset);
//				if (renderer != null)
//					renderer.material.SetColor("_Color", Color.red);
//				break;
//			}
//			case 2:
//			{
//				for (int i = 0; i < transform.childCount; i++)
//					transform.GetChild(i).renderer.material.SetColor("_Color", Color.blue - AlphaOffset);
//				if (renderer != null)
//					renderer.material.SetColor("_Color", Color.blue);
//				break;
//			}
//			default:
//			{
//				for (int i = 0; i < transform.childCount; i++)
//					transform.GetChild(i).renderer.material.SetColor("_Color", Color.white - AlphaOffset);
//				if (renderer != null)
//					renderer.material.SetColor("_Color", Color.white);
//				break;
//			}
//		}
	}

	public Point TilePosition() { return new Point(Mathf.RoundToInt(this.gameObject.transform.position.x), Mathf.RoundToInt(this.gameObject.transform.position.z)); }

	public string GetTypeName()
	{
		switch (Type)
		{
		case CITY: return "City";
		case HQ: return "GQ";
		case BASE: return "Base";
		}
		return "";
	}
}
