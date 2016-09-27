using UnityEngine;
using System.Collections;
using React;
using Action = System.Collections.Generic.IEnumerator<React.NodeResult>;
public class ReactTest_cyc : MonoBehaviour {
    public SimpleTimeSimulate timeSimulate;
    public bool clawBlood = true;
    public bool ClawBlood()
    {
        return clawBlood;
    }
    public Action ExcuteClawAttack()
    {
        Debug.Log("ExcuteClawAttack simulate--begin");
        Debug.Log("choose claw ");
        timeSimulate.ClawAttack();
        while (true)
        {
            if (timeSimulate.clawAttackAnimTime <= 0f)
            {
                Debug.Log("ExcuteClawAttack simulate--end");
                yield return NodeResult.Success;
            }
            else
            {
                yield return NodeResult.Continue;
            }
        }
    }
    public Action ExcutePhase2_enter()
    {
        Debug.Log("ExcutePhase2_enter simulate--begin");
        timeSimulate.EnterPhase_2();
        while (true)
        {
            if (timeSimulate.enterPhase2Time <= 0f)
            {
                Debug.Log("ExcutePhase2_enter simulate--end");
                yield return NodeResult.Success;
            }
            else
            {
                yield return NodeResult.Continue;
            }
        }
    }
    public Action Phase2Attack()
    {
        Debug.Log("Phase2Attack simulate--begin");
        timeSimulate.Phase2Attack();
        while (true)
        {
            if (timeSimulate.phase2Attack <= 0f)
            {
                Debug.Log("Phase2Attack simulate--end");
                yield return NodeResult.Success;
            }
            else
            {
                yield return NodeResult.Continue;
            }
        }
    }
}
