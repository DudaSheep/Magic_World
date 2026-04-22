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

    [Header("Limites da GhostFireball")]
    public int maxGhosts = 5;            // Limite total de 5
    private int currentGhostCount = 0;
    private Transform playerTransform;

    // Start is called before the first frame update
    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) playerTransform = playerObj.transform;

        // Come�a a chamar a fun��o de criar inimigos InvokeRepeating("NomeDaFuncao", tempoParaCome�ar, intervaloEntreRepeti��es)
        InvokeRepeating("Spawn", 1f, timeSpawn);
        // Chama a fun��o que aumenta a dificuldade a cada 7 segundos
        InvokeRepeating("IncreaseDifficulty", 7f, 7f);
    }

    void Spawn()
    {
        // Se já passou o tempo e ainda não atingiu o limite de 5
        if (gameTimer > timeToStartGhosts && currentGhostCount < maxGhosts && Random.value > 0.5f)
        {
            SpawnGhost();
        }
        else
        {
            SpawnNormal();
        }

    }

    void SpawnNormal()
    {
        float posYRandom = Random.Range(minHeight, maxHeight);
        Vector3 posSpawn = new Vector3(12f, posYRandom, 0);
        Instantiate(enemyPrefab, posSpawn, Quaternion.identity);
    }

    void SpawnGhost()
    {
        if (playerTransform == null) return;

        Vector3 posSpawn;

        // Randomico 50% de chance de vir da frente, 50% de vir de cima
        if (Random.value > 0.5f)
        {
            // Vendo da frente
            posSpawn = new Vector3(playerTransform.position.x + 8f, playerTransform.position.y, 0);
        }
        else
        {
            // Vindo de cima
            posSpawn = new Vector3(playerTransform.position.x + Random.Range(-2f, 2f), 6f, 0);
        }

        Instantiate(enemyGhostPrefab, posSpawn, Quaternion.identity);
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

        // Atualiza a contagem de fantasmas vivos no mapa
        currentGhostCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
    }
}
