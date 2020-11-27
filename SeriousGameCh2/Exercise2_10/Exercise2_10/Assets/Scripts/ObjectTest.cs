using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectTest : MonoBehaviour
{

   //private Vector3 targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
     void Update()
    {
       

        var v3T = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
        v3T = Camera.main.ScreenToWorldPoint(v3T);
        transform.position = v3T;

    }
           
}
