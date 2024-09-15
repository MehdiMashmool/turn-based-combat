using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using AS.Modules.GameStates;
using UnityEngine.SceneManagement;
using System.Collections;

namespace AS.Modules.Gameplay
{
    internal class FinishGameController : MonoBehaviour
    {
        [SerializeField] private FinishGameState m_FinishGameState;
        [SerializeField] private GameObject m_ResultPanel;
        [SerializeField] private TextMeshProUGUI m_ResultText;
        [SerializeField] private Button m_Button;
        [SerializeField] private float m_DisplayDelay;
        [SerializeField] private string m_WinText = "You Win!";
        [SerializeField] private string m_LoseText = "You Lose ):";

        private void OnEnable()
        {
            m_FinishGameState.OnFinish += OnFinishGame;
            m_Button.onClick.AddListener(OnClick);
        }

        private void OnDisable()
        {
            m_FinishGameState.OnFinish -= OnFinishGame;
            m_Button.onClick.RemoveListener(OnClick);
        }

        private void OnClick()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        private void OnFinishGame(bool result)
        {
            StartCoroutine(Delay(result));
        }

        private IEnumerator Delay(bool result)
        {
            yield return new WaitForSeconds(m_DisplayDelay);
            string resultText = result ? m_WinText : m_LoseText;
            m_ResultText.text = resultText;
            m_ResultPanel.SetActive(true);
        }
    }
}
