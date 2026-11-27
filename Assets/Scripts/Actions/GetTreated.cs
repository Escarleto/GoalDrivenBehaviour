using System.ComponentModel;
using UnityEngine;

public class GetTreated : GAction {

    GameObject resource;

    public override bool IsAchievable() {

        return inventory.FindItemWithTag("Cubicle") != null;
    }

    public override bool PrePerform() {

        target = inventory.FindItemWithTag("Cubicle");
        if (target == null) return false;

        GWorld.Instance.GetWorld().ModifyState("TreatingPatient", 1);
        return true;
    }

    public override bool PostPerform() {

        WorldStates world = GWorld.Instance.GetWorld();
        world.ModifyState("TreatingPatient", -1);
        world.ModifyState("Treated", 1);
        GWorld.Instance.AddCubicle(target);
        world.ModifyState("FreeCubicle", 1);
        beliefs.ModifyState("isTreated", 1);
        inventory.RemoveItem(target);
        return true;
    }

}