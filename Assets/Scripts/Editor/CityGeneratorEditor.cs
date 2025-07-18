using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CityGenerator))]
public class CityGeneratorEditor : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		var cgen = (CityGenerator)target;
		if (GUILayout.Button("Generate"))
			cgen.Generate();
		if (GUILayout.Button("Clear"))
			cgen.Clear();
	}
}
