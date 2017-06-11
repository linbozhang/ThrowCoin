﻿//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright (c) 2011-2014 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(UIStretch))]
public class UIStretchEditor : Editor
{
	public override void OnInspectorGUI ()
	{
		base.OnInspectorGUI();
		EditorGUILayout.HelpBox("UIStretch is a legacy component and should not be used anymore. All widgets have anchoring functionality built-in.", MessageType.Warning);
	}
}