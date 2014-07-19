using UnityEngine;

public static class GUIUtils
{
	public static void DrawTextWithOutline(Rect rect, string text, GUIStyle style, Color outlineColor, float thickness = 2)
	{
		GUIStyle outlineStyle = new GUIStyle(style);
		outlineStyle.normal.textColor = outlineColor;

		for (int i = 0; i < 8; i++)
		{
			Rect pos = new Rect(rect);
			switch (i)
			{
				case 0: pos.x -= thickness; break;
				case 1: pos.x += thickness; break;
				case 2: pos.y -= thickness; break;
				case 3: pos.y += thickness; break;
				case 4: pos.x -= thickness; pos.y -= thickness; break;
				case 5: pos.x -= thickness; pos.y += thickness; break;
				case 6: pos.x += thickness; pos.y -= thickness; break;
				case 7: pos.x += thickness; pos.y += thickness; break;
			}

			GUI.Label(pos, text, outlineStyle);
		}

		GUI.Label(rect, text, style);
	}

	public static void DrawBoxWithOutline(Rect rect, GUIContent guiContent, GUIStyle style, Texture2D outlineTexture, float thickness = 2)
	{
		GUIStyle outlineStyle = new GUIStyle(style);
		outlineStyle.normal.background = outlineTexture;
		Rect outlineRect = new Rect(rect.x - thickness, rect.y - thickness, rect.width + (thickness * 2), rect.height + (thickness * 2));
		GUI.Box(outlineRect, guiContent, outlineStyle);

		GUI.Box(rect, guiContent, style);
	}
}