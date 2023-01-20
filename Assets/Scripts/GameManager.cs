using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviourPun
{
    private static GameManager m_instance; // 싱글턴이 할당될 static 변수

    public bool isGameover { get; private set; } // 게임 종료 상태

    public GameObject playerPrefab; // 생성할 플레이어 캐릭터 프리팹

    // 외부에서 싱글턴 오브젝트를 가져올때 사용할 프로퍼티
    public static GameManager instance
    {
        get
        {
            // 만약 싱글턴 변수에 아직 오브젝트가 할당되지 않았다면
            if (m_instance == null)
            {
                // 씬에서 GameManager 오브젝트를 찾아 할당
                m_instance = FindObjectOfType<GameManager>();
            }

            // 싱글턴 오브젝트를 반환
            return m_instance;
        }
    }

    private void Awake()
    {
        if (instance != this) // 씬에 싱글턴 오브젝트가 된 다른 GameManager 오브젝트가 있다면
        {
            Destroy(gameObject); // 자신을 파괴
        }
    }

    // 게임 시작과 동시에 플레이어가 될 게임 오브젝트를 생성
    private void Start()
    {
        Vector2 randomSpawnPos = Random.insideUnitCircle * 2f; // 생성할 랜덤 위치 지정

        /*  네트워크의 모든 클라이언트들에서 플레이어 캐릭터 생성 실행
            해당 게임 오브젝트의 주도권은, 생성 메서드를 직접 실행한 클라이언트에게 있음 */
        PhotonNetwork.Instantiate(playerPrefab.name, randomSpawnPos, Quaternion.identity);
    }

    // 게임 오버 처리
    public void EndGame()
    {
        isGameover = true;
    }
}
