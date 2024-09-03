using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(FlyMonster))]
public class FlyMonsterEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        FlyMonster monster = target as FlyMonster;

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
