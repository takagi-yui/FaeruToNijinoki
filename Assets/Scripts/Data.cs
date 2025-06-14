using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using UnityEngine;

public class Data : MonoBehaviour {
	[Serializable]
	public class SaveData{
		public int stage;
		public int[] monsterNumber;
		public int[] monsterEXP;
		public Team team;
	}
	public static SaveData data;

	void Awake(){
		data = new SaveData ();
		data.team = new Team ();
		data.team.back = new TeamPosition ();
		data.team.front = new TeamPosition ();
		data.team.back.right = new TeamData ();
		data.team.back.middle = new TeamData ();
		data.team.back.left = new TeamData ();
		data.team.front.right = new TeamData ();
		data.team.front.middle = new TeamData ();
		data.team.front.left = new TeamData ();
		Load ();
	}

	/*void Update(){
		if (Input.GetKeyUp ("s")) {
			Save ();
		}
		if (Input.GetKeyUp ("d")) {
			Delete ();
		}
	}*/

	public static void Save(){
		ICryptoTransform encryptor = rijndaelManaged().CreateEncryptor();
		MemoryStream memoryStream = new MemoryStream ();
		CryptoStream cryptoStream = new CryptoStream (memoryStream,encryptor,CryptoStreamMode.Write);
		byte[] bytes = System.Text.Encoding.UTF8.GetBytes (JsonUtility.ToJson(data));
		cryptoStream.Write (bytes, 0, bytes.Length);
		cryptoStream.FlushFinalBlock ();
		File.WriteAllText(Application.persistentDataPath + "/data",System.Convert.ToBase64String(memoryStream.ToArray()));
	}

	void Load(){
		if (File.Exists(Application.persistentDataPath + "/data")) {
			ICryptoTransform decryptor = rijndaelManaged ().CreateDecryptor ();
			byte[] encrypted = System.Convert.FromBase64String (File.ReadAllText (Application.persistentDataPath + "/data"));
			MemoryStream memoryStream = new MemoryStream (encrypted);
			CryptoStream cryptoStream = new CryptoStream (memoryStream, decryptor, CryptoStreamMode.Read);
			byte[] decrypted = new byte[encrypted.Length];
			cryptoStream.Read (decrypted,0,decrypted.Length);
			JsonUtility.FromJsonOverwrite (System.Text.Encoding.UTF8.GetString (decrypted),data);
		} else {
			Create ();
		}
	}

	public static RijndaelManaged rijndaelManaged(){
		RijndaelManaged rijndael = new RijndaelManaged();
		rijndael.Padding = PaddingMode.Zeros;
		rijndael.Mode = CipherMode.CBC;
		rijndael.KeySize = 256;
		rijndael.BlockSize = 256;
		string pw = "+YjKwBe8Fm3n1B1/X27hEOFbFRKq9K/Z6J0AAEp2l4c=";
		string salt = "GP6K/QeUVqgZKBjljpPZ6pXM7++MoQTFJiYAca8fM10=";
		byte[] bSalt = System.Text.Encoding.UTF8.GetBytes (salt);
		Rfc2898DeriveBytes deriveBytes = new Rfc2898DeriveBytes (pw, bSalt);
		deriveBytes.IterationCount = 1000; 
		rijndael.Key = deriveBytes.GetBytes (rijndael.KeySize / 8);
		rijndael.IV = deriveBytes.GetBytes (rijndael.BlockSize / 8);
		return rijndael;
	}

	void Create(){
		data.stage = 0;
		data.monsterNumber = new int[21];
		data.monsterEXP = new int[21];
		for (int i = 0; i < data.monsterNumber.Length; i++) {
			data.monsterNumber [i] = -1;
		}
		data.team.back.right.number = -1;
		data.team.back.middle.number = 0;
		data.team.back.left.number = -1;
		data.team.front.right.number = -1;
		data.team.front.middle.number = -1;
		data.team.front.left.number = -1;
		data.team.back.right.EXP = 0;
		data.team.back.middle.EXP = 0;
		data.team.back.left.EXP = 0;
		data.team.front.right.EXP = 0;
		data.team.front.middle.EXP = 0;
		data.team.front.left.EXP = 0;
		Save ();
	}

	public void Delete(){
		Create ();
		StartCoroutine(World.Title ());
	}
}
