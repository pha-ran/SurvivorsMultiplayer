using System;
using Photon.Pun;
using UnityEngine;

// 생명체로서 동작할 게임 오브젝트들을 위한 뼈대
public class LivingEntity : MonoBehaviourPun, IDamageable
{
    public float startingHealth = 100f; // 시작 체력
    public float health { get; protected set; } // 현재 체력 프로퍼티
    public bool dead { get; protected set; } // 사망 상태 프로퍼티
    public event Action onDeath; // 델리게이트, 사망시 발동할 이벤트

    // 생명체 활성화시 초기화
    protected virtual void OnEnable()
    {
        dead = false; // 생존 상태로 초기화
        health = startingHealth; // 시작 체력으로 초기화
    }

    // 호스트 -> 모든 클라이언트 방향으로 체력과 사망 상태를 동기화
    [PunRPC]
    public void ApplyUpdatedHealth(float newHealth, bool newDead)
    {
        health = newHealth; // 체력 동기화
        dead = newDead; // 사망 상태 동기화
    }

    // 생명체 사망
    public virtual void Die()
    {
        if (onDeath != null)
        {
            onDeath(); // onDeath 이벤트에 등록된 메서드가 있다면 실행
        }

        dead = true; // 사망 상태로 변경
    }

    // 데미지 처리 (호스트에서 먼저 단독 실행, 호스트를 통해 다른 클라이언트들에서 일괄 실행)
    [PunRPC]
    public virtual void OnDamage(float damage, Vector2 hitPoint, Vector2 hitNormal)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            health -= damage; // 데미지만큼 체력 감소

            photonView.RPC("ApplyUpdatedHealth", RpcTarget.Others, health, dead); // 호스트에서 클라이언트로 동기화

            photonView.RPC("OnDamage", RpcTarget.Others, damage, hitPoint, hitNormal); // 다른 클라이언트들도 OnDamage를 실행
        }

        if (health <= 0 && !dead) // 체력이 0 이하이고 아직 죽지 않았다면 사망 처리
        {
            Die();
        }
    }

    // 체력 회복
    [PunRPC]
    public virtual void RestoreHealth(float newHealth)
    {
        if (dead)
        {
            return; // 이미 사망한 경우 체력 회복 불가
        }

        if (PhotonNetwork.IsMasterClient) // 호스트만 체력을 직접 갱신 가능
        {
            health += newHealth; // 체력 추가
            if (health > startingHealth) health = startingHealth; // 시작 체력 이하로 유지

            photonView.RPC("ApplyUpdatedHealth", RpcTarget.Others, health, dead); // 호스트에서 클라이언트로 동기화

            photonView.RPC("RestoreHealth", RpcTarget.Others, newHealth); // 다른 클라이언트들도 RestoreHealth를 실행
        }
    }
}
