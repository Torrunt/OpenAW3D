using UnityEngine;

public class WinMessage : Menu
{

	private GUIStyle MessageStyle;

	private string Message;
	private Color OutlineColor = Color.black;
	private GUIStyle BackStyle;
	private Texture2D BackTexture;

	protected override void Init()
	{
		base.Init();

		MessageStyle = new GUIStyle();
		MessageStyle.font = Font;
		MessageStyle.normal.textColor = Color.white;
		MessageStyle.fontSize = 80;
		MessageStyle.alignment = TextAnchor.MiddleCenter;

		BackTexture = new Texture2D(1, 1);
		BackTexture.wrapMode = TextureWrapMode.Repeat;
		BackTexture.SetPixel(0, 0, new Color(0.2f, 0.2f, 0.2f, 0.5f));
		BackTexture.Apply();
		BackStyle = new GUIStyle();
		BackStyle.normal.background = BackTexture;

		AddItem("Exit Map");
		ButtonStyle.contentOffset = new Vector2(4, 0);
	}

	public void SetTeamWon(int team)
	{
		if (team == 1)
		{
			Message = "Red";
			OutlineColor = Color.red;
		}
		else if (team == 2)
		{
			Message = "Blue";
			OutlineColor = Color.blue;
		}
		Message += " Team Wins!";
	}

	protected override void OnGUI()
	{
		if (!Visible)
			return;

		GUI.Box(new Rect(0, 0, Screen.width, Screen.height), GUIContent.none, BackStyle);


		base.OnGUI();

		GUIUtils.DrawTextWithOutline(new Rect(Screen.width / 2, (Screen.height / 2) - 200, 0, 0), Message, MessageStyle, OutlineColor, 4);
	}

	
	protected override void OnButtonPress(string item)
	{
		switch (item)
		{
		case "Exit Map":
			Application.LoadLevel("MainMenu");
			break;
		}
	}
}