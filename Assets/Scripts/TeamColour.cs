using UnityEngine;
using System.Collections;

public class TeamColour : MonoBehaviour
{

	public Material MaterialNormal;
	public Material MaterialRed;
	public Material MaterialBlue;

	public Shader ShaderNormal;
	public Shader ShaderAlpha;

	public void SetTeam(int team, Color colorOffset = default(Color), float colorMultiplier = 1)
	{
		if ((team == 1 && MaterialRed == null) || (team == 1 && MaterialBlue == null) || (team != 1 && team != 2 && MaterialNormal == null))
		{
			// Tint
			Color color;
			switch (team)
			{
				case 1: color = Color.red; break;
				case 2: color = Color.blue; break;
				default: color = Color.white; break;
			}
			if (GetComponent<Renderer>() != null)
				GetComponent<Renderer>().material.SetColor("_Color", (color - colorOffset) * colorMultiplier);
		}
		else
		{
			// Change Material
			switch (team)
			{
				case 1: GetComponent<Renderer>().material = MaterialRed; break;
				case 2: GetComponent<Renderer>().material = MaterialBlue; break;
				default: GetComponent<Renderer>().material = MaterialNormal; break;
			}
			GetComponent<Renderer>().material.SetColor("_Color", (Color.white - colorOffset) * colorMultiplier);

			if (colorOffset.a > 0)
				GetComponent<Renderer>().material.shader = ShaderAlpha;
			else
				GetComponent<Renderer>().material.shader = ShaderNormal;
		}
	}
}
