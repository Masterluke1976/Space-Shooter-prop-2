using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private Animator _cameraAnim;
    // Start is called before the first frame update
    void Start()
    {
        _cameraAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
   // void Update()
   // {
        
   // }

   public void CamShake()
   {
        _cameraAnim.SetTrigger("Shake");
   }
}
