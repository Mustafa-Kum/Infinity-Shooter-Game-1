using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    
    //public or private ---> Public her yerden gözükür ve kontrol edilir. Private sadece Player kontrol edebilir.
    [SerializeField] // ---> Gizli olsa da kontrol edilmesine izin verilir.
    private float _speed = 3.5f;
    private float _speedMultiplier = 2.0f;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private float _fireRate = 0.30f; // ---> Space'e bastığım anda her 0.5 saniyede bir ateş edecek. O hız burada.
    private float _canFire = -1.0f; // ---> Oyun zamanı ile birleştirmek için bu değeri atamak zorundayız.
    [SerializeField]
    private float _lives = 3.0f;
    private SpawnManager _spawnManager;
    [SerializeField]
    private bool _isTripleShotActive = false; // ---> Triple shot false çünkü daha sonra aldığımızda true olacak.
    private bool _isSpeedBoostActive = false;
    [SerializeField]
    private bool _isShieldActive = false;
    [SerializeField]
    private GameObject _shieldVisualizer;
    [SerializeField]
    private GameObject _rightEngine, _leftEngine, _Thruster;
    [SerializeField]
    private int _score;

    private UIManager _uiManager;
    [SerializeField]
    private AudioClip _laserSoundClip;
    [SerializeField]
    private AudioClip _explosionSoundClip;
    private AudioSource _audioSource;

    private Animator _anim;

    private bool _isDead = true;

    
    
    

    
    
    // Start is called before the first frame update
    void Start()
    {
        // Take the current position = new position (0, 0, 0) ---> First go transform. Second write what you want to change.

        transform.position = new Vector3(0, 0, 0);  // ---> Player'ın başladığı konum.
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>(); // ---> Spawn manager'a ulaşıyoruz, iletişim kurduruyoruz.

        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();

        _audioSource = GetComponent<AudioSource>();

        _anim = GetComponent<Animator>();

        if (_anim == null)
        {
            Debug.Log("Animator is Null");
        }

        if(_spawnManager == null)
        {
            Debug.LogError("The Spawn Manager is Null.");
        }

        if (_uiManager == null)
        {
            Debug.LogError("UIManager is Null.");
        }

        if ( _audioSource == null)
        {
            Debug.LogError("AudioSource is Null");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_isDead == true)
        {
            CalculateMovement(); // ---> CalculateMovement'ı Update içerisinde çalıştırır.

            if(Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire) // ---> Sürekli ateş etmesin diye direkt fonksyonu yazmak yerine if ile birlikte yazıyoruz.
            {
                FireLaser();
            }
        }

    }
    
    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        
        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0); // ---> Nereye doğru hareket edeceğimizin ayarı.

        transform.Translate(direction * _speed * Time.deltaTime); // ---> Hareket edeceğimiz yerlere hangi hızla gideceğimizin ayarı.

        // Kısıtlamalar. --------------------------------------------------------------------------------------------------
        
        //if(transform.position.y >= 0) // ---> y düzleminde 0'dan yukarı gitmesini kısıtlıyoruz.
        //{
            //transform.position = new Vector3(transform.position.x, 0, 0);
        //}
        //else if(transform.position.y <= -3.8f) // ---> y düzleminde -3.8'den aşağı gitmesini kısıtlıyoruz.
        //{
        //    transform.position = new Vector3(transform.position.x, -3.8f, 0);
        //}

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0), 0); // ---> üstteki kodun kısa yazılmış hali

        if(transform.position.x < -11.3f) // ---> x düzleminde -11.3f'den sola gitmesini kısıtlıyoruz.
        {
            transform.position = new Vector3(11.3f, transform.position.y, 0);
        }
        else if(transform.position.x > 11.3f) // ---> x düzleminde 11.3f'den sağa gitmesini kısıtlıyoruz.
        {
            transform.position = new Vector3(-11.3f, transform.position.y, 0);
        }

        // Kısıtlamalar Bitti -------------------------------------------------------------------------------------------------


    }
    
    void FireLaser()
    {     
        
        _canFire = Time.time + _fireRate; // ---> Atış hızımızı ayarlamak için gerekli olan kod.
        
        if (_isTripleShotActive == true)
        {
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity); // ---> Powerup'ı aldığımızda True olacak ve 3 tane lazer sıkacaz.
        }
        else
        {
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 0.8f, 0), Quaternion.identity); // ---> Lazer bizim pozisyonumuzdan çıkacak. 
        // ---> new Vector3, lazer'in 0.8 kadar dışımızda spawn olacağını söylüyor.
        }

        _audioSource.Play(); // ---> Sesi otamatik olarak oynatacak.
    }

    public void Damage()
    {
        
        if (_isShieldActive == true)
        {
            _isShieldActive = false;
            _shieldVisualizer.SetActive(false);
            return;
        }
        
        _lives = _lives - 1; // ---> Bu class çağırıldığı anda canımızdan 1 gidecek.

        if (_lives == 2)
        {
            _rightEngine.SetActive(true);
        }
        else if (_lives == 1)
        {
            _leftEngine.SetActive(true);
        }

        _uiManager.UpdateLives(Mathf.RoundToInt(_lives)); // ---> UI Manager'a ulaşıp oradaki UpdateLives fonksyonuna ulaştık.

        if(_lives < 1) // ---> Canımız 1'den düşük ise ölürüz.
        {
            _isDead = false;
            _spawnManager.OnPlayerDeath();
            _uiManager.CheckForBestScore();
            _Thruster.SetActive(false);
            _rightEngine.SetActive(false);
            _leftEngine.SetActive(false);
            _anim.SetTrigger("OnPlayerDeath");
            _audioSource.clip = _explosionSoundClip;
            _audioSource.Play();
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.8f);
        }
    }

    public void LaserDamage()
     {
        
        if (_isShieldActive == true)
        {
            _isShieldActive = false;
            _shieldVisualizer.SetActive(false);
            return;
        }
        
        _lives =_lives - 0.5f; // ---> Bu class çağırıldığı anda canımızdan 1 gidecek.


        if (_lives == 2f)
        {
            _rightEngine.SetActive(true);
        }
        else if (_lives == 1f)
        {
            _leftEngine.SetActive(true);
        }

        _uiManager.UpdateLives(Mathf.RoundToInt(_lives)); // ---> UI Manager'a ulaşıp oradaki UpdateLives fonksyonuna ulaştık.

        if(_lives < 1f) // ---> Canımız 1'den düşük ise ölürüz.
        {
            _isDead = false;
            _spawnManager.OnPlayerDeath();
            _uiManager.CheckForBestScore();
            _Thruster.SetActive(false);
            _rightEngine.SetActive(false);
            _leftEngine.SetActive(false);
            _anim.SetTrigger("OnPlayerDeath");
            _audioSource.clip = _explosionSoundClip;
            _audioSource.Play();
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.8f);
        }
    }

    public void TripleShotActive()
    {
        _isTripleShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    IEnumerator TripleShotPowerDownRoutine() // ---> Powerup için saniye ayarlama.
    {
        yield return new WaitForSeconds(5.0f); // ---> 5 saniye sonra tripleshot'ı false'a çevir.
        _isTripleShotActive = false;
    }

    public void SpeedBoostActive()
    {
        _isSpeedBoostActive = true;
        _speed *= _speedMultiplier;
        StartCoroutine(SpeedBoostPowerDownRoutine());
    }
    
    IEnumerator SpeedBoostPowerDownRoutine() // ---> Powerup için saniye ayarlama.
    {
        yield return new WaitForSeconds(5.0f); // ---> 5 saniye sonra speedboost'u false'a çevir.
        _isSpeedBoostActive = false;
        _speed /= _speedMultiplier; 
    }

    public void ShieldActive()
    {
        _isShieldActive = true;
        _shieldVisualizer.SetActive(true);
    }

    public void AddScore(int points) // ---> points'i int yaptık çünkü değişken bir puan sistemi istiyoruz.
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }

}
