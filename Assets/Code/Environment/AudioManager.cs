using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
	// future http://opengameart.org/content/dark-ambience-soundscapes
	public AudioClip[] ambient;

	void Start()
	{
		DontDestroyOnLoad(gameObject);
	}
}
