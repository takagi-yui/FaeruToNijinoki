using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Talk : MonoBehaviour {
	string[,] message = new string[,]{};
	public static bool talking;

	void Start () {
		talking = false;
		if (Data.data.stage + 1 == Main.stage && Main.stage >= 6) {
			StartCoroutine (Message (Main.stage));
		} else if (Data.data.stage == 20 && Main.enemy.back.middle.number == 14) {
			StartCoroutine (Message (0));
		}
	}

	IEnumerator Message(int stage){
		switch (stage) {
		case 0:
			message = new string[,] {
				{ "ファエル", "このたたかいに　かてば…………" }
			};
			break;
		case 6:
			message = new string[,] {
				{"ファエル","まえにも　このせかいが　おそわれたことがあった。"},
				{"ファエル","そのときは　おじいさんが　たたかいにいった。"},
				{"ファエル","てきが　せめてこなくなったから　ぼくは\nおじいさんが　かったとおもっていた。"},
				{"ファエル","でも　おじいさんは　もどってこなか\nった。"},
				{"ファエル","それに　さっき　てきにおそわれた。おじいさんは　まけたんだ。"}
			};
			break;
		case 7:
			message = new string[,] {
				{"ファエル","ぼくも　みんなをたすけて　ゆめのらくえんに　にげよう。"}
			};
			break;
		case 8:
			message = new string[,] {
				{"ファエル","あいてのマジシャニーは　じめんに　まほうじんを　つくって"},
				{"ファエル","うえにいるモンスターの　スキルを　つよくできるんだ。"}
			};
			break;
		case 9:
			message = new string[,] {
				{"ファエル","みんなで　ゆめのらくえんに　にげようと\nおもったけれど"},
				{"ファエル","にじのきは　ゆめのらくえんに\nいけない。"},
				{"ファエル","ぼくは　どうすればいいんだろう？"}
			};
			break;
		case 10:
			message = new string[,] {
				{"ファエル","あれは　トライホーン。とっしんが　とくいな　モンスターだ。"},
				{"ファエル","みじかく　せんをひいて　とっしんを　よけよう。"}
			};
			break;
		case 11:
			message = new string[,] {
				{"ファエル","ぼくは　きめたんだ。ずっと　このせかいにいる。"},
				{"ファエル","おじいさん　ぼく　がんばるよ。"}
			};
			break;
		case 12:
			message = new string[,] {
				{"ファエル","あいてのブロッカーは　じめんに　バリアを　つくって"},
				{"ファエル","うえにいるモンスターの　まもりを　つよくできるんだ。"}
			};
			break;
		case 14:
			message = new string[,] {
				{"ファエル","あいてのリリースは　まほうじんを　ばくはつさせる　ことができるんだ。"},
				{"ファエル","できるだけ　まほうじんに　のらないようにしよう。"}
			};
			break;
		case 16:
			message = new string[,] {
				{"ファエル","このやまを　のぼれば　もうすぐだ。"}
			};
			break;
		case 18:
			message = new string[,] {
				{"ファエル","あとすこしだ。　がんばろう。"}
			};
			break;
		case 20:
			message = new string[,] {
				{"ファエル","おじいさん？"},
				{"おじいさん","…………"},
				{"ファエル","おじいさん！！"}
			};
			break;
		}
		while (Main.start == false) {
			yield return null;
		}
		GameObject.Find ("skillButton").GetComponent<Button> ().enabled = false;
		Time.timeScale = 0;
		talking = true;
		for(int i = 0; i < message.Length / 2; i++){
			Tutorial.message (message[i,0],message[i,1]);
			do {
				yield return null;
			} while(!Input.GetMouseButtonUp (0) || GameObject.Find ("Cover").GetComponent<Image> ().enabled == true);
		}
		Tutorial.message ("","");
		GameObject.Find ("skillButton").GetComponent<Button> ().enabled = true;
		talking = false;
		Time.timeScale = 1;
	}
}
