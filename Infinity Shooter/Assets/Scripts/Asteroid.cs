using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField]
    private float _rotateSpeed = 8.0f;
    [SerializeField]
    private GameObject _explosionPrefab;
    private SpawnManager _spawnManager;

    private void Start()
    {
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>(); // ---> Spawn Manager'a ulaşıp Componentlerini çekiyoruz.
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward * _rotateSpeed * Time.deltaTime); // ---> Astreoid Z ekseninde 3 hızla dönecek.
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser")
        {
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity); // ---> Astreoid lazer ile çarpıştığında exploionPrefab'i çağır.
            Destroy(other.gameObject); // ---> Lazer'i yok et.
            _spawnManager.StartSpawning();
            Destroy(this.gameObject); // ---> Astreoid'i yok et.
            
        }
    }
}
