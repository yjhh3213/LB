using System.Collections;
using UnityEngine;

public class EnemyDash : MonoBehaviour
{
    public Transform player;

    [Header("�⺻ �̵�")]
    public float moveSpeed = 3f;

    [Header("���� ����/�Ķ����")]
    public float triggerDistance = 8f;   // �� �Ÿ� �ȿ� ������ 1ȸ ����
    public float chargeSpeed = 18f;      // ���� �ӵ�
    public float chargeDuration = 0.6f;  // ���� ���� �ð�

    private bool isCharging = false;
    private bool hasCharged = false;     // ������ �� 1����

    void Update()
    {
        if (player == null) return;

        // ���� ���� ���� �ƴϸ� �⺻ �̵�
        if (!isCharging)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                player.position,
                moveSpeed * Time.deltaTime
            );
        }

        // Ʈ����: ���� ���� �� �߰�, �Ÿ� ���� ���� �� 1ȸ ���� ����
        if (!hasCharged && !isCharging)
        {
            float dist = Vector3.Distance(transform.position, player.position);
            if (dist <= triggerDistance)
            {
                StartCoroutine(ChargeOnce());
            }
        }
    }

    IEnumerator ChargeOnce()
    {
        isCharging = true;

        // ���� ���� ������ ������ ���� (�÷��̾ ���߿� �������� ������ ������ ����)
        Vector3 dir = player != null ? (player.position - transform.position) : transform.forward;
        if (dir.sqrMagnitude < 0.0001f) dir = transform.forward;
        dir.Normalize();

        float t = 0f;
        while (t < chargeDuration)
        {
            transform.position += dir * chargeSpeed * Time.deltaTime;
            t += Time.deltaTime;
            yield return null;
        }

        hasCharged = true;   // �絹�� �Ұ�
        isCharging = false;
    }

#if UNITY_EDITOR
    // �����Ϳ��� Ʈ���� �Ÿ� Ȯ�ο�(����)
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, triggerDistance);
    }
#endif
}
