using UnityEngine;
using System.Collections;
namespace Pal
{
	public class TestAssetbundleContentRead : MonoBehaviour{

        public string FullPath = @"E:\CYCProject\TestProject\CYCData\";
        public string ResName = "AT_Fantasy";
        public bool isLoad = false;
        void Update()
        {
            if(isLoad)
            {
                isLoad = false;
                StartLoad();
            }
        }
        [ContextMenu("Load")]
		public  void StartLoad () {
            StartCoroutine(ReadAssetbundle());
		}
         IEnumerator  ReadAssetbundle()
        {

            string fullPath = FullPath + ResName+".assetbundle"; 
            Debug.Log("fullPath:"+fullPath);
            WWW w = new WWW("file:///" + fullPath);
            while (!w.isDone)
            {
                if(w.error!=null)
                {
                    break;
                }
                yield return null;
            }
            if(w.error!=null)
            {
                Debug.LogError("load error:"+w.error);
            }
            else
            {
                if(!w.isDone)
                {
                    Debug.LogError("load not done:"+w.isDone);
                }
                else
                {
                    AssetBundle ab = w.assetBundle;
                    Object[] all = ab.LoadAll();
                    if (all != null)
                    {
                        Debug.Log("all.length:" + all.Length);
                        for (int i = 0, imax = all.Length; i < imax; ++i)
                        {
                            if(all[i]!=null)
                            {
                                Debug.Log(i + " :" + all[i].name+",type:"+all[i].GetType().FullName);
                            }
                        }
                    }
//                    ab.Unload(true);
                }

            }


        }
	}
}
