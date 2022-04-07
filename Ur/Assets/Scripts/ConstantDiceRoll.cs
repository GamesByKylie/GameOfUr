using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantDiceRoll : MonoBehaviour
{
    public UrDiceRoller roller;

    private Coroutine rollRoutine;

    private void Start()
    {
        rollRoutine = StartCoroutine(roller.RollVisualsOnly());
    }

    private void OnDisable()
    {
        StopCoroutine(rollRoutine);
    }
}
