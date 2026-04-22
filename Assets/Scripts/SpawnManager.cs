using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    public GameObject enemyPrefab;
    public GameObject enemyGhostPrefab;

    public float timeSpawn = 2f; // Tempo de aparecimento dos enemys
    public float minHeight = -3f;
    public float maxHeight = 2f;

    // Configuraçoes de Fase
    public float timeToStartGhosts = 20f; // tempo para comecar a aparecer o ghost fireballs
    private float gameTimer = 0f; //tempo de game

    // Start is called before the first frame update
    void Start()
    {
        // Come�a a chamar a fun��o de criar inimigos InvokeRepeating("NomeDaFuncao", tempoParaCome�ar, intervaloEntreRepeti��es)
        InvokeRepeating("Spawn", 1f, timeSpawn);
        // Chama a fun��o que aumenta a dificuldade a cada 7 segundos
        InvokeRepeating("IncreaseDifficulty", 7f, 7f);
    }

    void Spawn()
    {
        // Altura random para o inimigo aparecer
        float posYRandom = Random.Range(minHeight, maxHeight);
        Vector3 posSpawn = new Vector3(12f, posYRandom, 0);

        int ghostCount = GameObject.FindGameObjectsWithTag("Enemy").Length;

        // Logica das enemy firaballs direfentes
        if (gameTimer > timeToStartGhosts)
        {
            // Spawna a fantasma roxa - ghost fireballs
            Instantiate(enemyGhostPrefab, posSpawn, Quaternion.identity);
        }
        else
        {
            // Spawna a Enemy_Fireball normal
            Instantiate(enemyPrefab, posSpawn, Quaternion.identity);
        }


    }

    void IncreaseDifficulty()
    {
        if (timeSpawn > 0.5f)
        {
            timeSpawn -= 0.2f;
            // Chama novamente a funcao Spawn com o novo valor timeSpawn(mais rapido)
            CancelInvoke("Spawn");
            InvokeRepeating("Spawn", timeSpawn, timeSpawn);
        }
    }

    // Update is called once per frame
    void Update()
    {
        gameTimer += Time.deltaTime; //contar o tempo real de game
    }
}
