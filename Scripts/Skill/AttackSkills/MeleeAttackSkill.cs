using UnityEngine;

[CreateAssetMenu(fileName = "New Melee Attack Skill", menuName = "Skills/MeleeAttackSkill")]
public class MeleeAttackSkill : AttackSkill
{
    public LayerMask EnemyLayer; // 적 레이어

    public override void Activate(BaseStat stat)
    {
        base.Activate(stat);
        if (!canSkill) return;

        DungeonThings player = stat.gameObject.GetComponent<DungeonThings>();

        Vector2 attackDir = Vector2.zero;
        Vector2 playerPos = player.transform.position;


        switch (player.Animation.DirectionWay)
        {
            case Direction.up:
                attackDir = Vector2.up; break;
            case Direction.down:
                attackDir = Vector2.down; break;
            case Direction.left:
                attackDir = Vector2.left; break;
            case Direction.right:
                attackDir = Vector2.right; break;
        }
        Vector2 boxCenter = playerPos + attackDir * forwardOffset + new Vector2(xOffset, yOffset);
        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(boxCenter, hitBoxSize, 0, EnemyLayer);

        foreach (Collider2D hit in hitEnemies)
        {
            var enemy = hit.GetComponent<Collider2D>().GetComponent<BaseStat>();
            if (enemy != null)
            {
                // 현재 플레이어 공격력의 Damage배 만큼의 스킬 고정 데미지(임의)
                enemy.TakeDamage(stat.STR.curValue * Damage);

                if (enchantType != EnchantType.None)
                {
                    GetDebuff(enchantType, enemy);
                }
            }
        }

        if (SkillSound != null)
            SoundManager.Instance.PlaySkillSound(SkillSound);

        // 파티클 효과 재생
        if (particlePrefab != null)
        {
            GameObject particle = Instantiate(particlePrefab, boxCenter, Quaternion.identity);
            Destroy(particle, CastTime); 
        }
    }
}