using UnityEngine;
using System.Collections;

public class TeasAbsImp : TestAbstract {

    public override void TestAbs()
    {
        throw new System.NotImplementedException();
    }
    public override void TestVirtual()
    {
        base.TestVirtual();
        TestInterFunction();
    }
}
