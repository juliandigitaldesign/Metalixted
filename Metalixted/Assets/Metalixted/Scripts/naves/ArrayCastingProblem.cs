using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrayCastingProblem : MonoBehaviour
{
    void Start()
    {
        // desired behaviour, the result of GetComponentsInChildren can be cast directly
        ArrayCastingProblem[] me = (ArrayCastingProblem[])GetComponentsInChildrenDesired(typeof(ArrayCastingProblem));
        Debug.Log(me[0].name);

        // current behaviour, casting the result of GetComponentsInChildren causes an exception
        me = (ArrayCastingProblem[])GetComponentsInChildrenCurrent(typeof(ArrayCastingProblem));
        Debug.Log(me[0].name);
    }

    // mockup of the current implementation of GetComponentsInChildren (obviously the real function does more than this one)
    // note that casting the result to a derived type causes an exception
    Component[] GetComponentsInChildrenCurrent(System.Type t)
    {
        Component[] returnArray = new Component[1];
        returnArray[0] = GetComponent(t);
        return returnArray;
    }

    // mockup of the desired implementation of GetComponentsInChildren (obviously the real function does more than this one)
    // note that casting the result to the derived type works as expected
    Component[] GetComponentsInChildrenDesired(System.Type t)
    {
        Component[] returnArray = (Component[])System.Array.CreateInstance(t, 1);
        returnArray[0] = GetComponent(t);
        return returnArray;
    }
}
