using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour{
	public GameObject monsterWindow;
	public static Monster moveMonster;
	List<Vector3> move;
	RaycastHit hit;
	public static Monster point;
	public static int mode;
	public static Vector3 downPosition;
	public static bool haveMoved;

	void Start (){
		moveMonster = null;
		move = new List<Vector3> ();
		hit = new RaycastHit ();
		mode = 0;
	}

	void Update (){
		if (mode == 0) {
			if (moveMonster == null) {
				for (int i = 0; i < Main.boxCollider.Length; i++) {
					Main.boxCollider [i].transform.rotation = Quaternion.Euler (new Vector3 (0, 0, 0));
					Main.boxCollider [i].transform.localScale = new Vector3 (1, 0.01f, 1);
					Main.boxCollider [i].GetComponent<MeshRenderer> ().materials [0].color = new Color (1, 1, 1, 0);
				}
			} else {
				for (int i = 0; i < Main.boxCollider.Length; i++) {
					Main.boxCollider [i].transform.rotation = Quaternion.Euler (new Vector3 (0, 45, 0));
					Main.boxCollider [i].transform.localScale = new Vector3 (0.7f, 0.01f, 0.7f);
					Main.boxCollider [i].GetComponent<MeshRenderer> ().materials [0].color = new Color (1, 1, 1, 0);
				}
			}
			GameObject.Find ("Arrow").GetComponentInChildren<Image> ().enabled = false;
			if (Input.GetMouseButtonDown (0)) {
				if (Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out hit, 100, LayerMask.GetMask ("Touch"))) {
					if (hit.collider.transform.position.z >= 0) {
						moveMonster = Main.monster [Mathf.RoundToInt (hit.collider.gameObject.transform.position.z), Mathf.RoundToInt (hit.collider.gameObject.transform.position.x)];
					}
					if (moveMonster != null) {
						if (moveMonster.data.number != -1 && (Tutorial.tutorial == 0 || moveMonster.team == 0 || Tutorial.tutorial == 3)) {
							point = moveMonster;
							AddPosition (moveMonster, Mathf.RoundToInt (hit.collider.gameObject.transform.position.x), Mathf.RoundToInt (hit.collider.gameObject.transform.position.z));
							if (!Main.start) {
								SetLayer (moveMonster.gameObject,11);
								downPosition = moveMonster.gameObject.transform.localPosition;
								haveMoved = false;
								moveMonster.color.transform.SetParent (null);
							}
						} else {
							moveMonster = null;
						}
					}
				}
			}

			if (Input.GetMouseButton (0)) {
				if (moveMonster != null) {
					if (moveMonster.gameObject != null) {
						if (moveMonster.team == 0 && moveMonster.maxEnergy == 0) {
							if (Main.start) {
								if (Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out hit, 100, LayerMask.GetMask ("Touch"))) {
									if (!Talk.talking && (Tutorial.tutorial == 0 || (Tutorial.tutorial == 1 && Mathf.RoundToInt (hit.collider.gameObject.transform.position.x) == 3 && GameObject.Find ("Outside").GetComponent<Image> ().enabled == true))) {
										AddPosition (moveMonster, Mathf.RoundToInt (hit.collider.gameObject.transform.position.x), Mathf.RoundToInt (hit.collider.gameObject.transform.position.z));
									}
								}
								if (moveMonster.energy == moveMonster.maxEnergy && moveMonster.energy != 0) {
									move.Clear ();
									GameObject.Find ("Line").GetComponent<LineRenderer> ().positionCount = 0;
									moveMonster = null;
								}
							} else{
								Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out hit, 100, LayerMask.GetMask ("Ground"));
								if (/*!Sync.connect && */(Vector3.Distance(hit.point, downPosition) > 0.8f || haveMoved)) {
									haveMoved = true;
								    moveMonster.gameObject.transform.position = new Vector3 (hit.point.x, 0, hit.point.z);
								}
							}
						}
					} else {
						move.Clear ();
						GameObject.Find ("Line").GetComponent<LineRenderer> ().positionCount = 0;
						moveMonster = null;
					}
				}
			}

			if (Input.GetMouseButtonUp (0)) {
				if (move.Count >= 2) {
					if (Tutorial.tutorial != 1 || move [move.Count - 1] == new Vector3 (3, 0, 6)) {
						SetMove (moveMonster, move);
					}
				}
				move.Clear ();
				GameObject.Find ("Line").GetComponent<LineRenderer> ().positionCount = 0;
				if (!Main.start && moveMonster != null/* && !Sync.connect*/) {
					if (Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out hit, 100, LayerMask.GetMask ("Touch"))){
						if (Tutorial.tutorial == 0 || (Mathf.RoundToInt (hit.collider.transform.position.x) == 2 && Mathf.RoundToInt (hit.collider.transform.position.z) == 0)) {
							if (hit.collider.transform.position.z >= 0) {
								if (Main.monster [Mathf.RoundToInt (hit.collider.gameObject.transform.position.z), Mathf.RoundToInt (hit.collider.gameObject.transform.position.x)] != null) {
									if (Main.monster [Mathf.RoundToInt (hit.collider.gameObject.transform.position.z), Mathf.RoundToInt (hit.collider.gameObject.transform.position.x)].team == 0 && moveMonster != Main.monster [Mathf.RoundToInt (hit.collider.gameObject.transform.position.z), Mathf.RoundToInt (hit.collider.gameObject.transform.position.x)]) {
										Change (Main.monster [Mathf.RoundToInt (hit.collider.gameObject.transform.position.z), Mathf.RoundToInt (hit.collider.gameObject.transform.position.x)].gameObject);
									}
								}
							} else {
								Change (hit.collider.gameObject);
							}
						}
					}
					if (moveMonster != null) {
						SetLayer (moveMonster.gameObject,8);
						if (downPosition.z < 0) {
							moveMonster.gameObject.transform.SetParent (GameObject.Find ("TeamMonster").transform);
						} else {
							moveMonster.gameObject.transform.SetParent (null);
						}
						moveMonster.gameObject.transform.localPosition = downPosition;
						if (moveMonster.color != null) {
							moveMonster.color.transform.SetParent (moveMonster.gameObject.transform);
						}
					}
				}
				moveMonster = null;
			}
		}
		if (mode > 0) {
			if (mode == 1) {
				for (int i = 0; i < Main.boxCollider.Length; i++) {
					Main.boxCollider [i].transform.rotation = Quaternion.identity;
					Main.boxCollider [i].transform.localScale = new Vector3 (0.7f, 0.01f, 0.7f);
					Main.boxCollider [i].GetComponent<MeshRenderer> ().materials [0].color = new Color (1, 1, 1, Mathf.Abs (0.2f - (Time.unscaledTime / 4 % 0.4f)));
				}
			}
			if (mode == 2) {
				GameObject.Find ("Arrow").GetComponentInChildren<Image> ().enabled = true;
			}
			if (mode == 3) {
				for (int i = 0; i < Main.boxCollider.Length; i++) {
					if ((Vector3.Distance (point.gameObject.transform.position, Main.boxCollider [i].gameObject.transform.position) < 1.8f && Vector3.Distance (point.gameObject.transform.position, Main.boxCollider [i].gameObject.transform.position) > 0.5f)) {
						Main.boxCollider [i].transform.rotation = Quaternion.identity;
						Main.boxCollider [i].transform.localScale = new Vector3 (0.7f, 0.01f, 0.7f);
						Main.boxCollider [i].GetComponent<MeshRenderer> ().materials [0].color = new Color (1, 1, 1, Mathf.Abs (0.2f - (Time.unscaledTime / 4 % 0.4f)));
					}
				}
			}
			if (Input.GetMouseButtonUp (0)) {
				if (Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out hit, 100, LayerMask.GetMask ("Ground"))) {
					Vector3 position = new Vector3 (Mathf.RoundToInt(hit.point.x),0,Mathf.RoundToInt(hit.point.z));
					if (position.x >= 0 && position.x < Main.X && position.z >= 0 && position.z < Main.Z) {
						if ((Tutorial.tutorial != 2 && (mode == 1 || (Vector3.Distance (point.gameObject.transform.position, position) < 1.8f && Vector3.Distance (point.gameObject.transform.position, position) > 0.5f))) || (Tutorial.tutorial == 2 && Mathf.RoundToInt (position.x) == 3 && Mathf.RoundToInt (position.z) == 6)) {
							Player.SetSkill (point, position);
							GameObject.Find ("Enter").GetComponent<AudioSource> ().Play ();
							if (mode == 1 || mode == 3) {
								point.targetObject.transform.position = new Vector3 (point.target.x, -0.42f, point.target.z);
								point.targetObject.GetComponentInChildren<Image> ().enabled = true;
							}
							if (mode == 2) {
								point.path.endColor = new Color (238 / 255f, 94 / 255f, 74 / 255f, 100 / 255f);
								move.Add (point.gameObject.transform.position);
								while (move [move.Count - 1].x >= 0 && move [move.Count - 1].x <= 6 && move [move.Count - 1].z >= 0 && move [move.Count - 1].z <= 6) {
									move.Add (move [move.Count - 1] + (point.target - point.gameObject.transform.position));
								}
								move.RemoveAt (move.Count - 1);
								Player.DrawLine (point.path, move.ToArray (), -0.42f);
								move.Clear ();
							}
							mode = 0;
						}
					}
				}
			}
		}
		SetStatus (point);
	}

	void AddPosition (Monster monster, int x, int z){
		if (!move.Contains (new Vector3 (x, 0, z))) {
			if (move.Count == 0) {
				move.Add (new Vector3 (x, 0, z));
			} else if (Vector3.Distance (move [move.Count - 1], new Vector3 (x, 0, z)) < 1.5f) {
				move.Add (new Vector3 (x, 0, z));
				GameObject.Find ("Line").GetComponent<AudioSource> ().Play ();
			}
		}
		if (move.Count >= 2) {
			if (move [move.Count - 2] == new Vector3 (x, 0, z)) {
				move.RemoveAt (move.Count - 1);
				GameObject.Find ("Line").GetComponent<AudioSource> ().Play ();
			}
		}
		DrawLine (GameObject.Find ("Line").GetComponent<LineRenderer> (), move.ToArray (), -0.38f);
	}

	void SetStatus (Monster monster){
		if (monster.gameObject == null) {
			if (Main.start) {
				point = Main.leader [0];
			} else {
				point = Main.monster[3,0];
			}
			monster = point;
			mode = 0;
		}
		if (monster.gameObject != null) {
			GameObject.Find ("Canvas").transform.Find ("skillButton").gameObject.SetActive (true);
			monsterWindow.transform.Find ("name").gameObject.GetComponent<Text> ().text = monster.data.monsterName;
			monsterWindow.transform.Find ("HP").gameObject.GetComponent<Text> ().text = monster.HP + "/" + monster.data.maxHP;
			monsterWindow.transform.Find ("attack").gameObject.GetComponent<Text> ().text = "" + monster.data.attack;
			monsterWindow.transform.Find ("speed").gameObject.GetComponent<Text> ().text = "" + monster.data.speed;
			monsterWindow.transform.Find ("skillName").gameObject.GetComponent<Text> ().text = "" + monster.data.skillName;
			monsterWindow.transform.Find ("skillPower").gameObject.GetComponent<Text> ().text = "" + Mathf.Abs(monster.data.skillPower);
			string text = monster.data.skillText;
			if (monster.data.skillText.Length > 11) {
				text = monster.data.skillText.Insert (12, "\n");
			}
			monsterWindow.transform.Find ("skillText").gameObject.GetComponent<Text> ().text = text;
			if (mode == 0) {
				if (monster.maxEnergy != 0 && monster.team == 0 && monster.energy != monster.maxEnergy) {
					GameObject.Find ("skillButton").gameObject.GetComponent<Image> ().color = new Color (238 / 255f, 94 / 255f, 74 / 255f);
					GameObject.Find ("SkillPanel").gameObject.GetComponent<Image> ().enabled = false;
					GameObject.Find ("skillButton").gameObject.GetComponentInChildren<Text> ().text = "キャンセル";
				} else {
					if (monster.team == 0) {
						if (monster.gameObject.transform.rotation == Quaternion.identity || Main.start) {
							if (Main.start && (monster.energy != monster.maxEnergy || monster.move.Count == 0 || monster.maxEnergy == 0)) {
								GameObject.Find ("SkillPanel").gameObject.GetComponent<Image> ().enabled = false;
							} else {
								GameObject.Find ("SkillPanel").gameObject.GetComponent<Image> ().enabled = true;
							}
							GameObject.Find ("skillButton").gameObject.GetComponentInChildren<Text> ().text = "" + monster.data.skillName;
					    } else {
							if (monster.data.number == 0 || monster.data.number == 13 || Tutorial.tutorial != 0) {
								GameObject.Find ("SkillPanel").gameObject.GetComponent<Image> ().enabled = true;
							} else {
								GameObject.Find ("SkillPanel").gameObject.GetComponent<Image> ().enabled = false;
							}
						    GameObject.Find ("skillButton").gameObject.GetComponentInChildren<Text> ().text = "おわかれ";
					    }
					GameObject.Find ("skillButton").gameObject.GetComponent<Image> ().color = new Color (218 / 255f, 227 / 255f, 133 / 255f);
					} else {
						GameObject.Find ("SkillPanel").gameObject.GetComponent<Image> ().enabled = true;
						GameObject.Find ("skillButton").gameObject.GetComponent<Image> ().color = new Color (153 / 255f, 102 / 255f, 1);
						GameObject.Find ("skillButton").gameObject.GetComponentInChildren<Text> ().text = "" + monster.data.skillName;
					}
				}
			} else {
				GameObject.Find ("SkillPanel").gameObject.GetComponent<Image> ().enabled = false;
				GameObject.Find ("skillButton").gameObject.GetComponentInChildren<Text> ().text = "もどる";
			}
			GameObject.Find ("Arrow").transform.position = new Vector3 (point.gameObject.transform.position.x, -0.42f, point.gameObject.transform.position.z);
			GameObject.Find ("Arrow").GetComponentInChildren<Image> ().color = new Color (1, 1, 1, Mathf.Abs (0.2f - (Time.time / 4 % 0.4f)));
			GameObject.Find ("Point").transform.position = new Vector3 (point.gameObject.transform.position.x, -0.4f, point.gameObject.transform.position.z);
		}
	}

	public static void DrawLine (LineRenderer lineRenderer, Vector3[] move, float y){
		lineRenderer.positionCount = move.Length;
		lineRenderer.SetPositions (move);
		for (int i = 0; i < lineRenderer.positionCount; i++) {
			Vector3 position = lineRenderer.GetPosition (i);
			position.y = y;
			lineRenderer.SetPosition (i, position);
		}
	}

	public static void SetLayer(GameObject monster,int layer){
		monster.layer = layer;
		for (int i = 0; i < monster.transform.GetChild (0).childCount; i++) {
			monster.transform.GetChild (0).GetChild(i).gameObject.gameObject.layer = layer;
		}
	}

	void Change(GameObject change){
		if (!(downPosition == new Vector3 (3, 0, 0) && ((change.transform.childCount == 2 && change.transform.position.z >= 0) || (change.transform.childCount == 0 && change.transform.position.z < 0)))) {
			if (moveMonster.color != null) {
				moveMonster.color.transform.SetParent (moveMonster.gameObject.transform);
				moveMonster.color.transform.localPosition = new Vector3 (0, moveMonster.color.transform.localPosition.y, 0);
			}
			moveMonster.gameObject.transform.position = change.transform.position;
			if (downPosition.z < 0) {
				change.transform.SetParent (GameObject.Find ("TeamMonster").transform);
			} else {
				change.transform.SetParent (null);
			}
			change.transform.localPosition = downPosition;
			moveMonster = null;
		}
	}

	public static void SetSkill(Monster monster, Vector3 target){
		monster.target = target;
		monster.maxEnergy = monster.data.skillEnergy;
		monster.energy = 0;
		monster.move.Clear ();
		/*if (Sync.connect && monster.team == 0) {
			Sync.SkillMessage skillMessage = new Sync.SkillMessage ();
			skillMessage.monsterPosition = Reverse (monster.gameObject.transform.position);
			skillMessage.target = Reverse (target);
			Sync.SendSkill (skillMessage);
		}*/
	}

	public static void SetMove(Monster monster,List<Vector3> move){
		monster.move = new List<Vector3> (move);
		monster.maxEnergy = move.Count * 200;
		monster.energy = 0;
		monster.target = new Vector3 (-1, -1, -1);
		if (monster.team == 0) {
			monster.path.endColor = new Color (74 / 255f, 119 / 255f, 238 / 255f, 100 / 255f);
			Player.DrawLine (monster.path, move.ToArray (), -0.42f);
		}
		/*if (Sync.connect && monster.team == 0) {
			Sync.MoveMessage moveMessage = new Sync.MoveMessage ();
			moveMessage.move = move.ToArray();
			for (int i = 0; i < moveMessage.move.Length; i++) {
				moveMessage.move [i] = Reverse (moveMessage.move[i]);
			}
			Sync.SendMove (moveMessage);
		}*/
	}

	public static Vector3 Reverse(Vector3 p){
		return (new Vector3 (6,0,6) - p);
	}
}
