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
    [Serializable]
    public class BaseNode
    {
        

        public string
            notes = "";
        [ReactVar(-2)]
        public bool
            enabled = true;
        [NonSerialized]
        bool
            initialised = false;
        [NonSerialized]
        public GameObject
            gameObject;
        [NonSerialized]
        public bool
            running;
        [NonSerialized]
        public string
            state = "";
        
        public BaseNode ()
        {
            
        }


        public IEnumerator<NodeResult> GetNodeTask ()
        {

#if UNITY_EDITOR            
            return DebugNodeTask ();    
#else
            return NodeTask ();
#endif
        }
        
        public virtual void Abort ()
        {
            state = "ABORTED";
            foreach (var c in Children()) {
                c.Abort (); 
            }
        }
        
        IEnumerator<NodeResult> DebugNodeTask ()
        {
            var task = NodeTask ();
            while (true) {
                if (task.MoveNext ()) {
                    if (task.Current == NodeResult.Continue) {
                        running = true; 
                        state = "RUNNING";
                        yield return NodeResult.Continue;
                    } else {
                        state = task.Current == NodeResult.Success ? "SUCCESS" : "FAILURE";
                        yield return NodeResult.Continue;
                        running = false;
                        yield return task.Current;
                    }
                } else {
                    running = false;
                    state = "";
                    yield break;    
                }
            }
        }
        
        public virtual IEnumerator<NodeResult> NodeTask ()
        {
            yield break;
        }
        
        public virtual void Add (BaseNode child)
        {
            throw new NotImplementedException ();
        }
        
        public virtual void Add (BaseNode child, BaseNode after)
        {
            throw new NotImplementedException ();
        }
        
        public virtual void Insert (int index, BaseNode child)
        {
            throw new NotImplementedException ();
        }
        
        public virtual void Remove (BaseNode child)
        {
            throw new NotImplementedException ();
        }
        
        public virtual IEnumerable<BaseNode> Children ()
        {
            throw new NotImplementedException ();
        }
        
        public virtual bool HasChild (BaseNode child)
        {
            return false;
        }
        
        public virtual BaseNode FindParent (BaseNode child)
        {
            return null;
        }
        
        public virtual bool IsParent ()
        {
            return false;
        }
        
        public void PreProcess (GameObject g, Reactor r)
        {
            if (initialised)
                return;
            this.gameObject = g;
            initialised = true;
            Init (r);
            foreach (var c in Children())
                c.PreProcess (g, r);
            
        }
        
        public virtual void Init (Reactor r)
        {
            
        }
        

#if UNITY_EDITOR
        
        public static BaseNode hotNode;
        public static BaseNode hoverNode;
        public static Reactable editorReactable;
        public bool activateContext = false;
        public bool hideChildren = false;
        
        [System.NonSerialized]
        public Rect
            rect;
        
        public static string GetHelpText ()
        {
            return "Help goes here.";   
        }
        
        public virtual void DrawInspector ()
        {
            var T = this.GetType ();
            var back = GUI.backgroundColor;
            GUI.backgroundColor = Color.clear;
            GUILayout.Label ("Node Type: " + T.Name, "button");
            GUILayout.Space (5);
            GUILayout.Label (GetHelpText (), "textarea");
            GUILayout.Space (10);
            GUI.backgroundColor = back;
            var fields = (from i in T.GetFields () where i.GetCustomAttributes (typeof(ReactVarAttribute), true).Length > 0 select i);
            GUILayout.BeginHorizontal ();
            GUILayout.Label ("Notes", GUILayout.Width (64));


            notes = EditorGUILayout.TextArea (notes, GUILayout.MaxWidth(256));
        
            GUILayout.EndHorizontal ();
            fields = (from i in fields orderby ((ReactVarAttribute)i.GetCustomAttributes (typeof(ReactVarAttribute), true) [0]).priority select i);
            foreach (var f in fields) {
                GUILayout.Space (5);
                GUILayout.BeginHorizontal ();
                GUILayout.Label (NiceName (f.Name));
                if(f.Name == "notes") {
                    f.SetValue(this, EditorGUILayout.TextArea((string)f.GetValue(this)));
                } else if (f.FieldType == typeof(float)) {
                    f.SetValue (this, EditorGUILayout.FloatField ((float)f.GetValue (this), GUILayout.Width (192)));
                } else if (f.FieldType == typeof(int)) {
                    f.SetValue (this, EditorGUILayout.IntField ((int)f.GetValue (this)));
                } else if (f.FieldType == typeof(string)) {
                    f.SetValue (this, EditorGUILayout.TextField ((string)f.GetValue (this)));
                } else if (f.FieldType == typeof(bool)) {
                    f.SetValue (this, EditorGUILayout.Toggle ((bool)f.GetValue (this)));
                } else if (f.FieldType == typeof(Reactable)) {
                    f.SetValue (this, EditorGUILayout.ObjectField ((Reactable)f.GetValue (this), typeof(Reactable), false));
                } else if (f.FieldType == typeof(ParallelPolicy)) {
                    f.SetValue (this, (ParallelPolicy)EditorGUILayout.EnumPopup ((ParallelPolicy)f.GetValue (this)));   
                } else if (f.FieldType == typeof(MutationPolicy)) {
                    f.SetValue (this, (MutationPolicy)EditorGUILayout.EnumPopup ((MutationPolicy)f.GetValue (this)));  
                } else {
                    DrawSpecialValueWidget (f);         
                }
                GUILayout.EndHorizontal ();
                GUILayout.Space (4);
            }
            
        }
        
        public virtual void DrawSpecialValueWidget (System.Reflection.FieldInfo field)
        {
            
        }
        
        public virtual Rect DrawEditorWidget ()
        {

            GUILayout.BeginHorizontal (GUILayout.Height (24));
            if (Children ().Count () > 0) {
                //hideChildren = GUILayout.Toggle (hideChildren, hideChildren ? "<" : ">", "button", GUILayout.Width (22));

            }
            DrawGUI ();
            if (Children ().Count () == 0) {


            }
            
            GUILayout.FlexibleSpace ();
            GUILayout.EndHorizontal ();
            GUILayout.BeginHorizontal ();
            GUILayout.Space (64);
            if (!hideChildren) {
                DrawChildren ();
            }
            GUILayout.EndHorizontal ();
            return rect;
            
        }
        
        public virtual void DrawChildren ()
        {
        }
        
        
        public virtual void DrawGUI ()
        {
            
            var back = GUI.backgroundColor;
            
            if (hotNode != null && hotNode == this) {
                GUI.backgroundColor = Color.red;
            } else {
                GUI.backgroundColor = NodeColor ();
            }
            if (!enabled) {
                GUI.backgroundColor = Color.black;  
            }
            
            var guiNote = notes == null ? "" : notes;
            GUILayout.BeginHorizontal ();
            GUILayout.Label (NiceName (ToString ()), "button", GUILayout.MinWidth (128));

            rect = GUILayoutUtility.GetLastRect ();
            if (running) {
                GUILayout.Label ("[ " + state + " ]");
            }
            GUILayout.Label (guiNote);
            GUILayout.EndHorizontal ();
            
            GUI.backgroundColor = back;

            activateContext = false;
            if (rect.Contains (Event.current.mousePosition)) {
                if (Event.current.type == EventType.MouseDown) {
                    GUI.FocusControl (null);
                    if (BaseNode.hotNode == this && Event.current.button == 1) {
                        activateContext = true;
                    }
                    hotNode = this;
                    Event.current.Use ();
                }
                if (Event.current.type == EventType.MouseDrag) {
                    if (BaseNode.hotNode.GetType () != typeof(Root)) {
                        DragAndDrop.PrepareStartDrag ();
                        DragAndDrop.paths = new string[] { "/p" };
                        DragAndDrop.StartDrag (BaseNode.hotNode.ToString ());
                    }
                }
                
                if (Event.current.type == EventType.DragUpdated || Event.current.type == EventType.DragPerform) {
                    hoverNode = this;
                }
                
            }
            
        }
        
        public virtual Color NodeColor ()
        {
            return Color.white; 
        }
        
        public string NiceName (string s)
        {
            var n = "";
            for (int i = 0; i < s.Length; i++) {
                if (n.Length > 0 && char.IsUpper (s [i]) && (!char.IsUpper (n.Last ()))) {
                    n += " ";                    
                }
                n += (i == 0 ? s [i].ToString ().ToUpper () : s [i].ToString ());
            }
            return n;
        }
            
        
#endif
    }

}







