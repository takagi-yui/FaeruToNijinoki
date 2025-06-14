using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour {
	public const int X = 7;
	public const int Z = 7;
	public static int[] levelUp = {10,20,40,60,90,120,160,200,300};
	public static bool start;
	public static float gameSpeed;
	public GameObject boxColliderPrefab;
	public Material rock;
	public static GameObject[] boxCollider;
	public static Monster[,] monster;
	public static GameObject[,] ground;
	public static Monster[] leader;
	public static int stage;
	public static Team enemy;
	public static int getEXP;
	public static List<Monster> friend;
	public static int resultCount;
	public static Monster action;

	void Awake(){
		Screen.SetResolution (Screen.height / 16 * 9,Screen.height,Screen.fullScreen);
	}

	void Start(){
		start = false;
		getEXP = 0;
		friend = new List<Monster> ();
		gameSpeed = 1;
		boxCollider = new GameObject[X * Z];
		monster = new Monster[Z,X];
		ground = new GameObject[Z,X];
		leader = new Monster[2];
		Monster.main = gameObject.GetComponent<Main> ();
		Message.main = gameObject.GetComponent<Main> ();
		if (stage >= 16)
			GameObject.Find ("Ground").GetComponent<MeshRenderer> ().material = rock;
		new Monster(Data.data.team.back.right.number,0,4,0,Data.data.team.back.right.EXP);
		new Monster(Data.data.team.back.middle.number,0,3,0,Data.data.team.back.middle.EXP);
		new Monster(Data.data.team.back.left.number,0,2,0,Data.data.team.back.left.EXP);
		new Monster(Data.data.team.front.right.number,0,4,1,Data.data.team.front.right.EXP);
		new Monster(Data.data.team.front.middle.number,0,3,1,Data.data.team.front.middle.EXP);
		new Monster(Data.data.team.front.left.number,0,2,1,Data.data.team.front.left.EXP);
		Enemy (enemy);
		Player.point = monster[0,3];
		for (int z = 0; z < Z; z++) {
			for (int x = 0; x < X; x++) {
				boxCollider[X * z + x] = Instantiate (boxColliderPrefab,new Vector3(x,-0.45f,z),Quaternion.Euler(new Vector3(0,45,0)));
			}
		}
		for (int i = 0; i < 21; i++) {
			new Monster (Data.data.monsterNumber[i], 0, i, -1, Data.data.monsterEXP[i]);
		}
		GameObject.Find ("SkillPanel").GetComponent<Image> ().enabled = true;
		if (Main.monster [6, 3] == null) {
			GameObject.Find("StartButton").GetComponentInChildren<Text>().text = "せつぞく";
		}
		StartCoroutine (Movie.FadeIn());
	}

	void Update(){
		if (!start && Tutorial.tutorial == 0) {
			int count = 0;
			for (int i = 0; i < 21; i++) {
				if (Data.data.monsterNumber [i] != -1) {
					count++;
				}
			}
			//if(!Sync.connect){
				if (count == 21) {
					Tutorial.message ("モンスターが　いっぱいです。", "したのボタンから　モンスターとわかれて\nモンスターリストを　あけてください。");
					GameObject.Find ("StartPanel").GetComponent<Image> ().enabled = true;
				} else {
					Tutorial.message ("", "");
					GameObject.Find ("StartPanel").GetComponent<Image> ().enabled = false;
				}
			//}
		}
	}

	public void GameStart(){
		GameObject.Destroy (GameObject.Find("Team"));
		GameObject.Destroy (GameObject.Find("TeamMonster"));
		GameObject.Destroy (GameObject.Find ("Page"));
		Main.leader[0] = Main.monster[0,3];
		Main.leader[1] = Main.monster [6,3];
		if (leader [1] == null) {
			GameObject.Find ("StartPanel").GetComponent<Image> ().enabled = true;
			GameObject.Find("StartButton").GetComponentInChildren<Text>().text = "スタート";
			Player.point = Main.leader [0];
			Tutorial.message ("","たいせんあいてを　さがしています………");
			//GameObject.Find ("NetworkManager").GetComponent<Match> ().Find ();
		} else {
			GameObject.Destroy (GameObject.Find ("StartButton"));
			/*if (Sync.connect) {
				Tutorial.message ("","たいせんあいてを　まっています………");
				Sync.Ready ();
			} else */{
				Main.start = true;
			}
		}
		Data.Save ();
	}

	public static int Half(int i){
		return Mathf.CeilToInt((float)i / 2);
	}

	public static string Tag(GameObject obj){
		if (obj == null) {
			return "";
		} else {
			return obj.tag;
		}
	}

	public static int Level(int EXP){
		int level = 1;
		for (int i = 0; i < 9; i++) {
			if (EXP >= levelUp [i]) {
				level++;
				EXP -= levelUp [i];
			}
		}
		return level;
	}

	public static int EXP(int level){
		int EXP = 0;
		for (int i = 0; i < level - 1; i++) {
			EXP += levelUp [i];
		}
		return EXP;
	}

	public static void statusUp(Monster monster){
		float up;
		if (Level(monster.EXP) == 10) {
			up = 2;
		} else {
			up = 1 + (0.1f * (Level(monster.EXP) - 1));
		}
		monster.data.maxHP = Mathf.CeilToInt((float)monster.data.maxHP * up);
		monster.data.attack = Mathf.CeilToInt((float)monster.data.attack * up);
		monster.data.speed = Mathf.CeilToInt((float)monster.data.speed * up);
		monster.data.skillPower = Mathf.CeilToInt((float)monster.data.skillPower * up);
		monster.data.getEXP = Mathf.CeilToInt((float)monster.data.getEXP * up);
		monster.data.friend = monster.data.friend / up;
	}

	public static IEnumerator Finish(){
		Time.timeScale = 0;
		GameObject.Find ("MenuPanel").GetComponent<Image> ().enabled = true;
		GameObject.Find ("HowToPlayPanel").GetComponent<Image> ().enabled = true;
		for (float i = 1; i > 0; i-= Time.unscaledDeltaTime){
			if (GameObject.Find ("TutorialWindow").GetComponent<Image> ().enabled == true) {
				i += Time.unscaledDeltaTime;
			}
			BGM.audioSource.volume = i;
			yield return null;
	    }
		/*if (Sync.connect) {
			Sync.StopConnect ();
		}*/
		if(GameObject.Find ("NetworkManager") != null)Destroy(GameObject.Find ("NetworkManager"));
		if (leader [0].gameObject == null && leader [1].gameObject == null) {
			if (action.team == 1) {
				Monster.main.StartCoroutine (Clear ());
			} else {
				Monster.main.StartCoroutine (GameOver ());
			}
		} else {
			if (leader [1].gameObject == null) {
				Monster.main.StartCoroutine (Clear ());
			}
			if (leader [0].gameObject == null) {
				Monster.main.StartCoroutine (GameOver ());
			}
		}
	}

	public static IEnumerator Clear(){
		resultCount = 0;
		GameObject clear = GameObject.Find ("Clear");
		Monster.main.StartCoroutine(GetEXP(Data.data.team.front.left));
		Monster.main.StartCoroutine(GetEXP(Data.data.team.front.middle));
		Monster.main.StartCoroutine(GetEXP(Data.data.team.front.right));
		Monster.main.StartCoroutine(GetEXP(Data.data.team.back.left));
		Monster.main.StartCoroutine(GetEXP(Data.data.team.back.middle));
		Monster.main.StartCoroutine(GetEXP(Data.data.team.back.right));
		for (int z = 0; z < Main.Z; z++) {
			for (int x = 0; x < Main.X; x++) {
				if (Main.monster [z, x] != null) {
					if (Main.monster [z, x].team == 1 && Random.Range (0, 100) < Main.monster [z, x].data.friend) {
						friend.Add (Main.monster [z, x]);
					}
				}
			}
		}
		if (Tutorial.tutorial == 1/* || Sync.connect*/) {
			friend.Clear ();
		}
		if (Tutorial.tutorial == 2) {
			friend.Clear ();
			Monster monster = new Monster (1, 1, 3, 6, 0);
			friend.Add (monster);
			Destroy (monster.gameObject);
		}
		if (Main.leader [1].data.number == 14 && Data.data.stage == 20) {
			friend.Clear ();
			Monster monster = new Monster (13, 1, 3, 6, 1000);
			friend.Add (monster);
			Destroy (monster.gameObject);
		}
		resultCount = 0;
		if (friend.Count > 0) {
			for (int i = 0; i < 21; i++) {
				if (Data.data.monsterNumber [i] == -1) {
					int r = Random.Range (0, friend.Count);
					Data.data.monsterNumber [i] = friend [r].data.number;
					Data.data.monsterEXP [i] = friend [r].EXP;
					Result (friend[r].data.monsterName + "が　なかまになった",friend [r].data.number);
					break;
				}
			}
		}
		if (Main.stage == Data.data.stage + 1) {
			Data.data.stage++;
		}
		if (Data.data.stage == 20 && Main.enemy.back.middle.number == 14) {
			Data.data.stage++;
			World.menu = true;
		}
		Data.Save ();
		Text ClearText = GameObject.Find ("ClearText").GetComponent<Text>();
		clear.SetActive (false);
		ClearText.enabled = true;
		for (float a = 0; a <= 1; a += Time.unscaledDeltaTime) {
			ClearText.color = new Color (218 / 255f,227 / 255f,133 / 255f,a);
			ClearText.fontSize = 180 - Mathf.RoundToInt(a * 120);
			ClearText.rectTransform.localRotation = Quaternion.Euler (0,0,a * 720);
			yield return null;
		}
		ClearText.color = new Color (218 / 255f,227 / 255f,133 / 255f,1);
		ClearText.fontSize = 60;
		ClearText.rectTransform.localRotation = Quaternion.identity;
		yield return new WaitForSecondsRealtime(1);
		ClearText.enabled = false;
		clear.SetActive (true);
		clear.GetComponent<Image> ().enabled = true;
		for (int count = 0; count < resultCount; count++) {
			while (!Input.GetMouseButtonUp (0)) {
				yield return null;
			}
			for (int i = 0; i < 15; i++) {
				clear.GetComponent<RectTransform> ().localPosition = new Vector3 (clear.GetComponent<RectTransform> ().localPosition.x - (338 / 15f), 0, 0);
				yield return null;
			}
		}
		while (!Input.GetMouseButtonUp (0)) {
			yield return null;
		}
		yield return Monster.main.StartCoroutine (Movie.FadeOut ());
		SceneManager.LoadScene ("World");
	}

	public static IEnumerator GameOver(){
		Image gameOver = GameObject.Find ("GameOver").GetComponent<Image>();
		gameOver.enabled = true;
		gameOver.GetComponentInChildren<Text> ().enabled = true;
		for (float a = 0; a <= 0.5f; a += Time.unscaledDeltaTime) {
			gameOver.color = new Color (0,0,0,a);
			gameOver.GetComponentInChildren<Text>().color = new Color (153 / 255f,102 / 255f,170 / 255f,a * 2);
			yield return null;
		}
		gameOver.color = new Color (0,0,0,0.5f);
		gameOver.GetComponentInChildren<Text>().color = new Color (153 / 255f,102 / 255f,170 / 255f,1);
		while (!Input.GetMouseButtonUp (0)) {
			yield return null;
		}
		yield return Monster.main.StartCoroutine (Movie.FadeOut());
		stage = 0;
		SceneManager.LoadScene ("World");
	}

	public static IEnumerator GetEXP(TeamData teamData){
		resultCount++;
		if (teamData.number != -1) {
			int exp = teamData.EXP;
			teamData.EXP += getEXP;
			if (teamData.EXP > 1000)
				teamData.EXP = 1000;
			GameObject obj = Instantiate (Resources.Load ("Level")as GameObject, new Vector3 (100 * ((resultCount - 1) % 3) - 100, resultCount > 3 ? 0 : 100, 0), Quaternion.identity, GameObject.Find ("Clear").transform);
			obj.GetComponent<RectTransform> ().localPosition = new Vector3 (100 * ((resultCount - 1) % 3) - 100, resultCount > 3 ? 0 : 100, 0);
			obj.GetComponent<RectTransform> ().localRotation = Quaternion.identity;
			obj.transform.Find("Name").GetComponent<Text> ().text = Instantiate (Resources.Load ("Data/" + teamData.number) as MonsterData).monsterName;
			Texture2D texture2D = Resources.Load ("Picture/" + teamData.number) as Texture2D;
			obj.transform.Find ("Picture").GetComponent<Image> ().sprite = Sprite.Create(texture2D,new Rect(0,0,texture2D.width,texture2D.height),Vector2.zero);
			yield return new WaitForSecondsRealtime (1.5f);
			float wait = 1 / (float)(teamData.EXP - exp);
			for (int i = 0; i <= getEXP; i++) {
				for (float t = 0; t < wait; t += Time.unscaledDeltaTime) {
					float plus = 0;
					if (i + 1 <= getEXP) {
						plus = (t / wait);
					}
					obj.GetComponentInChildren<Slider> ().value = Level(exp) == 10 ? 1 : ((exp + plus) - EXP (Level (exp))) / (float)levelUp [Level (exp) - 1];
					obj.transform.Find ("Level").GetComponent<Text> ().text = "レベル " + Level (exp);
					yield return null;
				}
				exp++;
			}
		}
	}

	public static void Result(string message,int number){
		resultCount++;
		GameObject result = Instantiate (Resources.Load ("Result")as GameObject, new Vector3 (338 * resultCount, 0, 0), Quaternion.identity, GameObject.Find ("Clear").transform);
		result.GetComponentInChildren<Text> ().text = message;
		Texture2D texture2D = Resources.Load ("Picture/" + number) as Texture2D;
		result.GetComponentInChildren<Image> ().sprite = Sprite.Create(texture2D,new Rect(0,0,texture2D.width,texture2D.height),Vector2.zero);
		result.GetComponent<RectTransform> ().localPosition = new Vector3 (338 * resultCount, 0, 0);
		result.GetComponent<RectTransform> ().localRotation = Quaternion.identity;
	}

	public static void Enemy(Team enemy){
		if (stage == 20 && Data.data.stage == 21 && enemy.back.middle.number == 13) {
			enemy.back.middle.number = 6;
		}
		if(enemy.back.right.number >= 0)new Monster(enemy.back.right.number,1,2,6,EXP(enemy.back.right.EXP));
		if(enemy.back.middle.number >= 0)new Monster(enemy.back.middle.number,1,3,6,EXP(enemy.back.middle.EXP));
		if(enemy.back.left.number >= 0)new Monster(enemy.back.left.number,1,4,6,EXP(enemy.back.left.EXP));
		if(enemy.front.right.number >= 0)new Monster(enemy.front.right.number,1,2,5,EXP(enemy.front.right.EXP));
		if(enemy.front.middle.number >= 0)new Monster(enemy.front.middle.number,1,3,5,EXP(enemy.front.middle.EXP));
		if(enemy.front.left.number >= 0)new Monster(enemy.front.left.number,1,4,5,EXP(enemy.front.left.EXP));
	}
}
