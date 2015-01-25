using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

	public float fadeSpeed = 1f;

	public GameObject canvas;

	public void LoadLevel()
	{
		StartCoroutine( Fade() );
	}

	IEnumerator Fade()
	{
		float i = 1f;
		UnityEngine.UI.Graphic[] all = canvas.GetComponentsInChildren<UnityEngine.UI.Graphic>();

		while(i > 0f)
		{
			i -= Time.deltaTime * fadeSpeed;

			foreach(UnityEngine.UI.Graphic graphic in all)
			{
				Color c = graphic.color;
				c.a = i;
				graphic.color = c;
			}

			yield return null;
		}

		Application.LoadLevel("Space");
	}
}
