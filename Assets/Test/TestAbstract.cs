using UnityEngine;
using System.Collections;

public abstract class TestAbstract : MonoBehaviour ,TestInterface
{
    public abstract void TestAbs();
    public virtual void TestVirtual()
    {

    }
    public void TestFunc()
    {

    }
    public void TestInterFunction()
    {
        Debug.Log("TestInterface");
    }

}
