using UnityEngine;

public class BossAnimationEvent : MonoBehaviour
{
    public GameObject AttackSwordEffect;
    public GameObject AttackGroundEffect;
    public GameObject Attack2Effect;
    public GameObject Attack2LastEffect;
    public GameObject Attack3GroundEffect;
    public GameObject Attack3BulletEffect;
    private PolygonCollider2D SwordCollider;
    private PolygonCollider2D GroundCollider;
    private PolygonCollider2D Attack2Collider;
    private PolygonCollider2D Attack2LastCollider;
    private PolygonCollider2D Attack3GroundCollider;
    public CapsuleCollider2D[] BulletCollider;

    private void Awake()
    {
        if (Attack3BulletEffect != null) BulletCollider = Attack3BulletEffect.GetComponentsInChildren<CapsuleCollider2D>();
        if(AttackSwordEffect != null) SwordCollider = AttackSwordEffect.GetComponent<PolygonCollider2D>();
        if(AttackGroundEffect != null) GroundCollider = AttackGroundEffect.GetComponent<PolygonCollider2D>();
        if (Attack2Effect != null) Attack2Collider = Attack2Effect.GetComponent<PolygonCollider2D>();
        if (Attack2LastEffect != null) Attack2LastCollider = Attack2LastEffect.GetComponent<PolygonCollider2D>();
        if (Attack3GroundEffect != null) Attack3GroundCollider = Attack3GroundEffect.GetComponent<PolygonCollider2D>();
    }

    public void AttackSwordActive()
    {
        // 오브젝트의 현재 활성 상태를 반전
        AttackSwordEffect.SetActive(!AttackSwordEffect.activeSelf);
        SwordCollider.enabled = !SwordCollider.enabled;
    }

    public void AttackGroundActive()
    {
        // 오브젝트의 현재 활성 상태를 반전
        AttackGroundEffect.SetActive(!AttackGroundEffect.activeSelf);
        GroundCollider.enabled = !GroundCollider.enabled;
    }

    public void Attack2Active()
    {
        // 오브젝트의 현재 활성 상태를 반전
        Attack2Effect.SetActive(!Attack2Effect.activeSelf);
        Attack2Collider.enabled = !Attack2Collider.enabled;
    }

    public void Attack2LastActive()
    {
        // 오브젝트의 현재 활성 상태를 반전
        Attack2LastEffect.SetActive(!Attack2LastEffect.activeSelf);
        Attack2LastCollider.enabled = !Attack2LastCollider.enabled;
    }

    public void Attack3GroundActive()
    {
        // 오브젝트의 현재 활성 상태를 반전
        Attack3GroundEffect.SetActive(!Attack3GroundEffect.activeSelf);
        Attack3GroundCollider.enabled = !Attack3GroundCollider.enabled;
    }

    public void Attack3BulletActive()
    {
        // 오브젝트의 현재 활성 상태를 반전
        Attack3BulletEffect.SetActive(!Attack3BulletEffect.activeSelf);
    }

    public void BulletColliderActive()
    {
        foreach(CapsuleCollider2D collider in BulletCollider)
        {
            collider.enabled = !collider.enabled;
        }
    }



}
