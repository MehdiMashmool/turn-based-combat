# if UNITY_EDITOR
using AS.Modules.Stating.Core;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace AS.Modules.Stating.Editor
{
    [CustomEditor(typeof(Blackboard))]
    public class BlackboardEditor : UnityEditor.Editor
    {
        [SerializeField] private VisualTreeAsset m_BlackboardUXML;
        [SerializeField] private VisualTreeAsset m_CategoryUXML;

        private Blackboard Blackboard => target as Blackboard;

        private VisualElement m_Blackboard;
        private List<VisualElement> m_Categories;

        private void OnEnable()
        {
            Blackboard.OnChangeCategory += OnChangeCategory;
        }

        private void OnDisable()
        {
            Blackboard.OnChangeCategory -= OnChangeCategory;
        }

        public override VisualElement CreateInspectorGUI()
        {
            m_Blackboard = m_BlackboardUXML.CloneTree();
            var container = m_Blackboard;
            SetCategories();
            m_Blackboard.Q<Button>("add").RegisterCallback<ClickEvent>(OnClickAdd);

            var iterator = serializedObject.GetIterator();
            if (iterator.NextVisible(true))
            {
                do
                {
                    var propertyField = new PropertyField(iterator.Copy()) { name = "PropertyField:" + iterator.propertyPath };

                    if (iterator.propertyPath == "m_Script" && serializedObject.targetObject != null)
                    {
                        propertyField.SetEnabled(value: false);
                    }
                    else
                    {
                        if (propertyField.bindingPath == "m_Fields")
                        {
                            SerializedProperty property = serializedObject.FindProperty("m_Fields");

                            for (int i = 0; i < property.arraySize; i++)
                            {
                                SerializedProperty serialized = property.GetArrayElementAtIndex(i);
                                object value = serialized.boxedValue;
                                BlackboardField blackboardField = value as BlackboardField;
                                PropertyField blackboardProperty = new PropertyField(serialized);
                                VisualElement category = GetFoldoutCategory(blackboardField.Category);
                                Foldout foldout = category.Q<Foldout>("Foldout");
                                foldout.Add(blackboardProperty);
                            }
                        }
                    }
                }
                while (iterator.NextVisible(false));
            }

            container.Bind(serializedObject);

            return container;
        }

        private void OnClickAdd(ClickEvent evt)
        {
            BlackboardField blackboardField = new BlackboardField("Default");
            Blackboard.AddField(blackboardField);
        }

        private void OnChangeCategory(List<string> obj)
        {
            SetCategories();
        }

        private void SetCategories()
        {
            //m_Blackboard.Clear();
            m_Categories = new List<VisualElement>();
            IReadOnlyList<string> categories = Blackboard.GetCategories();
            VisualElement blackboardFiellds = m_Blackboard.Q<VisualElement>("blackboardfields");

            for (int i = 0; i < categories.Count; i++)
            {
                VisualElement category = m_CategoryUXML.CloneTree();
                category.name = $"{categories[i]}";
                Foldout foldout = category.Q<Foldout>("Foldout");
                foldout.text = categories[i];
                m_Categories.Add(category);
                blackboardFiellds.Add(category);
            }
        }

        private VisualElement GetFoldoutCategory(string categoryName)
        {
            for (int i = 0; i < m_Categories.Count; i++)
            {
                if (m_Categories[i].name == categoryName)
                {
                    return m_Categories[i];
                }
            }

            return null;
        }
    }
}
#endif