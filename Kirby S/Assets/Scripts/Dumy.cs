using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dumy : MonoBehaviour
{
    [SerializeField]
    private float maxHealth,knockX,knockY,knockbackduration,KBDeathX,KBDeathY,deathTorque;
    [SerializeField]
    private bool applyknockback;

    private float CurrentHelth,knocbackStart;

    private int PlayerFcingDirection;
    private bool playeronLeft,knockBack;
    private Player pc;
    private GameObject AliveGO, BrokenTopGo, BorkenBotGo;
    private Rigidbody2D rbAlive, rbBrokenTop, rbBrokenBot;
    private Animator AliveAnim;
    private void Start()
    {
        CurrentHelth = maxHealth;
        pc = GameObject.Find("Player").GetComponent<Player>();//Referencia al jugador
        AliveGO = transform.Find("Alive").gameObject;
        BrokenTopGo = transform.Find("Broken Top").gameObject;
        BorkenBotGo = transform.Find("Broken Bottom").gameObject;


        AliveAnim = AliveGO.GetComponent < Animator>();
        rbAlive = AliveGO.GetComponent<Rigidbody2D>();
        rbBrokenTop = BrokenTopGo.GetComponent<Rigidbody2D>();
        rbBrokenBot = BorkenBotGo.GetComponent<Rigidbody2D>();

        AliveGO.SetActive(true);
        BrokenTopGo.SetActive(false);
        BorkenBotGo.SetActive(false);
    }
    private void Update()
    {
        CheckKnockBack();
    }
    private void Damage (float amount)
    {
        CurrentHelth -= amount;
        PlayerFcingDirection = pc.F_Direction();
        if (PlayerFcingDirection==1)
        {
            playeronLeft = true;
        }
        else
        {
            playeronLeft = false;
        }
        AliveAnim.SetBool("Player on left", playeronLeft);
        AliveAnim.ResetTrigger("Damege");
        if(applyknockback && CurrentHelth >0.0f)
        {
            //KnockBack
            KnockBack();
        }
        if(CurrentHelth<=0.0f)
        {
            Die();
        }

    }
    private void KnockBack()
    {
        knockBack = true;
        knocbackStart = Time.time;
        rbAlive.velocity = new Vector2(knockX * PlayerFcingDirection, knockY);
    }
    private void CheckKnockBack()
    {
        if (Time.time >= knocbackStart + knockbackduration&&knockBack)
        {
            knockBack = false;
            rbAlive.velocity = new Vector2(0.0f, rbAlive.velocity.y);
        }
    }

    private void Die()
    {
        AliveGO.SetActive(false);
        BrokenTopGo.SetActive(true);
        BorkenBotGo.SetActive(true);

        BrokenTopGo.transform.position = AliveGO.transform.position;
        BorkenBotGo.transform.position = AliveGO.transform.position;


        rbBrokenBot.velocity = new Vector2(knockX * PlayerFcingDirection, knockY);
        rbBrokenTop.velocity = new Vector2(KBDeathX * PlayerFcingDirection, KBDeathY);
        rbBrokenTop.AddTorque(deathTorque*-PlayerFcingDirection,ForceMode2D.Impulse);

    }
}
