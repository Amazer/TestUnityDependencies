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
	public class Condition : LeafNode
	{
		
		public string conditionMethod;
		Component component;
		System.Reflection.MethodInfo methodInfo;
		
		public override void Init (Reactor reactor)
		{
			var cm = reactor.FindMethod(conditionMethod);
			if(cm == null) {
				Debug.LogError("Could not load condition method: " + conditionMethod);	
			} else {
				if(cm.methodInfo.ReturnType == typeof(bool)) {
					component = cm.component;
					methodInfo = cm.methodInfo;
				} else {
					Debug.LogError("Condition method has invalid signature: " + conditionMethod);	
				}
			}
		}
		
		public override IEnumerator<NodeResult> NodeTask ()
		{
			yield return NodeResult.Continue;
			if (component != null) {
				var result = (bool)methodInfo.Invoke (component, null);
				yield return result?NodeResult.Success:NodeResult.Failure;
			} else {
				Debug.LogError ("Could not find condition method: " + conditionMethod);
				yield return NodeResult.Failure;
			}
		}
		
		public override string ToString ()
		{
			var actionMethodName = "";
			if (conditionMethod != null && conditionMethod.Length > 0) {
				actionMethodName = conditionMethod.Substring (conditionMethod.IndexOf ('.') + 1);
			}
			return string.Format ("? {0}", actionMethodName);
		}

#if UNITY_EDITOR
        public new static string GetHelpText ()
		{
			return "Checks a condition on a MonoBehaviour, which has the method signature 'public bool ConditionName()'.";
		}
		
		public override void DrawInspector ()
		{
			base.DrawInspector ();
			if(editorReactable != null) {
				var methods = editorReactable.Conditions;
				if(methods.Count > 0) {
					var idx = 0;
					if(methods.Contains(conditionMethod)) 
						idx = methods.IndexOf(conditionMethod);
					GUILayout.BeginHorizontal();
					GUILayout.Label("Condition");
					idx = EditorGUILayout.Popup(idx, methods.ToArray(), "button");
					GUILayout.EndHorizontal();
					conditionMethod = methods[idx];
					GUILayout.Space (8);
					GUILayout.Label(editorReactable.DocText(conditionMethod), "textarea");
				}
			}
		}
		
		public override Color NodeColor ()
		{
			return Color.green;
		}
#endif
	}
	


}







