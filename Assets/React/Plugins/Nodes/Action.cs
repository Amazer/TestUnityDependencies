using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace React
{
	[ReactNode]
	public class Action : LeafNode
	{
		public string actionMethod;
		Component component;
		System.Reflection.MethodInfo methodInfo;
		
		public override void Init (Reactor reactor)
		{
			var cm = reactor.FindMethod(actionMethod);
			if(cm == null) {
				Debug.LogError("Could not load action method: " + actionMethod);	
			} else {
				if(cm.methodInfo.ReturnType == typeof(IEnumerator<NodeResult>)) {
					component = cm.component;
					methodInfo = cm.methodInfo;
				} else {
					Debug.LogError("Action method has invalid signature: " + actionMethod);	
				}
			}
		}
		
		public override IEnumerator<NodeResult> NodeTask ()
		{
			if (component != null) {
				var task = (IEnumerator<NodeResult>)methodInfo.Invoke (component, null);
				while (task.MoveNext()) {
					if (task.Current == NodeResult.Continue) 
						yield return NodeResult.Continue;
					else
						yield return task.Current;
				}
			}
			yield return NodeResult.Failure;
		}
		
		public override string ToString ()
		{
			var actionMethodName = "";
			if (actionMethod != null && actionMethod.Length > 0) {
				actionMethodName = actionMethod.Substring (actionMethod.IndexOf ('.') + 1);
			}
			return string.Format ("! {0}", actionMethodName);
		}
		
		
#if UNITY_EDITOR
		public new static string GetHelpText ()
		{
			return "Runs an action from a MonoBehaviour, which has the method signature 'public IEnumerator<NodeResult> MethodName()'.";
		}
		
		public override void DrawInspector ()
		{
			base.DrawInspector ();
			if(editorReactable != null) {
				var methods = editorReactable.Actions;
				if(methods.Count > 0) {
					var idx = 0;
					if(methods.Contains(actionMethod)) 
						idx = methods.IndexOf(actionMethod);
					GUILayout.BeginHorizontal();
					GUILayout.Label("Action");
					idx = EditorGUILayout.Popup(idx, methods.ToArray(), "button");
					GUILayout.EndHorizontal();
					actionMethod = methods[idx];
					GUILayout.Space (8);
					GUILayout.Label(editorReactable.DocText(actionMethod), "textarea");
				}
			}
		}
		
		public override Color NodeColor ()
		{
			return Color.yellow;
		}
#endif
	}
	
	

}







