using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class Menu : MonoBehaviour {
	Menu menu;
	float time;

	void Start(){
		menu = GetComponent<Menu> ();
		time = 1;
	}

	public void Open(){
		time = Time.timeScale;
		Time.timeScale = 0;
		GameObject.Find ("Cover").GetComponent<Image> ().enabled = true;
		GameObject.Find ("Menu").GetComponent<Canvas> ().enabled = true;
		GameObject.Find ("Main Camera").GetComponent<Player> ().enabled = false;
	}

	public void HowToPlay(){
		time = Time.timeScale;
		Time.timeScale = 0;
		GameObject.Find ("Cover").GetComponent<Image> ().enabled = true;
		GameObject.Find ("Menu2").GetComponent<Canvas> ().enabled = true;
		GameObject.Find ("Main Camera").GetComponent<Player> ().enabled = false;
	}

	public void Close(){
		Time.timeScale = time;
		GameObject.Find ("Cover").GetComponent<Image> ().enabled = false;
		GameObject.Find ("Menu").GetComponent<Canvas> ().enabled = false;
		GameObject.Find ("Menu1").GetComponent<Canvas> ().enabled = false;
		GameObject.Find ("Menu2").GetComponent<Canvas> ().enabled = false;
		GameObject.Find ("Main Camera").GetComponent<Player> ().enabled = true;
	}

	public void World(){
		menu.StartCoroutine ("Back");
	}

	public void Yes(){
		Monster monster = Player.point;
		Player.point = Main.monster[0,3];
		new Monster (-1,0,Mathf.RoundToInt(monster.gameObject.transform.localPosition.x),-1,0);
		Destroy (monster.gameObject);
		Close ();
	}

	IEnumerator Back(){
		if (!Main.start)
			Data.Save ();
		yield return menu.StartCoroutine (Movie.FadeOut());
		/*if (Sync.connect) {
			Sync.StopConnect ();
		}*/
		if(GameObject.Find ("NetworkManager") != null)Destroy(GameObject.Find ("NetworkManager"));
		Main.stage = 0;
		SceneManager.LoadScene ("World");
	}
}
