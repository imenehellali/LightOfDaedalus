using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LTwoGoalHitBehavior : MonoBehaviour
{
    public GameObject _brokenWall;

    private void Awake()
    {
        foreach (Rigidbody rb in _brokenWall.GetComponentsInChildren<Rigidbody>())
            rb.Sleep();
    }
    public void Trigger()
    {
        foreach (Rigidbody rb in _brokenWall.GetComponentsInChildren<Rigidbody>())
            rb.WakeUp();
    }

    //IEnumerator WaitForSomeSeconds()
    //{
    //    yield return new WaitForSecondsRealtime(1);
    //    foreach (Rigidbody obj in _brokenWall.GetComponentsInChildren<Rigidbody>())
    //        obj.gameObject.SetActive(false);
    //}
}
