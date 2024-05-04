using System;
using UnityEngine;

public class Floor : MonoBehaviour
{
    public event Action<Cube> Collision;
    
    private void OnCollisionEnter(Collision other)
    {
        Cube cube = other.gameObject.GetComponent<Cube>();

        if (cube != null)
            Collision.Invoke(cube);
    }
}
