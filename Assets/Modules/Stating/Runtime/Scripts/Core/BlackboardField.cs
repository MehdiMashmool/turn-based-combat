using UnityEngine;
using UnityEngine.UI;


namespace AS.Modules.Stating.Core
{
    [System.Serializable]
    public class BlackboardField
    {
        [SerializeField] private string m_Category = "Default";
        [SerializeField] private string m_SelectedValueName;

        public string Category => m_Category;

        public BlackboardField(string category) 
        {
            m_Category = category;
        }

        //Generated variables
        [SerializeField] private GameObject m_AutoGenerated_UnityEngine_CoreModule_GameObject;
        [SerializeField] private Image m_AutoGenerated_UnityEngine_UI_Image;
        [SerializeField] private RawImage m_AutoGenerated_UnityEngine_UI_RawImage;
    }
}