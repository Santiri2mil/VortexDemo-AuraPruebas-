using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetector : MonoBehaviour
{
    public int radio;
    public Transform Ubicacion;
    
    public LayerMask Player;
    private bool isPlayerhere;
    private void CheckSurronding()
    {
        isPlayerhere = Physics2D.OverlapCircle(Ubicacion.position, radio, Player);
       
    }
    private void OnDrawGizmos()//Dibuja el detector
    {
        Gizmos.DrawWireSphere(Ubicacion.position,radio);

    }
    public bool pH()
    {
        return isPlayerhere;
    }
    private void FixedUpdate()
    {
        CheckSurronding();
        pH();
    }
   
}
