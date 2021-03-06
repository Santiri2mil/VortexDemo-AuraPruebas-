﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField]
    private bool combatEnabled;
    [SerializeField]
    private float inputTimer,attack1Radius,attack1Damage;
    [SerializeField]
    private Transform Attack1HitBoxPos;
    [SerializeField]
    private LayerMask whatIsDamageable;

    private bool gotInput, isAttacking,isFirstAttack;

    private float lastInputTime = Mathf.NegativeInfinity;

    private Animator anim;

    void Update()
    {
        CheckAttack();
        CheckCombatInput();
    }
    void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetBool("CanAttack", combatEnabled);
    }
    private void CheckCombatInput()
    {
        if(Input.GetMouseButton(0))
        {
            if(combatEnabled)
            {
                //AttempCombat
                gotInput = true;
                lastInputTime = Time.time;
            }
        }
    }
    private void CheckAttack()
    {
        if(gotInput)
        {
            //Perform Attack1
            if(!isAttacking)
            {
                gotInput = false;
                isAttacking = true;
                isFirstAttack = !isFirstAttack;
                anim.SetBool("attack1", true);
                anim.SetBool("firstAttack", isFirstAttack);
                anim.SetBool("isAttacking", isAttacking);
            }
        }
        if(Time.time>=lastInputTime+inputTimer)
        {
            //Wait for new input
            gotInput = false;
        }
    }
  private void CheckAttackHitBox()
  {  
        Collider2D[] detectedObjects = Physics2D.OverlapCircleAll(Attack1HitBoxPos.position,attack1Radius,whatIsDamageable);
      
        foreach (Collider2D collider in detectedObjects)
        {
            collider.transform.parent.SendMessage("Damage",attack1Damage);
            //Instantiate hit particles
        }
  }

    private void FinishAttack1()
    {
        isAttacking = false;
        anim.SetBool("isAttacking", isAttacking);
        anim.SetBool("attack1", false);
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(Attack1HitBoxPos.position, attack1Radius);
    }


}
