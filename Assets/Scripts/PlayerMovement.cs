using Photon.Pun;
using UnityEngine;

// �÷��̾� ĳ���͸� ����� �Է¿� ���� �̵�
public class PlayerMovement : MonoBehaviourPun
{
    private PlayerInput playerInput; // �÷��̾� �Է��� �˷��ִ� ������Ʈ
    private Rigidbody2D playerRigidbody; // �÷��̾� ĳ������ ������ٵ�
    private Vector2 playerInputVector; // �÷��̾� �Է°� ����

    public float speed = 3f; // �ӵ�

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerRigidbody = GetComponent<Rigidbody2D>();
    }

    // FixedUpdate�� ���� ���� �ֱ⿡ ���� ����
    private void FixedUpdate()
    {
        // ���� �÷��̾ ���� ��ġ ���� ����
        if (!photonView.IsMine)
        {
            return;
        }

        Move(); // �÷��̾� ĳ���� �̵�
    }

    // �Է°��� ���� �÷��̾� ĳ���� �̵�
    private void Move()
    {
        playerInputVector.x = playerInput.x; // ���� �Է°� ����
        playerInputVector.y = playerInput.y; // ���� �Է°� ����

        // ��������� �̵��� �Ÿ� ���
        Vector2 moveDistance = playerInputVector.normalized * speed * Time.fixedDeltaTime;

        // ������ٵ� ���� ���� ������Ʈ ��ġ ����
        playerRigidbody.MovePosition(playerRigidbody.position + moveDistance);
    }
}
