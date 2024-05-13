using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu]
public class AdvancedRuleTile : RuleTile<AdvancedRuleTile.Neighbor> {
    public bool checkself;
    public bool alwaysConnect;
    public TileBase[] tilesToConnect;

    public Sprite[] Walls;

    public class Neighbor : TilingRuleOutput.Neighbor
    {
        public const int Any = 3;
        public const int Specified = 4;
        public const int Empty = 5;
    }

    public override bool RuleMatch(int neighbor, TileBase tile) {
        switch (neighbor) {
            case Neighbor.This: return CheckForThis(tile);
            case Neighbor.Any: return CheckForAny(tile);
            case Neighbor.Specified: return CheckForSpecified(tile);
            case Neighbor.Empty: return CheckForEmpty(tile);
        }
        return base.RuleMatch(neighbor, tile);
    }

    private bool CheckForThis(TileBase tile)
    {
        if (!alwaysConnect) return tile == this;
        else return tilesToConnect.Contains(tile) || tile == this;
    }

    private bool CheckForAny(TileBase tile)
    {
        if(checkself) return tile != null;
        return tile != null && tile != this;
    }

    private bool CheckForSpecified(TileBase tile)
    {
        return tilesToConnect.Contains(tile);
    }

    private bool CheckForEmpty(TileBase tile)
    {
        return tile == null;
    }
}