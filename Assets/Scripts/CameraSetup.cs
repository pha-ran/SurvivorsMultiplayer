using Cinemachine; // �ó׸ӽ� ���� �ڵ�
using Photon.Pun; // PUN ���� �ڵ�
using UnityEngine;

// �ó׸ӽ� ī�޶� ���� �÷��̾ �����ϵ��� ����
public class CameraSetup : MonoBehaviourPun
{
    void Start()
    {
        if (photonView.IsMine)
        {
            CinemachineVirtualCamera followCam = FindObjectOfType<CinemachineVirtualCamera>(); // ���� �ִ� �ó׸ӽ� ���� ī�޶� ã��
            followCam.Follow = transform; // ���� ī�޶��� ���� ����� �ڽ��� Ʈ���������� ����
        }
    }
}