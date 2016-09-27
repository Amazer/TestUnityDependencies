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
	public class Import : LeafNode
	{
		[ReactVar]
		public Reactable reactable;
		
		BaseNode importedChild;
		
		public override void Init (Reactor reactor)
		{
			if (reactable != null) {
				var root = (React.Root)React.JsonSerializer.Decode (reactable.json);
				root.PreProcess (gameObject, reactor);
				importedChild = root.children[0];
			}
		}
		
		public override IEnumerator<NodeResult> NodeTask ()
		{
			var task = importedChild.GetNodeTask();
			while(task.MoveNext()) {
				yield return task.Current;
			}
		}
		
		public override string ToString ()
		{
			return string.Format ("Import - {0}", (reactable == null ? "" : reactable.name));
		}
#if UNITY_EDITOR
        public new static string GetHelpText ()
		{
			return "Run a sub-tree from another Reactable asset. Allows modular and re-usable trees.";
		}
		
		public override void DrawInspector ()
		{
			base.DrawInspector ();
			if(reactable != null) {
				GUILayout.Space(10);
				if(GUILayout.Button("Edit")) {
					Selection.activeObject = reactable;				
					EditorApplication.ExecuteMenuItem("Assets/Edit Reactable");
				}
			}
		}
#endif
	}
	


}







