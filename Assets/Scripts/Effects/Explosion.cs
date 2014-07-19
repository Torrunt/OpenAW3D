using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour
{
	private float ScaleStart = 0.05f;
	private float ScaleSpeed = 2;
	private float Scale;

	private float Alpha = 1;
	private float FadeSpeed = 1.5f;

	// Use this for initialization
	void Start ()
	{
		Scale = ScaleStart;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Alpha <= 0)
		{
			if (!audio.isPlaying)
				GameObject.Destroy(this.gameObject);
			return;
		}

		Scale += ScaleSpeed * Time.deltaTime;
		Alpha -= FadeSpeed * Time.deltaTime;

		transform.localScale = new Vector3(Scale, Scale, Scale);
		renderer.material.color = new Color(renderer.material.color.r, renderer.material.color.g, renderer.material.color.b, Alpha);
		renderer.material.SetColor("_OutlineColor", new Color(
		                           renderer.material.GetColor("_OutlineColor").r,
		                           renderer.material.GetColor("_OutlineColor").g,
		                           renderer.material.GetColor("_OutlineColor").b,
		                           Alpha));
	}
}
