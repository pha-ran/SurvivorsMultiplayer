using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviourPun
{
    private static GameManager m_instance; // �̱����� �Ҵ�� static ����

    public bool isGameover { get; private set; } // ���� ���� ����

    public GameObject playerPrefab; // ������ �÷��̾� ĳ���� ������

    // �ܺο��� �̱��� ������Ʈ�� �����ö� ����� ������Ƽ
    public static GameManager instance
    {
        get
        {
            // ���� �̱��� ������ ���� ������Ʈ�� �Ҵ���� �ʾҴٸ�
            if (m_instance == null)
            {
                // ������ GameManager ������Ʈ�� ã�� �Ҵ�
                m_instance = FindObjectOfType<GameManager>();
            }

            // �̱��� ������Ʈ�� ��ȯ
            return m_instance;
        }
    }

    private void Awake()
    {
        if (instance != this) // ���� �̱��� ������Ʈ�� �� �ٸ� GameManager ������Ʈ�� �ִٸ�
        {
            Destroy(gameObject); // �ڽ��� �ı�
        }
    }

    // ���� ���۰� ���ÿ� �÷��̾ �� ���� ������Ʈ�� ����
    private void Start()
    {
        Vector2 randomSpawnPos = Random.insideUnitCircle * 2f; // ������ ���� ��ġ ����

        /*  ��Ʈ��ũ�� ��� Ŭ���̾�Ʈ�鿡�� �÷��̾� ĳ���� ���� ����
            �ش� ���� ������Ʈ�� �ֵ�����, ���� �޼��带 ���� ������ Ŭ���̾�Ʈ���� ���� */
        PhotonNetwork.Instantiate(playerPrefab.name, randomSpawnPos, Quaternion.identity);
    }

    // ���� ���� ó��
    public void EndGame()
    {
        isGameover = true;
    }
}
