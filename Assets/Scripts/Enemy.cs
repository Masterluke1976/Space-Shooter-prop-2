using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.0f;

    private Player _player;
    private Animator _anim;

    private AudioSource _audioSource;
    [SerializeField]
    private GameObject _laserPrefab;
    private float _fireRate = 3.0f;
    private float _canFire = -1;

    private bool _isAlive = true;

    [SerializeField]
    private bool _moveHorizontaly = false;
    private bool _moveRight = false;
    private float _horizontalSpeed = 2;

     
   


    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _audioSource = GetComponent<AudioSource>();
        
        if (_player == null)
        {
            Debug.LogError("The Player is NULL");
        }

        _anim = GetComponent<Animator>();

        if (_anim == null)
        {
            Debug.LogError("The Animator is NULL ");
        }

        if (_moveHorizontaly == true)
        {
            StartCoroutine(SwitchDirectionRoutine());
        }
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
        if (_moveHorizontaly == true)
        {
            MoveHorizontaly();
        }
         


        if (Time.time > _canFire && _isAlive == true)
        {
            _fireRate = Random.Range(3f, 7f);
            _canFire = Time.time + _fireRate;
            GameObject enemyLaser = Instantiate(_laserPrefab, transform.position, Quaternion.identity);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();

            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].AssignEnemyLaser();
            }

        } 

    }

    void CalculateMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -10f)
        {
            float randomX = Random.Range(-8f, 8f);
            transform.position = new Vector3(randomX, 10, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.tag == "Player")
        {
            
            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {
                player.Damage();
            }
            _anim.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _audioSource.Play();
            _isAlive = false;
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.6f);


        }
        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);

            if (_player != null)
            {
                _player.AddScore(10);
            }
            _anim.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _audioSource.Play();
            _isAlive = false;
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.6f);
        }
    }

    void MoveHorizontaly()
    {
        if (_moveRight == true)
            transform.Translate(new Vector3(1, 0, 0) * _horizontalSpeed * Time.deltaTime);
        else if (_moveRight == false)
            transform.Translate(new Vector3(-1, 0, 0) * _horizontalSpeed * Time.deltaTime);
    }

    IEnumerator SwitchDirectionRoutine()
    {
        while (_moveHorizontaly == true)
        {
            _moveRight = true;
            yield return new WaitForSeconds(3);
            _moveRight = false;
            yield return new WaitForSeconds(3);
        }
    }

   
}
