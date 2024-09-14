using UnityEngine;
using UnityEngine.UI;
using AS.Modules.GameStates;

namespace AS.Modules.Gameplay
{
    internal class GameStarter : MonoBehaviour
    {
        [SerializeField] private Button m_FightButton;
        [SerializeField] private GameStateMachine m_GameStateMachine;

        private void OnEnable()
        {
            m_FightButton.onClick.AddListener(OnClick);
        }

        private void OnDisable()
        {
            m_FightButton.onClick.RemoveListener(OnClick);
        }

        private void OnClick()
        {
            m_GameStateMachine.Initialize();
            m_GameStateMachine.Run();
            m_FightButton.gameObject.SetActive(false);
            GameplayController.Instance.InvokeStartGame();
        }
    }
}
