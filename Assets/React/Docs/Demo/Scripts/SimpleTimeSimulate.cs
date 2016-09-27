using UnityEngine;
using System.Collections;

public class SimpleTimeSimulate : MonoBehaviour {
    public float clawAttackAnimTime = 2f;
    public float enterPhase2Time = 2f;
    public float phase2Attack = 4f;
    public void ClawAttack()
    {
        clawAttackAnimTime = 2f;
    }
    public void EnterPhase_2()
    {
        enterPhase2Time=2f;
    }
    public void Phase2Attack()
    {
        phase2Attack = 4f;
    }
	// Update is called once per frame
	void Update () {
        clawAttackAnimTime -= Time.deltaTime;
        enterPhase2Time -= Time.deltaTime;
        phase2Attack -= Time.deltaTime;
        if (clawAttackAnimTime < 0f)
        {
            clawAttackAnimTime = 0f;
        }
        if (enterPhase2Time < 0f)
        {
            enterPhase2Time = 0f;
        }
        if (phase2Attack < 0f)
        {
            phase2Attack = 0f;
        }
	
	}
}
