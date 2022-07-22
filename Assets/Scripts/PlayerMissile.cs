using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMissile : MonoBehaviour
{
    [SerializeField]
    private float _speed = 8f;
    private float _minDistance;

    [SerializeField]
    private GameObject _target = null;
    [SerializeField]
    private GameObject _missilesmoke;
    [SerializeField]
    private GameObject _missileExplosion;

    private Vector3 currentPosition;

    [SerializeField]
    private Animator _anim;

    [SerializeField]
    private AudioSource _audioSource;

    private Enemy _enemy;

   
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(TimeTillSelfDestruct());
        _target = FindEnemy();
        if (GameObject.Find("Enemy") != null)
        {
            _enemy = GameObject.Find("Enemy").GetComponent<Enemy>();
        }
       
        _anim = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    private void Update()
    {
        MovingTowardsEnemy();
    }

   

    GameObject FindEnemy()
    {
        GameObject badguy;
        badguy = GameObject.FindGameObjectWithTag("Enemy");

        _minDistance = Mathf.Infinity;
        currentPosition = transform.position;

        float distance = Vector3.Distance(badguy.transform.position, currentPosition);
        if (distance < _minDistance)
        {
            _target = badguy;
            _minDistance = distance;
        }
        return _target;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            _anim.SetTrigger("OnMissileDestruction");
            _audioSource.Play();
            _speed = 0;
            Destroy(GetComponent<Collider2D>());
            Destroy(_missilesmoke);
            
            Destroy(this.gameObject, 2.8f);

        }
        if (other.tag == "Explosion")
        {
            _anim.SetTrigger("OnMissileDestruction");
            _audioSource.Play();
            _speed = 0;
            Destroy(GetComponent<Collider2D>());
            Destroy(_missilesmoke);
            Destroy(this.gameObject, 2.8f);
        }
    }

    void MovingTowardsEnemy()
    {
        if (_target != null)
        {
            if (Vector3.Distance(transform.position, _target.transform.position) != 0)
            {
                transform.position = Vector2.MoveTowards(transform.position, _target.transform.position, _speed * Time.deltaTime);
                Vector2 direction = (transform.position - _target.transform.position).normalized;
                var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                var offset = 90f;

                transform.rotation = Quaternion.Euler(Vector3.forward * (angle + offset));
            }
        }
        if (_target == null)
        {
            transform.Translate(Vector3.up * _speed * Time.deltaTime);
            _target = FindEnemy();
        }
    }

    IEnumerator TimeTillSelfDestruct()
    {
       yield return new WaitForSeconds(2.0f);
        _anim.SetTrigger("OnMissileDestruction");
        _audioSource.Play();
        _speed = 0;
        Destroy(this.gameObject, 2.8f);
    }

    public void Damage()
    {
        _anim.SetTrigger("OnMissileDestruction");
        _audioSource.Play();
        _speed = 0;
        Destroy(GetComponent<Collider2D>());
        Destroy(_missilesmoke);
        Destroy(this.gameObject, 2.8f);
    }
}
