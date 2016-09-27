using UnityEngine;
using System;
using System.Collections;
namespace Pal
{
	public class TestResourceLoader{
        public static void LoadAssets(MonoBehaviour mb, string fullPath,string assetName,Action<UnityEngine.Object> cb)
        {
            new TestLoader().LoadAssets(mb,fullPath, assetName, cb);

        }
	}
    public class TestLoader
    {
        public void LoadAssets(MonoBehaviour mb,string fullPath,string assetName,Action<UnityEngine.Object> cb)
        {
            
            mb.StartCoroutine(this.ReadAssetbundle(fullPath, assetName, cb));

        }
         IEnumerator  ReadAssetbundle(string fullPath,string resName,Action<UnityEngine.Object> cb)
        {

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
                    if(ab.mainAsset!=null)
                    {
                        Debug.Log(" mainAsset:" + ab.mainAsset.name+",type:"+ab.mainAsset.GetType().FullName);
                    }
                    UnityEngine.Object[] all = ab.LoadAll();
                    if (all != null)
                    {
                        Debug.Log("all.length:" + all.Length);
                        for (int i = 0, imax = all.Length; i < imax; ++i)
                        {
                            if(all[i]!=null)
                            {
                                if(all[i].GetType()==typeof(GameObject)&& all[i].name==resName)
                                {
                                    if(cb!=null)
                                    {
                                        cb(all[i]);
                                        break;
                                    }
                                }
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
