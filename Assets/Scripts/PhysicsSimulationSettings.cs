using UnityEditor;
using UnityEngine;

public class PhysicsSimulationSettings : MonoBehaviour
{
}

#if UNITY_EDITOR
[CustomEditor(typeof(PhysicsSimulationSettings))]
public class PhysicsSimulationSettingsEditor : Editor
{
    private PhysicsSimulationSettings self;

    private void OnEnable()
    {
        self = (PhysicsSimulationSettings)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.LabelField("Physics simulation runs on " + Physics.simulationMode);

        if (GUILayout.Button("Disable"))
            Physics.simulationMode = SimulationMode.Script;
        if (GUILayout.Button("FixedUpdate"))
            Physics.simulationMode = SimulationMode.FixedUpdate;
        if (GUILayout.Button("Update"))
            Physics.simulationMode = SimulationMode.Update;
    }
}
#endif