using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueCae : MonoBehaviour
{
    // Start is called before the first frame update
    public float fallDelay = 1f;
    public float respawnDelay = 3f;
    public GameObject TB;
    private Rigidbody2D rdP;
    private BoxCollider2D pc2d;
    private Vector3 start;
    private Player player;

    void Start()
    {
        rdP = GetComponent<Rigidbody2D>();
        pc2d = GetComponent<BoxCollider2D>();
        start = transform.position;
        player = FindObjectOfType<Player>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Invoke("Fall", fallDelay);
            Invoke("Respawn", fallDelay + respawnDelay);
        }
    }
    void Fall()
    {
        player.transform.parent = null;
        LeanTween.scale(TB, new Vector3(0f, 0f, 0f), 0.1f);
        //rdP.isKinematic = false;
        pc2d.isTrigger = true;
        
    }
    void Respawn()
    {
        LeanTween.scale(TB, new Vector3(1f, 1f, 1f), 0.1f);
        transform.position = start;
        //rdP.isKinematic = true;
        pc2d.isTrigger = false;
        rdP.velocity = Vector3.zero;
    }
}
