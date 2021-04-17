using UnityEngine;
using UnityEditor;

//Original version of the ConditionalHideAttribute created by Brecht Lecluyse (www.brechtos.com)
//Modified by: -

[CustomPropertyDrawer(typeof(ConditionalHideAttribute))]
public class ConditionalHidePropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var condHAtt = (ConditionalHideAttribute)attribute;
        var enabled = GetConditionalHideAttributeResult(condHAtt, property);

        var wasEnabled = GUI.enabled;
        GUI.enabled = enabled;
        if (!condHAtt.HideInInspector || enabled)
        {
            EditorGUI.PropertyField(position, property, label, true);
        }

        GUI.enabled = wasEnabled;
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        var condHAtt = (ConditionalHideAttribute)attribute;
        var enabled = GetConditionalHideAttributeResult(condHAtt, property);

        if (!condHAtt.HideInInspector || enabled)
        {
            return EditorGUI.GetPropertyHeight(property, label);
        }
        else
        {
            //The property is not being drawn
            //We want to undo the spacing added before and after the property
            return -EditorGUIUtility.standardVerticalSpacing;
        }
    }

    private bool GetConditionalHideAttributeResult(ConditionalHideAttribute condHAtt, SerializedProperty property)
    {
        var enabled = true;

        var propertyPath =
            property.propertyPath; //returns the property path of the property we want to apply the attribute to
        string conditionPath;

        if (!string.IsNullOrEmpty(condHAtt.ConditionalSourceField))
        {
            //Get the full relative property path of the sourcefield so we can have nested hiding
            conditionPath =
                propertyPath.Replace(property.name,
                    condHAtt.ConditionalSourceField); //changes the path to the conditionalsource property path
            var sourcePropertyValue = property.serializedObject.FindProperty(conditionPath);

            if (sourcePropertyValue != null)
            {
                enabled = CheckPropertyType(sourcePropertyValue);
            }
            else
            {
                //Debug.LogWarning("Attempting to use a ConditionalHideAttribute but no matching SourcePropertyValue found in object: " + condHAtt.ConditionalSourceField);
            }
        }

        if (!string.IsNullOrEmpty(condHAtt.ConditionalSourceField2))
        {
            conditionPath =
                propertyPath.Replace(property.name,
                    condHAtt.ConditionalSourceField2); //changes the path to the conditionalsource property path
            var sourcePropertyValue2 = property.serializedObject.FindProperty(conditionPath);

            if (sourcePropertyValue2 != null)
            {
                enabled = enabled && CheckPropertyType(sourcePropertyValue2);
            }
            else
            {
                //Debug.LogWarning("Attempting to use a ConditionalHideAttribute but no matching SourcePropertyValue found in object: " + condHAtt.ConditionalSourceField);
            }
        }

        if (condHAtt.Inverse) enabled = !enabled;

        return enabled;
    }

    private bool CheckPropertyType(SerializedProperty sourcePropertyValue)
    {
        switch (sourcePropertyValue.propertyType)
        {
            case SerializedPropertyType.Boolean:
                return sourcePropertyValue.boolValue;
            case SerializedPropertyType.ObjectReference:
                return sourcePropertyValue.objectReferenceValue != null;
            default:
                Debug.LogError("Data type of the property used for conditional hiding [" +
                                sourcePropertyValue.propertyType + "] is currently not supported");
                return true;
        }
    }
}