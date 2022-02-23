using UnityEditor;
using UnityEditor.UI;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

public class CustomSliderEditor : SliderEditor
{
    public override VisualElement CreateInspectorGUI()
    {
        var root = new VisualElement();

        var transition = new PropertyField(serializedObject.FindProperty(CustomSlider.TransitionFieldName));
        var easing = new PropertyField(serializedObject.FindProperty(CustomSlider.EasingFieldName));
        var duration = new PropertyField(serializedObject.FindProperty(CustomSlider.DurationFieldName));
        var power = new PropertyField(serializedObject.FindProperty(CustomSlider.PowerFieldName));
        var label = new Label("Tween settings");

        root.Add(new IMGUIContainer(OnInspectorGUI));
        root.Add(label);
        root.Add(transition);
        root.Add(easing);
        root.Add(duration);
        root.Add(power);


        return root;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        serializedObject.Update();
        EditorGUI.BeginChangeCheck();
        serializedObject.ApplyModifiedProperties();
    }
}