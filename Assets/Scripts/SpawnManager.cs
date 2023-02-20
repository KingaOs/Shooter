using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _enemyPrefab;
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private GameObject[] powerups;

    [SerializeField]
    private int[] _enemiesPerWave;
    [SerializeField]
    private float[] _timeBetweenEnemiesSpawn;

    [SerializeField]
    public List<GameObject> _enemies = new List<GameObject>();

    [SerializeField]
    private UIManager _uIManager;
    private bool _stopSpawning = false;

    GameObject _newEnemy;
    Vector3 posToSpawn;


    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemies());
        StartCoroutine(SpawnPowerup());
    }
    IEnumerator SpawnEnemies()
    {
        yield return new WaitForSeconds(2);
  
            for (int i = 0; i < _enemiesPerWave.Length; i++) //which wave
            {
            _uIManager.NextWave(i + 1);
                for (int j = 0; j < _enemiesPerWave[i]; j++) //how many enemies in a wave
                {
                if (_stopSpawning == true)
                {
                    StopAllCoroutines();
                    break;
                }



                    posToSpawn = new Vector3(Random.Range(-9f, 9f), 7.4f, 0);
                    _newEnemy = Instantiate(_enemyPrefab[0], posToSpawn, Quaternion.identity);



                    _newEnemy.transform.parent = _enemyContainer.transform;
                    _enemies.Add(_newEnemy);  // adding enemies to the list
                    yield return new WaitForSeconds(_timeBetweenEnemiesSpawn[i]); 

                }
                yield return new WaitUntil(() => _enemies.Count == 0);
            }   
    }


    
        IEnumerator SpawnPowerup()
    {
        yield return new WaitForSeconds(2);
        while (_stopSpawning == false)
        {
            var randomPowerup =ChoosePowerup();
            Vector3 posToSpawn = new Vector3(Random.Range(-9f, 9f), 7.4f, 0);
            //int randomPowerUp = Random.Range(0, powerups.Length);
            Instantiate(powerups[randomPowerup], posToSpawn, Quaternion.identity);

            var random = Random.Range(3f, 7f);
            yield return new WaitForSeconds(random);
        }
    }

    private int ChoosePowerup()
    {
        var  random = Random.Range(0, 101);
        if(random >=75) // Ammo 25%
        {
            return 3;
        }
        else if(random >= 60 && random < 75) //Speed 15%
        {
            return 1;
        }
        else if (random >= 45 && random < 60) //Can't Shoot 15%
        {
            return 6;
        }
        else if (random >= 35 && random < 45) // Shield 10%
        {
            return 2;
        }
        else if (random >= 20 && random < 35) //Triple Shoot 15%
        {
            return 0;
        }
        else if (random >= 10 && random < 20) // Health 10%
        {
            return 4;
        }
        else if (random >= 5 && random < 10) // Laser Beam 5%
        {
            return 5;
        }
        else if (random >= 0 && random < 5) // Missle 5%
        {
            return 7;
        }
        return 3;

    }



    public void OnPLayerDeath()
    {
        _stopSpawning = true;
    }

}
