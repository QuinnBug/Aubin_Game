using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSelection : Singleton
{
    public List<ISelectable> selecteds = new List<ISelectable>();

    Vector3 squareStart;
    Vector3 squareEnd;
    bool selecting = false;

    public Transform drawingSquare;

    private void Update()
    {
        selecting = Input.GetMouseButton(1);

        if (Input.GetMouseButtonDown(1))
        {
            squareStart = MousePos();
            squareEnd = Vector3.negativeInfinity;
        }
        else if (Input.GetMouseButtonUp(1))
        {
            squareEnd = MousePos();
            CheckSelect(squareStart, squareEnd);
            squareStart = squareEnd = Vector3.negativeInfinity;
        }

        if (selecting)
        {
            drawingSquare.gameObject.SetActive(true);
            DrawSquare(squareStart, MousePos());
        }
        else
        {
            drawingSquare.gameObject.SetActive(false);
        }
    }

    private void DrawSquare(Vector3 startPos, Vector3 endPos)
    {
        Vector3 center = (startPos + endPos) / 2;
        center.y = 3;
        Vector3 extents = new Vector3(Mathf.Abs(startPos.x - endPos.x), Mathf.Abs(startPos.z - endPos.z), 1);

        drawingSquare.position = center;
        drawingSquare.localScale = extents;
    }

    Vector3 MousePos() 
    {
        Vector3 raycastHitLocation = Vector3.zero;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        LayerMask mask = 1 << LayerMask.NameToLayer("Floor");

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100, mask))
        {
            raycastHitLocation = hit.point;
        }

        return raycastHitLocation;
    }

    Vector3 lastCenter;
    Vector3 lastExtents;

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawCube(lastCenter, lastExtents*2);
    }

    void CheckSelect(Vector3 startPos, Vector3 endPos) 
    {
        Vector3 center = (startPos + endPos) / 2;
        Vector3 extents = new Vector3(Mathf.Abs(startPos.x - endPos.x)/2, 3, Mathf.Abs(startPos.z - endPos.z)/2);

        lastCenter = center;
        lastExtents = extents;

        Collider[] hits = Physics.OverlapBox(center, extents, Quaternion.Euler(0,0,0));

        ISelectable slt;

        Debug.Log("checking");
        foreach (Collider hit in hits)
        {
            Debug.Log("hit");
            if (hit.TryGetComponent(out slt))
            {
                selecteds.Add(slt);
                slt.OnSelected();
            }
        }
    }
}
