using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimientoPlataforma : MonoBehaviour
{
    public Transform target;
    public float speed;
    private Vector3 start, end;

    // Start is called before the first frame update
    void Start()
    {
        if(target!=null)
        {
            target.parent = null;//EL TARGET DEJA DE SER HIJO DE PLATAFORMA MOVIL
            start = transform.position;
            end = target.position;
            
        }
    }
 
    private void FixedUpdate()
    {
        if(target!=null)
        {
            float fixedSpeed = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, target.position, fixedSpeed);
        }
        if(transform.position==target.position)
        {
            target.position = (target.position == start) ? end : start;
        }

        
    }
    
}
