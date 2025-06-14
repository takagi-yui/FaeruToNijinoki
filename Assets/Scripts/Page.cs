using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Page : MonoBehaviour {
	GameObject TeamMonster;

	void Start(){
		TeamMonster = GameObject.Find("TeamMonster");
		StartCoroutine (page());
	}

	void Update(){
		GameObject.Find ("Page").GetComponent<Text> ().text = "" + (TeamMonster.transform.position.x / -7 + 1) + "/3";
	}

	IEnumerator page(){
		while (!Main.start) {
			if (Input.GetMouseButton (0) && Tutorial.tutorial == 0) {
				RaycastHit hit;
				if (Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out hit, 100, LayerMask.GetMask ("Next"))) {
					if (TeamMonster.transform.position.x > -14) {
						TeamMonster.transform.position = new Vector3 (TeamMonster.transform.position.x - 7, TeamMonster.transform.position.y, TeamMonster.transform.position.z);
						float time = 0;
						while (time < 0.5f) {
							time += Time.deltaTime;
							if (Input.GetMouseButtonUp (0))
								break;
							yield return null;
						}
					}
				}
				if (Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out hit, 100, LayerMask.GetMask ("Back"))) {
					if (TeamMonster.transform.position.x < 0) {
						TeamMonster.transform.position = new Vector3 (TeamMonster.transform.position.x + 7, TeamMonster.transform.position.y, TeamMonster.transform.position.z);
						float time = 0;
						while (time < 0.5f) {
							time += Time.deltaTime;
							if (Input.GetMouseButtonUp (0))
								break;
							yield return null;
						}
					}
				}
			}
			yield return null;
		}
	}
}
