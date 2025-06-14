using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Battle))]
public class StageEditor : Editor {
	public override void OnInspectorGUI() {
		Battle battle = target as Battle;
		EditorGUILayout.LabelField ("Back");

		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField ("  Number");
		battle.enemy.back.right.number = EditorGUILayout.IntField (battle.enemy.back.right.number,GUILayout.Width(48));
		battle.enemy.back.middle.number = EditorGUILayout.IntField (battle.enemy.back.middle.number,GUILayout.Width(48));
		battle.enemy.back.left.number = EditorGUILayout.IntField (battle.enemy.back.left.number,GUILayout.Width(48));
		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField ("  Level");
		battle.enemy.back.right.EXP = EditorGUILayout.IntField (battle.enemy.back.right.EXP,GUILayout.Width(48));
		battle.enemy.back.middle.EXP = EditorGUILayout.IntField (battle.enemy.back.middle.EXP,GUILayout.Width(48));
		battle.enemy.back.left.EXP = EditorGUILayout.IntField (battle.enemy.back.left.EXP,GUILayout.Width(48));
		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.LabelField ("Front");

		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField ("  Number");
		battle.enemy.front.right.number = EditorGUILayout.IntField (battle.enemy.front.right.number,GUILayout.Width(48));
		battle.enemy.front.middle.number = EditorGUILayout.IntField (battle.enemy.front.middle.number,GUILayout.Width(48));
		battle.enemy.front.left.number = EditorGUILayout.IntField (battle.enemy.front.left.number,GUILayout.Width(48));
		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField ("  Level");
		battle.enemy.front.right.EXP = EditorGUILayout.IntField (battle.enemy.front.right.EXP,GUILayout.Width(48));
		battle.enemy.front.middle.EXP = EditorGUILayout.IntField (battle.enemy.front.middle.EXP,GUILayout.Width(48));
		battle.enemy.front.left.EXP = EditorGUILayout.IntField (battle.enemy.front.left.EXP,GUILayout.Width(48));
		EditorGUILayout.EndHorizontal ();

		EditorUtility.SetDirty (target);
	}
}
