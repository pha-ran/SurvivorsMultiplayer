using Photon.Pun;
using UnityEngine;

// �÷��̾� ĳ���͸� �����ϱ� ���� ����� �Է��� ����
// ������ �Է°��� �ٸ� ������Ʈ���� ����� �� �ֵ��� ����
public class PlayerInput : MonoBehaviourPun
{
    // �� �Ҵ��� ���ο����� ����
    public float x { get; private set; } // ������ ���� �Է°�
    public float y { get; private set; } // ������ ���� �Է°�

    // ����� �Է��� ����
    private void FixedUpdate()
    {
        // ���� �÷��̾ �ƴ� ��� ����
        if (!photonView.IsMine)
        {
            return;
        }

        // ���ӿ��� ���¿����� ����� �Է� �̰���
        if (GameManager.instance != null
            && GameManager.instance.isGameover)
        {
            x = 0;
            y = 0;
            return;
        }

        // ���� �Է� ����
        x = Input.GetAxisRaw("Horizontal");
        // ���� �Է� ����
        y = Input.GetAxisRaw("Vertical");
    }
}