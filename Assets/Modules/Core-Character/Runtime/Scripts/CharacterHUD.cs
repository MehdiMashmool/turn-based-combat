using UnityEngine;
using UnityEngine.UI;

namespace AS.Modules.CoreCharacter
{
    internal class CharacterHUD : MonoBehaviour
    {
        [SerializeField] private Image m_Bar;

        /// <summary>
        /// </summary>
        /// <param name="amount">between 0-1</param>
        internal void UpdateBar(float amount)
        {
            m_Bar.fillAmount = amount / 1;
        }
    }
}
