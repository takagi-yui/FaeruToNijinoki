using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
public class World : MonoBehaviour {
	public AudioClip worldA;
	public AudioClip worldB;
	public VideoClip[] video;
	public AudioClip[] audio;
	GameObject map;
	float minY;
	float maxY;
	float y;
	float downY;
	public static bool menu = false;
	public static AudioSource audioSource;
	public static VideoPlayer videoPlayer;
	public static bool title = true;
	public static Vector3 mapPosition = new Vector3 (0,10,0);
	public static bool click;

	void Awake(){
		Screen.SetResolution (Screen.height / 16 * 9,Screen.height,Screen.fullScreen);
	}

	void Start () {
		audioSource = GetComponent<AudioSource> ();
		audioSource.clip = worldA;
		audioSource.loop = false;
		audioSource.volume = 0.6f;
		GameObject.Find ("Image").GetComponent<Image> ().material.SetFloat ("_TransitionTime", 0);
		GameObject.Find ("Image").GetComponent<Image> ().material.SetFloat ("_PositionX", 0);
		GameObject.Find ("Image").GetComponent<Image> ().material.SetFloat ("_PositionY", 0);
		DontDestroyOnLoad (GameObject.Find ("FadeCanvas"));
		minY = -10;
		maxY = 10;
		y = 0;
		downY = 0;
		Movie.world = GetComponent<World> ();
		map = GameObject.Find ("map");
		map.transform.position = mapPosition;
		click = false;
		Time.timeScale = 1;
		if (menu) {
			GameObject.Find ("Clear").GetComponent<Canvas> ().enabled = true;
			GameObject.Find ("MenuButton").GetComponent<Button> ().enabled = false;
		}
		if (title) {
			videoPlayer = GameObject.Find ("Video Player").GetComponent<VideoPlayer> ();
			videoPlayer.clip = video[5];
			videoPlayer.isLooping = true;
			videoPlayer.Play ();
			GameObject.Find ("Title").GetComponent<Canvas> ().enabled = true;
		}
		if (Main.stage == 20) {
			if (Main.enemy.back.middle.number != 14) {
				StartCoroutine (Movie.Play (2));
			} else {
				StartCoroutine (Movie.Play (3));
			}
		} else {
			audioSource.Play ();
			StartCoroutine (Movie.FadeIn());
		}
	}

	void Update () {
		if (Movie.videoPlayer.isPlaying == false && title == false && menu == false && GameObject.Find ("Transition").GetComponent<Canvas> ().enabled == false) {
			if (Input.GetMouseButtonDown (0)) {
				downY = Input.mousePosition.y;
			}
			if (Input.GetMouseButton (0)) {
				if (y != 0) {
					map.GetComponent<Rigidbody2D> ().linearVelocity = new Vector2 (0, Input.mousePosition.y - y);
				}
				y = Input.mousePosition.y;
			}
			if (Input.GetMouseButtonUp (0) && click == false) {
				if (Mathf.Abs (downY - y) < 10) {
					Collider2D hitCollider2D = Physics2D.OverlapPoint (Camera.main.ScreenToWorldPoint (Input.mousePosition));
					if (hitCollider2D != null) {
						if (Data.data.stage >= 5 || "" + (Data.data.stage + 1) == hitCollider2D.gameObject.name) {
							if (hitCollider2D.gameObject.tag == "Story" && hitCollider2D.gameObject.layer == 8) {
								hitCollider2D.gameObject.GetComponent<Story> ().Play ();
							}
							if (hitCollider2D.gameObject.tag == "Battle" && hitCollider2D.gameObject.layer == 8) {
								mapPosition = map.transform.position;
								hitCollider2D.gameObject.GetComponent<Battle> ().Play ();
							}
						}
					}
				}
				y = 0;
			}
			Vector3 position = map.transform.position;
			position.y -= Input.GetAxis ("Mouse ScrollWheel") * 8;
			map.transform.position = position;
			if (Data.data.stage <= 20) {
				if (GameObject.Find ("Touch").GetComponent<ParticleSystem> ().isPlaying == false) {
					GameObject.Find ("Touch").GetComponent<ParticleSystem> ().Play ();
				}
				GameObject.Find ("Touch").transform.Rotate (new Vector3 (0, 0, 3));
				if (Data.data.stage >= 19) {
					GameObject.Find ("Touch").transform.localScale = new Vector3 (2,2,2);
					GameObject.Find ("Touch").transform.position = GameObject.Find ("20").transform.position;
				} else {
					GameObject.Find ("Touch").transform.localScale = new Vector3 (1,1,1);
					GameObject.Find ("Touch").transform.position = GameObject.Find ("" + (Data.data.stage + 1)).transform.position;
				}
			} else {
				GameObject.Find ("Touch").GetComponent<ParticleSystem> ().Stop();
			}
		}
		if (audioSource.isPlaying == false && GameObject.Find("Audio Source").GetComponent<AudioSource>().isPlaying == false) {
		    audioSource.clip = worldB;
			audioSource.loop = true;
			audioSource.Play ();
		}
		if (menu) {
			GameObject.Find ("Touch").GetComponent<ParticleSystem> ().Stop();
		} else if(GameObject.Find ("Touch").GetComponent<ParticleSystem> ().isPlaying == false && !title && GameObject.Find ("Transition").GetComponent<Canvas> ().enabled == false){
			GameObject.Find ("Touch").GetComponent<ParticleSystem> ().Play();
		}
		if (map.transform.position.y < minY) {
			map.transform.position = new Vector3 (0, minY, 0);
		}
		if (map.transform.position.y > maxY) {
			map.transform.position = new Vector3 (0, maxY, 0);
		}
	}

	public void OnClickStart(){
		GameObject.Find ("Title").GetComponent<AudioSource> ().Play ();
		StartCoroutine (Fade());
	}

	public void OnClickQuit(){
		Application.Quit ();
	}

	public void Menu(){
		menu = true;
		GameObject.Find ("Continue").GetComponent<Button> ().enabled = true;
		GameObject.Find ("Battle").GetComponent<Button> ().enabled = true;
		if (Data.data.stage == 21) {
			GameObject.Find ("Battle").GetComponentInChildren<Text> ().text = "たいせん";
			GameObject.Find ("BattlePanel").GetComponent<Image> ().enabled = false;
		} else {
			GameObject.Find ("Battle").GetComponentInChildren<Text> ().text = "？？？";
			GameObject.Find ("BattlePanel").GetComponent<Image> ().enabled = true;
		}
		GameObject.Find ("Setting").GetComponent<Button> ().enabled = true;
		GameObject.Find ("Back").GetComponent<Button> ().enabled = true;
		GameObject.Find ("Menu").GetComponent<Canvas>().enabled = true;
		GameObject.Find ("Menu1").GetComponent<Canvas>().enabled = false;
		GameObject.Find ("Menu2").GetComponent<Canvas>().enabled = false;
	}

	public void Close(){
		StartCoroutine(SetMenu ());
		GameObject.Find ("Menu").GetComponent<Canvas>().enabled = false;
		GameObject.Find ("Clear").GetComponent<Canvas>().enabled = false;
		GameObject.Find ("MenuButton").GetComponent<Button> ().enabled = true;
	}

	public void Net(){
		StartCoroutine (StartBattle());
	}

	IEnumerator StartBattle(){
		click = true;
		yield return StartCoroutine (Movie.FadeOut());
		menu = false;
		Main.stage = -1;
		Team enemy = new Team();
		enemy.back = new TeamPosition ();
		enemy.front = new TeamPosition ();
		enemy.back.left = new TeamData ();
		enemy.back.middle = new TeamData ();
		enemy.back.right = new TeamData ();
		enemy.front.left = new TeamData ();
		enemy.front.middle = new TeamData ();
		enemy.front.right = new TeamData ();
		enemy.back.left.number = -1;
		enemy.back.middle.number = -1;
		enemy.back.right.number = -1;
		enemy.front.left.number = -1;
		enemy.front.middle.number = -1;
		enemy.front.right.number = -1;
		Main.enemy = enemy;
		SceneManager.LoadScene ("Main");
	}

	public void Setting(){
		GameObject.Find ("Continue").GetComponent<Button> ().enabled = false;
		GameObject.Find ("Battle").GetComponent<Button> ().enabled = false;
		GameObject.Find ("Setting").GetComponent<Button> ().enabled = false;
		GameObject.Find ("Back").GetComponent<Button> ().enabled = false;
		GameObject.Find ("Menu1").GetComponent<Canvas>().enabled = true;
	}

	public void Back(){
		StartCoroutine (Title());
	}

	public void Delete(){
		GameObject.Find ("Menu1").GetComponent<Canvas>().enabled = false;
		GameObject.Find ("Menu2").GetComponent<Canvas>().enabled = true;
	}

	IEnumerator Fade(){
		yield return StartCoroutine (Movie.FadeOut());
		videoPlayer.Stop ();
		videoPlayer.isLooping = false;
		GameObject.Find ("Title").GetComponent<Canvas> ().enabled = false;
		title = false;
		yield return StartCoroutine (Movie.FadeIn());
	}

	IEnumerator SetMenu(){
		yield return null;
		menu = false;
	}

	public static IEnumerator Title(){
		World world = GameObject.Find ("Main Camera").GetComponent<World> ();
		yield return world.StartCoroutine (Movie.FadeOut());
		title = true;
		menu = false;
		SceneManager.LoadScene ("World");
	}
}
