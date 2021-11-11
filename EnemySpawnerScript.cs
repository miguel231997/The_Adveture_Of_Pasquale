using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerScript : MonoBehaviour
{
    public GameObject enemy;
    float randx;
    Vector2 WhereToSpawn;
    public float spawnRate = 10f;
    //public int howMany = 5;
    float nextSpawn = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        //for (int i = howMany; i > 0; i--)
        //{
            if (Time.time > nextSpawn)
            {
                nextSpawn = Time.time + nextSpawn;
                randx = Random.Range(-1.60f, 65.40f);
                WhereToSpawn = new Vector2(randx, transform.position.y);
                Instantiate(enemy, WhereToSpawn, Quaternion.identity);
                Debug.Log("pass");
            }
            
        //}
    }
}
