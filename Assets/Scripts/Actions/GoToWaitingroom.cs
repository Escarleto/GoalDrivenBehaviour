using UnityEngine;

public class GoToWaitingroom : GAction
{
    public override bool PrePeform()
    {
        return true;
    }

    public override bool PostPerform()
    {
        GWorld.Instance.GetWorld().ModifyState("Waiting", 1);
        GWorld.Instance.AddPatient(gameObject);
        return true;
    }
}
