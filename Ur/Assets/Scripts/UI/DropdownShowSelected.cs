using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

[RequireComponent(typeof(Dropdown))]
public class DropdownShowSelected : MonoBehaviour
{
    private Dropdown d;

    private void Awake()
    {
        d = GetComponent<Dropdown>();
    }

    public void ScrollDropdownToValue()
    {
        StartCoroutine(DoScrollDropdown());
    }

    private IEnumerator DoScrollDropdown()
    {
        yield return null;

        Scrollbar dropdownScroll = GetComponentsInChildren<Scrollbar>().FirstOrDefault(x => x.gameObject.activeSelf);

        if (d.options.Count >= 3)
        {
            if (d.value < 2)
            {
                dropdownScroll.value = 1f;
            }
            else if (d.options.Count - d.value < 3)
            {
                dropdownScroll.value = 0f;
            }
            else
            {
                dropdownScroll.value = Mathf.Clamp(1 - ((float)(d.value + 1) / d.options.Count), 0f, 1f);
            }
        }
    }
}
