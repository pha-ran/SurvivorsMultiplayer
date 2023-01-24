using System;
using Photon.Pun;
using UnityEngine;

// ����ü�μ� ������ ���� ������Ʈ���� ���� ����
public class LivingEntity : MonoBehaviourPun, IDamageable
{
    public float startingHealth = 100f; // ���� ü��
    public float health { get; protected set; } // ���� ü�� ������Ƽ
    public bool dead { get; protected set; } // ��� ���� ������Ƽ
    public event Action onDeath; // ��������Ʈ, ����� �ߵ��� �̺�Ʈ

    // ����ü Ȱ��ȭ�� �ʱ�ȭ
    protected virtual void OnEnable()
    {
        dead = false; // ���� ���·� �ʱ�ȭ
        health = startingHealth; // ���� ü������ �ʱ�ȭ
    }

    // ȣ��Ʈ -> ��� Ŭ���̾�Ʈ �������� ü�°� ��� ���¸� ����ȭ
    [PunRPC]
    public void ApplyUpdatedHealth(float newHealth, bool newDead)
    {
        health = newHealth; // ü�� ����ȭ
        dead = newDead; // ��� ���� ����ȭ
    }

    // ����ü ���
    public virtual void Die()
    {
        if (onDeath != null)
        {
            onDeath(); // onDeath �̺�Ʈ�� ��ϵ� �޼��尡 �ִٸ� ����
        }

        dead = true; // ��� ���·� ����
    }

    // ������ ó�� (ȣ��Ʈ���� ���� �ܵ� ����, ȣ��Ʈ�� ���� �ٸ� Ŭ���̾�Ʈ�鿡�� �ϰ� ����)
    [PunRPC]
    public virtual void OnDamage(float damage, Vector2 hitPoint, Vector2 hitNormal)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            health -= damage; // ��������ŭ ü�� ����

            photonView.RPC("ApplyUpdatedHealth", RpcTarget.Others, health, dead); // ȣ��Ʈ���� Ŭ���̾�Ʈ�� ����ȭ

            photonView.RPC("OnDamage", RpcTarget.Others, damage, hitPoint, hitNormal); // �ٸ� Ŭ���̾�Ʈ�鵵 OnDamage�� ����
        }

        if (health <= 0 && !dead) // ü���� 0 �����̰� ���� ���� �ʾҴٸ� ��� ó��
        {
            Die();
        }
    }

    // ü�� ȸ��
    [PunRPC]
    public virtual void RestoreHealth(float newHealth)
    {
        if (dead)
        {
            return; // �̹� ����� ��� ü�� ȸ�� �Ұ�
        }

        if (PhotonNetwork.IsMasterClient) // ȣ��Ʈ�� ü���� ���� ���� ����
        {
            health += newHealth; // ü�� �߰�
            if (health > startingHealth) health = startingHealth; // ���� ü�� ���Ϸ� ����

            photonView.RPC("ApplyUpdatedHealth", RpcTarget.Others, health, dead); // ȣ��Ʈ���� Ŭ���̾�Ʈ�� ����ȭ

            photonView.RPC("RestoreHealth", RpcTarget.Others, newHealth); // �ٸ� Ŭ���̾�Ʈ�鵵 RestoreHealth�� ����
        }
    }
}
