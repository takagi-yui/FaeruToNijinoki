using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour {
	public static int tutorial;
	Image cover;

	void Start () {
		cover = GameObject.Find ("Cover").GetComponent<Image> ();
		tutorial = 0;
		if (Data.data.stage == 1 && Main.stage == 2) {
			tutorial = 1;
			StartCoroutine (Tutorial1());
		}
		if (Data.data.stage == 3 && Main.stage == 4) {
			tutorial = 2;
			StartCoroutine (Tutorial2());
		}
		if (Data.data.stage == 4 && Main.stage == 5) {
			tutorial = 3;
			StartCoroutine (Tutorial3());
		}
	}

	IEnumerator Tutorial1(){
		GameObject.Find ("skillButton").GetComponent<Button> ().enabled = false;
		while (Main.start == false) {
			float scale = Mathf.Abs (0.2f - (Time.time / 4 % 0.4f)) + 1;
			GameObject.Find ("StartButton").GetComponent<RectTransform>().localScale = new Vector3(scale,scale,scale);
			yield return null;
		}
		GameObject.Find ("TutorialWindow").GetComponent<Image> ().enabled = true;
		Time.timeScale = 0;
		message ("にじのき","わたしは　にじのきです。あなたに　おねがいしたいことが　あります。");
		do {
			yield return null;
		} while(!Input.GetMouseButtonUp (0) || cover.enabled == true);
		message ("ファエル","にじのき?");
		do {
			yield return null;
		} while(!Input.GetMouseButtonUp (0) || cover.enabled == true);
		message ("にじのき","なかまのモンスターたちが　あやつられてしまいました。たすけてください。");
		do {
			yield return null;
		} while(!Input.GetMouseButtonUp (0) || cover.enabled == true);
		message ("ファエル","どうやって　たすけるの?");
		do {
			yield return null;
		} while(!Input.GetMouseButtonUp (0) || cover.enabled == true);
		message ("にじのき","あなたが　たたかうのです。");
		do {
			yield return null;
		} while(!Input.GetMouseButtonUp (0) || cover.enabled == true);
		message ("にじのき","あいてのモンスターまで　せんをひくと\nこうげきすることが　できます。");
		StartCoroutine(Touch (new Vector3(0,-58,0),new Vector3(0,176,0)));
		Monster monster = Main.leader [0];
		while (monster.move.Count == 0) {
			yield return null;
		}
		GameObject.Find ("Inside").GetComponent<Image> ().enabled = false;
		GameObject.Find ("Outside").GetComponent<Image> ().enabled = false;
		message ("にじのき","うえにあるのは　エナジーゲージです。\nゲージがいっぱいになると　うごけます。");
		Time.timeScale = 1;
		while (!(Main.leader[1].HP < Main.leader[1].data.maxHP && Main.gameSpeed == 1)) {
			yield return null;
		}
		Time.timeScale = 0;
		message ("にじのき","あいてのHPが　へりました。0にすると\nたおすことが　できます。");
		do {
			yield return null;
		} while(!Input.GetMouseButtonUp (0) || cover.enabled == true);
		message ("にじのき","もういちど　こうげきして　あいてを　たおしましょう。");
		StartCoroutine(Touch (new Vector3(0,140,0),new Vector3(0,176,0)));
		monster.move.Clear ();
		while (monster.move.Count == 0) {
			yield return null;
		}
		GameObject.Find ("Inside").GetComponent<Image> ().enabled = false;
		GameObject.Find ("Outside").GetComponent<Image> ().enabled = false;
		Time.timeScale = 1;
		while (!(monster.HP < monster.data.maxHP && Main.gameSpeed == 1)) {
			yield return null;
		}
		Time.timeScale = 0;
		message ("にじのき","だいじょうぶですか?");
		do {
			yield return null;
		} while(!Input.GetMouseButtonUp (0) || cover.enabled == true);
		message ("ファエル","まだ　ぼくは　たたかえる。");
		do {
			yield return null;
		} while(!Input.GetMouseButtonUp (0) || cover.enabled == true);
		message ("にじのき","がんばってください。");
		do {
			yield return null;
		} while(!Input.GetMouseButtonUp (0) || cover.enabled == true);
		Time.timeScale = 1;
		while (Main.getEXP == 0) {
			yield return null;
		}
		message ("ファエル","ぼく　かったよ。");
		do {
			yield return null;
		} while(!Input.GetMouseButtonUp (0) || cover.enabled == true);
		message ("にじのき","あいてのモンスターは　ゆめのらくえんに\nにげていきました。");
		do {
			yield return null;
		} while(!Input.GetMouseButtonUp (0) || cover.enabled == true);
		message ("", "");
	}

	IEnumerator Tutorial2(){
		GameObject.Find ("skillButton").GetComponent<Button> ().enabled = false;
		while (Main.start == false) {
			float scale = Mathf.Abs (0.2f - (Time.time / 4 % 0.4f)) + 1;
			GameObject.Find ("StartButton").GetComponent<RectTransform>().localScale = new Vector3(scale,scale,scale);
			yield return null;
		}
		GameObject.Find ("TutorialWindow").GetComponent<Image> ().enabled = true;
		Time.timeScale = 0;
		message ("にじのき","こんどは　モンスターが　たくさんい\nます。");
		do {
			yield return null;
		} while(!Input.GetMouseButtonUp (0) || cover.enabled == true);
		message ("にじのき","スキルを　つかってみましょう。");
		GameObject.Find ("skillButton").GetComponent<Button> ().enabled = true;
		while (Player.mode == 0) {
			float scale = Mathf.Abs (0.2f - (Time.unscaledTime / 4 % 0.4f)) + 1;
			GameObject.Find ("skillButton").GetComponent<RectTransform>().localScale = new Vector3(scale,scale,scale);
			yield return null;
		}
		GameObject.Find ("skillButton").GetComponent<RectTransform>().localScale = new Vector3(1,1,1);
		GameObject.Find ("skillButton").GetComponent<Button> ().enabled = false;
		StartCoroutine(Touch (new Vector3(0,176,0),new Vector3(0,176,0)));
		Monster monster = Main.leader [0];
		while (monster.target == new Vector3(-1,-1,-1)) {
			yield return null;
		}
		GameObject.Find ("Inside").GetComponent<Image> ().enabled = false;
		GameObject.Find ("Outside").GetComponent<Image> ().enabled = false;
		Time.timeScale = 1;
		message ("にじのき","ゲージがたまると　スキルを　つかえ\nます。");
		while (Main.getEXP == 0) {
			yield return null;
		}
		message ("ファエル","ぼく　つよくなった　きがする。");
		do {
			yield return null;
		} while(!Input.GetMouseButtonUp (0) || cover.enabled == true);
		message ("にじのき","モンスターを　たおすと　レベルアップして　つよくなることが　あります。");
		do {
			yield return null;
		} while(!Input.GetMouseButtonUp (0) || cover.enabled == true);
		message ("", "");
	}

	IEnumerator Tutorial3(){
		GameObject.Find ("StartButton").GetComponent<Button> ().enabled = false;
		if (Main.monster [0,2].data.number == -1) {
			message ("にじのき", "ジュエリーは　きょうりょくしたいようです。チームに　いれてみましょう。");
			StartCoroutine(Touch (new Vector3(-130,-100,0),new Vector3(-42,-58,0)));
			while (Main.monster [0,2].data.number == -1) {
				yield return null;
			}
			GameObject.Find ("Inside").GetComponent<Image> ().enabled = false;
			GameObject.Find ("Outside").GetComponent<Image> ().enabled = false;
		}
		message ("にじのき","いまは　なかまを　えらんでいます。");
		do {
			yield return null;
		} while(!Input.GetMouseButtonUp (0) || cover.enabled == true);
		message ("にじのき","あいてのモンスターを　おしてみま\nしょう。");
		while (Player.point.team == 0) {
			yield return null;
		}
		do {
			yield return null;
		} while(!Input.GetMouseButtonUp (0) || cover.enabled == true);
		message ("にじのき","あいてのモンスターを　えらぶと　つよさを　みれます。");
		do {
			yield return null;
		} while(!Input.GetMouseButtonUp (0) || cover.enabled == true);
		message ("にじのき","スキルは　じぶんのモンスターを　えらんでから　つかいましょう。");
		do {
			yield return null;
		} while(!Input.GetMouseButtonUp (0) || cover.enabled == true);
		message ("にじのき","じめんのいろがこい　モンスターが　リーダーです。");
		do {
			yield return null;
		} while(!Input.GetMouseButtonUp (0) || cover.enabled == true);
		message ("にじのき","あいてのリーダーを　たおすと　ほかのモンスターも　たすけられます。");
		do {
			yield return null;
		} while(!Input.GetMouseButtonUp (0) || cover.enabled == true);
		message ("にじのき","じぶんのリーダーが　たおれると　バトルに　まけたことに　なります。");
		do {
			yield return null;
		} while(!Input.GetMouseButtonUp (0) || cover.enabled == true);
		message ("にじのき","あいてのリーダーを　たおしましょう。");
		GameObject.Find ("StartButton").GetComponent<Button> ().enabled = true;
		while (Main.start == false) {
			float scale = Mathf.Abs (0.2f - (Time.time / 4 % 0.4f)) + 1;
			GameObject.Find ("StartButton").GetComponent<RectTransform>().localScale = new Vector3(scale,scale,scale);
			yield return null;
		}
		message ("", "");
		tutorial = 0;
		Monster monster = Main.monster [0, 2];
		while (monster.maxEnergy == 0 && Main.leader [0].maxEnergy == 0) {
			yield return null;
		}
		Time.timeScale = 0;
		message ("にじのき","エナジーを　あつめているとき　したのボタンが　キャンセルに　かわります。");
		do {
			yield return null;
		} while(!Input.GetMouseButtonUp (0) || cover.enabled == true);
		message ("にじのき","キャンセルを　おすと　やめることが　できます。");
		do {
			yield return null;
		} while(!Input.GetMouseButtonUp (0) || cover.enabled == true);
		message ("","");
		Time.timeScale = 1;
		while (Main.leader[1].gameObject != null) {
			yield return null;
		}
		message ("にじのき","ここからさきへいくと　わたしのこえは　とどかなくなります。");
		do {
			yield return null;
		} while(!Input.GetMouseButtonUp (0) || cover.enabled == true);
		message ("ファエル","これからも　がんばるよ。");
		do {
			yield return null;
		} while(!Input.GetMouseButtonUp (0) || cover.enabled == true);
		message ("にじのき","わからないことがあったら　みぎしたの　あそびかたを　みてください。");
		do {
			yield return null;
		} while(!Input.GetMouseButtonUp (0) || cover.enabled == true);
		message ("ファエル","まえのステージに　もどって　とっくんしたり");
		do {
			yield return null;
		} while(!Input.GetMouseButtonUp (0) || cover.enabled == true);
		message ("ファエル","なかまをふやして　みようかな？");
		do {
			yield return null;
		} while(!Input.GetMouseButtonUp (0) || cover.enabled == true);
		message ("", "");
	}

	public static void message(string name, string text){
		GameObject.Find ("TutorialWindow").transform.Find ("name").GetComponent<Text> ().text = name;
		string talk = text;
		if (text.Length > 18 && !text.Contains ("\n")) {
			talk = text.Insert (19, "\n");
		}
		GameObject.Find ("TutorialWindow").transform.Find ("text").GetComponent<Text> ().text = talk;
		if (name == "" && text == "") {
			GameObject.Find ("TutorialWindow").GetComponent<Image> ().enabled = false;
		} else {
			GameObject.Find ("TutorialWindow").GetComponent<Image> ().enabled = true;
		}
	}

	IEnumerator Touch(Vector3 position1, Vector3 position2){
		Image inside = GameObject.Find ("Inside").GetComponent<Image> ();
		Image outside = GameObject.Find ("Outside").GetComponent<Image> ();
		inside.enabled = true;
		outside.enabled = true;
		while (true) {
			GameObject.Find ("Touch").GetComponent<RectTransform>().localPosition = position1;
			outside.GetComponent<RectTransform> ().localScale = new Vector3 (1,1,1);
			inside.GetComponent<RectTransform> ().localScale = new Vector3 (0.4f,0.4f,0.4f);
			outside.color = new Color (1, 1, 1, 100 / 255f);
			inside.color = new Color (1, 1, 1, 100 / 255f);
			while (outside.GetComponent<RectTransform> ().localScale.x > inside.GetComponent<RectTransform> ().localScale.x) {
				float scale = outside.GetComponent<RectTransform> ().localScale.x;
				scale -= 0.02f;
				outside.GetComponent<RectTransform> ().localScale = new Vector3 (scale,scale,scale);
				yield return null;
			}
			if (position1 != position2) {
				for (int i = 0; i < 60; i++) {
					GameObject.Find ("Touch").GetComponent<RectTransform> ().localPosition = Vector3.Lerp(position1,position2,i / 60f);
					yield return null;
				}
			}
			for (float a = 100 / 255f; a >= 0; a -= 1/150f) {
				outside.color = new Color (1, 1, 1, a);
				inside.color = new Color (1, 1, 1, a);
				yield return null;
			}
			if (outside.enabled == false)
				yield break;
		}
	}
}
