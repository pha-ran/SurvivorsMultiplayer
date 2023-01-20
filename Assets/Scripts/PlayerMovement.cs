using Photon.Pun;
using UnityEngine;

// 플레이어 캐릭터를 사용자 입력에 따라 이동
public class PlayerMovement : MonoBehaviourPun
{
    private PlayerInput playerInput; // 플레이어 입력을 알려주는 컴포넌트
    private Rigidbody2D playerRigidbody; // 플레이어 캐릭터의 리지드바디
    private Vector2 playerInputVector; // 플레이어 입력값 벡터

    public float speed = 3f; // 속도

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerRigidbody = GetComponent<Rigidbody2D>();
    }

    // FixedUpdate는 물리 갱신 주기에 맞춰 실행
    private void FixedUpdate()
    {
        // 로컬 플레이어만 직접 위치 변경 가능
        if (!photonView.IsMine)
        {
            return;
        }

        Move(); // 플레이어 캐릭터 이동
    }

    // 입력값에 따라 플레이어 캐릭터 이동
    private void Move()
    {
        playerInputVector.x = playerInput.x; // 수평 입력값 설정
        playerInputVector.y = playerInput.y; // 수직 입력값 설정

        // 상대적으로 이동할 거리 계산
        Vector2 moveDistance = playerInputVector.normalized * speed * Time.fixedDeltaTime;

        // 리지드바디를 통해 게임 오브젝트 위치 변경
        playerRigidbody.MovePosition(playerRigidbody.position + moveDistance);
    }
}
