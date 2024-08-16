using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "New Buff Hooray Skill", menuName = "Skills/Buff_HooraySkill")]
public class Buff_HooraySkill : BuffSkill
{
    public float multipleSize;

    private Player player;
    private BoxCollider2D playerCollider;
    private Vector2 originPlayerColliderSize;
    private Vector3 originPlayerScale;
    private float originalX, originalY;

    bool _isSkillUsed = false;

    public override void Activate(BaseStat stat)
    {
        base.Activate(stat);

        if (!_isSkillUsed && canSkill)
        {
            _isSkillUsed = true;

            player = Player.Instance;
            playerCollider = player.GetComponent<BoxCollider2D>();

            if (playerCollider == null)
            {
                return;
            }

            // 원래 크기를 저장
            originPlayerColliderSize = playerCollider.size;
            originPlayerScale = player.transform.localScale;

            originalX = Weapon.size_x;
            originalY = Weapon.size_y;

            if (SkillSound != null)
                SoundManager.Instance.PlaySkillSound(SkillSound);

            // 파티클 효과 재생
            if (particlePrefab != null)
            {
                GameObject particle = Instantiate(particlePrefab, player.transform);
                particle.transform.position = new Vector3(player.transform.position.x,
                    player.transform.position.y);
                Destroy(particle, BuffTime);
            }

            // BuffTime이 지난 후 원상태로 되돌리기 위한 코루틴 시작
            player.StartCoroutine(GrowPlayerSize());
        }
    }

    private IEnumerator GrowPlayerSize()
    {
        float time = 0f;
        Vector3 targetScale = originPlayerScale * multipleSize;
        Vector3 targetColliderSize = originPlayerColliderSize * multipleSize;

        while (time < CastTime)
        {
            float growTime = time / CastTime;
            player.transform.localScale = Vector3.Lerp(originPlayerScale, targetScale, growTime);
            playerCollider.size = Vector2.Lerp(originPlayerColliderSize, targetColliderSize, growTime);
            time += Time.deltaTime;
            yield return null;
        }

        player.transform.localScale = targetScale;
        playerCollider.size = targetColliderSize;

        Weapon.size_x *= multipleSize;
        Weapon.size_y *= multipleSize;

        player.StartCoroutine(ResetPlayerSizeAfterBuffTime());
    }

    private IEnumerator ResetPlayerSizeAfterBuffTime()
    {
        yield return wfs;

        float time = 0f;
        Vector3 currentScale = player.transform.localScale;
        Vector2 currentColliderSize = playerCollider.size;

        while (time < CastTime)
        {
            float shrinkTime = time / CastTime;
            player.transform.localScale = Vector3.Lerp(currentScale, originPlayerScale, shrinkTime);
            playerCollider.size = Vector2.Lerp(currentColliderSize, originPlayerColliderSize, shrinkTime);
            time += Time.deltaTime;
            yield return null;
        }

        // 원래 크기로 되돌리기
        player.transform.localScale = originPlayerScale;
        playerCollider.size = originPlayerColliderSize;

        Weapon.size_x = originalX;
        Weapon.size_y = originalY;

        _isSkillUsed = false; // 스킬 사용 종료
    }
}
