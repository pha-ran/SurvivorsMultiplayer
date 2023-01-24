using UnityEngine;

// �������� ���� �� �ִ� Ÿ�Ե��� ���������� ������ �ϴ� �������̽�
public interface IDamageable
{
    // �������� ���� �� �ִ� Ÿ�Ե��� IDamageable�� ����ϰ� OnDamage �޼��带 �ݵ�� ����
    void OnDamage(float damage, Vector2 hitPoint, Vector2 hitNormal); // ������ ũ��(damage), ���� ����(hitPoint), ���� ǥ���� ����(hitNormal)
}