using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SE : MonoBehaviour {
	AudioSource audioSource;

	void Start () {
		audioSource = GetComponent<AudioSource> ();
	}

	void Update () {
		if (Time.timeScale == 0) {
			audioSource.Pause ();
		} else {
			audioSource.UnPause ();
		}
	}
}
