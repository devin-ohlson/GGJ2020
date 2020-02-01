using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleTimed : Simple
{
    public float holdDuration = 2.5f;

    [SerializeField] private CircleFillAnimation animation;

    private void Start()
    {
        animation.gameObject.SetActive(false);
    }

    protected override IEnumerator StartRepairing()
    {
        Debug.Log("Made it!");
        animation.gameObject.SetActive(true);
        animation.SetDuration(holdDuration);
        animation.Listen(FinishRepairing);

        while (true)
        {
            animation.Activate(Input.GetAxisRaw("Interact") != 0);
            yield return null;
        }
    }

    protected bool FinishRepairing()
    {
        Repair();
        StopRepairing();
        return true;
    }

    protected override void StopRepairing()
    {
        StopCoroutine(StartRepairing());
        animation.Reset();
        animation.gameObject.SetActive(false);
    }
}
