using Photon.Pun;
using UnityEngine;

// 플레이어 캐릭터를 조작하기 위한 사용자 입력을 감지
// 감지된 입력값을 다른 컴포넌트들이 사용할 수 있도록 제공
public class PlayerInput : MonoBehaviourPun
{
    // 값 할당은 내부에서만 가능
    public float x { get; private set; } // 감지된 수평 입력값
    public float y { get; private set; } // 감지된 수직 입력값

    // 사용자 입력을 감지
    private void FixedUpdate()
    {
        // 로컬 플레이어가 아닌 경우 무시
        if (!photonView.IsMine)
        {
            return;
        }

        // 게임오버 상태에서는 사용자 입력 미감지
        if (GameManager.instance != null
            && GameManager.instance.isGameover)
        {
            x = 0;
            y = 0;
            return;
        }

        // 수평 입력 감지
        x = Input.GetAxisRaw("Horizontal");
        // 수직 입력 감지
        y = Input.GetAxisRaw("Vertical");
    }
}