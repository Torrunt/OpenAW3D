using UnityEngine;
using System.Collections;

public class HeadsUpDisplay : MonoBehaviour
{

	private Game Game;

	public Font Font;
	public Font Font2;

	private GUIStyle StyleDefault = new GUIStyle();

	private int Team = 0;
	private int Resources = 0;

	// Colours
	private Texture2D Texture_White;
	private Texture2D Texture_Red;
	private Texture2D Texture_Blue;

	// Team Banner
	private Rect TeamBanner_Rect = new Rect(2, 2, 150, 30);
	private GUIStyle TeamBanner_Style = new GUIStyle();

	// Resources
	public Color Resources_FontColor = Color.grey;
	private Rect ResourcesTag_Position = new Rect(4, 30, 1, 1);
	private Rect Resources_Position = new Rect(150, 30, 1, 1);
	private GUIStyle Resources_Style;
	private GUIStyle ResourcesTag_Style;

	// Help
	private int Help_CurrentPrompt;
	private string Help_CurrentMsg;
	private GUIStyle Help_Style;
	private Texture Help_CurrentTexture;

	public const int PROMPT_LEFTMOUSE = 0;
	public const int PROMPT_RIGHTMOUSE = 1;

	// Tile Info
	private Rect TileInfoBox_Rect = new Rect(0, 0, 100, 120);
	private Rect TileInfoName_Rect = new Rect(0, 0, 100, 0);
	private GUIStyle TileInfoBox_Style = new GUIStyle();
	private GUIStyle TileInfoName_Style;
	private GUIStyle TileInfoHitPoints_Style;
	private Texture2D TileInfoBox_Texture;
	private float TileInfoIcon_Scale = 2;
	private Vector2 TileInfoIncoOffset = new Vector2();

	private Texture2D TileInfoIcon_Texture;
	private string TileInfoName = "Plain";
	private int TileInfoHitPoints = -1;

	// Tutorial
	private bool Tutorial_Visible = true;
	private GUIStyle Tutorial_Style;
	private const string Tutorial_Message =
		"Capture all Enemy Buildings and Destroy their Tanks!\n" +
		"Build more Tanks at Bases and Capture Buildings for more Resources.\n\n" +
		"Capturing Buildings take multiple turns, make sure to select 'Capture' each time.\n\n" +
		"Heal Tanks by leaving them in friendly buildings.\n" +
		"(Cities=1hp  Bases=2hp).\n\n\n" +
		"Play using the Mouse/Mouse Buttons/Mouse Wheel.";

	// Day Change
	private bool Day_Visible = false;
	private GUIStyle Day_Style;
	private Color Day_Color;
	private Color Day_OutlineColor;
	private float Day_ShowTime = 0;
	private float Day_AlphaOffset = 0;
	private int Day_No = 1;

	// Menus
	public ActionPopupMenu ActionPopup;
	public OptionsPopupMenu OptionsPopup;
	public BuyMenu BuyMenu;
	public WinMessage WinMessage;

	// Crosshair
	private bool Crosshair_Visible = false;
	private Rect Crosshair_Rect = new Rect(0, 0, 200, 200);

	// Hit Points
	private GUIStyle HitPoints_Style;

	// Textures
	public Texture2D Crosshair_Texture;
	public Texture2D Icon_Help_LeftMouse;
	public Texture2D Icon_Help_RightMouse;

	public Texture2D Icon_Building;

	public Texture2D Icon_Plain;
	public Texture2D Icon_Sea;
	public Texture2D Icon_Road;
	public Texture2D Icon_Bridge;
	public Texture2D Icon_City_White;
	public Texture2D Icon_Base_White;
	public Texture2D Icon_City_Red;
	public Texture2D Icon_Base_Red;
	public Texture2D Icon_City_Blue;
	public Texture2D Icon_Base_Blue;

	// Use this for initialization
	void Start ()
	{
		Game = GameObject.Find("Game").GetComponent<Game>();
		ActionPopup = GameObject.Find("GUI").GetComponent<ActionPopupMenu>();
		OptionsPopup = GameObject.Find("GUI").GetComponent<OptionsPopupMenu>();
		BuyMenu = GameObject.Find("GUI").GetComponent<BuyMenu>();
		WinMessage = GameObject.Find("GUI").GetComponent<WinMessage>();

		useGUILayout = false;

		StyleDefault.font = Font;
		StyleDefault.fontSize = 24;

		// Colours
		Texture_Red = new Texture2D(1, 1);
		Texture_Red.wrapMode = TextureWrapMode.Repeat;
		Texture_Red.SetPixel(0, 0, Color.red);
		Texture_Red.Apply();

		Texture_Blue = new Texture2D(1, 1);
		Texture_Blue.wrapMode = TextureWrapMode.Repeat;
		Texture_Blue.SetPixel(0, 0, Color.blue);
		Texture_Blue.Apply();

		Texture_White = new Texture2D(1, 1);
		Texture_White.wrapMode = TextureWrapMode.Repeat;
		Texture_White.SetPixel(0, 0, Color.white);
		Texture_White.Apply();

		// Team Banner
		SetTeam(Game.CurrentTeam, Game.GetCurrentTeam().Resources);

		// Resources
		Resources_Style = new GUIStyle(StyleDefault);
		Resources_Style.font = Font2;
		Resources_Style.normal.textColor = Resources_FontColor;

		ResourcesTag_Style = new GUIStyle(Resources_Style);

		Resources_Style.alignment = TextAnchor.UpperRight;

		// Help
		Help_Style = new GUIStyle(StyleDefault);
		Help_Style.font = Font2;
		Help_Style.normal.textColor = Color.white;
		Help_Style.fontSize = 16;
		SetCurrentHelp(PROMPT_LEFTMOUSE, "MENU");

		// Tile Info
		TileInfoBox_Texture = new Texture2D(1, 1);
		TileInfoBox_Texture.wrapMode = TextureWrapMode.Repeat;
		TileInfoBox_Texture.SetPixel(0, 0, new Color(0.2f, 0.2f, 0.2f, 0.5f));
		TileInfoBox_Texture.Apply();
		TileInfoBox_Style.normal.background = TileInfoBox_Texture;

		TileInfoName_Style = new GUIStyle(StyleDefault);
		TileInfoName_Style.normal.textColor = Color.white;
		TileInfoName_Style.fontSize = 28;
		TileInfoName_Style.alignment = TextAnchor.UpperCenter;
		TileInfoName_Style.contentOffset = new Vector2(0, 12);

		TileInfoHitPoints_Style = new GUIStyle(StyleDefault);
		TileInfoHitPoints_Style.normal.textColor = Color.white;
		TileInfoHitPoints_Style.fontSize = 20;
		TileInfoHitPoints_Style.alignment = TextAnchor.UpperLeft;
		TileInfoHitPoints_Style.contentOffset = new Vector2(0, 6);

		SetTileInfo("Plain");

		// Tutorial
		Tutorial_Style = new GUIStyle(StyleDefault);
		Tutorial_Style.fontSize = 32;
		Tutorial_Style.normal.textColor = Color.white;
		Tutorial_Style.alignment = TextAnchor.MiddleCenter;

		// Day Change
		Day_Style = new GUIStyle(StyleDefault);
		Day_Style.fontSize = 120;
		Day_Style.normal.textColor = Color.white;
		Day_Style.alignment = TextAnchor.MiddleCenter;

		// Hit Points
		HitPoints_Style = new GUIStyle(StyleDefault);
		HitPoints_Style.normal.textColor = Color.white;
	}
	
	// Update is called once per frame
	void OnGUI ()
	{
		// Team Banner
		GUIUtils.DrawBoxWithOutline(TeamBanner_Rect, GUIContent.none, TeamBanner_Style, Texture_White, 2);

		// Resource
		GUIUtils.DrawTextWithOutline(ResourcesTag_Position, "G.", ResourcesTag_Style, Color.white, 2);
		GUIUtils.DrawTextWithOutline(Resources_Position, Resources.ToString(), Resources_Style, Color.white, 2);

		// Help
		GUI.DrawTexture(new Rect((Screen.width / 2) - 12, 4, Help_CurrentTexture.width * 2, Help_CurrentTexture.height * 2), Help_CurrentTexture);
		GUIUtils.DrawTextWithOutline(new Rect(Screen.width / 2, 16, 0, 0), Help_CurrentMsg, Help_Style, Color.black);

		// Tile Info
		//// Box
		TileInfoBox_Rect.x = Screen.width - TileInfoBox_Rect.width;
		TileInfoBox_Rect.y = Screen.height - TileInfoBox_Rect.height - 30;
		GUI.Box(TileInfoBox_Rect, GUIContent.none, TileInfoBox_Style);
		//// Icon
		GUI.color = new Color(1, 1, 1, 0.85f);
		GUI.DrawTexture(new Rect(TileInfoBox_Rect.x + ((TileInfoBox_Rect.width - (TileInfoIcon_Texture.width * TileInfoIcon_Scale)) / 2) + TileInfoIncoOffset.x,
		                         TileInfoBox_Rect.y + 35 + TileInfoIncoOffset.y,
		                         TileInfoIcon_Texture.width * TileInfoIcon_Scale,
		                         TileInfoIcon_Texture.height * TileInfoIcon_Scale), TileInfoIcon_Texture);
		GUI.color = new Color(1, 1, 1);
		//// Name
		TileInfoName_Rect.x = TileInfoBox_Rect.x;
		TileInfoName_Rect.y = TileInfoBox_Rect.y;
		GUIUtils.DrawTextWithOutline(TileInfoName_Rect, TileInfoName, TileInfoName_Style, Color.black);
		//// Building Hit Points
		if (TileInfoHitPoints != -1)
		{
			GUI.DrawTexture(new Rect(TileInfoBox_Rect.x + 20,
			                         TileInfoBox_Rect.y + TileInfoBox_Rect.height - (Icon_Building.height * 2) - 10,
			                         Icon_Building.width * 2,
			                         Icon_Building.height * 2), Icon_Building);
			GUIUtils.DrawTextWithOutline(new Rect(TileInfoBox_Rect.x + 45,
			                                      TileInfoBox_Rect.y + TileInfoBox_Rect.height - (Icon_Building.height * 2) - 10,
			                                      0, 0), TileInfoHitPoints.ToString(), TileInfoHitPoints_Style, Color.black);
		}

		// Crosshair
		if (Crosshair_Visible)
			GUI.DrawTexture(Crosshair_Rect, Crosshair_Texture);

		// Unit HitPoints
		for (int u = 0; u < Game.Units.childCount; u++)
		{
			Unit unit = Game.Units.GetChild(u).GetComponent<Unit>();
			if (unit.GetHitPoints() == 10)
				continue;

			Vector3 pos = Game.Camera.WorldToScreenPoint(unit.transform.position);

			GUIUtils.DrawTextWithOutline(new Rect(pos.x + 5, Screen.height - pos.y, 0, 0), unit.GetHitPoints().ToString(), HitPoints_Style, Color.black);
		}

		// Day No
		if (Day_Visible)
		{
			Day_Style.normal.textColor = Day_Color - new Color(0, 0, 0, Day_AlphaOffset);
			Day_OutlineColor = new Color(0, 0, 0, 0.75f - Day_AlphaOffset);
			GUIUtils.DrawTextWithOutline(new Rect(Screen.width/2, Screen.height/2, 0, 0), "Day " + Day_No, Day_Style, Day_OutlineColor);

			if (Day_ShowTime >= 0.75f)
			{
				if (Day_AlphaOffset + (1 * Time.deltaTime) >= 1)
				{
					Day_AlphaOffset = 1;
					Day_Visible = false;
				}
				else
					Day_AlphaOffset += 1f * Time.deltaTime;
			}
			else
				Day_ShowTime += Time.deltaTime;
		}

		// Tutorial
		if (Tutorial_Visible)
		{
			GUIUtils.DrawTextWithOutline(new Rect(Screen.width/2, Screen.height/2, 0, 0), Tutorial_Message, Tutorial_Style, Color.black);
			if (GUI.Button(new Rect(0, 0, Screen.width, Screen.height), GUIContent.none, GUIStyle.none))
			{
				HideTutorial();
				ShowDayNo(1);
			}
		}
	}

	public void CancelOutOfMenu()
	{
		if (OptionsPopup.Visible)
		{
			OptionsPopup.Hide();
			ActionPopup.Show();
		}
		else if (ActionPopup.Visible)
			ActionPopup.Hide();
	}
	public bool InMenu() { return OptionsPopup.Visible || ActionPopup.Visible; }


	public void SetResources(int resources)
	{
		if (Resources == resources)
			return;
		
		Resources = resources;
	}
	public void SetTeam(int team, int resources = -1)
	{
		if (Team == team)
			return;

		Team = team;
		if (resources != -1)
			SetResources(resources);

		switch (Team)
		{
			case 1: TeamBanner_Style.normal.background = Texture_Red; break;
			case 2: TeamBanner_Style.normal.background = Texture_Blue; break;
		}
	}

	public void ShowCrosshair(Vector3 position = default(Vector3))
	{
		Crosshair_Visible = true;

		position = Game.Camera.WorldToScreenPoint(position);
		Crosshair_Rect.x = position.x - (Crosshair_Rect.width/2);
		Crosshair_Rect.y = Screen.height - position.y - (Crosshair_Rect.height/2);
	}
	public void HideCrosshair() { Crosshair_Visible = false; }

	public void ShowTeamWomMessage(int team)
	{
		WinMessage.SetTeamWon(team);
		WinMessage.Show();
	}

	public void ShowTutorial() { Tutorial_Visible = true; }
	public void HideTutorial() { Tutorial_Visible = false; }

	public void ShowDayNo(int no)
	{
		Day_No = no;
		if (Team == 1)
			Day_Color = Color.red;
		else if (Team == 2)
			Day_Color = Color.blue;
		Day_Style.normal.textColor = Day_Color;
		Day_AlphaOffset = 0;
		Day_ShowTime = 0;
		Day_OutlineColor = Color.black;
		Day_Visible = true;
	}

	public void SetCurrentHelp(int no, string msg)
	{
		Help_CurrentPrompt = no;
		Help_CurrentMsg = msg;

		switch (Help_CurrentPrompt)
		{
		case PROMPT_LEFTMOUSE: Help_CurrentTexture = Icon_Help_LeftMouse; break;
		case PROMPT_RIGHTMOUSE: Help_CurrentTexture = Icon_Help_RightMouse; break;
		}
	}

	public void SetTileInfo(string name, int team = 0, int hitPoints = -1)
	{
		TileInfoName = name;
		TileInfoHitPoints = hitPoints;


		TileInfoIcon_Scale = 1;
		TileInfoIncoOffset = new Vector2();
		switch (name)
		{
		case "Plain": TileInfoIcon_Texture = Icon_Plain; break;
		case "Road": TileInfoIcon_Texture = Icon_Road; break;
		case "Sea": TileInfoIcon_Texture = Icon_Sea; break;
		case "Bridge":
			TileInfoIcon_Texture = Icon_Bridge;
			TileInfoIcon_Scale = 2;
			break;
		case "City":
			if (team == 1)
				TileInfoIcon_Texture = Icon_City_Red;
			else if (team == 2)
				TileInfoIcon_Texture = Icon_City_Blue;
			else
				TileInfoIcon_Texture = Icon_City_White;
			TileInfoIcon_Scale = 2;
			TileInfoIncoOffset.x = 2;
			TileInfoIncoOffset.y = -5;
			break;
		case "Base":
			if (team == 1)
				TileInfoIcon_Texture = Icon_Base_Red;
			else if (team == 2)
				TileInfoIcon_Texture = Icon_Base_Blue;
			else
				TileInfoIcon_Texture = Icon_Base_White;
			TileInfoIcon_Scale = 2;
			TileInfoIncoOffset.x = 2;
			TileInfoIncoOffset.y = -23;
			break;
		}
	}
}
