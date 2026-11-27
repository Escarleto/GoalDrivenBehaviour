using UnityEngine;

public class GetPatient : GAction
{
    public override bool PrePeform()
    {
        target = GWorld.Instance.RemovePatient();
        if (target == null)
            return false;

        GWorld.Instance.GetWorld().ModifyState("Waiting", -1);
        return true;
    }

    public override bool PostPerform()
    {
        return true;
    }
}
