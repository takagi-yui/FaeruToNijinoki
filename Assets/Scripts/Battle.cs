using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Battle : MonoBehaviour {
	public Team enemy;

	public void Play(){
		World.click = true;
		Main.stage = int.Parse (this.name);
		Main.enemy = enemy;
		StartCoroutine (Transition.transition(gameObject,"Main"));
	}

	void Update(){
		if (int.Parse (this.name) <= Data.data.stage + 1) {
			gameObject.layer = 8;
			gameObject.GetComponent<SpriteRenderer> ().color = new Color (238/255f,160/255f,160/255f);
			if ((int.Parse (this.name) != 20 && int.Parse (this.name) <= Data.data.stage) || (int.Parse (this.name) == 20 && int.Parse (this.name) + 1 <= Data.data.stage)) {
				gameObject.transform.Find("Frame").gameObject.GetComponent<SpriteRenderer> ().color = new Color (218/255f,227/255f,133/255f);
			} else {
				gameObject.transform.Find("Frame").gameObject.GetComponent<SpriteRenderer> ().color = new Color (255/255f,255/255f,255/255f);
			}
		} else {
			gameObject.layer = 0;
			gameObject.GetComponent<SpriteRenderer> ().color = new Color (119/255f,80/255f,80/255f);
			gameObject.transform.Find("Frame").gameObject.GetComponent<SpriteRenderer> ().color = new Color (128/255f,128/255f,128/255f);
		}
	}
}
