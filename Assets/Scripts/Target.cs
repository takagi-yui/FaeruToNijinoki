using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Target : MonoBehaviour {
	Color c;

	void Start () {
		c = GetComponent<Image> ().color;
	}

	void Update () {
		GetComponent<Image> ().color = new Color (c.r,c.g,c.b,0.3f + Mathf.Abs (0.4f - (Time.time / 4 % 0.8f)));
	}
}
