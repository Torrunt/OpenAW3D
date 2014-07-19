using UnityEngine;

public class OptionsPopupMenu : Menu
{
	protected override void Init()
	{
		AddItem("Exit Map");

		ButtonStyle.contentOffset = new Vector2(4, 0);
	}

	protected override void OnButtonPress(string item)
	{
		switch (item)
		{
		case "Exit Map":
			Application.LoadLevel("MainMenu");
			break;
		}
		
		Hide();
	}
}