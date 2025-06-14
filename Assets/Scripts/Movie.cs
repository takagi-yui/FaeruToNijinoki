using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class Movie : MonoBehaviour {
	public static VideoPlayer videoPlayer;
	public static AudioSource audioSource;
	public static Image image;
	public static World world;
	public static bool skip;

	void Awake(){
		image = GameObject.Find ("Fade").GetComponent<Image> ();
		image.transform.parent.gameObject.SetActive (false);
		if (GameObject.Find ("FadeCanvas")) {
			Destroy (GameObject.Find ("FadeCanvas"));
		}
		image.transform.parent.gameObject.SetActive (true);
		videoPlayer = GameObject.Find ("Video Player").GetComponent<VideoPlayer> ();
		audioSource = GameObject.Find ("Audio Source").GetComponent<AudioSource> ();
	}

	public static IEnumerator Play(int number){
		Movie movie = GameObject.Find("Main Camera").GetComponent<Movie> ();
		if(number < 2)yield return movie.StartCoroutine (FadeOut());
		GameObject.Find ("Movie").GetComponent<Canvas> ().enabled = true;
		if (Main.stage == Data.data.stage + 1 && Main.stage != 20) {
			Data.data.stage++;
			Data.Save ();
		}
		videoPlayer.enabled = true;
		videoPlayer.clip = world.video[number];
		audioSource.clip = world.audio[number];
		if (number == 0 || number == 2 || number == 3) {
			audioSource.loop = true;
		} else {
			audioSource.loop = false;
		}
		if (number == 4) {
			videoPlayer.playbackSpeed = 0.8f;
		} else {
			videoPlayer.playbackSpeed = 0.7f;
		}
		videoPlayer.Play ();
		audioSource.Play ();
		yield return new WaitForSeconds (0.3f);
		while (videoPlayer.frameCount == 0) {
			yield return null;
		}
		yield return movie.StartCoroutine (FadeIn());
		if (number == 2) {
			movie.StartCoroutine (Movie2());
		}
		skip = false;
		while (videoPlayer.isPlaying && skip == false) {
			yield return null;
		}
		if(!image.gameObject.activeInHierarchy)yield return movie.StartCoroutine (FadeOut());
		if (number == 3) {
			movie.StartCoroutine (Play(4));
			yield break;
		}
		GameObject.Find ("Movie").GetComponent<Canvas> ().enabled = false;
		videoPlayer.enabled = false;
		audioSource.Stop ();
		if (number == 2) {
			Main.enemy.back.left.number = -1;
			Main.enemy.back.middle.number = 14;
			Main.enemy.back.right.number = -1;
			Main.enemy.front.left.number = -1;
			Main.enemy.front.middle.number = -1;
			Main.enemy.front.right.number = -1;
			SceneManager.LoadScene ("Main");
		} else {
			World.audioSource.UnPause ();
			World.audioSource.volume = 0.6f;
		}
		World.click = false;
		yield return movie.StartCoroutine (FadeIn());
		Main.stage = 0;
	}

	public static IEnumerator Movie2(){
		Movie movie = GameObject.Find("Main Camera").GetComponent<Movie> ();
		yield return new WaitForSeconds (38.5f);
		GameObject.Find ("Movie").GetComponent<Canvas> ().renderMode = RenderMode.ScreenSpaceCamera;
		movie.StartCoroutine(Transition.transition (videoPlayer.gameObject,""));
	}

	public void Skip(){
		videoPlayer.Pause();
		skip = true;
	}

	public static IEnumerator FadeIn(){
		image.color = new Color (0,0,0,1);
		if (World.title == true) {
			yield return new WaitForSeconds(0.5f);
		}
		for(float a = 1; a >= 0; a -= 0.05f){
			image.color = new Color (0,0,0,a);
			yield return null;
		}
		image.gameObject.SetActive (false);
	}

	public static IEnumerator FadeOut(){
		image.gameObject.SetActive (true);
		float volume = 0;
		if (SceneManager.GetActiveScene ().name == "Main")volume = BGM.audioSource.volume;
		for(float a = 0; a <= 1; a += 0.05f){
			image.color = new Color (0,0,0,a);
			if(SceneManager.GetActiveScene().name == "World" && !World.title)World.audioSource.volume -= 0.03f;
			if (SceneManager.GetActiveScene ().name == "Main")BGM.audioSource.volume = volume * (1 - a);
			yield return null;
		}
	}
}
