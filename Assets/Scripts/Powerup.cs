using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.0f;
    
    [SerializeField]
    private int powerupID;  // 0 = Triple Shot  1 = Speed  2 = shields 3 = ammo powerup 

    [SerializeField]
    private AudioClip _clip;

    
    private Player _player;
    private Vector3 direction;
    private float _speedC = 5;

    //6.15
    [SerializeField]
    private GameObject _explosionPrefab;


    // Start is called before the first frame update
    void Start()
    {
       // get the player component
       _player = GameObject.Find("Player").GetComponent<Player>(); 
    }

    // Update is called once per frame
    void Update()
    {
        //5.28 get key code for C
        if (Input.GetKey(KeyCode.C) && _player != null) 
        {
            direction = (_player.transform.position) - transform.position; 
            direction = direction.normalized; 
            transform.Translate(direction * _speedC * Time.deltaTime); 
        }
        else
        {
            transform.Translate(Vector3.down * _speed * Time.deltaTime); 

        }
        
        
        //transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -4.5f)
        {
            Destroy(this.gameObject);
        }
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
       if (other.tag == "Player")
       {
            //communicate with the player script
            Player player = other.transform.GetComponent<Player>();

            AudioSource.PlayClipAtPoint(_clip, transform.position);
            if (player != null)
            {
                switch (powerupID)
                {
                    case 0:
                        player.TripleShotActive();
                        break;
                    case 1:
                        player.SpeedBoostActive();
                        break;
                    case 2:
                        player.ShieldsActive();
                        break;
                    case 3:
                        player.AmmoActive();
                        break;
                    case 4:
                        player.HealthActive();
                        break;

                    //6.22
                    case 5:
                        player.NegativeSpeed();
                        break;

                   
                       
                       
                    default:
                        Debug.Log("Default Value");
                        break;
                }
            }

            
            Destroy(this.gameObject);
       }

       //6.15
       if (other.tag == "EnemyLaser"  || other.tag == "Laser")
        {
            Destroy(other.gameObject);
            BoxCollider2D _boxCollider = GetComponent<BoxCollider2D>();
            _boxCollider.enabled = false;
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(this.gameObject, 0.15f);
        }
    }
}

