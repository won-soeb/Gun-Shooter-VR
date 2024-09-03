using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(Skeleton))]
public class MonsterEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Skeleton monster = target as Skeleton;

        if (GUILayout.Button("Attack"))
        {
            monster.Attack(true);
        }
        if (GUILayout.Button("Damage"))
        {
            monster.Damage(1);
        }
        if (GUILayout.Button("Die"))
        {
            monster.Die();
        }
    }
}
