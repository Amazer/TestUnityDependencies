using UnityEngine;
using System.Collections;
namespace Pal
{
	public class TestLoadAndRun : MonoBehaviour{

		public  void Start () {
            TestResourceLoader.LoadAssets(this, @"E:\CYCProject\TestDep\TestUnityDependencies\CYCData\Initial.assetbundle", "Initial",delegate(UnityEngine.Object obj)
            {
                if(obj==null)
                {
                    Debug.LogError("initial obj==null");
                    return;
                }
                GameObject go = GameObject.Instantiate(obj) as GameObject;
                go.name = "Initial";

                TestResourceLoader.LoadAssets(this, @"E:\CYCProject\TestDep\TestUnityDependencies\CYCData\ShadersList.assetbundle", "ShaderList", delegate (UnityEngine.Object objShader)
                 {
                     if (objShader == null)
                     {
                         Debug.LogError("objShader ==null");
                         return;
                     }
                     GameObject shaderGO = GameObject.Instantiate(objShader) as GameObject;
                     shaderGO.name = "objShader";

                     LoadUIAtlas();
                 });
            });
		}
        private void LoadUIAtlas()
        {
            StartCoroutine(LoadUIAtlas_corutine());
        }
        IEnumerator LoadUIAtlas_corutine()
        {
            int i = 0;
            TestResourceLoader.LoadAssets(this, @"E:\CYCProject\TestDep\TestUnityDependencies\CYCData\AT_Fantasy.assetbundle", "Fantasy Atlas", delegate (UnityEngine.Object fobj)
             {
                 if (fobj == null)
                 {
                     Debug.LogError("fobj ==null");
                     return;
                 }
                 GameObject fgo = GameObject.Instantiate(fobj) as GameObject;
                 fgo.name = "fansy_atlas";
                 ++i;
             });
            TestResourceLoader.LoadAssets(this, @"E:\CYCProject\TestDep\TestUnityDependencies\CYCData\AT_Wooden.assetbundle", "Wooden Atlas", delegate (UnityEngine.Object wobj)
             {
                 if (wobj == null)
                 {
                     Debug.LogError("wobj ==null");
                     return;
                 }
                 GameObject wgo = GameObject.Instantiate(wobj) as GameObject;
                 wgo.name = "wood_atlas";
                 ++i;
             });
            while(i<2)
            {
                yield return null;
            }
            TestResourceLoader.LoadAssets(this, @"E:\CYCProject\TestDep\TestUnityDependencies\CYCData\UI_FlagPanel.assetbundle", "FlagPanel", delegate (UnityEngine.Object panelObj)
             {
                 if (panelObj == null)
                 {
                     Debug.LogError("panelObj ==null");
                     return;
                 }
                 GameObject panelGO = GameObject.Instantiate(panelObj) as GameObject;
                 panelGO.transform.parent = GameObject.Find("UI Root/Camera").transform;
                 panelGO.transform.localScale = Vector3.one;

             });

        }

	}
}
