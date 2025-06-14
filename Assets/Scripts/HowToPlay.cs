using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HowToPlay : MonoBehaviour {
	public static int number;

	public void Open(int n){
		number = n;
		GameObject.Find ("HowToPlay" + number).GetComponent<Canvas> ().enabled = true;
	}

	public void Close(){
		GameObject.Find ("HowToPlay" + number).GetComponent<Canvas> ().enabled = false;
	}
}
