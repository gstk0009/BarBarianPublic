using UnityEngine;



public class Weapon : MonoBehaviour
{
    [SerializeField] private LayerMask atkTarget;

    private Player player;

    private DungeonThings dt;
    private BaseStat dtStat;
    public Collider2D col;

    private Vector2 size;
    private Vector2 point;

    static public float size_x = 0.4f, size_y = 0.8f;

    private void Awake()
    {
        dt = GetComponentInParent<DungeonThings>();
        dtStat = dt.GetComponent<BaseStat>();
        col = GetComponent<Collider2D>();
        if(gameObject.layer == (int)LayerType.Player)
        {
            player = GetComponentInParent<Player>();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if ((atkTarget.value & (1 << other.gameObject.layer)) == 0) return; //공격 대상으로 설정한 오브젝트가 아니면, return
        if (other.gameObject.TryGetComponent(out IDamagable damagable))
        {
            damagable.CalDamage(dtStat);
        }
    }

    public void HitRange()
    {
        Collider2D[] hit = null;
        switch(player.Animation.DirectionWay)
        {
            case Direction.down:
                size = new Vector2(size_x, -size_y);
                point = new Vector2(transform.position.x, transform.position.y - 0.10f);
                hit = Physics2D.OverlapBoxAll(point, size, 0, atkTarget);
                break;
            case Direction.up:
                size = new Vector2(size_x, size_y);
                point = new Vector2(transform.position.x, transform.position.y+0.38f);
                hit = Physics2D.OverlapBoxAll(point, size, 0, atkTarget);
                break;
            case Direction.left:
                size = new Vector2(size_y, -size_x);
                point = new Vector2(transform.position.x - 0.22f, transform.position.y + 0.2f);
                hit = Physics2D.OverlapBoxAll(point, size, 0, atkTarget);
                break;
            case Direction.right:
                size = new Vector2(size_y, size_x);
                point = new Vector2(transform.position.x + 0.22f, transform.position.y + 0.2f);
                hit = Physics2D.OverlapBoxAll(point, size, 0, atkTarget);
                break;
        }

        if (hit != null)
        {
            foreach (var monster in hit)
            {
                Vector3 knockbackDirection = monster.transform.position - transform.position;
                monster.GetComponent<IDamagable>().CalDamage(player.playerStat);
                if(monster.GetComponent<MonsterStat>() != null) monster.GetComponent<MonsterStat>().ApplyKnockback(knockbackDirection);
            }
        }
    }

    public void activeCollider()
    {
        col.enabled = !col.enabled;
    }


    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(point, size);
    }
}
