using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balloon : MonoBehaviour
{
	//constantes
	private const int fps = 60;
	private	const float maxScale = 8.0f;
	private	const int nbFramesWithoutActionBeforeQuit = 600;
	private	const int nbFramesWithoutActionBeforeDegonfle = 10;

	//compteurs de frames
	private int nbFrames = 0;
	private int nbFramesWithoutAction = 0;

	//infos de souffle
	private int souffle = 50;
	private int souffleHits;

	void Update ()
	{
		nbFrames++;
		if (Input.GetKeyDown ("space"))
		{
			if (nbFramesWithoutAction > 10)
				souffleHits = nbFramesWithoutActionBeforeDegonfle;
			else
				souffleHits = nbFramesWithoutActionBeforeDegonfle * 2 - nbFramesWithoutAction;
		}
		if (Input.GetKeyDown ("space") && souffle >= souffleHits)
		{
			souffle -= souffleHits;
			nbFramesWithoutAction = 0;
			transform.localScale += new Vector3 (0.1f, 0.1f, 0);
			if (transform.localScale.x > maxScale)
			{
				Debug.Log ("Balloon life time: " + Mathf.RoundToInt ((float)nbFrames / fps) + "s");
				GameObject.Destroy (transform.root.gameObject);
			}
		}
		else
		{
			if (souffle < 50)
				souffle += 1;
			nbFramesWithoutAction++;
			if (nbFramesWithoutAction % nbFramesWithoutActionBeforeDegonfle == 0)
				if (transform.localScale.x > 1.0f)
					transform.localScale -= new Vector3 (0.1f, 0.1f, 0);
			if (nbFramesWithoutAction >= nbFramesWithoutActionBeforeQuit)
			{
				Debug.Log ("Balloon life time: " + Mathf.RoundToInt ((float)nbFrames / fps) + "s");
				GameObject.Destroy (this);
			}
		}
	}
}
