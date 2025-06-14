using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BGM : MonoBehaviour {
	public AudioClip battleA;
	public AudioClip battleB;
	public AudioClip bossA;
	public AudioClip bossB;
	public AudioClip winA;
	public AudioClip winB;
	public AudioClip lose;
	public static AudioSource audioSource;

	void Start () {
		if (Main.enemy.back.middle.number == 14) {
			battleA = bossA;
			battleB = bossB;
		}
		audioSource = GetComponent<AudioSource> ();
		audioSource.clip = battleA;
		audioSource.loop = false;
		audioSource.volume = 1;
		audioSource.Play ();
	}
	
	void Update () {
		if (GameObject.Find("ClearText").GetComponent<Text>().enabled == true && (audioSource.clip == battleA || audioSource.clip == battleB)) {
			audioSource.clip = winA;
			audioSource.loop = false;
			audioSource.volume = 0.7f;
			audioSource.Play ();
		}
		if (GameObject.Find("GameOverText").GetComponent<Text>().enabled == true && (audioSource.clip == battleA || audioSource.clip == battleB)) {
			audioSource.clip = lose;
			audioSource.loop = false;
			audioSource.volume = 0.7f;
			audioSource.Play ();
		}
		if (audioSource.isPlaying == false && audioSource.clip != lose) {
			if (audioSource.clip == winA)
				audioSource.clip = winB;
			if (audioSource.clip == battleA)
				audioSource.clip = battleB;
			audioSource.loop = true;
			audioSource.Play ();
		}
	}
}
