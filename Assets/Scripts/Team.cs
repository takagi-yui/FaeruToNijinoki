using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TeamData{
	public int number;
	public int EXP;
}

[System.Serializable]
public class TeamPosition{
	public TeamData right;
	public TeamData middle;
	public TeamData left;
}

[System.Serializable]
public class Team{
	public TeamPosition back;
	public TeamPosition front;
}
