/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

public class Sync :  NetworkBehaviour{
	public static NetworkConnection networkConnection;
	public static bool connect = false;
	public NetworkClient client;
	public static int ready;
	public static bool check;
	public class TeamMessage : MessageBase{
		public Team team;
	}
	public class MoveMessage : MessageBase{
		public Vector3[] move;
	}
	public class SkillMessage : MessageBase{
		public Vector3 monsterPosition;
		public Vector3 target;
	}
	public class CancelMessage : MessageBase{
		public Vector3 monsterPosition;
	}
	public class EnergyMessage : MessageBase{
		public Vector3[] position;
		public float[] energy;
	}
	public class ReadyMessage: MessageBase{
	}
	void Start(){
		gameObject.transform.SetParent (GameObject.Find ("NetworkManager").transform);
	}
	void Update(){
		if (isLocalPlayer && Main.start && isServer) {
			EnergyMessage energyMessage = new EnergyMessage ();
			List<Vector3> position = new List<Vector3> ();
			List<float> energy = new List<float> ();
			for (int z = 0; z < Main.X; z++) {
				for (int x = 0; x < Main.X; x++) {
					if (Main.monster [z, x] != null) {
						position.Add (Player.Reverse (new Vector3 (x, 0, z)));
						energy.Add (Main.monster [z, x].energy);
					}
				}
			}
			energyMessage.position = position.ToArray ();
			energyMessage.energy = energy.ToArray ();
			networkConnection.Send (54, energyMessage);
		}
	}
	public override void OnStartLocalPlayer(){
		ready = 0;
		client = GameObject.Find ("NetworkManager").GetComponent<NetworkManager> ().client;
		if (isServer) {
			NetworkServer.RegisterHandler (50, ReadTeam);
			NetworkServer.RegisterHandler (51, ReadMove);
			NetworkServer.RegisterHandler (52, ReadSkill);
			NetworkServer.RegisterHandler (53, ReadCancel);
			NetworkServer.RegisterHandler (55, ReadReady);
		} else {
			client.RegisterHandler (50, ReadTeam);
			client.RegisterHandler (51, ReadMove);
			client.RegisterHandler (52, ReadSkill);
			client.RegisterHandler (53, ReadCancel);
			client.RegisterHandler (54, ReadEnergy);
			client.RegisterHandler (55, ReadReady);
		}
		StartCoroutine (Connect());
	}
	IEnumerator Connect(){
		while (GameObject.Find ("NetworkManager").transform.childCount < 2) {
			yield return null;
		}
		check = true;
		GameObject.Find ("MenuPanel").GetComponent<Image> ().enabled = true;
		GameObject.Find ("HowToPlayPanel").GetComponent<Image> ().enabled = true;
		if (isServer) {
			networkConnection = NetworkServer.connections[1];
		} else {
			networkConnection = client.connection;
		}
		TeamMessage teamMessage = new TeamMessage ();
		teamMessage.team = Data.data.team;
		networkConnection.Send (50,teamMessage);
		Tutorial.message ("","");
		GameObject.Find ("StartPanel").GetComponent<Image> ().enabled = false;
	}
	public static void SendMove(MoveMessage moveMessage){
		networkConnection.Send (51,moveMessage);
	}
	public static void SendSkill(SkillMessage skillMessage){
		networkConnection.Send (52,skillMessage);
	}
	public static void SendCancel(CancelMessage cancelMessage){
		networkConnection.Send (53,cancelMessage);
	}
	public void ReadTeam(NetworkMessage netMsg){
		TeamMessage msg = netMsg.ReadMessage<TeamMessage> ();
		msg.team.back.right.EXP = Main.Level(msg.team.back.right.EXP);
		msg.team.back.middle.EXP = Main.Level(msg.team.back.middle.EXP);
		msg.team.back.left.EXP = Main.Level(msg.team.back.left.EXP);
		msg.team.front.right.EXP = Main.Level(msg.team.front.right.EXP);
		msg.team.front.middle.EXP = Main.Level(msg.team.front.middle.EXP);
		msg.team.front.left.EXP = Main.Level(msg.team.front.left.EXP);
		Main.Enemy (msg.team);
		Main.leader[0] = Main.monster[0,3];
		Main.leader[1] = Main.monster [6,3];
	}
	public void ReadMove(NetworkMessage netMsg){
		MoveMessage msg = netMsg.ReadMessage<MoveMessage> ();
		Player.SetMove (Main.monster[Mathf.RoundToInt(msg.move[0].z),Mathf.RoundToInt(msg.move[0].x)],new List<Vector3>(msg.move));
	}
	public void ReadSkill(NetworkMessage netMsg){
		SkillMessage msg = netMsg.ReadMessage<SkillMessage> ();
		Player.SetSkill (Main.monster[Mathf.RoundToInt(msg.monsterPosition.z),Mathf.RoundToInt(msg.monsterPosition.x)],msg.target);
	}
	public void ReadCancel(NetworkMessage netMsg){
		CancelMessage msg = netMsg.ReadMessage<CancelMessage> ();
		Skill.Cancel (Main.monster[Mathf.RoundToInt(msg.monsterPosition.z),Mathf.RoundToInt(msg.monsterPosition.x)]);
	}
	public void ReadEnergy(NetworkMessage netMsg){
		EnergyMessage msg = netMsg.ReadMessage<EnergyMessage> ();
		for (int i = 0; i < msg.position.Length; i++) {
			if (msg.energy [i] != 0) {
				Main.monster [Mathf.RoundToInt (msg.position [i].z), Mathf.RoundToInt (msg.position [i].x)].energy = msg.energy [i];
			}
		}
	}
	public static void Ready(){
		ready++;
		if (ready == 2) {
			Main.start = true;
			Tutorial.message ("","スタート!!");
			Skill.skill.StartCoroutine (Wait());
		}
		ReadyMessage msg = new ReadyMessage ();
		networkConnection.Send (55,msg);
	}
	public void ReadReady(NetworkMessage networkMessage){
		ready++;
		if (ready == 2) {
			Main.start = true;
			Tutorial.message ("","スタート!!");
			Skill.skill.StartCoroutine (Wait());
		}
	}
	public static IEnumerator Wait(){
		yield return new WaitForSeconds (1);
		Tutorial.message ("","");
	}
	public static void StopConnect(){
		Application.runInBackground = false;
		if (Match.networkMatch != null) {
			NetworkManager.singleton.StopServer ();
			NetworkManager.singleton.StopClient ();
			Match.DestroyMatch ();
			//NetworkTransport.Shutdown ();
			//NetworkTransport.Init ();
		}
	}
}*/

