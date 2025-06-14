using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Message{
	public static Main main;
	private Text text;

	public Message (Vector3 position,string message,Transform parent,Color color){
		this.text = Main.Instantiate (Resources.Load ("Message") as GameObject, position, Quaternion.Euler (new Vector3 (80, 0, 0)),parent).GetComponentInChildren<Text> ();
		this.text.gameObject.transform.localPosition = Vector3.zero;
		this.text.text = message;
		this.text.color = color;
		main.StartCoroutine (Fade());
	}

	private IEnumerator Fade(){
		Color color = text.color;
		color.a = 1;
		text.color = color;
		while (text.color.a > 0) {
			text.transform.parent.gameObject.transform.rotation = Quaternion.Euler (new Vector3 (80, 0, 0));
			color = text.color;
			color.a -= 1 * Time.deltaTime;
			text.color = color;
			text.gameObject.transform.position = new Vector3 (text.gameObject.transform.position.x,text.gameObject.transform.position.y + (0.5f * Time.deltaTime),text.gameObject.transform.position.z);
			yield return null;
		}
		Main.Destroy (text.transform.parent.gameObject);
	}
}
