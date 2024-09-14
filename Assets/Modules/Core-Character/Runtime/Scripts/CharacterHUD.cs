using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AS.Modules.CoreCharacter
{
    internal class CharacterHUD : MonoBehaviour
    {
        [SerializeField] private Image m_Bar;
        [SerializeField] private TextMeshProUGUI m_Amount;

        private Camera m_Main;

        private void Start()
        {
            m_Main = Camera.main;
        }

        private void Update()
        {
            transform.LookAt(m_Main.transform);
        }

        /// <summary>
        /// </summary>
        /// <param name="amount">between 0-1</param>
        internal CharacterHUD UpdateBar(float amount)
        {
            m_Bar.fillAmount = amount / 1;
            return this;
        }

        internal CharacterHUD UpdateText(float amount)
        {
            m_Amount.text = ((int)amount).ToString();
            return this;
        }
    }
}
