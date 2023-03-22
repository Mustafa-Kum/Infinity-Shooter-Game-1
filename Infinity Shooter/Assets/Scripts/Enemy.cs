using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.0f;
    private Player _player;
    private Animator _anim;
    [SerializeField]
    private GameObject _explosionPrefab;
    private AudioSource _audioSource;
    [SerializeField]
    private GameObject _laserPrefab;
    private float _fireRate = 3.0f;
    private float _canFire = -1;
    private bool isDead = false;
    
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>(); // ---> Tag'i Player olan nesneyi bul Componentlerine ulaş.
        _audioSource = GetComponent<AudioSource>();
       
        if (_player == null)
        {
            Debug.LogError("The Player is Null");
        }

        _anim = GetComponent<Animator>(); // ---> Animator zaten Enemy'nin içinde oşduğu için ona ulaşmak için direkt GetComponent'i kullanıyoruz.

        if (_anim == null)
        {
            Debug.LogError("_anim is null");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead)
        {    
            CalculateMovement();

            if (Time.time > _canFire) // ---> Zaman kontorlü.
            {
                _fireRate = 2.0f;
                _canFire = Time.time + _fireRate;
                GameObject enemyLaser =  Instantiate(_laserPrefab, transform.position, Quaternion.identity); // ---> Düşmanın ateşleyecek olan laserini GameObjesine dönüştürdük.
                Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>(); // ---> Game Objeye iki tane lazer alacağımız için Children'ı kullandık.

                for (int i = 0; i < lasers.Length; i++) // ---> Lazer'in 2'den küçük olduğu durumları kontrol ediyoruz.
                {
                    lasers[i].AssignEnemyLaser();
                }
            }
        }    
    }

    void CalculateMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if(transform.position.y < -8.0f) // ---> y düzleminde -8.0f'den küçük ise tekrar başladığı yere dönecek..
        {
            float randomX = Random.Range(-8f, 8f);
            transform.position = new Vector3(randomX, 8, 0); // ---> x düzleminde rastgele yerlerden gelecek.
             
        }
    }

    private void OnTriggerEnter2D(Collider2D other) // ---> Lazer ya da karakterler birbirine çarparsa yok olacak.
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();

            if (player != null)
            {
                player.Damage();
            }

            _anim.SetTrigger("OnEnemyDeath");
            _speed = 0.5f;
            _audioSource.Play();
            Destroy(GetComponent<Collider2D>());
            Destroy(gameObject, 2.8f);
            isDead = true;
        }
        else if (other.CompareTag("Laser"))
        {
            Laser laser = other.GetComponent<Laser>();

            if (laser != null && !laser._isEnemyLaser) // Sadece player'ın attığı lazerlere tepki verin
            {
                Destroy(other.gameObject);

                if (_player != null)
                {
                    _player.AddScore(10);
                }

                _anim.SetTrigger("OnEnemyDeath");
                _speed = 0.5f;
                _audioSource.Play();
                Destroy(GetComponent<Collider2D>());
                Destroy(gameObject, 2.8f);
                isDead = true;
            }
        }
    }

}
