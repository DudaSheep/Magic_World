using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public TextMeshProUGUI scoreText;
    private float score = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        score += 0.5f * Time.deltaTime; // ganha 1.5 pontos por segundo
        scoreText.text = "Score: " + (int)score; // atualiza o texto de Score do game
    }

    // Ao acertar o inimigo com sua fireball
    public void AddBonus(int amount)
    {
        score += amount;
    }
}
