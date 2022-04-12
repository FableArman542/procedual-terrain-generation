using UnityEditor;
using UnityEngine;
using System.Collections;

[CustomEditor(typeof(CameraController))]
[CanEditMultipleObjects]
public class CameraControllerEditor : Editor
{
    SerializedProperty _zoomSensitivity;
    SerializedProperty _smoothness;
    SerializedProperty _minimumAngle;
    SerializedProperty _maximumAngle;
    SerializedProperty _sensitivity;
    SerializedProperty minCamDistance;
    SerializedProperty maxCamDistance;
    SerializedProperty targetObject;
    SerializedProperty invertXAxis;
    SerializedProperty invertYAxis;
    SerializedProperty invertZoom;
    SerializedProperty preventOcclusion;
    SerializedProperty occlusionCorrection;

    private void OnEnable()
    {
        _zoomSensitivity = serializedObject.FindProperty("_zoomSensitivity");
        _smoothness = serializedObject.FindProperty("_inverseSmoothness");
        _minimumAngle = serializedObject.FindProperty("_minimumYAngle");
        _maximumAngle = serializedObject.FindProperty("_maximumYAngle");
        _sensitivity = serializedObject.FindProperty("_sensitivity");
        invertXAxis = serializedObject.FindProperty("invertXAxis");
        invertYAxis = serializedObject.FindProperty("invertYAxis");
        invertZoom = serializedObject.FindProperty("invertZoom");
        minCamDistance = serializedObject.FindProperty("minCamDistance");
        maxCamDistance = serializedObject.FindProperty("maxCamDistance");
        targetObject = serializedObject.FindProperty("target");
        preventOcclusion = serializedObject.FindProperty("preventOcclusion");
        occlusionCorrection = serializedObject.FindProperty("occlusionCorrection");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.Slider(_zoomSensitivity, 0f, .9f);
        EditorGUILayout.Slider(_smoothness, 0.1f, 1f);
        EditorGUILayout.Slider(_sensitivity, .1f, 10f);
        EditorGUILayout.Slider(_minimumAngle, -1.5f, 1.5f);
        EditorGUILayout.Slider(_maximumAngle, -1.5f, 1.5f);
        EditorGUILayout.Slider(minCamDistance, 0.1f, 150f);
        EditorGUILayout.Slider(maxCamDistance, 0.1f, 150f);
        EditorGUILayout.PropertyField(targetObject, new GUIContent("Target"));
        EditorGUILayout.PropertyField(preventOcclusion, new GUIContent("Prevent Target Occlusion"));
        EditorGUILayout.PropertyField(occlusionCorrection, new GUIContent("Occlusion Correction Offset"));
        EditorGUILayout.PropertyField(invertXAxis, new GUIContent("Invert X Axis"));
        EditorGUILayout.PropertyField(invertYAxis, new GUIContent("Invert Y Axis"));
        EditorGUILayout.PropertyField(invertZoom, new GUIContent("Invert Zoom"));
        serializedObject.ApplyModifiedProperties();
    }
}
