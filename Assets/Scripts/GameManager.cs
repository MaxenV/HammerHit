
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Transform boxesParent;
    [SerializeField] private TMP_Text victoryText;

    private bool gameEnded;

    private void Start()
    {
        Time.timeScale = 1f;
        victoryText.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (gameEnded)
            return;

        if (boxesParent.childCount == 0)
        {
            WinGame();
        }
    }

    private void WinGame()
    {
        gameEnded = true;

        victoryText.text = "VICTORY!";
        victoryText.gameObject.SetActive(true);

        Time.timeScale = 0f;
    }

    private void LoseGame()
    {
        gameEnded = true;

        victoryText.text = "YOU LOSE!";
        victoryText.gameObject.SetActive(true);

        Time.timeScale = 0f;
    }
}
