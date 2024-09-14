#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using Assembly = System.Reflection.Assembly;
using BlackboardField = AS.Modules.Stating.Core.BlackboardField;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor.UIElements;

namespace AS.Modules.Stating.Editor.Core
{
    [CustomPropertyDrawer(typeof(BlackboardField))]
    public class BlackboardFieldEditor : PropertyDrawer
    {
        private VisualTreeAsset m_BlackboardDataUXML;

        private VisualElement m_BlackboardDataElement;
        private SerializedProperty m_Property;

        private const string DEFAULTCATEGORY = "Default";

        private string BlackboardFieldClassName = $"{nameof(BlackboardField)}";
        private string BlackboardFieldScriptPath;

        private TextField CategoryName => m_BlackboardDataElement.Q<TextField>("TextField");
        private DropdownField CategoryDropdown => m_BlackboardDataElement.Q<DropdownField>("DropdownField");
        private Blackboard Blackboard => m_Property.serializedObject.targetObject as Blackboard;

        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            BlackboardFieldScriptPath = $"{Application.dataPath}/Modules/Stating/Runtime/Scripts/Core/{BlackboardFieldClassName}.cs";
            m_Property = property;

            m_BlackboardDataUXML = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Modules/Stating/Editor/UXML/BlackboardData.uxml");
            m_BlackboardDataElement = m_BlackboardDataUXML.CloneTree();
            Button button = m_BlackboardDataElement.Q<Button>("typebutton");
            UpdatePropertyFiled();
            button.RegisterCallback<ClickEvent>(OnClick);
            UpdateCategory();
            return m_BlackboardDataElement;
        }

        private void OnClick(ClickEvent evt)
        {
            var currentEvent = Event.current;
            Vector2 mousePosition = Vector2.zero;
            if (currentEvent.type == EventType.MouseUp && currentEvent.button == 0)
            {
                mousePosition = currentEvent.mousePosition;
            }
            SearchWindowContext context = new SearchWindowContext(mousePosition);

            SearchWindowProvider provider = ScriptableObject.CreateInstance<SearchWindowProvider>();
            provider.OnSetData += OnSetData;
            SearchWindow.Open<SearchWindowProvider>(context, provider);
        }

        private void OnSetData(SearchWindowProvider provider, Type type)
        {
            provider.OnSetData -= OnSetData;

            Type blackboardFieldTtype = typeof(BlackboardField);
            FieldInfo[] blackboardFields = blackboardFieldTtype.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            bool haveType = false;

            foreach (FieldInfo field in blackboardFields)
            {
                if (field.FieldType == type)
                {
                    haveType = true;
                    break;
                }
            }

            string className = BlackboardFieldClassName;
            string variableType = type.Name;
            string assemblyName = type.Assembly.GetName().Name;
            assemblyName = assemblyName.Replace(".", "_");
            string variableName = $"m_AutoGenerated_{assemblyName}_{type.Name}";

            if (!haveType)
            {

                if (File.Exists(BlackboardFieldScriptPath))
                {
                    // Read the existing content of the class file
                    string fileContent = File.ReadAllText(BlackboardFieldScriptPath);

                    // Find the last semicolon ';' in the class
                    Match lastSemicolonMatch = Regex.Match(fileContent, @"(?<=;)[^;]*$");

                    if (lastSemicolonMatch.Success)
                    {
                        // Calculate the position to insert the new variable on the line immediately following the last semicolon
                        int insertIndex = lastSemicolonMatch.Index;

                        // Construct the new variable declaration
                        string newVariableDeclaration = $"\n        [SerializeField] private {variableType} {variableName};";

                        // Insert the new variable declaration after the last semicolon
                        fileContent = fileContent.Insert(insertIndex, $"{newVariableDeclaration}");

                        // Write the modified content back to the file
                        fileContent = AnalyzeAndAddUsingDirectives(fileContent, type);

                        File.WriteAllText(BlackboardFieldScriptPath, fileContent);

                        // Refresh the Unity Editor to reflect changes
                        AssetDatabase.SaveAssets();
                        AssetDatabase.Refresh();
                        Debug.Log($"Variable '{variableName}' added to class '{className}'.");
                    }
                    else
                    {
                        Debug.LogError("Failed to locate the last semicolon ';' in the class.");
                    }
                }
            }

            m_Property.FindPropertyRelative("m_SelectedValueName").stringValue = variableName;
            EditorUtility.SetDirty(m_Property.serializedObject.targetObject);
            m_Property.serializedObject.ApplyModifiedProperties();

            UpdatePropertyFiled();
        }

        private void UpdatePropertyFiled()
        {
            string path = m_Property.FindPropertyRelative("m_SelectedValueName").stringValue;
            if (!string.IsNullOrEmpty(path))
            {
                PropertyField propertyField = m_BlackboardDataElement.Q<PropertyField>("value");
                SerializedProperty property = m_Property.FindPropertyRelative(path);
                if (property != null)
                {
                    propertyField.BindProperty(property);
                    propertyField.Bind(m_Property.serializedObject);
                }
            }
        }

        private void UpdateCategory()
        {
            CategoryName.RegisterCallback<FocusOutEvent>(OnEditingFinished);
            CategoryDropdown.RegisterValueChangedCallback(OnChangeDropDown);
            CheckCategory();
        }

        private void OnChangeDropDown(ChangeEvent<string> evt)
        {
            m_Property.FindPropertyRelative("m_Category").stringValue = evt.newValue;
            m_Property.serializedObject.ApplyModifiedProperties();
            CheckCategory();
        }

        private void OnEditingFinished(FocusOutEvent evt)
        {
            List<string> categories = GetCategories();

            if (string.IsNullOrEmpty(CategoryName.value))
            {
                string currentCategory = m_Property.FindPropertyRelative("m_Category").stringValue;

                if (currentCategory != DEFAULTCATEGORY)
                {
                    Blackboard.RemoveCategory(currentCategory);
                }

                m_Property.FindPropertyRelative("m_Category").stringValue = DEFAULTCATEGORY;
                m_Property.serializedObject.ApplyModifiedProperties();

                CheckCategory();
            }
            else
            {
                if (!categories.Contains(CategoryName.value))
                {
                    m_Property.FindPropertyRelative("m_Category").stringValue = CategoryName.value;
                    Blackboard.AddCategory(CategoryName.value);
                    m_Property.serializedObject.ApplyModifiedProperties();
                    CheckCategory();
                }
            }
        }

        private void CheckCategory()
        {
            string category = m_Property.FindPropertyRelative("m_Category").stringValue;
            CategoryName.value = category;
            CategoryDropdown.choices = GetCategories();
            CategoryDropdown.index = GetCategoryIndex();
        }

        private int GetCategoryIndex()
        {
            List<string> categories = GetCategories();
            string category = m_Property.FindPropertyRelative("m_Category").stringValue;

            for (int i = 0; i < categories.Count; i++)
            {
                if (categories[i] == category)
                {
                    return i;
                }
            }

            return -1;
        }

        private List<string> GetCategories()
        {
            List<string> categories = new List<string>();
            IReadOnlyList<string> readOnlyCategories = Blackboard.GetCategories();

            for (int i = 0; i < readOnlyCategories.Count; i++)
            {
                categories.Add(readOnlyCategories[i]);
            }

            return categories;
        }

        private string AnalyzeAndAddUsingDirectives(string fileContent, Type type)
        {
            string[] lines = fileContent.Split("\n");
            string nameSpace = $"using {type.Namespace};\r";
            int index = 0;
            int nameSpaceindex = 0;
            bool haveNamespace = false;

            foreach (string line in lines)
            {
                index += line.Length;
                if (line == nameSpace)
                {
                    haveNamespace = true;
                    break;
                }

                if (!string.IsNullOrWhiteSpace(line))
                {
                    if (line == nameSpace)
                    {
                        haveNamespace = true;
                        break;
                    }

                    if (line.Contains("using"))
                    {
                        nameSpaceindex = index;
                    }
                }
            }

            if (!haveNamespace)
            {
                fileContent = fileContent.Insert(nameSpaceindex + 1, $"\n{nameSpace}");
            }

            return fileContent;
        }

        public class SearchWindowProvider : ScriptableObject, ISearchWindowProvider
        {
            public event Action<SearchWindowProvider, Type> OnSetData;

            public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
            {
                return BuildSearchBar();
            }

            public bool OnSelectEntry(SearchTreeEntry SearchTreeEntry, SearchWindowContext context)
            {
                OnSetData?.Invoke(this, SearchTreeEntry.userData as Type);
                Debug.Log(SearchTreeEntry.userData);
                return true;
            }

            private List<SearchTreeEntry> BuildSearchBar()
            {
                Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
                List<SearchTreeEntry> result = new List<SearchTreeEntry>();
                SearchTreeGroupEntry title = new SearchTreeGroupEntry(new GUIContent("Types"), 0);
                result.Add(title);
                //Assembly assembly;

                for (int i = 0; i < assemblies.Length; i++)
                {
                    Assembly assembly = assemblies[i];
                    SearchTreeGroupEntry assemblyGroup = new SearchTreeGroupEntry(new GUIContent(assembly.GetName().Name), 1);
                    result.Add(assemblyGroup);
                    Type[] assemblyTypes = assembly.GetTypes();

                    for (int j = 0; j < assemblyTypes.Length; j++)
                    {

                        if (assemblyTypes[j].IsPublic && (assemblyTypes[j].IsSerializable || typeof(UnityEngine.Object).IsAssignableFrom(assemblyTypes[j])))
                        {
                            SearchTreeEntry searchTreeEntry =
                                new SearchTreeEntry(new GUIContent($"{assemblyTypes[j].Namespace}.{assemblyTypes[j].Name}"))
                                { level = 2, userData = assemblyTypes[j] };
                            result.Add(searchTreeEntry);
                        }
                    }
                }

                return result;
            }
        }
    }
}
#endif