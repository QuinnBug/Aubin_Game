using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct UnitIndex 
{
    public int index { get; private set; }
    public Enemy unit { get; private set; }

    public UnitIndex(Enemy _unit, int i)
    {
        unit = _unit;
        index = i;
    }
}

public class Enemy_Squad : MonoBehaviour
{
    private List<Enemy> units = new List<Enemy>();
    private List<UnitIndex> unitIndexes = new List<UnitIndex>();
    public NavigationStats navStats;
    public Navigation nav;

    public Vector3 offset = new Vector3(2, 1, 2);

    private int height;
    private int width;
    private int currentCount;

    public void Start()
    {
        nav = new Navigation(transform);
        Vector3 _pos = Vector3.zero;

        _pos = GameObject.FindGameObjectWithTag("TowerHeart").transform.position;
        _pos = Vector3.Lerp(_pos, transform.position, 0.1f);

        SetTarget(_pos);

        UpdateSize();
        SetIndexes();
    }

    private void SetIndexes()
    {
        for (int i = 0; i < units.Count; i++)
        {
            unitIndexes.Add(new UnitIndex(units[i], i));
        }
    }

    private void UpdateSize()
    {
        List<Vector3> positions = new List<Vector3>();
        int square = Mathf.CeilToInt(Mathf.Sqrt(units.Count));

        height = square;
        width = square;
        currentCount = square * square;
    }

    internal void RemoveUnit(Enemy unit)
    {
        units.Remove(unit);
    }

    public void Update()
    {
        if (units.Count == 0)
        {
            Destroy(gameObject);
        }

        SquadUpdate();
        Move();
        RotateTowardTarget();
    }

    public void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, nav.targetPosition,
            navStats.moveSpeed * Time.deltaTime);
    }

    private void RotateTowardTarget() 
    {
        Vector3 flatYTargetPos = nav.targetPosition;
        flatYTargetPos.y = transform.position.y;

        transform.rotation = Quaternion.LookRotation(flatYTargetPos - transform.position, transform.up);
    }

    public void SquadUpdate() 
    {

        List<Vector3> _positions = GetUnitTargetPositions();

        foreach (UnitIndex ui in unitIndexes)
        {
            ui.unit.nav.SetTarget(_positions[ui.index]);
        }

        /*
        for (int i = 0; i < units.Count; i++)
        {
            units[i].nav.SetTarget(_positions[i]);
        }
        */
    }

    public void AddUnit(Enemy unit) 
    {
        if (units.Contains(unit) == false)
        {
            units.Add(unit);
            if (unit.squad != null)
            {
                unit.squad.RemoveUnit(unit);
            }
            unit.squad = this;


            if (units.Count > currentCount)
            {
                UpdateSize();
            }
            SetIndexes();
        }
    }

    public List<Vector3> GetUnitTargetPositions() 
    {
        List<Vector3> positions = new List<Vector3>();

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                Vector3 p = new Vector3(offset.x * i, offset.y, offset.z * j);
                p -= new Vector3((offset.x * width)/2, 0, (offset.z * height)/2);
                float angle = Vector3.SignedAngle(Vector3.forward, transform.forward, Vector3.up);
                p = (Quaternion.Euler(0, angle, 0) * p) + transform.position;
                positions.Add(p);
            }
        }

        return positions;
    }

    public void SetTarget(Vector3 _pos)
    {
        navStats.target = _pos;
    }

    public void MergeSquads(Enemy_Squad _squad) 
    {
        foreach (Enemy unit in _squad.units)
        {
            AddUnit(unit);
        }

        Destroy(_squad.gameObject);
    }

    public void MergeSquads(Enemy_Squad[] squads) 
    {
        foreach (Enemy_Squad squad in squads)
        {
            MergeSquads(squad);
        }
    }
}
