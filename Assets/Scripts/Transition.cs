using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Transition : MonoBehaviour {
	public static IEnumerator transition(GameObject obj, string scene){
		GameObject.Find ("Transition").GetComponent<Canvas> ().enabled = true;
		Vector3 cameraPosition = GameObject.Find ("Main Camera").transform.position;
		Movie.image.gameObject.SetActive (true);
		for (float i = 0; i < 1; i += Time.deltaTime) {
			GameObject.Find ("Main Camera").GetComponent<Camera> ().orthographicSize -= 4.5f * Time.deltaTime;
			GameObject.Find ("Camera").GetComponent<Camera> ().orthographicSize -= 4.5f * Time.deltaTime;
			GameObject.Find ("Main Camera").transform.position = Vector3.Lerp (cameraPosition, new Vector3 (obj.transform.position.x, obj.transform.position.y, obj.transform.position.z - 10), i);
			GameObject.Find ("Camera").transform.position = Vector3.Lerp (cameraPosition, new Vector3 (obj.transform.position.x, obj.transform.position.y, obj.transform.position.z - 10), i);
			GameObject.Find ("Transition").GetComponentInChildren<Image> ().material.SetFloat ("_TransitionTime", i);
			GameObject.Find ("Transition").GetComponentInChildren<Image> ().material.SetFloat ("_PositionX", Camera.main.WorldToViewportPoint(obj.transform.position).x);
			GameObject.Find ("Transition").GetComponentInChildren<Image> ().material.SetFloat ("_PositionY", Camera.main.WorldToViewportPoint(obj.transform.position).y);
			Movie.image.color = new Color (0, 0, 0,3 * i - 2);
			World.audioSource.volume = Mathf.Min(0.6f,0.6f - (0.6f * (3 * i - 2)));
			yield return null;
		}
		Movie.image.color = new Color (0, 0, 0, 1);
		if(scene != "")SceneManager.LoadScene (scene);
	}
}
