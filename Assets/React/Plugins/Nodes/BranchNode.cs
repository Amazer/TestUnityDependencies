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
	public class BranchNode : BaseNode
	{
		public List<BaseNode> children = new List<BaseNode> ();
		
		public override void Add (BaseNode child)
		{
			children.Add (child);
		}
		public override void Add (BaseNode child, BaseNode after)
		{
			var index = children.IndexOf(after);
			children.Insert(index+1, child);
		}
		
		public override void Insert (int index, BaseNode child)
		{
			children.Insert(index, child);
		}
		
		public override IEnumerable<BaseNode> Children ()
		{
			return children.ToArray();
		}
		
		public override bool HasChild (BaseNode child)
		{
			return children.Contains(child);
		}
		
		public override void Remove (BaseNode child)
		{
			children.Remove(child);
		}
		
		public override bool IsParent ()
		{
			return true;
		}
		
		public IEnumerable<BaseNode> ActiveChildren() {
			return (from i in children where i.enabled select i);
		}
		
		public override BaseNode FindParent (BaseNode child)
		{
			if(HasChild(child))
				return this;
			else {
				foreach(var c in children) {
					var p = c.FindParent(child);
					if(p != null) return p;
				}
			}
			return null;
		}

		public T Add<T> () where T : BaseNode, new()
		{
			var n = new T ();
			children.Add (n);
			return n;
		}
#if UNITY_EDITOR
		public override void DrawChildren ()
		{
			if(children.Count > 0) {
				Rect cell = new Rect(0,0,0,0);
				GUILayout.BeginVertical ();
				foreach (var i in children) {
					if(i != null) {
						cell = i.DrawEditorWidget ();
                        var A = new Vector2(rect.xMin+17, rect.yMax);
                        var B = new Vector2(rect.xMin+17, cell.yMin+10);
                        var C = new Vector2(cell.xMin, cell.yMin+10);
                        Handles.lighting = false;
                        Handles.DrawPolyLine(A,B,C);

					}
				}
				GUILayout.EndVertical ();
			}
		}
	
		public override Color NodeColor ()
		{
			return Color.cyan;
		}

#endif
		
	}


}







