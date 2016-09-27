using UnityEngine;
using System.Collections;

public class ExceptionTest : MonoBehaviour {
	void Start () {
        TestInstance.Instanc.Test();
        try
        {
            TestInstance.Instanc.Test();
        }
        catch (System.Exception e)
        {
            Debug.Log(e.Message);
            Debug.Log(e.StackTrace);
        }
	
	}
	
	void Update () {
        
	}
}
