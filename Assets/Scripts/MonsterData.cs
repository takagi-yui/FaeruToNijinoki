using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MonsterData/Create",fileName = "Data")]
public class MonsterData : ScriptableObject {
	public int number;
	public string monsterName;
	public int maxHP,attack,speed;
	public bool isFlying;
	public string skillName,skillText;
	public int skillPower;
	public float skillEnergy;
	public int targetType;
	public float targetSize;
	public int getEXP;
	public float friend;
}
