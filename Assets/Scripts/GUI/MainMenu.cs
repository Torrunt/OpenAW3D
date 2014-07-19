using UnityEngine;

public class MainMenu : Menu
{
	public Texture2D Title;

	private float TitleWidth;
	private float TitleHeight;

	private GUIStyle CreditStyle;
	private const string Credits =
		"Created by Corey Zeke Womack (Torrunt.net)\n" +
		"Based on Advance Wars VG Remix by klamp on Polycount.com\n" +
		"Advance Wars Created by Nintendo and Intelligent Systems.";

	protected override void Init()
	{
		base.Init();

		AddItem("Play (Hotseat Multiplayer)");
		AddItem("Quit");

		BoxWidth = 400;
		ButtonHeight = 45;
		ButtonStyle.fontSize = 45;
		ButtonStyle.alignment = TextAnchor.MiddleCenter;
		ButtonStyle.contentOffset = new Vector2(0, 0);
		SeperatorBetweenButtons = true;

		CreditStyle = new GUIStyle();
		CreditStyle.font = Font;
		CreditStyle.normal.textColor = Color.white;
		CreditStyle.fontSize = 24;
		CreditStyle.alignment = TextAnchor.LowerLeft;

		Show();
	}

	protected override void OnGUI()
	{
		base.OnGUI();
		if (!Visible)
			return;

		// Title
		TitleWidth = Mathf.Ceil(711 * (Screen.width / 1920f));
		TitleHeight = Mathf.Ceil(280 * (Screen.height / 1080f));
		GUI.DrawTexture(new Rect((Screen.width - TitleWidth)/2, 50, TitleWidth, TitleHeight), Title);

		// Credits
		GUIUtils.DrawTextWithOutline(new Rect(4, Screen.height - 2, 0, 0), Credits, CreditStyle, Color.black);
	}

	protected override void OnButtonPress(string item)
	{
		switch (item)
		{
		case "Play (Hotseat Multiplayer)":
			Application.LoadLevel("Scene01");
			break;
		case "Quit":
			Application.Quit();
			break;
		}
	}
}