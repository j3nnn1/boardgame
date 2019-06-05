using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSelector : MonoBehaviour
{
    public GameObject moveLocationPrefab;
    public GameObject tileHighlightPrefab;
    public GameObject attackLocationPrefab;

    private GameObject tileHighlight;
    private GameObject movingPiece;
    private List<Vector2Int> moveLocations;
    private List<GameObject> locationHighlights;

    void Start()
    {
        this.enabled = false;
        tileHighlight = Instantiate(tileHighlightPrefab, Geometry.PointFromGrid(new Vector2Int(0, 0)),
            Quaternion.identity, gameObject.transform);
        tileHighlight.SetActive(false);
    }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Vector3 point = hit.point;
            Vector2Int gridPoint = Geometry.GridFromPoint(point);

            tileHighlight.SetActive(true);
            tileHighlight.transform.position = Geometry.PointFromGrid(gridPoint);
            if (Input.GetMouseButtonDown(0))
            {
                // Reference Point 2: check for valid move location
                if (!moveLocations.Contains(gridPoint))
                {
                    return;
                }

                if (Manager.instance.PieceAtGrid(gridPoint) == null)
                {
                    Manager.instance.Move(movingPiece, gridPoint);
                }
                else
                {
                    Manager.instance.CapturePieceAt(gridPoint);
                    Manager.instance.Move(movingPiece, gridPoint);
                }
                // Reference Point 3: capture enemy piece here later
                ExitState();
            }
        }
        else
        {
            tileHighlight.SetActive(false);
        }
    }
    public void EnterState(GameObject piece)
    {
        movingPiece = piece;
        this.enabled = true;

        moveLocations = Manager.instance.MovesForPiece(movingPiece);
        locationHighlights = new List<GameObject>();

        //if (moveLocations.Count == 0)
        //{
           // CancelMove();
        //}

        foreach (Vector2Int loc in moveLocations)
        {
            GameObject highlight;
            if (Manager.instance.PieceAtGrid(loc))
            {
                highlight = Instantiate(attackLocationPrefab, Geometry.PointFromGrid(loc), Quaternion.identity, gameObject.transform);
            }
            else
            {
                highlight = Instantiate(moveLocationPrefab, Geometry.PointFromGrid(loc), Quaternion.identity, gameObject.transform);
            }
            locationHighlights.Add(highlight);
        }
    }

    private void ExitState()
    {
        this.enabled = false;
        TileSelector selector = GetComponent<TileSelector>();
        tileHighlight.SetActive(false);
        Manager.instance.DeselectPiece(movingPiece);
        movingPiece = null;
        Manager.instance.NextPlayer();
        selector.EnterState();
        foreach (GameObject highlight in locationHighlights)
        {
            Destroy(highlight);
        }
    }
}
