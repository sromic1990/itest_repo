using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class IdleToRunAndBack : MonoBehaviour
{
    Animator animator;
    [SerializeField]
    private Cycle Cycle;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        Cycle = Cycle.Idle;
        AnimateAccordingToCycle();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            switch (Cycle)
            {
                case Cycle.Idle:
                    Cycle = Cycle.Run;
                    break;

                case Cycle.Run:
                    Cycle = Cycle.Idle;
                    break;
            }

            AnimateAccordingToCycle();
        }
    }

    void AnimateAccordingToCycle()
    {
        switch (Cycle)
        {
            case Cycle.Run:
                animator.ResetTrigger("ideal");
                animator.SetTrigger("run");
                break;

            case Cycle.Idle:
                animator.ResetTrigger("run");
                animator.SetTrigger("ideal");
                break;
        }
    }
}

public enum Cycle
{
    Run,
    Idle
}
