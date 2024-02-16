using Core.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpawnController : MonoBehaviour
{
    public List<Transform> spawnBotPosition = new List<Transform>();
    public List<GameObject> botCount = new List<GameObject>();
    public BotController botPrefab;
    private float timeBetweenWaves; //Thời gian spawn giữa các waves
    private float countdown = 2f; // thời gian đếm ngược để spawn
    private int enemiesCountdown = 50; // số lượng enemies count down
    private bool isSpawning = true; // Check true false count down

    
    void Update()
    {
        countdown -= Time.deltaTime;
        if (countdown < 0f)
        {
            StartCoroutine(SpawnEnemyAtRandomPoint());
            countdown = timeBetweenWaves;
        }
    }


    IEnumerator SpawnEnemyAtRandomPoint()
    {
        timeBetweenWaves = Random.Range(3f, 5f);
        if (isSpawning && enemiesCountdown > 0) 
        {
            int randomIndex = Random.Range(0, spawnBotPosition.Count);
            Transform spawnPoint = spawnBotPosition[randomIndex];
            SmartPool.Instance.Spawn(botPrefab.gameObject, spawnPoint.position, spawnPoint.rotation);
            enemiesCountdown -= 1;
            yield return new WaitForSeconds(timeBetweenWaves);
        }
        else if(enemiesCountdown <=0)
        {
            isSpawning = false;
        }    
    }
}
