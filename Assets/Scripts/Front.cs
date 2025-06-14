using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Front : MonoBehaviour {
	void Start () {
		GetComponent<MeshRenderer> ().sortingOrder += 1;
	}

}
