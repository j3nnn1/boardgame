using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class Piece : MonoBehaviour
{
    ///public PieceType type;
    public abstract List<Vector2Int> MoveLocations(Vector2Int gridPoint);
}