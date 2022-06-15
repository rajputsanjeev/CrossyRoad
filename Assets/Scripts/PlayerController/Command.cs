using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Command
{
    public abstract void Execute(Animator animator);
}

public class PerformJump : Command
{
    public override void Execute(Animator animator)
    {
        animator.SetTrigger("IsJumping");
    }
}
