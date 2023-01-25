using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : LivingEntity
{
    private PlayerMovement playerMovement; // PlayerMovement 컴포넌트

    public Slider healthSlider; // 체력 표시 UI 슬라이더

    private void Awake()
    {
        // 컴포넌트 할당
        playerMovement = GetComponent<PlayerMovement>();
    }

    protected override void OnEnable()
    {
        base.OnEnable(); // LivingEntity의 OnEnable() 실행 (활성화시 상태 초기화)

        healthSlider.gameObject.SetActive(true); // 체력 슬라이더 활성화
        healthSlider.maxValue = startingHealth; // 체력 슬라이더의 최대값을 기본 체력값으로 변경
        healthSlider.value = health; // 체력 슬라이더의 값을 현재 체력값으로 변경

        // 플레이어 조작을 받는 컴포넌트들 활성화
        playerMovement.enabled = true;
    }

    // 체력 회복
    [PunRPC]
    public override void RestoreHealth(float newHealth)
    {
        base.RestoreHealth(newHealth); // LivingEntity의 RestoreHealth() 실행 (체력 회복)
        healthSlider.value = health; // 체력 슬라이더 값 변경
    }

    // 데미지 처리
    [PunRPC]
    public override void OnDamage(float damage, Vector2 hitPoint, Vector2 hitDirection)
    {
        base.OnDamage(damage, hitPoint, hitDirection); // LivingEntity의 OnDamage() 실행 (데미지 처리)
        healthSlider.value = health; // 체력 슬라이더 값 변경
    }

    // 플레이어 사망
    public override void Die()
    {
        base.Die(); // LivingEntity의 Die() 실행 (사망 처리)

        healthSlider.gameObject.SetActive(false); // 체력 슬라이더 비활성화

        // 플레이어 조작을 받는 컴포넌트들 비활성화
        playerMovement.enabled = false;

        Invoke("Respawn", 10f); // 10초 뒤에 리스폰
    }

    // 아이템과 충돌한 경우 해당 아이템 사용
    private void OnTriggerEnter(Collider other)
    {
        if (!dead) // 사망하지 않은 경우에만 아이템 사용가능
        {
            IItem item = other.GetComponent<IItem>(); // 충돌한 상대방 오브젝트의 Item 컴포넌트 할당

            if (item != null) // Item 컴포넌트가 존재 (아이템인 경우)
            {
                if (PhotonNetwork.IsMasterClient) // 호스트만 아이템 직접 사용 가능, 사용 후 결과를 모든 클라이언트에게 동기화
                {
                    item.Use(gameObject); // Use 메서드를 실행하여 아이템 사용
                }
            }
        }
    }

    // 리스폰 처리
    public void Respawn()
    {
        if (photonView.IsMine) // 로컬 플레이어만 직접 위치를 변경 가능
        {
            Vector2 randomSpawnPos = Random.insideUnitCircle * 10f; // 원점에서 반경 10유닛 내부의 랜덤한 위치 지정
            transform.position = randomSpawnPos; // 지정된 랜덤 위치로 이동
        }

        // 컴포넌트들을 리셋하기 위해 게임 오브젝트 비활성화 후 활성화 (OnDisable(), OnEnable() 메서드 실행)
        gameObject.SetActive(false);
        gameObject.SetActive(true);
    }
}
