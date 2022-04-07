using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class Utils
{
	public static float GetRange(float Xinput, float Xmax, float Xmin, float Ymax, float Ymin) {

		return (((Xinput - Xmin) / (Xmax - Xmin)) * (Ymax - Ymin)) + Ymin;

	}
#if UNITY_EDITOR
	
	public static void drawString(string text, Vector3 worldPos, Color? colour = null) {
		UnityEditor.Handles.BeginGUI();

		var restoreColor = GUI.color;

		if (colour.HasValue) GUI.color = colour.Value;
		var view = UnityEditor.SceneView.currentDrawingSceneView;
		Vector3 screenPos = view.camera.WorldToScreenPoint(worldPos);

		if (screenPos.y < 0 || screenPos.y > Screen.height || screenPos.x < 0 || screenPos.x > Screen.width || screenPos.z < 0)
            {
			GUI.color = restoreColor;
			UnityEditor.Handles.EndGUI();
			return;
		}

		Vector2 size = GUI.skin.label.CalcSize(new GUIContent(text));
		GUI.Label(new Rect(screenPos.x - (size.x / 2), -screenPos.y + view.position.height + 4, size.x, size.y), text);
		GUI.color = restoreColor;
		UnityEditor.Handles.EndGUI();
	}
#endif

	public static T WeightedRandomElement<T>(this IEnumerable<T> self, IEnumerable<float> weights) {
		var totalWeight = weights.Sum();
		var random = UnityEngine.Random.Range(0f, 1f) * totalWeight;
		var currWeight = 0f;
		for (var i = 0; i < weights.Count(); i++) {
			currWeight += weights.ElementAt(i);
			if (random < currWeight) {
				return self.ElementAt(i);
			}
		}

		return default(T);
	}

	public static Vector2 XZ(this Vector3 self) => new Vector2(self.x, self.z);
	public static Vector2 Reverse(this Vector2 self) => new Vector2(self.y, self.x);

	public static Vector2 With(this Vector2 self, float? x = null, float? y = null) =>
		new Vector2(x ?? self.x, y ?? self.y);

	public static Vector3 With(this Vector3 self, float? x = null, float? y = null, float? z = null) =>
		new Vector3(x ?? self.x, y ?? self.y, z ?? self.z);

	public static Color With(this Color self, float? r = null, float? g = null, float? b = null, float? a = null) =>
		new Color(r ?? self.r, g ?? self.g, b ?? self.b, a ?? self.a);

	public static T RandomElement<T>(this IEnumerable<T> list) {
		return list.ElementAtOrDefault(UnityEngine.Random.Range(0, list.Count()));
	}

	public static IEnumerable<Transform> GetChildren(this Transform self) {
		var result = new List<Transform>();
		for (var i = 0; i < self.childCount; i++) {
			result.Add(self.GetChild(i));
		}
		return result;
	}

	// Based on: https://stackoverflow.com/questions/11883469/takewhile-but-get-the-element-that-stopped-it-also
	public static IEnumerable<T> TakeUntil<T>(this IEnumerable<T> data, Func<T, bool> predicate) {
		foreach (var item in data) {
			yield return item;
			if (predicate(item))
				break;
		}
	}

	public static bool FastApproximately(float a, float b, float threshold) {
		return ((a < b) ? (b - a) : (a - b)) <= threshold;
	}
}

#region [ReadOnly] Attribute

/// <summary>
/// Place [ReadOnly] attribute on fields that should be shown in the inspector for debug purposes but should not be editable.
/// </summary>
[AttributeUsage(AttributeTargets.Field, Inherited = true)]
public class ReadOnlyAttribute : PropertyAttribute { }
#if UNITY_EDITOR
[UnityEditor.CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
public class ReadOnlyAttributeDrawer : UnityEditor.PropertyDrawer
{
	public override void OnGUI(Rect rect, UnityEditor.SerializedProperty prop, GUIContent label) {
		bool wasEnabled = GUI.enabled;
		GUI.enabled = false;
		UnityEditor.EditorGUI.PropertyField(rect, prop);
		GUI.enabled = wasEnabled;
	}
}
#endif

#endregion
