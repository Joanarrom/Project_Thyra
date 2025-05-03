using UnityEngine;

public class AlembicSwitcher : StateMachineBehaviour
{
   public string alembicNameToActivate;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Transform parent = animator.transform;

        foreach (Transform child in parent)
        {
            bool shouldActivate = child.name == alembicNameToActivate;
            child.gameObject.SetActive(shouldActivate);
        }
    }
}