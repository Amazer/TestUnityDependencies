using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class Reactor : MonoBehaviour
{
	
	public Reactable reactable;
	public float tickDuration = 0.1f;
	React.Root root;
	IEnumerator<React.NodeResult> task;
	Dictionary<string,ComponentMethod> methods = new Dictionary<string, ComponentMethod> ();
	[HideInInspector]
	public bool pause = false;
	[HideInInspector]
	public bool step = false;
	
	void Start ()
	{
		LoadMethods ();
		if (reactable != null) {
			root = (React.Root)React.JsonSerializer.Decode (reactable.json);
			root.PreProcess (gameObject, this);
			task = root.NodeTask ();
			StartCoroutine (RunReactable ());
		}
	}
	
	public ComponentMethod FindMethod(string name) {
		if(methods.ContainsKey(name)) return methods[name];
		return null;
	}
	
	void LoadMethods ()
	{
		var skip = new List<string> { "Component", "Transform", "MonoBehaviour", "Object" };
		foreach (var c in gameObject.GetComponents<Component> ()) {
			foreach (var i in c.GetType ().GetMethods ()) {
				if (skip.Contains (i.DeclaringType.Name))
					continue;
				if (i.IsPublic && i.GetParameters ().Length == 0) {
					methods[string.Format("{0}.{1}", i.DeclaringType.Name, i.Name)] = new ComponentMethod() { component = c, methodInfo = i };
				}
			}
		}
	}
	
#if UNITY_EDITOR	
	[ContextMenu("Add reactable components")]
	void AddComponents() {
		if(reactable != null) {
			foreach(var c in reactable.behaviours) {
				gameObject.AddComponent(c.GetClass());	
			}
		}
	}
#endif
	
	IEnumerator RunReactable ()
	{
		var delay = new WaitForSeconds (tickDuration);
		if (tickDuration <= 0)
			delay = null;
		while (true) {
#if UNITY_EDITOR
			
			if(step) {
				step = false;
				pause = true;
				yield return delay;
				task.MoveNext ();
			}

			if(pause) {
				yield return null;
				continue;
			} 
			
			yield return delay;
			task.MoveNext ();
		
#else
			yield return delay;
			task.MoveNext ();
#endif
		}
	}
	
	public React.Root GetRoot() {
		return root;	
	}
	
	
	
}
