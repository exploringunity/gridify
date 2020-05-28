using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

public class Gridify : EditorWindow
{
    FloatField spread;

    [MenuItem("Window/UIElements/Gridify %#g")]
    public static void OpenWindow()
    {
        Gridify wnd = GetWindow<Gridify>();
        var icon = (Texture2D)EditorGUIUtility.Load("Assets/Editor/GridifyIcon.png");
        wnd.titleContent = new GUIContent("Gridify", icon);
    }

    public void OnEnable()
    {
        var uxmlTemplate = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/Gridify.uxml");
        var ui = uxmlTemplate.CloneTree();
        rootVisualElement.Add(ui);
        spread = ui.Q<FloatField>("spread");
        var btn = ui.Q<Button>("gridifyBtn");
        btn.clicked += GridifySelected;
    }

    void GridifySelected()
    {
        var selected = Selection.gameObjects;
        var numObjs = selected.Length;
        var start = new Vector3(0, 0, 0);
        var sideLength = Mathf.CeilToInt(Mathf.Sqrt(numObjs));
        var transforms = selected.Select(x => x.transform).ToArray();
        Undo.RecordObjects(transforms, $"[Gridify] Moved {numObjs} objects");
        Debug.Log($"[Gridify] Spreading {numObjs} objects {spread.value} units.");
        for (var i = 0; i < numObjs; i++)
        {
            var xOffset = (i % sideLength) * spread.value;
            var zOffset = (i / sideLength) * spread.value;
            var pos = start;
            pos.x += xOffset;
            pos.z += zOffset;
            selected[i].transform.position = pos;
        }
    }
}
