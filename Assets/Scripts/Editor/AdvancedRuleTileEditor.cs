using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AdvancedRuleTile))]
[CanEditMultipleObjects]
public class AdvancedRuleTileEditor : RuleTileEditor
{
    public Texture2D Any;
    public Texture2D Specified;
    public Texture2D Empty;

    public override void RuleOnGUI(Rect rect, Vector3Int position, int neighbor)
    {
        switch (neighbor)
        {
            case 3:
                GUI.DrawTexture(rect, Any);
                return;
            case 4:
                GUI.DrawTexture(rect, Specified);
                return;
            case 5:
                GUI.DrawTexture(rect, Empty);
                return;
        }

        base.RuleOnGUI(rect, position, neighbor);
    }
}
