using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    public GameObject enemyPrefab;
    public float timeSpawn = 2f; // Tempo de aparecimento dos enemys
    public float minHeight = -3f;
    public float maxHeight = 2f;

    // Start is called before the first frame update
    void Start()
    {
        // Começa a chamar a função de criar inimigos InvokeRepeating("NomeDaFuncao", tempoParaComeçar, intervaloEntreRepetições)
        InvokeRepeating("Spawn", 1f, timeSpawn);
        // Chama a função que aumenta a dificuldade a cada 7 segundos
        InvokeRepeating("IncreaseDifficulty", 7f, 7f);
    }

    void Spawn()
    {
        // Altura random para o inimigo aparecer
        float posYRandom = Random.Range(minHeight, maxHeight);
        Vector3 posSpawn = new Vector3(12f, posYRandom, 0); 

        Instantiate(enemyPrefab, posSpawn, Quaternion.identity);
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
        
    }
}
