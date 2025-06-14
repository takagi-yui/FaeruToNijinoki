using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour {
	public static AI ai;
	int turn;

	void Awake(){
		ai = GetComponent<AI> ();
		turn = 0;
	}

	public static void Think(Monster monster){
		if (monster != null) {
			ai.StartCoroutine ("Think" + monster.data.number, monster);
		}
	}

	private IEnumerator Think0(Monster monster){
		yield return StartCoroutine (Think6(monster));
	}

	private IEnumerator Think1(Monster monster){
		List<Monster> monsterList = FindMonster (monster,monster.team,true);
		for (int i = 0; i < monsterList.Count; i++) {
			if (monsterList [i].HP < monsterList [i].data.maxHP) {
				Player.SetSkill (monster,monsterList [i].gameObject.transform.position);
			}
		}
		if(monster.maxEnergy == 0)Attack (monster);
		yield return null;
	}

	private IEnumerator Think2(Monster monster){
		List<Monster> monsterList = FindMonster (monster,Enemy(monster.team),false);
		for (int i = 0; i < monsterList.Count; i++) {
			GameObject obj = new GameObject ();
			obj.transform.position = monster.gameObject.transform.position;
			obj.transform.LookAt (monsterList [i].gameObject.transform);
			if (Mathf.RoundToInt(obj.transform.eulerAngles.y) % 45 == 0) {
				obj.transform.position += obj.transform.forward;
				Player.SetSkill (monster,new Vector3(Mathf.RoundToInt(obj.transform.position.x),0,Mathf.RoundToInt(obj.transform.position.z)));
			}
			if(monster.maxEnergy == 0)Attack (monster);
			Destroy (obj);
		}
		yield return null;
	}

	private IEnumerator Think3(Monster monster){
		if (monster.gameObject == null)yield break;
		if (Tutorial.tutorial != 1) {
			GameObject obj = new GameObject ();
			obj.transform.position = monster.gameObject.transform.position;
			obj.transform.rotation = Quaternion.Euler (new Vector3 (0, 45 * Random.Range (0, 8), 0));
			obj.transform.position += obj.transform.forward;
			if (Mathf.RoundToInt (obj.transform.position.x) >= 0 && Mathf.RoundToInt (obj.transform.position.x) < Main.X && Mathf.RoundToInt (obj.transform.position.z) >= 0 && Mathf.RoundToInt (obj.transform.position.z) < Main.Z) {
				if (Main.monster [Mathf.RoundToInt (obj.transform.position.z), Mathf.RoundToInt (obj.transform.position.x)] == null) {
					Player.SetSkill (monster, new Vector3 (Mathf.RoundToInt (obj.transform.position.x), 0, Mathf.RoundToInt (obj.transform.position.z)));
				}
			}
			Destroy (obj);
		}
		if(monster.maxEnergy == 0)Attack (monster);
		yield return null;
	}

	private IEnumerator Think4(Monster monster){
		List<Monster> monsterList = FindMonster (monster,Enemy(monster.team),false);
		for (int i = 0; i < monsterList.Count; i++) {
			if(Main.Tag(Main.ground[Mathf.RoundToInt(monsterList[i].gameObject.transform.position.z),Mathf.RoundToInt(monsterList[i].gameObject.transform.position.x)]) == "MagicCircle")Player.SetSkill (monster,new Vector3(-100,0,-100));
		}
		if(monster.maxEnergy == 0)Attack (monster);
		yield return null;
	}

	private IEnumerator Think5(Monster monster){
		yield return StartCoroutine("Think2",monster);
	}

	private IEnumerator Think6(Monster monster){
		List<Monster> monsterList = FindMonster (monster,Enemy(monster.team),false);
		for (int i = 0; i < monsterList.Count; i++) {
			Player.SetSkill (monster,monsterList[i].gameObject.transform.position);
		}
		if(monster.maxEnergy == 0)Attack (monster);
		yield return null;
	}

	private IEnumerator Think7(Monster monster){
		List<Monster> monsterList = FindMonster (monster,Enemy(monster.team),false);
		List<Vector3> move = pass (monsterList [0].gameObject.transform.position, monster.gameObject.transform.position);
		if (!monsterList[0].data.isFlying && Main.ground [Mathf.RoundToInt (move [move.Count - 2].z), Mathf.RoundToInt (move [move.Count - 2].x)] == null && Main.monster [Mathf.RoundToInt (move [move.Count - 2].z), Mathf.RoundToInt (move [move.Count - 2].x)] == null) {
			Player.SetSkill (monster,move [move.Count - 2]);
		}
		if(monster.maxEnergy == 0)Attack (monster);
		yield return null;
	}

	private IEnumerator Think8(Monster monster){
		List<Monster> monsterList = FindMonster (monster,monster.team,false);
		for (int i = 0; i < monsterList.Count; i++) {
			if (monsterList [i].data.number == 4) {
				monsterList = FindMonster (monster,Enemy(monster.team),false);
			}
		}
		for (int i = 0; i < monsterList.Count; i++) {
			Vector3 p = monsterList [i].gameObject.transform.position;
			if((monsterList[i].data.skillPower != 0 && Main.ground[Mathf.RoundToInt(p.z),Mathf.RoundToInt(p.x)] == null && !monsterList[i].data.isFlying) || monsterList[i].team == Enemy(monster.team))Player.SetSkill (monster,p);
		}
		if(monster.maxEnergy == 0)Attack (monster);
		yield return null;
	}
	private IEnumerator Think9(Monster monster){
		int count = 0;
		List<Monster> monsterList = FindMonster (monster,monster.team,false);
		for (int i = 0; i < monsterList.Count; i++) {
			if (monsterList [i].data.number == 4) {
				count++;
			}
		}
		if (count == 0) {
			monsterList = FindMonster (monster, Enemy (monster.team), false);
		}
		for (int i = 0; i < monsterList.Count; i++) {
			if (monsterList [i].data.skillPower != 0 && monsterList[i].data.isFlying == false && Main.ground[Mathf.RoundToInt(monsterList [i].gameObject.transform.position.z),Mathf.RoundToInt(monsterList [i].gameObject.transform.position.x)] != null) {
				Player.SetSkill (monster, monsterList [i].gameObject.transform.position);
			}
		}
		if(monster.maxEnergy == 0)Attack (monster);
		yield return null;
	}

	private IEnumerator Think10(Monster monster){
		List<Monster> monsterList = FindMonster (monster,monster.team,true);
		for (int i = 0; i < monsterList.Count; i++) {
			Vector3 p = monsterList [i].gameObject.transform.position;
			if(monsterList[i].data.isFlying == false && Main.ground[Mathf.RoundToInt(p.z),Mathf.RoundToInt(p.x)] == null)Player.SetSkill (monster,p);
		}
		if(monster.maxEnergy == 0)Attack (monster);
		yield return null;
	}

	private IEnumerator Think11(Monster monster){
		if (Random.Range(0,5) == 0) {
			Player.SetSkill (monster,new Vector3(-100,0,-100));
		}
		if(monster.maxEnergy == 0)Attack (monster);
		yield return null;
	}

	private IEnumerator Think12(Monster monster){
		List<Monster> monsterList = FindMonster (monster,monster.team,true);
		for (int i = 0; i < monsterList.Count; i++) {
			if (monsterList [i].data.maxHP - monsterList [i].HP >= 30) {
				Player.SetSkill (monster,monsterList [i].gameObject.transform.position);
			}
		}
		if(monster.maxEnergy == 0)Attack (monster);
		yield return null;
	}

	private IEnumerator Think13(Monster monster){
		yield return StartCoroutine (Think0 (monster));
	}

	private IEnumerator Think14(Monster monster){
		turn++;
		if (turn % 2 == 0) {
			monster.data.skillName = "しょうかん";
			monster.data.skillText = "ターゲットに　モンスターを　よぶ";
			monster.data.skillPower = 0;
			int x = Random.Range (0,Main.X);
			int z = Random.Range (0,Main.Z);
			if (Main.monster [z, x] == null && Main.Tag(Main.ground[z,x]) != "Rock") {
				Player.SetSkill (monster,new Vector3(x,0,z));
			} else {
				monster.data.skillName = "やみのうず";
				monster.data.skillText = "ターゲットの　HPを　きゅうしゅう";
				monster.data.skillPower = 80;
				if (Random.Range (0, 3) == 0) {
					Attack (monster);
				} else {
					yield return StartCoroutine (Think6(monster));
				}
			}
		} else {
			monster.data.skillName = "やみのうず";
			monster.data.skillText = "ターゲットの　HPを　きゅうしゅう";
			monster.data.skillPower = 80;
			if (Random.Range (0, 3) == 0) {
				Attack (monster);
			} else {
				yield return StartCoroutine (Think6(monster));
			}
		}
	}

	public static void Attack(Monster monster){
		float score = -1;
		float targetX = -1;
		float targetZ = -1;
		for (int z = 0; z < Main.Z; z++) {
			for (int x = 0; x < Main.X; x++) {
				if (Main.monster [z, x] != null) {
					if (Main.monster [z, x].team != monster.team) {
						List<Vector3> move = pass (monster.gameObject.transform.position, Main.monster [z, x].gameObject.transform.position);
						if(monster.data.attack / (move.Count * 200 / monster.data.speed) > score || (monster.data.attack / (move.Count * 200 / monster.data.speed) == score && Random.Range(0,2) == 0)){
							score = monster.data.attack / (move.Count * 200 / monster.data.speed);
							targetX = x;
							targetZ = z;
						}
					}
				}
			}
		}
		monster.move = new List<Vector3>(pass(monster.gameObject.transform.position, new Vector3(targetX,0,targetZ)));
		monster.maxEnergy = monster.move.Count * 200;
		monster.energy = 0;
		monster.target = new Vector3 (-1, -1, -1);
	}

	public static List<Vector3> pass(Vector3 position,Vector3 targetPosition){
		position = new Vector3 (Mathf.RoundToInt(position.x),Mathf.RoundToInt(position.y),Mathf.RoundToInt(position.z));
		targetPosition = new Vector3 (Mathf.RoundToInt(targetPosition.x),Mathf.RoundToInt(targetPosition.y),Mathf.RoundToInt(targetPosition.z));
		List<Vector3> pass = new List<Vector3> ();
		Vector3 p = position;
		pass.Add (p);
		while (p != targetPosition) {
			if (p.x != targetPosition.x) {
				if (p.x < targetPosition.x) {
					p.x++;
				} else {
					p.x--;
				}
			}
			if (p.z != targetPosition.z) {
				if (p.z < targetPosition.z) {
					p.z++;
				} else {
					p.z--;
				}
			}
			pass.Add (p);
		}
		return pass;
	}

	private List<Monster> FindMonster(Monster monster,int team,bool include){
		List<Monster> monsterList = new List<Monster>();
		for (int z = 0; z < Main.Z; z++) {
			for (int x = 0; x < Main.X; x++) {
				if (Main.monster [z, x] != null) {
					if (team == -1 || Main.monster [z, x].team == team) {
						if (include == true || Main.monster [z, x] != monster) {
							monsterList.Add (Main.monster [z, x]);
						}
					}
				}
			}
		}
		for (int i = monsterList.Count; i > 0; i--) {
			int r = Random.Range (0,i);
			monsterList.Add (monsterList[r]);
			monsterList.RemoveAt (r);
		}
		return monsterList;
	}

	private int Enemy(int team){
		if (team == 0) {
			return 1;
		} else {
			return 0;
		}
	}
}
