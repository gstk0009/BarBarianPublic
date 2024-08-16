using NavMeshPlus.Components;
using UnityEngine;

public class Baking : MonoBehaviour
{
    public static Baking instance;

    public NavMeshSurface[] surfaces;

    private void Awake()
    {
        if(instance == null)
            instance = this;

    }

    public void Bake()
    {
        for(int i=0;i<surfaces.Length; i++)
        {
            surfaces[i].BuildNavMesh();
        }    
    }
    public void ClearNavMeshes()
    {
        if(surfaces.Length > 0)
        {
            foreach (NavMeshSurface surface in surfaces)
            {
                surface.RemoveData();  // 각 NavMeshSurface의 데이터를 클리어합니다.
            }
        }
        
    }
}
