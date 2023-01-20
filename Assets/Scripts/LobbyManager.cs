using UnityEngine;
using UnityEngine.UI;
using Photon.Pun; // ����Ƽ�� ���� ������Ʈ��
using Photon.Realtime; // ���� ���� ���� ���̺귯��

// ������(��ġ ����ŷ) ������ �� ���� ���
public class LobbyManager : MonoBehaviourPunCallbacks
{
    private string gameVersion = "1"; // ���� ����
    private bool joinedRoom = false; // �� ���� ����
    private bool isStarted = false; // ���� ���� ����

    public Text connectionInfoText; // ��Ʈ��ũ ���� ǥ��
    public Button joinButton; // �� ���� ��ư

    // ���� ����� ���ÿ� ������ ���� ���� �õ�
    void Start()
    {
        PhotonNetwork.GameVersion = gameVersion; // ���ӿ� �ʿ��� ���� (���� ����) ����
        PhotonNetwork.AutomaticallySyncScene = true; // ȣ��Ʈ�� ���� �ε��ϸ�, Ŭ���̾�Ʈ�� �ڵ� ��ũ
        PhotonNetwork.ConnectUsingSettings(); // ������ ������ ������ ���� ���� �õ�

        joinButton.interactable = false; // �� ���� ��ư ��Ȱ��ȭ
        connectionInfoText.text = "������ ������ ���� �� ...";
    }

    // ������ ���� ���� ������ �ڵ� ����
    public override void OnConnectedToMaster()
    {
        joinButton.interactable = true; // �� ���� ��ư Ȱ��ȭ
        connectionInfoText.text = "�¶��� : ������ ������ �����";
    }

    // ������ ���� ���� ���н� �ڵ� ����
    public override void OnDisconnected(DisconnectCause cause)
    {
        joinButton.interactable = false; // �� ���� ��ư ��Ȱ��ȭ
        connectionInfoText.text = "�������� : ������ ������ ������� ����\n���� ��õ� �� ...";
        PhotonNetwork.ConnectUsingSettings(); // ������ �������� ������ �õ�
    }

    // �� ���� �õ�
    public void Connect()
    {
        joinButton.interactable = false; // �ߺ� ���� ������ ���� ���� ��ư ��Ȱ��ȭ

        if (PhotonNetwork.IsConnected) // ������ ������ ���� ���� ���
        {
            connectionInfoText.text = "�뿡 ���� ...";
            PhotonNetwork.JoinRandomRoom(); // �� ���� ����
        }
        else
        {
            connectionInfoText.text = "�������� : ������ ������ ������� ����\n���� ��õ� �� ...";
            PhotonNetwork.ConnectUsingSettings(); // ������ �������� ������ �õ�
        }
    }

    // (�� ���� ����)���� �� ������ ������ ��� �ڵ� ����
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        connectionInfoText.text = "���ο� �� ���� ...";
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 4 }); // �ִ� 4���� ���� ������ �� �� ����
    }

    // �뿡 ���� �Ϸ�� ��� �ڵ� ����
    public override void OnJoinedRoom()
    {
        joinedRoom = true;
        connectionInfoText.text = "�� ���� ���� (" + PhotonNetwork.PlayerList.Length + " / 4)"; // ���� ���� ǥ��
    }

    // �뿡 ���� �Ϸ�� ��� ���� �ο� ���
    private void Update()
    {
        if (joinedRoom) connectionInfoText.text = "�� ���� ���� (" + PhotonNetwork.PlayerList.Length + " / 4)"; // �ǽð� ���� �ο� ���

        if (PhotonNetwork.IsMasterClient && !isStarted && PhotonNetwork.PlayerList.Length == 4)
        {
            isStarted = true;
            PhotonNetwork.LoadLevel("Main"); // ȣ��Ʈ�̰� 4���� �뿡 ������ ��� ��� �� �����ڰ� Main ���� �ε� (���� ����)
        }
    }
}
