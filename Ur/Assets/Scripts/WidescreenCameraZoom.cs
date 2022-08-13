using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[RequireComponent(typeof(Camera))]
public class WidescreenCameraZoom : MonoBehaviour
{
    public Vector3 normalPos;
    public Quaternion normalRot;
    public Vector3 widescreenPos;
    public Quaternion widescreenRot;
    public float wideAspect = 2;

    private Camera cam;

    private void Awake()
    {
        cam = GetComponent<Camera>();
    }

	private void OnEnable()
	{
        MenuButtons.ResolutionChanged += CameraZoom;		
	}

	private void OnDisable()
	{
		MenuButtons.ResolutionChanged -= CameraZoom;
	}

	private void Start()
    {
        CameraZoom();        
    }

    private void CameraZoom()
    {
        StartCoroutine(DoCameraZoom());
    }

    private IEnumerator DoCameraZoom()
    {
        yield return null;
        
        float aspect = (Screen.width * 1.0f) / Screen.height;

        if (aspect >= wideAspect)
        {
            transform.position = widescreenPos;
            transform.rotation = widescreenRot;
        }
        else
        {
            transform.position = normalPos;
            transform.rotation = normalRot;
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(WidescreenCameraZoom))]
    private class Editor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button(new GUIContent("Set Normal Values")))
            {
                WidescreenCameraZoom target = this.target as WidescreenCameraZoom;
                target.normalPos = target.transform.position;
                target.normalRot = target.transform.rotation;
                EditorUtility.SetDirty(target);
            }
            if (GUILayout.Button(new GUIContent("Restore Normal Values")))
            {
                WidescreenCameraZoom target = this.target as WidescreenCameraZoom;
                target.transform.position = target.normalPos;
                target.transform.rotation = target.normalRot;
                EditorUtility.SetDirty(target);
            }

            if (GUILayout.Button(new GUIContent("Set Widescreen Values")))
            {
                WidescreenCameraZoom target = this.target as WidescreenCameraZoom;
                target.widescreenPos = target.transform.position;
                target.widescreenRot = target.transform.rotation;
                EditorUtility.SetDirty(target);
            }
            if (GUILayout.Button(new GUIContent("Restore Widescreen Values")))
            {
                WidescreenCameraZoom target = this.target as WidescreenCameraZoom;
                target.transform.position = target.widescreenPos;
                target.transform.rotation = target.widescreenRot;
                EditorUtility.SetDirty(target);
            }
        }
    }
#endif
}
