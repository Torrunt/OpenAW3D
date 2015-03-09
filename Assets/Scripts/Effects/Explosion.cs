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
			if (!GetComponent<AudioSource>().isPlaying)
				GameObject.Destroy(this.gameObject);
			return;
		}

		Scale += ScaleSpeed * Time.deltaTime;
		Alpha -= FadeSpeed * Time.deltaTime;

		transform.localScale = new Vector3(Scale, Scale, Scale);
		GetComponent<Renderer>().material.color = new Color(GetComponent<Renderer>().material.color.r, GetComponent<Renderer>().material.color.g, GetComponent<Renderer>().material.color.b, Alpha);
		GetComponent<Renderer>().material.SetColor("_OutlineColor", new Color(
		                           GetComponent<Renderer>().material.GetColor("_OutlineColor").r,
		                           GetComponent<Renderer>().material.GetColor("_OutlineColor").g,
		                           GetComponent<Renderer>().material.GetColor("_OutlineColor").b,
		                           Alpha));
	}
}
