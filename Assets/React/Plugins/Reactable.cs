using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
using System.Reflection;

[System.Serializable]
public class ReactableEditorOperation
{
    public string desc;
    public string json;
}

#endif


public class Reactable : ScriptableObject
{
	
	
    [HideInInspector]
    public string
        json;
	
#if UNITY_EDITOR
    public MonoScript[] behaviours = new MonoScript[0];
	
    [HideInInspector]
    public List<ReactableEditorOperation>
        history;
    [HideInInspector]
    public List<ReactableEditorOperation>
        future;
	
    public void RegisterUndo (string desc, React.Root root)
    {
        if (!(history.Count > 0 && history.Last ().json == json)) {
            history.Add (new ReactableEditorOperation () { desc = desc, json = React.JsonSerializer.Encode(root)});
            future.Clear ();
        }
    }
	
    public void PerformRedo ()
    {
        if (future.Count > 0) {
            var op = future.Last ();
            future.RemoveAt (future.Count - 1);
            history.Add (new ReactableEditorOperation () { desc = op.desc, json = json });
            json = op.json;
        }
    }
	
    public void PerformUndo ()
    {
        if (history.Count > 0) {
            var op = history.Last ();
            history.RemoveAt (history.Count - 1);
            future.Add (new ReactableEditorOperation () { desc = op.desc, json = json });
            json = op.json;
			
        }
    }
	
    public List<string> Actions {
        get {
            var actions = new List<string> ();
            foreach (var b in behaviours) {
                if (b == null)
                    continue;
                foreach (var i in b.GetClass().GetMethods().OrderBy((arg) => arg.Name)) {
                    if (i.IsPublic && i.ReturnType == typeof(IEnumerator<React.NodeResult>) && i.GetParameters ().Length == 0) {
                        actions.Add (string.Format ("{0}.{1}", i.DeclaringType.Name, i.Name));
                    }
                }
            }
            return actions;
        }
    }
    public List<string> Conditions {
        get {
            var actions = new List<string> ();
            foreach (var b in behaviours) {
                if (b == null)
                    continue;
                foreach (var i in b.GetClass().GetMethods().OrderBy((arg) => arg.Name)) {
                    if (i.IsPublic && i.ReturnType == typeof(bool) && i.GetParameters ().Length == 0) {
                        actions.Add (string.Format ("{0}.{1}", i.DeclaringType.Name, i.Name));
                    }
                }
            }
            return actions;
        }
    }
    public List<string> Functions {
        get {
            var actions = new List<string> ();
            foreach (var b in behaviours) {
                if (b == null)
                    continue;
                foreach (var i in b.GetClass().GetMethods().OrderBy((arg) => arg.Name)) {
                    if (i.IsPublic && i.GetParameters ().Length == 0) {
                        actions.Add (string.Format ("{0}.{1}", i.DeclaringType.Name, i.Name));
                    }
                }
            }
            return actions;
        }
    }
	
    public string DocText (string methodName)
    {
		
        var text = "";
        var parts = methodName.Split ('.');
        foreach (var b in behaviours) {
            if (b == null)
                continue;
            MethodInfo mi = null;
            if (b.name == parts [0]) {
                try {
                    mi = b.GetClass ().GetMethod (parts [1]);
                } catch (AmbiguousMatchException) {
                    mi = null;
                } 
                if (mi != null) {
                    foreach (React.ReactDocAttribute doc in mi.GetCustomAttributes(typeof(React.ReactDocAttribute), true)) {
                        text = text + "\r\n" + doc.docText + "\r\n";
                    }
                }

				

            }
        }
        return text;
	
    }
#endif
}
