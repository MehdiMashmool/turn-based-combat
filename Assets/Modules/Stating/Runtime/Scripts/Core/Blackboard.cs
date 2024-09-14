using System;
using UnityEngine;
using AS.Modules.Stating.Core;
using System.Collections.Generic;

namespace AS.Modules.Stating
{
    public class Blackboard : MonoBehaviour
    {
        [SerializeField] private List<BlackboardField> m_Fields;
        private List<string> m_Categories = new List<string>()
        {
            "Default"
        };

        public event Action<List<string>> OnChangeCategory;

        [SerializeField] private BlackboardField m_Filed;

        public void AddField(BlackboardField field)
        {
            m_Fields.Add(field);
        }

        public void AddCategory(string category)
        {
            m_Categories.Add(category);
            OnChangeCategory?.Invoke(m_Categories);
        }

        public void RemoveCategory(string category)
        {
            if (m_Categories.Remove(category))
            {
                OnChangeCategory?.Invoke(m_Categories);
            }

        }

        public IReadOnlyList<string> GetCategories()
        {
            return m_Categories;
        }
    }
}
