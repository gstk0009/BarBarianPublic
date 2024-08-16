using UnityEngine;

public class SummonBoss : MonoBehaviour
{
    [SerializeField] GameObject bossPrefab;
    [SerializeField] GameObject grave;
    [SerializeField] GameObject door;

    private bool isSummon = false;

    // Start is called before the first frame update
    void Start()
    {
        //Summon(); //테스트용
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if ( collision.gameObject.layer == (int)LayerType.Player && collision != null && !isSummon)
        {
            Summon();
        }
    }


    void Summon()
    {
        isSummon = true;
        if(door != null)
        {
            door.SetActive(true);

        }

        if(grave != null)
        {
            Instantiate(bossPrefab, grave.transform.position, grave.transform.rotation);
            grave.SetActive(false);
        }
        
    }
}
