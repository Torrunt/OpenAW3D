using UnityEngine;
using System.Collections.Generic;

public class Menu : MonoBehaviour
{
	protected Game Game;
	protected Camera Camera;
	public bool Visible = false;

	public Font Font;

	protected int BoxWidth = 120;
	protected int ButtonHeight = 30;
	protected int IconSize = 30;
	protected List<string> Items = new List<string>();
	protected List<Texture2D> Icons = new List<Texture2D>();
	protected List<Color> IconColors = new List<Color>();

	protected Rect Rect = new Rect(0, 0, 0, 0);
	protected Rect TopStripe_Rect = new Rect(0, 0, 0, 0);
	protected Rect BottomStripe_Rect = new Rect(0, 0, 0, 0);
	protected Rect Button_Rect = new Rect(0, 0, 0, 0);

	protected GUIStyle Style = new GUIStyle();
	protected GUIStyle TopStripe_Style = new GUIStyle();
	protected GUIStyle BottomStripe_Style = new GUIStyle();
	protected GUIStyle ButtonStyle = new GUIStyle();

	protected bool SeperatorBetweenButtons = false;
	protected Rect Seperator_Rect;
	protected GUIStyle Seperator_Style = new GUIStyle();

	// Colours
	protected Texture2D Texture_White;
	protected Texture2D Texture_Red;
	protected Texture2D Texture_Blue;
	protected Texture2D Texture_Grey;

	// Use this for initialization
	void Start ()
	{
		if (GameObject.Find("Game") != null)
		{
			Game = GameObject.Find("Game").GetComponent<Game>();
			Camera = Game.Camera;
		}
		else
		Camera = GameObject.Find("Main Camera").GetComponent<Camera>();

		useGUILayout = false;

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

		Texture_Grey = new Texture2D(1, 1);
		Texture_Grey.wrapMode = TextureWrapMode.Repeat;
		Texture_Grey.SetPixel(0, 0, Color.grey);
		Texture_Grey.Apply();

		// Box and Stripes
		Style.normal.background = Texture_White;
		TopStripe_Style.normal.background = Texture_Red;
		BottomStripe_Style.normal.background = Texture_Blue;
		Seperator_Style.normal.background = Texture_Grey;

		// Buttons
		ButtonStyle = new GUIStyle();
		ButtonStyle.font = Font;
		ButtonStyle.fontSize = 30;
		ButtonStyle.alignment = TextAnchor.MiddleLeft;
		ButtonStyle.contentOffset = new Vector2(34, 0);

		Init();
	}
	protected virtual void Init()
	{
	}
	
	protected virtual void OnGUI()
	{
		if (!Visible)
			return;

		// Back
		GUI.Box(Rect, GUIContent.none, Style);
		
		// Red/Blue Strips
		GUI.Box(TopStripe_Rect, GUIContent.none, TopStripe_Style);
		GUI.Box(BottomStripe_Rect, GUIContent.none, BottomStripe_Style);

		// Buttons
		Button_Rect.y = Rect.y + 4;
		for (int i = 0; i < Items.Count; i++)
		{
			DrawButton(i);
		}
	}
	protected virtual void DrawButton(int i)
	{
		// icon
		if (Icons[i] != null)
		{
			if (IconColors[i] != default(Color))
				GUI.color = IconColors[i];
			
			GUI.DrawTexture(new Rect(Rect.x + 4, Button_Rect.y, IconSize, IconSize), Icons[i]);
			
			if (IconColors[i] != default(Color))
				GUI.color = new Color(1, 1, 1, 1);
		}
		
		// button
		bool clicked = GUI.Button(Button_Rect, new GUIContent(Items[i]), ButtonStyle);
		
		// click event
		if (clicked)
			OnButtonPress(Items[i]);
		
		Button_Rect.y += ButtonHeight;
		
		if (SeperatorBetweenButtons && i < Items.Count-1)
		{
			Seperator_Rect.y = Button_Rect.y;
			GUI.Box(Seperator_Rect, GUIContent.none, Seperator_Style);
		}
	}


	protected virtual void OnButtonPress(string item)
	{
	}

	public virtual void Show(bool middleOfScreen = true, Vector3 position = default(Vector3))
	{
		int BoxHeight = (ButtonHeight * Items.Count) + 8;

		Vector3 pos;
		if (!middleOfScreen)
		{
			pos = Camera.WorldToScreenPoint(position);
			pos.x = pos.x - (BoxWidth/2);
			pos.y = Screen.height - pos.y - (BoxHeight/2);
		}
		else
		{
			pos.x = (Screen.width - BoxWidth) / 2;
			pos.y = (Screen.height - BoxHeight) / 2;
		}
		
		Rect = new Rect(pos.x, pos.y, BoxWidth, BoxHeight);
		
		TopStripe_Rect = new Rect(Rect);
		TopStripe_Rect.height = 4;
		BottomStripe_Rect = new Rect(Rect);
		BottomStripe_Rect.height = 4;
		BottomStripe_Rect.y = Rect.y + Rect.height - BottomStripe_Rect.height;
		Seperator_Rect = new Rect(Rect);
		Seperator_Rect.height = 1;
		
		Button_Rect = new Rect(Rect);
		Button_Rect.x += 4;
		Button_Rect.height = ButtonHeight;

		Visible = true;

		if (Game != null)
			Game.HUD.SetCurrentHelp(HeadsUpDisplay.PROMPT_RIGHTMOUSE, "CANCEL");
	}
	public void Hide()
	{
		Visible = false;

		if (Game != null)
			Game.HUD.SetCurrentHelp(HeadsUpDisplay.PROMPT_LEFTMOUSE, "MENU");
	}


	public virtual void AddItem(string item, Texture2D icon = null)
	{
		if (icon == null)
			icon = GetActionIcon(item);

		Items.Add(item);
		Icons.Add(icon);
		IconColors.Add(default(Color));
	}

	public void SetItems(params string[] items)
	{
		ClearItems();
		
		for (int i = 0; i < items.Length; i++)
			AddItem(items[i]);
	}

	public void ClearItems()
	{
		Items.Clear();
		Icons.Clear();
		IconColors.Clear();
	}

	protected virtual Texture2D GetActionIcon(string item)
	{
		return null;
	}
}
