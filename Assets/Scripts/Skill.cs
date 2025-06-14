using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skill : MonoBehaviour {
	public static Skill skill;
	Collider[] collisions;
	Vector3 colliderPosition;
	Vector3 colliderSize;
	List<Monster> hitMonster;
	List<GameObject> hitRock;

	void Awake(){
		skill = GetComponent<Skill> ();
		hitMonster = new List<Monster> ();
		hitRock = new List<GameObject> ();
	}

	public void Touch(){
		if (Main.start) {
			if (Player.mode == 0) {
				if (GameObject.Find ("skillButton").gameObject.GetComponentInChildren<Text> ().text != "キャンセル") {
					Player.mode = Player.point.data.targetType;
					if (Player.mode == 0) {
						Player.SetSkill (Player.point, new Vector3 (-100, 0, -100));
					}
				} else {
					Cancel (Player.point);
				}
			} else {
				Player.mode = 0;
			}
		} else {
			Time.timeScale = 0;
			GameObject.Find ("Cover").GetComponent<Image> ().enabled = true;
			GameObject.Find ("Menu1").GetComponent<Canvas> ().enabled = true;
			GameObject.Find ("Main Camera").GetComponent<Player> ().enabled = false;
		}
	}

	public static void Cancel(Monster monster){
		monster.energy = 0;
		monster.maxEnergy = 0;
		monster.gauge.transform.Find ("MaxEnergy").Find ("Energy").gameObject.GetComponent<Image> ().fillAmount = 0;
		monster.gauge.transform.Find ("MaxEnergy").gameObject.GetComponent<Image> ().enabled = false;
		monster.targetObject.GetComponentInChildren<Image> ().enabled = false;
		monster.move.Clear ();
		monster.path.positionCount = 0;
		/*if (Sync.connect && monster.team == 0) {
			Sync.CancelMessage cancelMessage = new Sync.CancelMessage ();
			cancelMessage.monsterPosition = Player.Reverse (monster.gameObject.transform.position);
			Sync.SendCancel (cancelMessage);
		}*/
	}

	public static void Use(int number,Monster monster){
		if (monster.team == 0) {
			monster.targetObject.GetComponentInChildren<Image> ().enabled = false;
		}
		monster.Reset ();
		new Message (monster.gameObject.transform.position,monster.data.skillName,monster.gameObject.transform,monster.targetObject.GetComponentInChildren<Image>().color);
		skill.StartCoroutine ("Skill" + number,monster);
	}

	private IEnumerator Skill0(Monster monster){
		GameObject obj = Instantiate (Resources.Load("Skill/" + monster.data.number) as GameObject,monster.target,Quaternion.identity);
		yield return StartCoroutine (Damage(obj.transform.position,new Vector3(1,1,1),monster.data.skillPower,2.9f));
		while (obj.GetComponent<AudioSource> ().isPlaying == true) {
			yield return null;
		}
		Destroy (obj);
		Main.gameSpeed = 1;
	}

	private IEnumerator Skill1(Monster monster){
		GameObject obj = Instantiate (Resources.Load("Skill/1") as GameObject,monster.target,Quaternion.identity);
		yield return StartCoroutine (Heal(obj.transform.position,new Vector3(0.1f,1,0.1f),monster.data.skillPower,1));
		Destroy (obj);
		Main.gameSpeed = 1;
	}

	private IEnumerator Skill2(Monster monster){
		Main.monster[Mathf.RoundToInt(monster.gameObject.transform.position.z),Mathf.RoundToInt(monster.gameObject.transform.position.x)] = null;
		monster.gameObject.transform.LookAt (new Vector3(monster.target.x,monster.gameObject.transform.position.y,monster.target.z));
		monster.gauge.transform.rotation = Quaternion.Euler (new Vector3(80,0,0));
		monster.color.transform.rotation = Quaternion.Euler (new Vector3(0,0,0));
		monster.time = 0;
		Vector3 position;
		Vector3 position1 = monster.gameObject.transform.position + monster.gameObject.transform.forward;
		while (Mathf.RoundToInt (position1.x) >= 0 && Mathf.RoundToInt (position1.z) >= 0 && Mathf.RoundToInt (position1.x) < Main.X && Mathf.RoundToInt (position1.z) < Main.Z) {
			if (Main.monster [Mathf.RoundToInt (position1.z), Mathf.RoundToInt (position1.x)] != null)
				break;
			if (Main.Tag (Main.ground [Mathf.RoundToInt (position1.z), Mathf.RoundToInt (position1.x)]) == "Rock")
				break;
			position = monster.gameObject.transform.position;
			position1 = new Vector3 (Mathf.RoundToInt(position1.x),0,Mathf.RoundToInt(position1.z));
			while (monster.time < 0.15f) {
				monster.gameObject.transform.position = Vector3.Lerp (position, position1, monster.time / 0.15f);
				monster.time += Time.deltaTime;
				yield return null;
			}
			monster.time -= 0.15f;
			monster.gameObject.transform.position = position1;
			position1 = monster.gameObject.transform.position + monster.gameObject.transform.forward;
		}
		Main.monster[Mathf.RoundToInt(monster.gameObject.transform.position.z),Mathf.RoundToInt(monster.gameObject.transform.position.x)] = monster;
		if (Mathf.RoundToInt (position1.x) >= 0 && Mathf.RoundToInt (position1.z) >= 0 && Mathf.RoundToInt (position1.x) < Main.X && Mathf.RoundToInt (position1.z) < Main.Z) {
			monster.time = 0;
			GameObject obj = monster.gameObject.transform.Find (monster.data.monsterName).gameObject;
			position = obj.transform.position;
			while (monster.time < 0.15f) {
				obj.transform.position = Vector3.Lerp (position, obj.transform.position + (obj.transform.forward * 0.15f), monster.time / 0.15f);
				monster.time += Time.deltaTime;
				yield return null;
			}
			if (Main.monster [Mathf.RoundToInt (position1.z), Mathf.RoundToInt (position1.x)] != null)
				Main.monster [Mathf.RoundToInt (position1.z), Mathf.RoundToInt (position1.x)].Damage (monster.data.skillPower, monster.gameObject.transform.position);
			if (Main.Tag (Main.ground [Mathf.RoundToInt (position1.z), Mathf.RoundToInt (position1.x)]) == "Rock")
				Main.ground [Mathf.RoundToInt (position1.z), Mathf.RoundToInt (position1.x)].GetComponent<Rock> ().Break ();
			GameObject.Instantiate (Resources.Load("Hit") as GameObject, position1,Quaternion.Euler(new Vector3(80,0,0)));
			while (monster.time < 0.2f) {
				obj.transform.position = Vector3.Lerp (obj.transform.position + (obj.transform.forward * 0.1f), position, monster.time / 0.2f);
				monster.time += Time.deltaTime;
				yield return null;
			}
		}
		monster.time = 0;
		Main.gameSpeed = 1;
	}

	private IEnumerator Skill3(Monster monster){
		if (Main.monster [Mathf.RoundToInt (monster.target.z), Mathf.RoundToInt (monster.target.x)] == null) {
			GameObject obj = Instantiate (Resources.Load("Skill/3") as GameObject,monster.target,Quaternion.identity);
			monster.data.maxHP = Mathf.CeilToInt ((float)monster.data.maxHP / 2);
			monster.HP = Mathf.CeilToInt ((float)monster.HP / 2);
			monster.gauge.transform.Find ("HP").gameObject.GetComponent<UnityEngine.UI.Slider> ().maxValue = monster.data.maxHP;
			monster.gauge.transform.Find ("HP").gameObject.GetComponent<UnityEngine.UI.Slider> ().value = monster.HP;
			Monster clone = new Monster (3, monster.team, Mathf.RoundToInt (monster.target.x), Mathf.RoundToInt (monster.target.z),monster.EXP);
			clone.data = Instantiate (monster.data);
			clone.HP = monster.HP;
			clone.gauge.transform.Find ("HP").gameObject.GetComponent<UnityEngine.UI.Slider> ().maxValue = clone.data.maxHP;
			clone.gauge.transform.Find ("HP").gameObject.GetComponent<UnityEngine.UI.Slider> ().value = clone.HP;
			if (clone.HP > clone.data.maxHP)
				clone.HP = clone.data.maxHP;
			MeshRenderer[] meshRenderer = clone.gameObject.transform.Find (clone.data.monsterName).GetComponentsInChildren<MeshRenderer> ();
			for (float f = 0; f < 1; f += Time.deltaTime) {
				for (int i = 0; i < meshRenderer.LongLength; i++) {
					Color c = meshRenderer [i].material.color;
					c.a = f;
					if (c.a < 0) {
						c.a = 0;
					}
					meshRenderer [i].material.color = c;
				}
				yield return null;
			}
			Main.monster [Mathf.RoundToInt (monster.target.z), Mathf.RoundToInt (monster.target.x)] = clone;
			Destroy (obj);
		} else {
			monster.gameObject.GetComponentInChildren<Text> ().text = "ミス";
		}
		Main.gameSpeed = 1;
	}

	private IEnumerator Skill4(Monster monster){
		int count = 0;
		for (int z = 0; z < Main.Z; z++) {
			for (int x = 0; x < Main.X; x++) {
				if (Main.Tag (Main.ground [Mathf.RoundToInt (z), Mathf.RoundToInt (x)]) == "MagicCircle") {
					Instantiate (Resources.Load("Skill/4") as GameObject,new Vector3(x,0,z),Quaternion.identity);
					StartCoroutine(Damage4 (Main.monster [Mathf.RoundToInt (z), Mathf.RoundToInt (x)],monster.data.skillPower));
					Destroy (Main.ground [Mathf.RoundToInt (z), Mathf.RoundToInt (x)]);
					count++;
				}
			}
		}
		if (count > 0) {
			GameObject obj = Instantiate (Resources.Load("Skill/4_audio") as GameObject,new Vector3(3,0,3),Quaternion.identity);
			yield return new WaitForSeconds (2);
			Destroy (obj);
		} else {
			monster.gameObject.GetComponentInChildren<Text> ().text = "ミス";
		}
		Main.gameSpeed = 1;
	}

	private IEnumerator Damage4(Monster monster,int power){
		if (monster != null) {
			yield return new WaitForSeconds (0.5f);
			monster.Damage (power, monster.gameObject.transform.position + monster.gameObject.transform.forward);
		}
	}

	private IEnumerator Skill5(Monster monster){
		monster.gameObject.transform.LookAt (new Vector3(monster.target.x,monster.gameObject.transform.position.y,monster.target.z));
		monster.gauge.transform.rotation = Quaternion.Euler (new Vector3(80,0,0));
		monster.color.transform.rotation = Quaternion.Euler (new Vector3(0,0,0));
		GameObject obj = Instantiate (Resources.Load("Skill/5") as GameObject,monster.target,monster.gameObject.transform.rotation);
		List<Monster> hit = new List<Monster> ();
		monster.time = 0;
		while (monster.time < 2.5f) {
			if (Mathf.RoundToInt (obj.transform.position.x) >= 0 && Mathf.RoundToInt (obj.transform.position.z) >= 0 && Mathf.RoundToInt (obj.transform.position.x) < Main.X && Mathf.RoundToInt (obj.transform.position.z) < Main.Z) {
				if (Main.Tag (Main.ground [Mathf.RoundToInt (obj.transform.position.z), Mathf.RoundToInt (obj.transform.position.x)]) == "Rock") {
					Main.ground [Mathf.RoundToInt (obj.transform.position.z), Mathf.RoundToInt (obj.transform.position.x)].GetComponent<Rock> ().Break ();
				}
				if (Main.monster [Mathf.RoundToInt (obj.transform.position.z), Mathf.RoundToInt (obj.transform.position.x)] != null && !hit.Contains(Main.monster [Mathf.RoundToInt (obj.transform.position.z), Mathf.RoundToInt (obj.transform.position.x)])) {
					hit.Add (Main.monster [Mathf.RoundToInt (obj.transform.position.z), Mathf.RoundToInt (obj.transform.position.x)]);
					Main.monster [Mathf.RoundToInt (obj.transform.position.z), Mathf.RoundToInt (obj.transform.position.x)].Damage (monster.data.skillPower,monster.gameObject.transform.position);
				}
			}
			obj.transform.position += obj.transform.forward * Time.deltaTime * 4;
			monster.time += Time.deltaTime;
			yield return null;
		}
		monster.time = 0;
		GameObject.Destroy (obj);
		Main.gameSpeed = 1;
	}

	private IEnumerator Skill6(Monster monster){
		GameObject obj = Instantiate (Resources.Load("Skill/6") as GameObject,monster.target,Quaternion.identity);
		yield return StartCoroutine (Damage(obj.transform.position,new Vector3(0.1f,1,0.1f),monster.data.skillPower,1));
		while (obj.GetComponent<ParticleSystem> ().isPlaying == true) {
			yield return null;
		}
		Destroy (obj);
		Main.gameSpeed = 1;
	}

	private IEnumerator Skill7(Monster monster){
		bool place;
		if (Main.monster [Mathf.RoundToInt (monster.target.z), Mathf.RoundToInt (monster.target.x)] == null) {
			place = true;
		} else {
			if (Main.monster [Mathf.RoundToInt (monster.target.z), Mathf.RoundToInt (monster.target.x)].data.isFlying) {
				place = true;
			} else {
				place = false;
			}
		}
		if (Main.ground [Mathf.RoundToInt (monster.target.z), Mathf.RoundToInt (monster.target.x)] == null && place) {
			GameObject obj = Instantiate (Resources.Load("Skill/7") as GameObject,monster.target,Quaternion.identity);
			Main.ground [Mathf.RoundToInt (monster.target.z), Mathf.RoundToInt (monster.target.x)] = Instantiate (Resources.Load ("Rock") as GameObject, new Vector3 (Mathf.RoundToInt (monster.target.x), -1, Mathf.RoundToInt (monster.target.z)), Quaternion.identity);
			for (float y = -1; y <= -0.45; y += Time.deltaTime) {
				Main.ground [Mathf.RoundToInt (monster.target.z), Mathf.RoundToInt (monster.target.x)].transform.position = new Vector3 (Mathf.RoundToInt (monster.target.x), y, Mathf.RoundToInt (monster.target.z));
				yield return null;
			}
			Destroy (obj);
		} else {
			monster.gameObject.GetComponentInChildren<Text> ().text = "ミス";
		}
		Main.gameSpeed = 1;
	}

	private IEnumerator Skill8(Monster monster){
		if (Main.ground [Mathf.RoundToInt (monster.target.z), Mathf.RoundToInt (monster.target.x)] == null) {
			GameObject obj = Instantiate (Resources.Load("Skill/8") as GameObject,monster.target,Quaternion.identity);
			Main.ground [Mathf.RoundToInt (monster.target.z), Mathf.RoundToInt (monster.target.x)] = Instantiate (Resources.Load ("MagicCircle") as GameObject, new Vector3 (Mathf.RoundToInt (monster.target.x), 0, Mathf.RoundToInt (monster.target.z)), Quaternion.identity);
			SpriteRenderer spriteRenderer = Main.ground [Mathf.RoundToInt (monster.target.z), Mathf.RoundToInt (monster.target.x)].GetComponentInChildren<SpriteRenderer> ();
			for (float f = 0; f < 1; f += Time.deltaTime) {
				Color c = spriteRenderer.color;
				c.a += Time.deltaTime;
				spriteRenderer.color = c;
				yield return null;
			}
			spriteRenderer.color = new Color (1, 1, 1, 1);
			Destroy (obj);
		} else {
			monster.gameObject.GetComponentInChildren<Text> ().text = "ミス";
		}
		Main.gameSpeed = 1;
	}

	private IEnumerator Skill9(Monster monster){
		if (Main.ground [Mathf.RoundToInt (monster.target.z), Mathf.RoundToInt (monster.target.x)] == null) {
			monster.gameObject.GetComponentInChildren<Text> ().text = "ミス";
		} else {
			Instantiate (Resources.Load("Skill/9") as GameObject,monster.target,monster.gameObject.transform.rotation);
			Destroy (Main.ground [Mathf.RoundToInt (monster.target.z), Mathf.RoundToInt (monster.target.x)]);
			yield return new WaitForSeconds (1);
		}
		Main.gameSpeed = 1;
	}

	private IEnumerator Skill10(Monster monster){
		if (Main.ground [Mathf.RoundToInt (monster.target.z), Mathf.RoundToInt (monster.target.x)] == null) {
			GameObject obj = Instantiate (Resources.Load("Skill/8") as GameObject,monster.target,Quaternion.identity);
			Main.ground [Mathf.RoundToInt (monster.target.z), Mathf.RoundToInt (monster.target.x)] = Instantiate (Resources.Load ("Barrier") as GameObject, new Vector3 (Mathf.RoundToInt (monster.target.x), 0, Mathf.RoundToInt (monster.target.z)), Quaternion.identity);
			SpriteRenderer spriteRenderer = Main.ground [Mathf.RoundToInt (monster.target.z), Mathf.RoundToInt (monster.target.x)].GetComponentInChildren<SpriteRenderer> ();
			for (float f = 0; f < 1; f += Time.deltaTime) {
				Color c = spriteRenderer.color;
				c.a += Time.deltaTime;
				spriteRenderer.color = c;
				yield return null;
			}
			spriteRenderer.color = new Color (1, 1, 1, 1);
			Destroy (obj);
		} else {
			monster.gameObject.GetComponentInChildren<Text> ().text = "ミス";
		}
		Main.gameSpeed = 1;
	}

	private IEnumerator Skill11(Monster monster){
		GameObject obj = Instantiate (Resources.Load("Skill/11") as GameObject,new Vector3(3,0,3),Quaternion.identity);
		StartCoroutine (Damage(new Vector3(3,0,3),new Vector3(100,1,100),monster.data.skillPower,1));
		Vector3 p = GameObject.Find("Main Camera").transform.position;
		for (int i = 0; i < 5; i++) {
			GameObject.Find ("Main Camera").transform.position = new Vector3 (p.x + 0.01f, p.y, p.z);
			yield return new WaitForSeconds(0.1f);
			GameObject.Find ("Main Camera").transform.position = new Vector3 (p.x - 0.01f, p.y, p.z);
			yield return new WaitForSeconds(0.1f);
		}
		Destroy (obj);
		GameObject.Find ("Main Camera").transform.position = p;
		Main.gameSpeed = 1;
	}

	private IEnumerator Skill12(Monster monster){
		GameObject obj = Instantiate (Resources.Load("Skill/12") as GameObject,monster.target,Quaternion.identity);
		yield return StartCoroutine (Heal(obj.transform.position,new Vector3(0.1f,1,0.1f),monster.data.skillPower,2));
		Destroy (obj);
		Main.gameSpeed = 1;
	}

	private IEnumerator Skill13(Monster monster){
		yield return StartCoroutine (Skill0(monster));
	}

	private IEnumerator Skill14(Monster monster){
		if (monster.data.skillPower > 0) {
			GameObject obj = Instantiate (Resources.Load ("Skill/14") as GameObject, monster.target, Quaternion.identity);
			yield return StartCoroutine (Damage (obj.transform.position, new Vector3 (0.1f, 1, 0.1f), monster.data.skillPower, 1.5f));
			while (obj.GetComponent<ParticleSystem> ().isPlaying == true) {
				obj.transform.Rotate (0, 20, 0);
				yield return null;
			}
			Destroy (obj);
			if(hitMonster.Count > 0)monster.Heal (int.Parse (hitMonster [0].gameObject.transform.Find ("Message(Clone)").GetComponentInChildren<Text> ().text) * -1);
		} else {
			if (Main.monster [Mathf.RoundToInt (monster.target.z), Mathf.RoundToInt (monster.target.x)] == null && Main.Tag(Main.ground [Mathf.RoundToInt (monster.target.z), Mathf.RoundToInt (monster.target.x)]) != "Rock") {
				GameObject obj = Instantiate (Resources.Load("Skill/14(1)") as GameObject,monster.target,Quaternion.identity);
				Monster clone = new Monster (Random.Range(1,13), monster.team, Mathf.RoundToInt (monster.target.x), Mathf.RoundToInt (monster.target.z),1000);
				MeshRenderer[] meshRenderer = clone.gameObject.transform.Find (clone.data.monsterName).GetComponentsInChildren<MeshRenderer> ();
				for (float f = 0; f < 1; f += Time.deltaTime) {
					for (int n = 0; n < meshRenderer.Length; n++) {
						for (int m = 0; m < meshRenderer [n].materials.Length; m++) {
							Color c = meshRenderer [n].materials[m].color;
							c.a = f;
							meshRenderer [n].materials[m].color = c;
						}
					}
					yield return null;
				}
				Main.monster [Mathf.RoundToInt (monster.target.z), Mathf.RoundToInt (monster.target.x)] = clone;
				Destroy (obj);
			} else {
				monster.gameObject.GetComponentInChildren<Text> ().text = "ミス";
			}
		}
		Main.gameSpeed = 1;
	}

	private IEnumerator Damage(Vector3 position,Vector3 size,int power,float time){
		colliderPosition = position;
		colliderSize = size;
		yield return new WaitForSeconds (time);
		for (int i = 0; i < hitMonster.Count; i++) {
			if(size.x != 100 || hitMonster[i].data.isFlying == false)
			hitMonster [i].Damage (power,hitMonster[i].gameObject.transform.position + hitMonster[i].gameObject.transform.forward);
		}
		for (int i = 0; i < hitRock.Count; i++) {
			hitRock [i].GetComponent <Rock>().Break();
		}
	}

	private IEnumerator Heal(Vector3 position,Vector3 size,int power,float time){
		colliderPosition = position;
		colliderSize = size;
		yield return new WaitForSeconds (time);
		for (int i = 0; i < hitMonster.Count; i++) {
			hitMonster [i].Heal (power);
		}
	}

	void FixedUpdate()
	{
		collisions = Physics.OverlapBox(colliderPosition, colliderSize, Quaternion.identity);
		hitMonster.Clear ();
		hitRock.Clear ();
		for (int i = 0; i < collisions.Length; i++) {
			if (collisions[i].gameObject.layer == 8 && Main.monster [Mathf.RoundToInt (collisions [i].gameObject.transform.position.z), Mathf.RoundToInt (collisions [i].gameObject.transform.position.x)] != null) {
				hitMonster.Add (Main.monster [Mathf.RoundToInt (collisions [i].gameObject.transform.position.z), Mathf.RoundToInt (collisions [i].gameObject.transform.position.x)]);
			}
			if (collisions [i].gameObject.layer == 8 &&Main.Tag(Main.ground [Mathf.RoundToInt (collisions [i].gameObject.transform.position.z), Mathf.RoundToInt (collisions [i].gameObject.transform.position.x)]) == "Rock") {
				hitRock.Add (Main.ground [Mathf.RoundToInt (collisions [i].gameObject.transform.position.z), Mathf.RoundToInt (collisions [i].gameObject.transform.position.x)]);
			}
		}
	}
}
