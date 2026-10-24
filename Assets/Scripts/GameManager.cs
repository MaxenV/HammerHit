
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Transform boxesParent;
    [SerializeField] private TMP_Text victoryText;
    [SerializeField] private TMP_Text gameOverText;
    [SerializeField] private GameObject restartButton;

    private bool gameEnded;
    public bool IsGameEnded => gameEnded;

    private void Start()
    {
        Time.timeScale = 1f;
        gameEnded = false;

        victoryText.gameObject.SetActive(false);
        gameOverText.gameObject.SetActive(false);
        restartButton.SetActive(false);
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
        restartButton.SetActive(true);

        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }
}
