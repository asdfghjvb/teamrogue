using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class NavMeshBaker : MonoBehaviour
{
    [SerializeField] NavMeshSurface surface;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void rebakeNavMesh()
    {
        surface.BuildNavMesh();
    }
}
