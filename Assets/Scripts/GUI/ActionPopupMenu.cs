using UnityEngine;

public class ActionPopupMenu : Menu
{
	public Texture2D Icon_Wait;
	public Texture2D Icon_End;
	public Texture2D Icon_Options;
	public Texture2D Icon_Fire;
	public Texture2D Icon_Capture;

	protected override void OnButtonPress(string item)
	{
		Hide();

		switch (item)
		{
		case "Wait":
			if (Game.Selector.CurrentUnit != null)
				Game.Selector.CurrentUnit.AcceptMove();
			break;
		case "Fire":
			if (Game.Selector.CurrentUnit != null)
				Game.Selector.CurrentUnit.SetAttackTarget();
			break;
		case "Capture":
			if (Game.Selector.CurrentUnit != null)
				Game.Selector.CurrentUnit.CaptureBuilding();
			break;
		case "End":
			Game.EndTurn();
			break;
		case "Options":
			Game.HUD.OptionsPopup.Show();
			break;
		}
	}
	
	protected override Texture2D GetActionIcon(string item)
	{
		switch (item)
		{
			case "Wait": return Icon_Wait;
			case "End": return Icon_End;
			case "Options": return Icon_Options;
			case "Fire": return Icon_Fire;
			case "Capture": return Icon_Capture;
		}
		return null;
	}
}