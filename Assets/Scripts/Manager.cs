using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//not need to use "using" when the file with the class is in the same directory

public class Manager : MonoBehaviour
{
    public Board board; //the game object need to have the class asociated
    private GameObject[,] pieces;
    //singleton
    public static Manager instance = null;
    
    //players
    private Player blue;
    private Player green;
    
    //player state
    public Player currentPlayer;
    public Player otherPlayer;

    void Start()
    {
        pieces = new GameObject[5, 8];
        green = new Player("green", true);
        blue = new Player("blue", false);
        currentPlayer = green;
        otherPlayer = blue;
    }
    // como saber que gameobjects tiene X script asociado????

    // sugar sintax sigleton????
    //void Awake()
    //{
    //    instance = this;
    //}

    public static Manager Instance {
        get {
            if (instance==null) {
                instance = new Manager();
            }
            return instance;
        }
    }
    public void SelectPieceAtGrid(Vector2Int gridPoint)
    {
        GameObject selectedPiece = pieces[gridPoint.x, gridPoint.y];
        if (selectedPiece)
        {
            board.SelectPiece(selectedPiece);
        }
    }
    public GameObject PieceAtGrid(Vector2Int gridPoint)
    {
        Debug.Log(gridPoint);
        if (gridPoint.x > 7 || gridPoint.y > 7 || gridPoint.x < 0 || gridPoint.y < 0)
        {
            return null;
        }
        return pieces[gridPoint.x, gridPoint.y];
    }
    public bool DoesPieceBelongToCurrentPlayer(GameObject piece)
    {
        return currentPlayer.pieces.Contains(piece);
    }
    public void SelectPiece(GameObject piece)
    {
        board.SelectPiece(piece);
    }

    public void CapturePieceAt(Vector2Int gridPoint)
    {
        GameObject pieceToCapture = PieceAtGrid(gridPoint);
        //if (pieceToCapture.GetComponent<Piece>().type == PieceType.King)
        //{
        //    Debug.Log(currentPlayer.name + " wins!");
        //   Destroy(board.GetComponent<TileSelector>());
        //  Destroy(board.GetComponent<MoveSelector>());
        //}
        currentPlayer.capturedPieces.Add(pieceToCapture);
        pieces[gridPoint.x, gridPoint.y] = null;
        Destroy(pieceToCapture);
    }

    public void Move(GameObject piece, Vector2Int gridPoint)
    {
        Debug.Log(piece);
        Debug.Log(gridPoint);
        //Piece pieceComponent = piece.GetComponent<Piece>();
        //if (pieceComponent.type == PieceType.Pawn && !HasPawnMoved(piece))
        //{
        //    movedPawns.Add(piece);
        //}

        //Vector2Int startGridPoint = GridForPiece(piece);
        //pieces[startGridPoint.x, startGridPoint.y] = null;
        //pieces[gridPoint.x, gridPoint.y] = piece;
        //board.MovePiece(piece, gridPoint);
    }
    public List<Vector2Int> MovesForPiece(GameObject pieceObject)
    {
        Piece piece = pieceObject.GetComponent<Piece>();
        Vector2Int gridPoint = GridForPiece(pieceObject);
        List<Vector2Int> locations = piece.MoveLocations(gridPoint);

        // filter out offboard locations
        locations.RemoveAll(gp => gp.x < 0 || gp.x > 7 || gp.y < 0 || gp.y > 7);

        // filter out locations with friendly piece
        locations.RemoveAll(gp => FriendlyPieceAt(gp));

        return locations;
    }
    public Vector2Int GridForPiece(GameObject piece)
    {
        for (int i = 0; i < 5; i++) //@TODO change to a variable
        {
            for (int j = 0; j < 8; j++)
            {
                if (pieces[i, j] == piece)
                {
                    return new Vector2Int(i, j);
                }
            }
        }

        return new Vector2Int(-1, -1);
    }
    public bool FriendlyPieceAt(Vector2Int gridPoint)
    {
        GameObject piece = PieceAtGrid(gridPoint);

        if (piece == null)
        {
            return false;
        }

        if (otherPlayer.pieces.Contains(piece))
        {
            return false;
        }

        return true;
    }
    public void NextPlayer()
    {
        Player tempPlayer = currentPlayer;
        currentPlayer = otherPlayer;
        otherPlayer = tempPlayer;
    }
    public void DeselectPiece(GameObject piece)
    {
        board.DeselectPiece(piece);
    }

}
