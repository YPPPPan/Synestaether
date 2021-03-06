﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum directions
{
    // red, w
    North,
    // green, d
    East,
    // blue, s
    South,
    // yellow, a
    West,
    Up,
    Down
}

public class Qubit : MonoBehaviour
{
    public bool editable;//!!!True means uneditable, False means editable
    //public Vector2 orientation; //we don't need this, can just use unity transform
    public Vector3 index;
    public List<availableFace> availableFaces;
    [HideInInspector]
    public Editor _editor;
    
    [HideInInspector]
    public float originalYRotation;

    // a general startup method for Qubits
    // You must add this MANUALLY to your startup method
    public void Initialize()
    {
        originalYRotation = 0f;
        availableFaces = GetComponents<availableFace>().ToList();
        _editor = GameObject.Find("Editor").GetComponent<Editor>();
    }

    public void Deselect()
    {
        for (int i = 0; i < this.transform.Find("availableFaces").childCount; i++)
        {
            this.transform.Find("availableFaces").GetChild(i).gameObject.GetComponent<availableFace>().DeselectColor();
        }
    }

    public void Rotate(int input, bool isDeserialize = false)
    {
        for (int i = 0; i < this.transform.childCount; i++)
        {
            Transform thisChild = this.transform.GetChild(i);
            if (thisChild.gameObject.tag != "no-rotate")
            {
                thisChild.transform.Rotate(new Vector3(0, 90 * input, 0));
            }
        }

        if (!isDeserialize)
        {
            _editor.UpdateTransformHandle();
        }
    }

    public void SetRotation(float angle, bool isDeserialize = false)
    {
        
        for (int i = 0; i < this.transform.childCount; i++)
        {
            Transform thisChild = this.transform.GetChild(i);
            if (thisChild.gameObject.tag != "no-rotate")
            {
                thisChild.transform.rotation = 
                    Quaternion.Euler(new Vector3(0, angle + originalYRotation, 0));
                    // originalYRotation lets us save rotation in prefab
            }
        }

        //if (!isDeserialize)
        //{
        //    _editor.UpdateTransformHandle();
        //}
    }

    public void Translate(directions direction)
    {
        Transform par = GameObject.Find("QBlocks").GetComponent<Transform>();
        switch (direction)
        {
            case directions.North:
                if (index.x > 0 && !_editor._grid[(int)index.x - 1, (int)index.y, (int)index.z])
                {
                    _editor._grid[(int)index.x-1, (int) index.y, (int) index.z] = _editor._grid[(int)index.x, (int)index.y, (int)index.z];
                    _editor._grid[(int)index.x, (int)index.y, (int)index.z] = null;
                    index.x -= 1;
                    transform.position = index * 10.0f;
                }
                break;
            case directions.East:
                if (index.z < FindObjectOfType<Editor>().GetComponent<Editor>().size-1 && !_editor._grid[(int)index.x, (int)index.y, (int)index.z+1])
                {
                    _editor._grid[(int)index.x, (int)index.y, (int)index.z+1] = _editor._grid[(int)index.x, (int)index.y, (int)index.z];
                    _editor._grid[(int)index.x, (int)index.y, (int)index.z] = null;
                    index.z += 1; 
                    transform.position = index * 10.0f;
                }
                break;
            case directions.South:
                if (index.x < FindObjectOfType<Editor>().GetComponent<Editor>().size-1 && !_editor._grid[(int)index.x + 1, (int)index.y, (int)index.z])
                {
                    _editor._grid[(int)index.x + 1, (int)index.y, (int)index.z] = _editor._grid[(int)index.x, (int)index.y, (int)index.z];
                    _editor._grid[(int)index.x, (int)index.y, (int)index.z] = null;
                    index.x += 1;
                    transform.position = index * 10.0f;
                }
                break;
            case directions.West:
                if (index.z > 0 && !_editor._grid[(int)index.x, (int)index.y, (int)index.z-1])
                {
                    _editor._grid[(int)index.x, (int)index.y, (int)index.z-1] = _editor._grid[(int)index.x, (int)index.y, (int)index.z];
                    _editor._grid[(int)index.x, (int)index.y, (int)index.z] = null;
                    index.z -= 1;
                    transform.position = index * 10.0f;
                }
                break;
            case directions.Up:
                if (index.y < FindObjectOfType<Editor>().GetComponent<Editor>().size-1 && !_editor._grid[(int)index.x, (int)index.y+1, (int)index.z])
                {
                    _editor._grid[(int)index.x, (int)index.y+1, (int)index.z] = _editor._grid[(int)index.x, (int)index.y, (int)index.z];
                    _editor._grid[(int)index.x, (int)index.y, (int)index.z] = null;
                    index.y += 1;
                    transform.position = index * 10.0f;
                }
                break;
            case directions.Down:
                if (index.y > 0 && !_editor._grid[(int)index.x, (int)index.y-1, (int)index.z])
                {
                    _editor._grid[(int)index.x, (int)index.y-1, (int)index.z] = _editor._grid[(int)index.x, (int)index.y, (int)index.z];
                    _editor._grid[(int)index.x, (int)index.y, (int)index.z] = null;
                    index.y -= 1;
                    transform.position = index * 10.0f;
                }
                break;
            default:
                Debug.LogWarning("Invalid Direction, no movement");
                return;
        }
        _editor.UpdateTransformHandle();
    }

    // Start is called before the first frame update
    public virtual void Start()
    {
        // Whatever you want to put here, put in 
        // Initialize(), bc inheritance
        Initialize();
    }

    public virtual void Update()
    {
    }

    public virtual void OnPlace()
    {
        // this method does nothing here
        // but is overriden in derived classes
        // so pls don't delete it thx
    }

    public void SetEditable(bool state)
    {
        editable = state;
    }
}
