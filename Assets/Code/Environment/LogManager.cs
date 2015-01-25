using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LogManager : MonoBehaviour {

	public TextAsset logSource;

	public Text logText;

	private string[] logs;
	private int curIndex = 0;

	private float fadeSpeed = .2f;

	void Start()
	{
		if(!logText)
		{
			Debug.LogError("ASSIGN LOG TEXT");
			return;
		}

		logs = logSource.text.Split(new string[] {"==="}, System.StringSplitOptions.RemoveEmptyEntries);

		StartCoroutine( NextLog() );
	}

	IEnumerator NextLog()
	{
		Color c = logText.color;
		c.a = 1f;
		logText.color = c;

		if(curIndex > logs.Length-1)
		{
			yield break;
		}

		string text = logs[curIndex].Trim();
		int ellipses = text.IndexOf("...");

		for(int i = 0; i <= text.Length; i++)
		{
			logText.text = text.Substring(0, i);

			if(i > ellipses && i < ellipses+3)
				yield return new WaitForSeconds(1);
			else
				yield return new WaitForSeconds(Random.Range(.05f, .09f));
		}

		yield return new WaitForSeconds( 5f );

		float alpha = 1f;
		while(alpha > 0f)
		{
			alpha -= Time.deltaTime * fadeSpeed;
			c.a = alpha;
			logText.color = c;
			yield return null;
		}

		yield return new WaitForSeconds( 5f );

		curIndex++;

		StartCoroutine( NextLog() );
	}
}
