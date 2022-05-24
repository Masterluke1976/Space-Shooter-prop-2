using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private GameObject[] powerups;

    private bool _stopSpawning = false;

    //5.24 wave system need variable for three waves
    [SerializeField]
    private GameObject[] _wave1;
    [SerializeField]
    private GameObject[] _wave2;
    [SerializeField]
    private GameObject[] _wave3;

    private int _currentWave = 1;
    
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnEnemyRoutine()
    {
        Vector3 postToSpawn;
        yield return new WaitForSeconds(3.0f);
        // 5.24
        while (_stopSpawning == false)
        {
            //switch statements for waves
            switch (_currentWave)
            {
                case 1:
                    
                    foreach (GameObject enemy in _wave1)
                    {
                        postToSpawn = new Vector3(Random.Range(-8f, 8f), 10, 0);
                        GameObject newEnemy = Instantiate(enemy, postToSpawn, Quaternion.identity);
                        newEnemy.transform.parent = _enemyContainer.transform;
                        yield return new WaitForSeconds(5.0f);
                    }
                    break;
                case 2:
                    
                    foreach (GameObject enemy in _wave2)
                    {
                        postToSpawn = new Vector3(Random.Range(-8f, 8f), 10, 0);
                        GameObject newEnemy = Instantiate(enemy, postToSpawn, Quaternion.identity);
                        newEnemy.transform.parent = _enemyContainer.transform;
                        yield return new WaitForSeconds(5.0f);
                    }
                    break;
                case 3:
                    
                    foreach (GameObject enemy in _wave3)
                    {
                        postToSpawn = new Vector3(Random.Range(-8f, 8f), 10, 0);
                        GameObject newEnemy = Instantiate(enemy, postToSpawn, Quaternion.identity);
                        newEnemy.transform.parent = _enemyContainer.transform;
                        yield return new WaitForSeconds(5.0f);
                    }
                    break;
                default:
                    Debug.Log("No more Waves");
                    break;

            }
            yield return new WaitForSeconds(2);
            _currentWave++;
            //Vector3 postToSpawn = new Vector3(Random.Range(-8f, 8f), 10, 0);
            //GameObject newEnemy = Instantiate(_enemyPrefab, postToSpawn, Quaternion.identity);
            //newEnemy.transform.parent = _enemyContainer.transform;
            //yield return new WaitForSeconds(5.0f);
            
        }
        
    }

    IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(3.0f);

        while (_stopSpawning == false)
        {
            Vector3 postToSpawn = new Vector3(Random.Range(-8f, 8f), 10, 0);
            int randomPowerUp = Random.Range(0, powerups.Length);
            Instantiate(powerups[randomPowerUp], postToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(3, 8));
        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }

    
}
