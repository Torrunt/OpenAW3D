using UnityEngine;
using System.Collections;

public class TeamColour : MonoBehaviour
{

	public Material MaterialNormal;
	public Material MaterialRed;
	public Material MaterialBlue;

	public Shader ShaderNormal;
	public Shader ShaderAlpha;

	public void SetTeam(int team, Color colorOffset = default(Color))
	{
		if (MaterialNormal == null || MaterialRed == null || MaterialBlue == null)
		{
			// Tint
			switch (team)
			{
				case 1:
				{
					if (renderer != null)
						renderer.material.SetColor("_Color", Color.red - colorOffset);
					break;
				}
				case 2:
				{
					if (renderer != null)
						renderer.material.SetColor("_Color", Color.blue - colorOffset);
					break;
				}
				default:
				{
					if (renderer != null)
						renderer.material.SetColor("_Color", Color.white - colorOffset);
					break;
				}
			}
		}
		else
		{
			// Change Material
			switch (team)
			{
				case 1: renderer.material = MaterialRed; break;
				case 2: renderer.material = MaterialBlue; break;
				default: renderer.material = MaterialNormal; break;
			}
			renderer.material.SetColor("_Color", Color.white - colorOffset);

			if (colorOffset.a > 0)
				renderer.material.shader = ShaderAlpha;
			else
				renderer.material.shader = ShaderNormal;
		}
	}
}
