using UnityEngine;
using System.Collections;

public class TestInstance : MonoBehaviour {
    public static TestInstance Instanc;
         
	// Use this for initialization
	void Start () {
        Instanc = this;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void Test()
    {
        Debug.Log("TestInstance----test");
    }
}
