using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandGenerateTest : MonoBehaviour
{
    [SerializeField] private Transform cube0;
    [SerializeField] private Transform cube1;
    [SerializeField] private Transform cube2;
    [SerializeField] private Transform cube3;


    private void Update()
    {
        Vector2 fDir = new Vector2(cube1.position.x, cube1.position.z) - new Vector2(cube0.position.x, cube0.position.z);
        Vector2 sDir = new Vector2(cube3.position.x, cube3.position.z) - new Vector2(cube2.position.x, cube2.position.z);

        float angle = Vector2.SignedAngle(fDir, sDir);

        Debug.Log(angle);
    }
}
