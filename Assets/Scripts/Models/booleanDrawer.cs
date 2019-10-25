using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(boolean))]
class booleanDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var field = property.FindPropertyRelative("boolValue");
        field.intValue = EditorGUI.Toggle(position, label, field.intValue != 0) ? 1 : 0;
    }
}