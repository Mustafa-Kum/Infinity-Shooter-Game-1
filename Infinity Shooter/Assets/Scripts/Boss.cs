using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4f;
    private Player _player;
    private Animator _anim;
    [SerializeField]
    private GameObject _explosionPrefab;
    private AudioSource _audioSource;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _BosslaserPrefab;
    private float _fireRate = 3.0f;
    private float _fireRate2 = 4.0f;
    private float _canFire = -1;
    private float _canFire2 = -1;
    private bool isDead = false;
    private int _hitCount = 0;
    private bool _isMovingRight = true;

    

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _audioSource = GetComponent<AudioSource>();

        if (_player == null)
        {
            Debug.LogError("The Player is Null");
        }

        _anim = GetComponent<Animator>();

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

            if (_hitCount >= 75)
            {
                Destroy(gameObject);
            }

            if (Time.time > _canFire)
            {
                _fireRate = Random.Range(1f, 3f);
                _canFire = Time.time + _fireRate;
                GameObject enemyLaser = Instantiate(_laserPrefab, transform.position, Quaternion.identity);
                Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();

                for (int i = 0; i < lasers.Length; i++)
                {
                    lasers[i].AssignEnemyLaser();
                }
            }

            if (Time.time > _canFire2)
            {
                _fireRate2 = Random.Range(2f, 4f);
                _canFire2 = Time.time + _fireRate2;
                GameObject bossLaser = Instantiate(_BosslaserPrefab, transform.position, Quaternion.identity);
                Laser[] lasers = bossLaser.GetComponentsInChildren<Laser>();

                for (int i = 0; i < lasers.Length; i++)
                {
                    lasers[i].AssignEnemyLaser();
                }
            }
        }
    }

    void CalculateMovement()
    {
        float yPosition = transform.position.y - (_speed * Time.deltaTime);
        float xPosition = transform.position.x;

        if (yPosition <= 4.81f) // Y ekseninde 4.81'e geldiğinde duracak
        {
            _speed = 0;
        }

        if (xPosition <= -11 || xPosition >= 11) // X ekseninde -11 ile +11 arasında rastgele hareket edecek
        {
            _isMovingRight = !_isMovingRight;
        }

        xPosition = _isMovingRight ? xPosition + ((_speed + 2f) * Time.deltaTime) : xPosition - ((_speed + 2f) * Time.deltaTime);

        transform.position = new Vector3(xPosition, yPosition, 0f);
    
    }

    private void OnTriggerEnter2D(Collider2D other)
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

            if (laser != null && !laser._isEnemyLaser)
            {
                Destroy(other.gameObject);

                if (_player != null)
                {
                    _player.AddScore(10);
                }

                _hitCount++;

                if (_hitCount >= 75)
                {
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
}
