using UnityEngine;
using UnityEditor;

// 告诉Unity：这个Drawer专门处理StateEntity类型的属性
[CustomPropertyDrawer(typeof(StateEntity))]
public class StateEntityDrawer : PropertyDrawer
{
    // 重写属性绘制逻辑
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // 1. 从StateEntity里拿到info字段的序列化属性
        SerializedProperty infoProp = property.FindPropertyRelative("info");

        // 2. 自定义显示名称：如果info有值就用info，否则用默认标签（比如Element X）
        string displayName = !string.IsNullOrEmpty(infoProp.stringValue)
            ? infoProp.stringValue
            : label.text;

        // 3. 用自定义名称绘制属性，保持折叠显示的功能
        EditorGUI.PropertyField(position, property, new GUIContent(displayName), true);
    }

    // 可选：保持属性的默认高度（如果需要折叠显示的话）
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property, label, true);
    }
}