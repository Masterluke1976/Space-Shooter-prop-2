using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingMissile : MonoBehaviour
{
    
    [SerializeField]
    private GameObject _target;

    private bool _isTargetAvailable;

    
    [SerializeField]
    private float _speed = 3f;

    
    private GameObject  player;

    //7.20 tonight
    [SerializeField]
    private GameObject _thruster;
    [SerializeField]
    private AudioSource _audioSource;
    [SerializeField]
    private Animator _anim;
    //[SerializeField]
    //private GameObject _missileExplosion;
    

    // Start is called before the first frame update
    void Start()
    {
        //7.14
        player = GameObject.Find("Player");
        _target = player;
        //7.21 tonight
        StartCoroutine(TimeTillSelfDestruct());


        
       if (_target != null)
       {
            _isTargetAvailable = true;
           // Debug.LogError("Target is null");
       } 
       else
       {
            _isTargetAvailable = false;
            //return;
       }
    }

    // Update is called once per frame
    void Update()
    {

        Vector2 direction = (transform.position - _target.transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        var offset = 90f;
        transform.rotation = Quaternion.Euler(Vector3.forward * (angle + offset));

        transform.position= Vector2.MoveTowards(transform.position, _target.transform.position,  _speed * Time.deltaTime);
 
        Destroy(this.gameObject, 7f);


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
            _anim.SetTrigger("OnMissileDestruction");
            _audioSource.Play();
            _speed = 0;
            Destroy(_thruster);
            Destroy(gameObject, 2.8f);
        }


    }

    IEnumerator TimeTillSelfDestruct()
    {
        yield return new WaitForSeconds(4.0f);
        _anim.SetTrigger("OnMissileDestruction");
        _audioSource.Play();
        _speed = 0;
        Destroy(_thruster);
        Destroy(gameObject, 2.8f);
    }


}
