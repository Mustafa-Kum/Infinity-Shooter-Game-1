using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    
    [SerializeField]
    private float _speed = 8.0f; // ---> Lazer'in gidiş hızı.
    public bool _isEnemyLaser = false; // ---> Düşmanın lazeri.
    

    // Update is called once per frame
    void Update()
    {
        if (_isEnemyLaser == false) // ---> Player'ın lazeri ise yukarı yönlü git.
        {
            MoveUp();
        }
        else
        {
            MoveDown();
        }
    }

    void MoveUp()
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime); // ---> Lazer'in gidiş yönü ve hızı ayarı.

        if(transform.position.y > 8f)
        {
            if(transform.parent != null) // ---> Üst klasör null'a eşit değilse.
            {
                Destroy(transform.parent.gameObject); // ---> Parent'i yok et.
            }
            Destroy(this.gameObject); // ---> Lazer ekrandan çıktığı anda siliniyor.
        }
    }

    void MoveDown()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime); // ---> Lazer'in gidiş yönü ve hızı ayarı.

        if(transform.position.y < -8f)
        {
            if(transform.parent != null) // ---> Üst klasör null'a eşit değilse.
            {
                Destroy(transform.parent.gameObject); // ---> Parent'i yok et.
            }
            Destroy(this.gameObject); // ---> Lazer ekrandan çıktığı anda siliniyor.
        }
    }

    public void AssignEnemyLaser()
    {
        _isEnemyLaser = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_isEnemyLaser && other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>(); 

            if (player != null )
            {
                player.LaserDamage();
            }

            Destroy(this.gameObject);
        }
        else if (!_isEnemyLaser && other.CompareTag("Enemy"))
        {
        // Lazer player'a değil, başka bir düşmana çarptı. Hiçbir şey yapma.
        }
    }   

}
