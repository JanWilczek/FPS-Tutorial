using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour {
    public float walkSpeed = 5.0f;
    public float attackDistance = 2.0f;
    public float attackDamage = 10.0f;
    public float attackDelay = 1.0f;
    public float hp = 20.0f;
    public Transform[] transforms;

    private float timer = 0;
    private string currentState;
    private Animator animator;
    private AnimatorStateInfo stateInfo;

    private void OnTriggerStay(Collider other)
    {
        if (other.tag.Equals("Player") && hp > 0)
        {
            Quaternion targetRotation = Quaternion.LookRotation(other.transform.position - transform.position);
            float originalX = transform.rotation.x;
            float originalZ = transform.rotation.z;

            Quaternion finalRotation = Quaternion.Slerp(transform.rotation, targetRotation, 5.0f * Time.deltaTime);
            finalRotation.x = originalX;
            finalRotation.z = originalZ;
            transform.rotation = finalRotation;

            float distance = Vector3.Distance(transform.position, other.transform.position);
            if (distance > attackDistance && !stateInfo.IsName("Base Layer.wound"))
            {
                AnimationSet("run");
                transform.Translate(Vector3.forward * walkSpeed * Time.deltaTime);
            }
            else
            {
                if (timer<=0)
                {
                    AnimationSet("attack0");
                    other.SendMessage("TakeHit", attackDamage);
                    timer = attackDelay;
                }
            }

            if (timer > 0) timer -= Time.deltaTime;

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals("Player")) AnimationSet("idle0");
    }

    void TakeHit(float damage)
    {
        hp -= damage;
        if (hp<=0)
        {
            AnimationSet("death");
        }
        else
        {
            animator.CrossFade("wound", 0.5f,0);
        }
    }

    private void AnimationSet(string animationToPlay)
    {
        stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        animationReset();

        if (currentState == "")
        {
            currentState = animationToPlay;

            if (stateInfo.IsName("Base Layer.run") && currentState != "run") animator.SetBool("runToIdle0", true);

            string state = "idle0To" + currentState.Substring(0, 1).ToUpper() + currentState.Substring(1);
            animator.SetBool(state, true);
            currentState = "";
        }
    }

    // Ok, kodu tej funkcji nie do konca rozumiem
    private void animationReset()   
    {
        if (!stateInfo.IsName("Base Layer.idle0"))
        {
            animator.SetBool("idle0ToIdle1", false);
            animator.SetBool("idle0ToWalk", false);
            animator.SetBool("idle0ToRun", false);
            animator.SetBool("idle0ToWound", false);
            animator.SetBool("idle0ToSkill0", false);
            animator.SetBool("idle0ToAttack1", false);
            animator.SetBool("idle0ToAttack0", false);
            animator.SetBool("idle0ToDeath", false);
        }
        else
        {
            animator.SetBool("runToIdle0", false);
        }
    }

    // Use this for initialization
    void Start () {
        animator = transforms[0].GetComponent<Animator>();
        currentState = "";
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
