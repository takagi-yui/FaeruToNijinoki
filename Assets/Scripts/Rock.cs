using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour {
	public void Break(){
		for (int i = 0; i < 8; i++) {
			Rigidbody rigidbody = gameObject.transform.GetChild (i).GetComponent<Rigidbody> ();
			rigidbody.isKinematic = false;
			rigidbody.AddForce (Random.onUnitSphere * Random.Range(2000,3000));
			rigidbody.AddTorque (Random.onUnitSphere * Random.Range(2000,3000));
			StartCoroutine(Fade (gameObject.transform.GetChild (i).gameObject));
		}
		Main.ground [Mathf.RoundToInt (gameObject.transform.position.z), Mathf.RoundToInt (gameObject.transform.position.x)] = null;
		GameObject.Destroy (gameObject,2.0f);
	}

	IEnumerator Fade(GameObject obj){
		yield return new WaitForSeconds (1);
		for (float f = 1; f > 0; f -= Time.deltaTime * 2) {
			Color c = obj.GetComponent<MeshRenderer> ().material.color;
			c.a = f;
			obj.GetComponent<MeshRenderer> ().material.color = c;
			yield return null;
		}
	}
}
