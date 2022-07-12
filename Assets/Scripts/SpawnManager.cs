
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
    [SerializeField]
    private GameObject[] _wave1;
    [SerializeField]
    private GameObject[] _wave2;
    [SerializeField]
    private GameObject[] _wave3;
    [SerializeField]
    private GameObject[] _wave4;

    private int _currentWave = 1;

    private bool _stopSpawning = false;

    
    [SerializeField]
    private int[] _table = { 10, 10, 10, 50, 10, 10 }; // triple shot, speed, shield, ammo, health, multishot
    [SerializeField]
    private int _total, _randomNumber;
    [SerializeField]
    private List<GameObject> _powerUps;

    private bool _powerUpSpawn;

    



    // Start is called before the first frame update
    void Start()
    {
        //6.15
        foreach(var item in _table)
        {
            _total += item;
        }
        
    }

    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
        //7.11
        StartCoroutine(SpawnRarePowerupRoutine());

        
    }

    // Update is called once per frame
    void Update()
    {
      
      RestartPowerUpSpawnRoutine();
    }

    IEnumerator SpawnEnemyRoutine()
    {
        Vector3 postToSpawn;
        yield return new WaitForSeconds(3.0f);
        
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
                case 4:

                    foreach (GameObject enemy in _wave4)
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
            
            
        }
        
    }

    IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(3.0f);

        while (_stopSpawning == false)
        {
            Vector3 postToSpawn = new Vector3(Random.Range(-8f, 8f), 10, 0);

            
            int randomPowerUp = Random.Range(0, 5);


            //int randomPowerUp = Random.Range(0, powerups.Length);
            Instantiate(powerups[randomPowerUp], postToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(5.0f, 10.0f));

            
            _randomNumber = Random.Range(0, _total);

            if (_stopSpawning == true)
            {
                _powerUpSpawn = false;
                yield break;
            }
            else
            {
                for (int i = 0; i < _table.Length; i++)
                {
                    if (_randomNumber <= _table[i])
                    {
                        Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 10, 0);
                        Instantiate(_powerUps[i], posToSpawn, Quaternion.identity);
                        _powerUpSpawn = true;

                        yield break;
                    }
                    else
                    {
                        _randomNumber -= _table[i];
                    }
                }
            }
        }
    }

    //7.11
    IEnumerator SpawnRarePowerupRoutine()
    {
        yield return new WaitForSeconds(2.0f);
        while (_stopSpawning == false)
        {
            Vector3 postToSpawn = new Vector3(Random.Range(-8f, 8f), 10, 0);
            int randomPowerUp = Random.Range(0, 7);
            Instantiate(powerups[randomPowerUp], postToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(5.0f, 10.0f));
        }
    }

    
    private void RestartPowerUpSpawnRoutine()
    {
        if (_powerUpSpawn == true)
        {
            _powerUpSpawn = false;
            StartCoroutine(SpawnPowerupRoutine());
        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }

   

    
}
