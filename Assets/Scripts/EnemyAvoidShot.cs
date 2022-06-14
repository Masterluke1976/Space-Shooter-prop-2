using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAvoidShot : MonoBehaviour
{
    private bool _isShotDectected;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       transform.Translate(Vector3.down * 3 * Time.deltaTime);
        if (transform.position.y < -7)
        {
            transform.position = new Vector3(Random.Range(-9, 10), 8, 0);
        }

        if (_isShotDectected == true)
        {
            StartCoroutine(AvoidShot());
        }
    }

    private IEnumerator AvoidShot()
    {
        transform.Translate(Vector3.left * 4 * Time.deltaTime);
        yield return new WaitForSeconds(2f);
        _isShotDectected = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag =="Laser")
        {
            _isShotDectected =true;
        }

       
    }

   
}
