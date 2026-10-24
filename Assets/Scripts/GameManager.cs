
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Transform boxesParent;
    [SerializeField] private TMP_Text victoryText;
    [SerializeField] private TMP_Text gameOverText;

    private bool gameEnded;
    public bool IsGameEnded => gameEnded;

    private void Start()
    {
        Time.timeScale = 1f;
        victoryText.gameObject.SetActive(false);
        gameOverText.gameObject.SetActive(false);
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
        EndGame(victoryText, "VICTORY!");
    }


    public void LoseGame()
    {
        EndGame(gameOverText, "YOU LOSE!");
    }

    private void EndGame(TMP_Text message, string text)
    {
        if (gameEnded)
            return;

        gameEnded = true;

        message.text = text;
        message.gameObject.SetActive(true);

        Time.timeScale = 0f;
    }
}
