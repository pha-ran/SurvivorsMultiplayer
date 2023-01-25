using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : LivingEntity
{
    private PlayerMovement playerMovement; // PlayerMovement ������Ʈ

    public Slider healthSlider; // ü�� ǥ�� UI �����̴�

    private void Awake()
    {
        // ������Ʈ �Ҵ�
        playerMovement = GetComponent<PlayerMovement>();
    }

    protected override void OnEnable()
    {
        base.OnEnable(); // LivingEntity�� OnEnable() ���� (Ȱ��ȭ�� ���� �ʱ�ȭ)

        healthSlider.gameObject.SetActive(true); // ü�� �����̴� Ȱ��ȭ
        healthSlider.maxValue = startingHealth; // ü�� �����̴��� �ִ밪�� �⺻ ü�°����� ����
        healthSlider.value = health; // ü�� �����̴��� ���� ���� ü�°����� ����

        // �÷��̾� ������ �޴� ������Ʈ�� Ȱ��ȭ
        playerMovement.enabled = true;
    }

    // ü�� ȸ��
    [PunRPC]
    public override void RestoreHealth(float newHealth)
    {
        base.RestoreHealth(newHealth); // LivingEntity�� RestoreHealth() ���� (ü�� ȸ��)
        healthSlider.value = health; // ü�� �����̴� �� ����
    }

    // ������ ó��
    [PunRPC]
    public override void OnDamage(float damage, Vector2 hitPoint, Vector2 hitDirection)
    {
        base.OnDamage(damage, hitPoint, hitDirection); // LivingEntity�� OnDamage() ���� (������ ó��)
        healthSlider.value = health; // ü�� �����̴� �� ����
    }

    // �÷��̾� ���
    public override void Die()
    {
        base.Die(); // LivingEntity�� Die() ���� (��� ó��)

        healthSlider.gameObject.SetActive(false); // ü�� �����̴� ��Ȱ��ȭ

        // �÷��̾� ������ �޴� ������Ʈ�� ��Ȱ��ȭ
        playerMovement.enabled = false;

        Invoke("Respawn", 10f); // 10�� �ڿ� ������
    }

    // �����۰� �浹�� ��� �ش� ������ ���
    private void OnTriggerEnter(Collider other)
    {
        if (!dead) // ������� ���� ��쿡�� ������ ��밡��
        {
            IItem item = other.GetComponent<IItem>(); // �浹�� ���� ������Ʈ�� Item ������Ʈ �Ҵ�

            if (item != null) // Item ������Ʈ�� ���� (�������� ���)
            {
                if (PhotonNetwork.IsMasterClient) // ȣ��Ʈ�� ������ ���� ��� ����, ��� �� ����� ��� Ŭ���̾�Ʈ���� ����ȭ
                {
                    item.Use(gameObject); // Use �޼��带 �����Ͽ� ������ ���
                }
            }
        }
    }

    // ������ ó��
    public void Respawn()
    {
        if (photonView.IsMine) // ���� �÷��̾ ���� ��ġ�� ���� ����
        {
            Vector2 randomSpawnPos = Random.insideUnitCircle * 10f; // �������� �ݰ� 10���� ������ ������ ��ġ ����
            transform.position = randomSpawnPos; // ������ ���� ��ġ�� �̵�
        }

        // ������Ʈ���� �����ϱ� ���� ���� ������Ʈ ��Ȱ��ȭ �� Ȱ��ȭ (OnDisable(), OnEnable() �޼��� ����)
        gameObject.SetActive(false);
        gameObject.SetActive(true);
    }
}
