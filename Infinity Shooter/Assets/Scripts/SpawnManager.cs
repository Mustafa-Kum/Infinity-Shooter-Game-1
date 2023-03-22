using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _enemyContainer;
    private bool _stopSpawning = false;
    [SerializeField]
    private GameObject[] powerups; // ---> Listemiz olduğu için array şeklini kullanıyoruz.
    [SerializeField]
    private GameObject _bossPrefab;
    private UIManager _uiManager;

    private void Start()
    {
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
    }
    
    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine()); // ---> 5 saniyede bir başlamasını burada aktive ediyoruz.
        StartCoroutine(SpawnPowerupRoutine());
        StartCoroutine(SpawnBossRoutine());
    }

    IEnumerator SpawnEnemyRoutine()
    {
        
        yield return new WaitForSeconds(3.0f); // ---> Kaç saniye sonra Spawn olacağının ayarı.
        while (_stopSpawning == false) // ---> Player öldüğünde düşman spawnlamayı bırak.
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8f,8f), 7, 0); // ---> Nerede spawn olacağını söyleyen kod.
            GameObject newEnemy = Instantiate(_enemyPrefab, posToSpawn, Quaternion.identity); // ---> Quanternion ---> Mevcut yönünde durması.
            newEnemy.transform.parent = _enemyContainer.transform;// ---> Çıkan düşmanları direkt oyun objesi olarak değil, Container'ın içine koyuyoruz.
            yield return new WaitForSeconds(Random.Range(1f, 5f)); // ---> Bu objenin oluşması için 5 saniye bekle.
        }
    }

    IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(10.0f); // ---> Kaç saniye sonra Spawn olacağının ayarı.
        while (_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8f,8f), 7, 0);
            int randomPowerup = Random.Range(0, 3);
            Instantiate(powerups[randomPowerup], posToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(7, 20)); // ---> Bu objenin oluşması için 7 ila 20 saniye arası.
        }
        
    }

    IEnumerator SpawnBossRoutine()
    {
        yield return new WaitForSeconds(30.0f); // 30 saniye sonra spawn olacak
        Vector3 posToSpawn = new Vector3(0, 9, 0); // BossEnemy'in spawn pozisyonu
        Instantiate(_bossPrefab, posToSpawn, Quaternion.identity); // BossEnemy spawn ediliyor
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
        _uiManager.ShowGameOverTextAfterDelay();
    }

}
