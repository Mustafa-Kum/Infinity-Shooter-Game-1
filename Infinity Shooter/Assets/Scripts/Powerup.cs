using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Powerup : MonoBehaviour
{
    
    [SerializeField]
    private float _speed = 3.0f;
    [SerializeField]
    private int powerupID; // ---> 0 = TripleShot - 1 = Speed - 2 = Shield
    [SerializeField]
    private AudioClip _clip;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if(transform.position.y < -8.0f) // ---> y düzleminde -8.0f'den küçük ise obje yok edilecek.
        {
            Destroy(this.gameObject);      
        }
    }
    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.tag == "Player") // ---> Tag'i Player olan obje çarptığı zaman.
        {
            Player player = other.transform.GetComponent<Player>();// ---> Fonksyon çapırmak için.

            AudioSource.PlayClipAtPoint(_clip, transform.position); // ---> Anında yok etmek istediğimiz objeler için ses oynatma.
            
            if (player != null) // ---> powerupID 0-1-2 arasındaki değerlerde bunları yapsın.
            {
                switch(powerupID) 
                {
                    case 0:
                        player.TripleShotActive();
                        break;
                    case 1:
                        player.SpeedBoostActive();
                        break;
                    case 2:
                        player.ShieldActive();
                        break;
                    default:
                        Debug.Log("Default.");
                        break;
                }
                
            }
            Destroy(this.gameObject);
        }
    }
}
