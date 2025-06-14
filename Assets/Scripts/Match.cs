/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

public class Match : MonoBehaviour {
	public static MatchInfo info;
	public static NetworkMatch networkMatch;
	void Awake(){
		networkMatch = gameObject.AddComponent<NetworkMatch> ();
		Sync.connect = false;
		info = new MatchInfo ();
		Sync.check = false;
	}
	void Update(){
		if (Time.timeScale == 1 && Sync.check && GameObject.Find ("NetworkManager").transform.childCount < 2) {
			Sync.check = false;
			Time.timeScale = 0;
			GameObject.Find ("Main Camera").GetComponent<Player> ().enabled = false;
			GameObject.Find ("Menu4").GetComponent<Canvas>().enabled = true;
		}
	}
	public void Find(){
		Application.runInBackground = true;
		Sync.connect = true;
		networkMatch.ListMatches (0,10,"",true,0,0,OnMatchList);
	}
	public void OnMatchList(bool success,string extendedInfo, List<MatchInfoSnapshot> matches){
		if (success) {
			if (matches != null && matches.Count > 0 && matches[0].networkId != info.networkId) {
				networkMatch.JoinMatch (matches [0].networkId, "", "", "", 0, 0, NetworkManager.singleton.OnMatchJoined);
			} else {
				networkMatch.CreateMatch ("room", 2, true, "", "", "", 0, 0, OnMatchCreate);
			}
			StartCoroutine (Wait());
		} else {
			GameObject.Find ("Main Camera").GetComponent<Player> ().enabled = false;
			GameObject.Find ("Menu3").GetComponent<Canvas>().enabled = true;
		}
	}
	void OnMatchCreate(bool success,string extendedInfo,MatchInfo matchInfo){
		if (success) {
			info = matchInfo;
			NetworkManager.singleton.OnMatchCreate (success,extendedInfo,matchInfo);
		} else {
			GameObject.Find ("Main Camera").GetComponent<Player> ().enabled = false;
			GameObject.Find ("Menu3").GetComponent<Canvas>().enabled = true;
		}
	}
	IEnumerator Wait(){
		yield return new WaitForSeconds (Random.Range(2.0f,4.0f));
		if (GameObject.Find ("NetworkManager").transform.childCount < 2) {
			Sync.StopConnect ();
			Find ();
		}
	}
	public static void DestroyMatch(){
		if (info != null) {
			networkMatch.DestroyMatch (info.networkId, 0, NetworkManager.singleton.OnDestroyMatch);
		}
	}
	void OnApplicationPause (bool pauseStatus){
		if (pauseStatus) {
			Sync.StopConnect ();
		}
	}
}*/
