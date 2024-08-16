using NavMeshPlus.Components;
using System.Collections;
using UnityEngine;

public class SurfaceLayerChanger : MonoBehaviour
{
    public NavMeshSurface navMeshSurface;
    public LayerMask layerMask;

    private void Start()
    {
        if (GameManager.Instance.SurfaceLayerChanger != null) return;

        navMeshSurface = GetComponent<NavMeshSurface>();
        GameManager.Instance.SurfaceLayerChanger = this;
    }
   
    public void SetSurfaceLayer(LayerType layerType)
    {
        if (navMeshSurface != null)
        {
            navMeshSurface.layerMask = (1 << (int)layerType);
            navMeshSurface.BuildNavMesh();
            Baking.instance.Bake();

            navMeshSurface.layerMask = 0; // 베이크가 완료된 후 Include layers를 초기화
        }
        else
        {
            navMeshSurface = GetComponent<NavMeshSurface>();
            SetSurfaceLayer(layerType);
        }
    }
    public IEnumerator UpdateNavMeshForStage(LayerType layerType)
    {
        yield return new WaitUntil(() => GameManager.Instance.SurfaceLayerChanger != null);

        if (GameManager.Instance.SurfaceLayerChanger != null)
            GameManager.Instance.SurfaceLayerChanger.SetSurfaceLayer(layerType);
        
        yield return null;
    }

}