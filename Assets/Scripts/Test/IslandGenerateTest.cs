using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandGenerateTest : MonoBehaviour
{
    [SerializeField] private int x;
    [SerializeField] private int y;
 
    private void Update()
    {
        Mathf.PerlinNoise(x, y);
    }
}
