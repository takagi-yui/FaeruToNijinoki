using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Story : MonoBehaviour {
	public int number;

	public void Play(){
		World.click = true;
		Main.stage = int.Parse (this.name);
		StartCoroutine(Movie.Play (number));
	}

	void Update(){
		if (int.Parse (this.name) <= Data.data.stage + 1) {
			gameObject.layer = 8;
			gameObject.GetComponent<SpriteRenderer> ().color = new Color (160/255f,180/255f,230/255f);
			if (int.Parse (this.name) <= Data.data.stage) {
				gameObject.transform.Find("Frame").gameObject.GetComponent<SpriteRenderer> ().color = new Color (218/255f,227/255f,133/255f);
			} else {
				gameObject.transform.Find("Frame").gameObject.GetComponent<SpriteRenderer> ().color = new Color (255/255f,255/255f,255/255f);
			}
		} else {
			gameObject.layer = 0;
			gameObject.GetComponent<SpriteRenderer> ().color = new Color (80/255f,90/255f,115/255f);
			gameObject.transform.Find("Frame").gameObject.GetComponent<SpriteRenderer> ().color = new Color (128/255f,128/255f,128/255f);
		}
	}
}
