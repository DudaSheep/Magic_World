using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public TextMeshProUGUI scoreText;
    private float score = 0;
    public static int highestScore = 0;

    //UI pause menu gameover
    public GameObject pausePanel;
    public GameObject gameOverPanel;
    public GameObject mainMenuPanel;
    private bool isPaused = false;
    public TextMeshProUGUI finalScoreText;
    public TextMeshProUGUI highestScoreText;
    public TextMeshProUGUI highestScoreTextMenu;

    // Start is called before the first frame update
    void Start()
    {
        // Carrega o recorde salvo, se nao tiver assume 0
        highestScore = PlayerPrefs.GetInt("RecordeMago", 0);
        UpdateHighScoreTexts();

        // Jogo comeca parado
        Time.timeScale = 0f;

        // paineis desligados
        mainMenuPanel.SetActive(true);
        pausePanel.SetActive(false);
        gameOverPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        score += 0.5f * Time.deltaTime; // ganha 1.5 pontos por segundo
        scoreText.text = "Score: " + (int)score; // atualiza o texto de Score do game

        // ESC menu pause
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    // Funcao auxiliar atualizar o texto do HS
    void UpdateHighScoreTexts()
    {
        if (highestScoreText != null)
            highestScoreText.text = "Highest Score: " + highestScore;

        if (highestScoreTextMenu != null)
            highestScoreTextMenu.text = "Highest Score: " + highestScore;
    }

    // Ao acertar o inimigo com sua fireball
    public void AddBonus(int amount)
    {
        score += amount;
    }

    public void ResumeGame()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f; //descongela o mundo
        isPaused = false;
    }

    void PauseGame()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0f; //congela o mundo (fisica, animacao)
        isPaused = true;
    }

    public void ShowGameOver()
    {
        if (score > highestScore)
        {
            highestScore = (int)score;
            // SALVA O NOVO RECORDE NO DISCO
            PlayerPrefs.SetInt("RecordeMago", highestScore);
            PlayerPrefs.Save();
        }

        gameOverPanel.SetActive(true);
        Time.timeScale = 0f;

        // Pontuaçao final e highest do Panel
        finalScoreText.text = "Final Score: " + (int)score;
        highestScoreText.text = "Highest Score: " + highestScore;
    }

    public void RestartGame()
    {
        Debug.Log("Jogo reiniciado");
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void StartGame()
    {
        mainMenuPanel.SetActive(false); // esconder o menu
        Time.timeScale = 1f;
        score = 0;
    }

    public void QuitGame()
    {
        Debug.Log("Saindo do jogo");
        Application.Quit();
    }

    // Funcao para gastar score em troca de habilidade especial
    public bool SpendScore(int amount)
    {
        if (score >= amount)
        {
            score -= amount;
            scoreText.text = "Score: " + (int)score; // atualiza o texto na tela
            return true;
        }
        return false;
    }
}
