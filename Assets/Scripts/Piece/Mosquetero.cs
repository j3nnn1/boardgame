﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mosquetero : Piece
{
    public override List<Vector2Int> MoveLocations(Vector2Int gridPoint)
    {
        List<Vector2Int> locations = new List<Vector2Int>();

        int forwardDirection = Manager.instance.currentPlayer.forward;
        Vector2Int forwardOne = new Vector2Int(gridPoint.x, gridPoint.y + forwardDirection);
        if (Manager.instance.PieceAtGrid(forwardOne) == false)
        {
            locations.Add(forwardOne);
        }

        Vector2Int forwardTwo = new Vector2Int(gridPoint.x, gridPoint.y + 2 * forwardDirection);
        if (Manager.instance.PieceAtGrid(forwardTwo) == false)
        {
            locations.Add(forwardTwo);
        }

        Vector2Int forwardRight = new Vector2Int(gridPoint.x + 1, gridPoint.y + forwardDirection);
        if (Manager.instance.PieceAtGrid(forwardRight))
        {
            locations.Add(forwardRight);
        }

        Vector2Int forwardLeft = new Vector2Int(gridPoint.x - 1, gridPoint.y + forwardDirection);
        if (Manager.instance.PieceAtGrid(forwardLeft))
        {
            locations.Add(forwardLeft);
        }

        return locations;
    }
}
