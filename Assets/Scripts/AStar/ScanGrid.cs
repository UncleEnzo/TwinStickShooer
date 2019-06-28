using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScanGrid : MonoBehaviour
{
    public AstarPath AstarPath;
    // Start is called before the first frame update
    void Start()
    {
        AstarPath.active.Scan();
    }
}
