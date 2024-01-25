using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    public List<Transform> spawnBotPosition = new List<Transform>();
    public BotController botPrefab;
    public float timeInterval;


    private void Start()
    {
        StartCoroutine(SpawnBot());
    }
    // Update is called once per frame
    void Update()
    {
    }



    IEnumerator SpawnBot()
    {
        for (int i = 0;i< spawnBotPosition.Count; i++)
        {
            Instantiate(botPrefab, spawnBotPosition[i].transform.position, transform.rotation);
            yield return new WaitForSeconds(timeInterval);
        }
    }
  
}
