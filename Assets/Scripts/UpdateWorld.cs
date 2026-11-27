using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UpdateWorld : MonoBehaviour {

    public Text states;

    private void Start() {

        Time.timeScale = 1.0f;
    }

    private void LateUpdate() {

        if (states == null) {

            Debug.LogError("UpdateWorld requires a Text component reference.", this);
            enabled = false;
            return;
        }

        Dictionary<string, int> worldStates = GWorld.Instance.GetWorld().GetStates();
        SortedDictionary<string, int> displayedStates = new SortedDictionary<string, int>(worldStates);
        string[] counterNames = { "FreeCubicle", "Treated", "TreatingPatient", "Waiting" };

        foreach (string key in counterNames) {

            if (!displayedStates.ContainsKey(key)) {

                displayedStates.Add(key, 0);
            }
        }

        states.text = string.Join("\n", displayedStates.Select(state => $"{state.Key}: {state.Value}"));
    }
}