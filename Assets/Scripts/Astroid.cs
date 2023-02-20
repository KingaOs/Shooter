using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Astroid : MonoBehaviour
{
    [SerializeField]
    private float _rotateSpeed = 15f;
    [SerializeField]
    private GameObject _explosionPrefab;
    SpawnManager _spawnManager;
    [SerializeField]
    GameObject _uiText;
    void Start()
    {
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();

        if (_spawnManager == null)
            Debug.Log("Spawn Manager not found");
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0,0,1) * _rotateSpeed * Time.deltaTime);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
  
        if (other.tag == "Laser")
        {
            var explosion =Instantiate(_explosionPrefab, gameObject.transform.position, Quaternion.identity);
            Destroy(other.gameObject);
            Destroy(explosion, 3);
            _spawnManager.StartSpawning();
            Destroy(this.gameObject,0.25f);
            _uiText.SetActive(false);
        }
    }

}
