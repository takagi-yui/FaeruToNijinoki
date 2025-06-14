using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Monster{
	public static Main main;
	public GameObject gameObject;
	public MonsterData data;
	public int team;
	public int EXP;
	public int HP;
	public float energy;
	public float maxEnergy;
	public float time;
	public List<Vector3> move;
	public GameObject gauge;
	public GameObject color;
	public GameObject targetObject;
	public LineRenderer path;
	public Vector3 target;

	public Monster (int number,int team,int x,int z,int EXP){
		this.gameObject = Main.Instantiate(Resources.Load("Monster/" + number) as GameObject,new Vector3(x,0,z),Quaternion.Euler(new Vector3(0,180 * team,0)));
		this.data = MonsterData.Instantiate(Resources.Load ("Data/" + number) as MonsterData);
		this.team = team;
		this.EXP = EXP;
		Main.statusUp (this);
		this.HP = this.data.maxHP;
		this.energy = 0;
		this.maxEnergy = 0;
		this.time = 0;
		this.move = new List<Vector3>();
		this.target = new Vector3 (-1,-1,-1);
		if (z < 0) {
			if (this.data.isFlying == true) {
				Vector3 position = this.gameObject.transform.GetChild (0).position;
				position.y -= 0.6f;
				this.gameObject.transform.GetChild (0).position = position;
			}
			this.gameObject.transform.rotation = Quaternion.Euler (0,180,0);
			this.gameObject.transform.Rotate (new Vector3(-80,0,0));
			this.gameObject.transform.Rotate (new Vector3(0,30,0));
			this.gameObject.transform.localScale = new Vector3 (0.8f,0.8f,0.8f);
			this.gameObject.AddComponent<BoxCollider> ().isTrigger = true;
			this.gameObject.GetComponent<BoxCollider> ().size = new Vector3 (1,1,0.01f);
			this.gameObject.layer = 8;
			MeshRenderer[] meshRenderer = this.gameObject.transform.Find (this.data.monsterName).GetComponentsInChildren<MeshRenderer> ();
			for (int i = 0; i < meshRenderer.Length; i++) {
				meshRenderer [i].shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
			}
			this.gameObject.transform.SetParent (GameObject.Find("TeamMonster").transform);
			this.gameObject.transform.localPosition = new Vector3 (x, 0, z);
			Data.data.monsterNumber [Mathf.RoundToInt (this.gameObject.transform.localPosition.x)] = this.data.number;
			Data.data.monsterEXP [Mathf.RoundToInt (this.gameObject.transform.localPosition.x)] = this.EXP;
			main.StartCoroutine (Standby());
		} else {
			Main.monster [z, x] = this;
			this.gauge = Main.Instantiate (Resources.Load ("Gauge") as GameObject, new Vector3 (this.gameObject.transform.position.x, 0, this.gameObject.transform.position.z), Camera.main.transform.rotation);
			this.gauge.transform.SetParent (this.gameObject.transform);
			this.gauge.transform.localPosition = new Vector3 (0, -0.2f, 0);
			this.gauge.transform.Find ("HP").gameObject.GetComponent<UnityEngine.UI.Slider> ().maxValue = this.data.maxHP;
			this.gauge.transform.Find ("HP").gameObject.GetComponent<UnityEngine.UI.Slider> ().value = this.HP;
			this.color = Main.Instantiate (Resources.Load ("Color") as GameObject, new Vector3 (this.gameObject.transform.position.x, -0.45f, this.gameObject.transform.position.z), Quaternion.identity);
			this.color.transform.SetParent (this.gameObject.transform);
			this.targetObject = Main.Instantiate (Resources.Load ("Target") as GameObject, Vector3.zero, Quaternion.Euler (new Vector3 (90, 0, 0)));
			if (this.data.skillPower > 0) {
				targetObject.GetComponentInChildren<Image> ().color = new Color (238 / 255f, 94 / 255f, 74 / 255f);
			}
			if (this.data.skillPower < 0) {
				targetObject.GetComponentInChildren<Image> ().color = new Color (133 / 255f, 255 / 255f, 105 / 255f);
			}
			this.targetObject.transform.localScale = new Vector3 (this.data.targetSize, this.data.targetSize, this.data.targetSize);
			this.path = Main.Instantiate (Resources.Load ("Path") as GameObject, Vector3.zero, Quaternion.identity).GetComponent<LineRenderer> ();
			this.path.transform.SetParent (this.gameObject.transform);
			if (this.team == 1) {
				this.gauge.transform.Find ("HP").Find ("Background").Find ("Fill").gameObject.GetComponent<Image> ().color = new Color (153 / 255f, 102 / 255f, 1);
				this.color.GetComponent<MeshRenderer> ().material.color = new Color (153 / 255f, 102 / 255f, 1, 0.2f);
			}
			if (this.data.number == -1) {
				GameObject.Destroy (this.gauge);
			}
			if (!Main.start) {
				if (this.gameObject.transform.position.x == 3 && this.gameObject.transform.position.z % 6 == 0 && this.color.transform.childCount == 0) {
					Main.Instantiate (this.color, new Vector3 (this.gameObject.transform.position.x, -0.45f, this.gameObject.transform.position.z), Quaternion.identity).gameObject.transform.SetParent (this.color.transform);
					Main.Instantiate (this.color, new Vector3 (this.gameObject.transform.position.x, -0.45f, this.gameObject.transform.position.z), Quaternion.identity).gameObject.transform.SetParent (this.color.transform);
				}
				if (this.gameObject.transform.position == new Vector3 (4, 0, 0)) {
					Data.data.team.back.right.number = this.data.number;
					Data.data.team.back.right.EXP = this.EXP;
				}
				if (this.gameObject.transform.position == new Vector3 (3, 0, 0)) {
					Data.data.team.back.middle.number = this.data.number;
					Data.data.team.back.middle.EXP = this.EXP;
				}
				if (this.gameObject.transform.position == new Vector3 (2, 0, 0)) {
					Data.data.team.back.left.number = this.data.number;
					Data.data.team.back.left.EXP = this.EXP;
				}
				if (this.gameObject.transform.position == new Vector3 (4, 0, 1)) {
					Data.data.team.front.right.number = this.data.number;
					Data.data.team.front.right.EXP = this.EXP;
				}
				if (this.gameObject.transform.position == new Vector3 (3, 0, 1)) {
					Data.data.team.front.middle.number = this.data.number;
					Data.data.team.front.middle.EXP = this.EXP;
				}
				if (this.gameObject.transform.position == new Vector3 (2, 0, 1)) {
					Data.data.team.front.left.number = this.data.number;
					Data.data.team.front.left.EXP = this.EXP;
				}
			}
			main.StartCoroutine (Ground ());
			main.StartCoroutine (Charge ());
		}
	}

	private IEnumerator Ground(){
		while (Main.start == false) {
			yield return null;
		}
		int skillPower = data.skillPower;
		while (true) {
			if (gameObject != null) {
				if (!data.isFlying) {
					if (data.skillPower != 0) {
						if (Main.Tag (Main.ground [Mathf.RoundToInt (gameObject.transform.position.z), Mathf.RoundToInt (gameObject.transform.position.x)]) == "MagicCircle") {
							data.skillPower = skillPower * 2;
						} else {
							data.skillPower = skillPower;
						}
					}
				}
				if(gauge != null)
				gauge.transform.Find ("HP").gameObject.GetComponent<UnityEngine.UI.Slider> ().value = HP;
			}
			yield return null;
		}
	}

	private IEnumerator MoveCheck(){
		Vector3 position = this.gameObject.transform.localPosition;
		yield return null;
		if (this.gameObject != null) {
			if (this.gameObject.transform.localPosition != position && Player.moveMonster != this) {
				if (this.gameObject.transform.position.z < 0) {
					this.gameObject.transform.SetParent (GameObject.Find ("TeamMonster").transform);
				} else {
					this.gameObject.transform.SetParent (null);
				}
				Monster monster = new Monster (this.data.number, this.team, Mathf.RoundToInt (this.gameObject.transform.localPosition.x), Mathf.RoundToInt (this.gameObject.transform.localPosition.z),this.EXP);
				if (Player.point == this)
					Player.point = monster;
				GameObject.Destroy (targetObject);
				GameObject.Destroy (this.gameObject);
			}
		}
	}

	private IEnumerator Standby(){
		while (Main.start == false) {
			if (Input.GetMouseButtonDown (0) && this.data.number != -1 && Time.timeScale == 1) {
				RaycastHit hit;
				if (Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out hit, 100, LayerMask.GetMask ("Touch"))) {
					if (hit.collider.gameObject == this.gameObject && this.gameObject.transform.position.x >= 0 && this.gameObject.transform.position.x <= 6) {
						Player.moveMonster = this;
						Player.point = Player.moveMonster;
						Player.SetLayer (Player.moveMonster.gameObject,11);
						Player.downPosition = Player.moveMonster.gameObject.transform.localPosition;
						Player.haveMoved = false;
						this.gameObject.transform.SetParent (null);
					}
				}
			}
			yield return main.StartCoroutine (MoveCheck());
			if (!this.gameObject)
				break;
		}
	}

	private IEnumerator Charge(){
		while (Main.start == false/* && !Sync.connect*/) {
			yield return main.StartCoroutine (MoveCheck());
			if (!this.gameObject)
				goto start;
		}
		if(this.data.number == -1){
			Main.monster [Mathf.RoundToInt (gameObject.transform.position.z), Mathf.RoundToInt (gameObject.transform.position.x)] = null;
			GameObject.Destroy (this.gameObject);
		}
		start:
		while (maxEnergy == 0) {
			if (team == 1/* && !Sync.connect*/) {
				if (Tutorial.tutorial == 1) {
					yield return new WaitForSeconds (2.0f);
				} else {
					yield return new WaitForSeconds (Random.Range (2.0f, 4.0f));
				}
				if(team == 1)AI.Think (this);
			}
			yield return null;
		}
		gauge.transform.Find ("MaxEnergy").gameObject.GetComponent<Image> ().enabled = true;
		while (energy < maxEnergy || Main.gameSpeed == 0 || HP == 0){
			if (target == new Vector3 (-1, -1, -1)) {
				gauge.transform.Find ("MaxEnergy").gameObject.GetComponent<Image> ().color = new Color (160 / 255f,180 / 255f,230 / 255f);
				gauge.transform.Find ("MaxEnergy").Find ("Energy").gameObject.GetComponent<Image> ().color = new Color (74 / 255f,119 / 255f,238 / 255f);
			} else {
				if (data.skillPower > 0) {
					gauge.transform.Find ("MaxEnergy").gameObject.GetComponent<Image> ().color = new Color (238 / 255f, 160 / 255f, 160 / 255f);
					gauge.transform.Find ("MaxEnergy").Find ("Energy").gameObject.GetComponent<Image> ().color = new Color (238 / 255f, 94 / 255f, 74 / 255f);
				}
				if (data.skillPower == 0) {
					gauge.transform.Find ("MaxEnergy").gameObject.GetComponent<Image> ().color = new Color (200 / 255f, 200 / 255f, 200 / 255f);
					gauge.transform.Find ("MaxEnergy").Find ("Energy").gameObject.GetComponent<Image> ().color = new Color (255 / 255f, 255 / 255f, 255 / 255f);
				}
				if (data.skillPower < 0) {
					gauge.transform.Find ("MaxEnergy").gameObject.GetComponent<Image> ().color = new Color (133 / 255f, 255 / 255f, 105 / 255f);
					gauge.transform.Find ("MaxEnergy").Find ("Energy").gameObject.GetComponent<Image> ().color = new Color (133 / 5 * 3 / 255f, 255 / 5 * 3 / 255f, 105 / 5 * 3 / 255f);
				}
			}
			energy += data.speed * Main.gameSpeed * Time.deltaTime;
			gauge.transform.Find ("MaxEnergy").Find ("Energy").gameObject.GetComponent<Image> ().fillAmount = energy / maxEnergy;
			yield return null;
			if (HP == 0)
				yield break;
			if (maxEnergy == 0)
				goto start;
			if (team == 1 && move.Count > 0/* && !Sync.connect*/) {
				if(Main.monster [Mathf.RoundToInt(move[move.Count - 1].z), Mathf.RoundToInt(move[move.Count - 1].x)] != null){
				    if (Main.monster [Mathf.RoundToInt(move[move.Count - 1].z), Mathf.RoundToInt(move[move.Count - 1].x)].HP == 0) {
					    Reset();
					    goto start;
				    }
				}
			}
		}
		while (time != 0 || Main.gameSpeed == 0) {
			yield return null;
		}
		energy = maxEnergy;
		path.positionCount = 0;
		Main.gameSpeed = 0;
		Main.action = this;
		if (target == new Vector3 (-1, -1, -1)) {
			if (MoveData ()) {
				main.StartCoroutine (Move());
			} else {
				Stop();
			}
		} else {
			gauge.transform.Find ("MaxEnergy").Find("Energy").gameObject.GetComponent<Image> ().fillAmount = 0;
			gauge.transform.Find ("MaxEnergy").gameObject.GetComponent<Image> ().enabled = false;
			Skill.Use (data.number,this);
		}
	}

	private IEnumerator Move(){
		gameObject.transform.LookAt (move [1]);
		gauge.transform.rotation = Quaternion.Euler (new Vector3(80,0,0));
		color.transform.rotation = Quaternion.Euler (new Vector3(0,0,0));
		yield return main.StartCoroutine (MoveObject(gameObject,move[0],move[1],0.2f));
		move.RemoveAt (0);
		if (move.Count >= 2) {
			if (MoveData ()) {
				main.StartCoroutine (Move());
			} else {
				Stop();
			}
		}else{
			Stop ();
		}
	}

	public void Stop(){
		if (target == new Vector3 (-1, -1, -1)) {
			gameObject.transform.position = move [0];
		}
		if (move.Count >= 2) {
			if (Main.monster [Mathf.RoundToInt (move [1].z), Mathf.RoundToInt (move [1].x)] != null) {
				if (Main.monster [Mathf.RoundToInt (move [1].z), Mathf.RoundToInt (move [1].x)].team != team) {
					main.StartCoroutine (Attack ());
				} else {
					move.Clear ();
					Main.gameSpeed = 1;
				}
			} else {
				if (Main.Tag (Main.ground [Mathf.RoundToInt (move [1].z), Mathf.RoundToInt (move [1].x)]) == "Rock") {
					main.StartCoroutine (Attack ());
				}
			}
		} else {
			move.Clear ();
			Main.gameSpeed = 1;
		}
		Reset ();
	}

	public void Reset(){
		energy = 0;
		maxEnergy = 0;
		gauge.transform.Find ("MaxEnergy").Find("Energy").gameObject.GetComponent<Image> ().fillAmount = 0;
		gauge.transform.Find ("MaxEnergy").gameObject.GetComponent<Image> ().enabled = false;
		main.StartCoroutine(Charge());
	}

	private IEnumerator Attack(){
		GameObject obj = gameObject.transform.Find (data.monsterName).gameObject;
		yield return main.StartCoroutine (MoveObject(obj,obj.transform.position,obj.transform.position + (obj.transform.forward * 0.4f),0.1f));
		if (Main.Tag (Main.ground [Mathf.RoundToInt (move [1].z), Mathf.RoundToInt (move [1].x)]) == "Rock") {
			Main.ground [Mathf.RoundToInt (move [1].z), Mathf.RoundToInt (move [1].x)].GetComponent<Rock> ().Break ();
			if (Main.monster [Mathf.RoundToInt ((gameObject.transform.position + gameObject.transform.forward).z), Mathf.RoundToInt ((gameObject.transform.position + gameObject.transform.forward).x)] == null) {
				Main.gameSpeed = 1;
			}
		}
		if (Main.monster [Mathf.RoundToInt ((gameObject.transform.position + gameObject.transform.forward).z), Mathf.RoundToInt ((gameObject.transform.position + gameObject.transform.forward).x)] != null){
		    Main.monster [Mathf.RoundToInt ((gameObject.transform.position + gameObject.transform.forward).z), Mathf.RoundToInt ((gameObject.transform.position + gameObject.transform.forward).x)].Damage (data.attack, move [0]);
		}
		GameObject.Instantiate (Resources.Load("Hit") as GameObject, move[1],Quaternion.Euler(new Vector3(80,0,0)));
		yield return main.StartCoroutine (MoveObject(obj,obj.transform.position,obj.transform.position + (obj.transform.forward * -0.4f),0.2f));
	}

	public void Damage(int damage,Vector3 position){
		gameObject.transform.LookAt (position);
		gauge.transform.rotation = Quaternion.Euler (new Vector3(80,0,0));
		color.transform.rotation = Quaternion.Euler (new Vector3(0,0,0));
		if (Main.Tag (Main.ground [Mathf.RoundToInt (gameObject.transform.position.z), Mathf.RoundToInt (gameObject.transform.position.x)]) == "Barrier" && data.isFlying == false) {
			damage = Main.Half (damage);
		}
		HP -= damage;
		if (HP <= 0) {
			HP = 0;
		}
		main.StartCoroutine (DamageMotion(damage));
		new Message (gameObject.transform.position,"" + damage,gameObject.transform,new Color(1,1,1));
	}

	public void Heal(int power){
		HP -= power;
		if (HP > data.maxHP) {
			HP = data.maxHP;
		}
		gauge.transform.Find("HP").gameObject.GetComponent<UnityEngine.UI.Slider> ().value = HP;
		new Message (gameObject.transform.position,"" + (-power),gameObject.transform,new Color(133/255f,255/255f,105/255f));
	}

	private IEnumerator DamageMotion(int damage){
		if (Main.Tag (Main.ground [Mathf.RoundToInt (gameObject.transform.position.z), Mathf.RoundToInt (gameObject.transform.position.x)]) == "Barrier" && data.isFlying == false) {
			Material material = Main.ground [Mathf.RoundToInt (gameObject.transform.position.z), Mathf.RoundToInt (gameObject.transform.position.x)].GetComponentInChildren<MeshRenderer> ().material;
			Color c;
			for (float f = 0; f < 0.3f; f += Time.deltaTime) {
				c = material.color;
				if (f < 0.1f) {
					c.a += Time.deltaTime * 2;
				}
				if(f > 0.2f){
					c.a -= Time.deltaTime * 2;
				}
				if (c.a < 0) {
					c.a = 0;
				}
				material.color = c;
				yield return null;
			}
			c = material.color;
			c.a = 0;
			material.color = c;
		}else{
			GameObject obj = gameObject.transform.Find (data.monsterName).gameObject;
			yield return main.StartCoroutine (MoveObject (obj, obj.transform.position, obj.transform.position + (obj.transform.forward * -0.2f), 0.1f));
			yield return main.StartCoroutine (MoveObject (obj, obj.transform.position, obj.transform.position + (obj.transform.forward * 0.2f), 0.2f));
		}
		gauge.transform.Find("HP").gameObject.GetComponent<UnityEngine.UI.Slider> ().value = HP;
		if (HP == 0) {
			MeshRenderer[] meshRenderer = gameObject.transform.Find (data.monsterName).GetComponentsInChildren<MeshRenderer> ();
			for (float f = 0; f < 1; f += Time.deltaTime) {
				for (int n = 0; n < meshRenderer.Length; n++) {
					for (int m = 0; m < meshRenderer [n].materials.Length; m++) {
						Color c = meshRenderer [n].materials[m].color;
						c.a -= Time.deltaTime;
						if (c.a < 0) {
							c.a = 0;
						}
						meshRenderer [n].materials[m].color = c;
					}
				}
				yield return null;
			}
			if (team == 1) {
				Main.getEXP += data.getEXP;
				if (Random.Range (0, 100) < data.friend) {
					Main.friend.Add (this);
				}
			}
			if (Main.leader [1] == this) {
				main.StartCoroutine(Main.Finish ());
			};
			if (Main.leader [0] == this) {
				main.StartCoroutine(Main.Finish ());
			};
			GameObject.Destroy (targetObject);
			GameObject.Destroy (this.gameObject);
			Main.monster [Mathf.RoundToInt (gameObject.transform.position.z), Mathf.RoundToInt (gameObject.transform.position.x)] = null;
			main.StopCoroutine (DamageMotion(0));
		}
		if (data.number == 14 && HP <= Main.Half(data.maxHP) && HP + damage > Main.Half(data.maxHP) && data.speed == MonsterData.Instantiate(Resources.Load ("Data/" + data.number) as MonsterData).speed * 2) {
			Time.timeScale = 0;
			data.speed *= 2;
			Tutorial.message ("きかいしん","これからが　ほんばんだ。");
			yield return new WaitForSecondsRealtime (0.5f);
			do {
				yield return null;
			} while(!Input.GetMouseButtonUp (0) || GameObject.Find ("Cover").GetComponent<Image> ().enabled == true);
			Tutorial.message ("ファエル","きかいしんの　つよいエナジーを\nかんじる。");
			do {
				yield return null;
			} while(!Input.GetMouseButtonUp (0) || GameObject.Find ("Cover").GetComponent<Image> ().enabled == true);
			Tutorial.message ("","");
			Time.timeScale = 1;
		}
		if(Main.action.move.Count != 0)Main.gameSpeed = 1;
	}

	private IEnumerator MoveObject(GameObject obj, Vector3 position0,Vector3 position1,float moveTime){
		time = 0;
		while (time < moveTime) {
			obj.transform.position = Vector3.Lerp (position0,position1,time / moveTime);
			time += Time.deltaTime;
			yield return null;
		}
		time = 0;
		obj.transform.position = position1;
	}

	bool MoveData(){
		if (Main.monster [Mathf.RoundToInt (move [1].z), Mathf.RoundToInt (move [1].x)] == null && (Main.Tag(Main.ground [Mathf.RoundToInt (move [1].z), Mathf.RoundToInt (move [1].x)]) != "Rock" || data.isFlying == true)) {
			Main.monster[Mathf.RoundToInt(move[0].z),Mathf.RoundToInt(move[0].x)] = null;
			Main.monster[Mathf.RoundToInt(move[1].z),Mathf.RoundToInt(move[1].x)] = this;
			return true;
		} else {
			gameObject.transform.LookAt (move[1]);
			gauge.transform.rotation = Quaternion.Euler (new Vector3(80,0,0));
			color.transform.rotation = Quaternion.Euler (new Vector3(0,0,0));
			return false;
		}
	}
}
