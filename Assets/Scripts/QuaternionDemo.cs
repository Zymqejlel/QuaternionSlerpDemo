using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// 方位的表达与插值：四元数演示
/// </summary>
public class QuaternionDemo : MonoBehaviour
{
    [Header("被旋转物体")]
    public GameObject targetObj;

    [Header("关键帧姿态1（轴‑角）")]
    public Vector3 axis1 = Vector3.up;
    public float angle1 = 0f;

    [Header("关键帧姿态2（轴‑角）")]
    public Vector3 axis2 = Vector3.right;
    public float angle2 = 120f;

    [Header("动画参数")]
    public float animationSpeed = 0.3f;

    [Header("UI组件")]
    public Toggle toggleSlerp;
    public TextMeshProUGUI infoText;

    private Quaternion q1;
    private Quaternion q2;
    private float t;
    private int direction = 1;

    void Start()
    {
        q1 = Quaternion.AngleAxis(angle1, axis1.normalized);
        q2 = Quaternion.AngleAxis(angle2, axis2.normalized);

        // 处理q和-q，选择最短旋转弧
        float dot = Quaternion.Dot(q1, q2);
        if (dot < 0)
        {
            q2 = new Quaternion(-q2.x, -q2.y, -q2.z, -q2.w);
        }
        t = 0;
        direction = 1;
    }

    void Update()
    {
        t += Time.deltaTime * animationSpeed * direction;
        if (t >= 1.0f)
        {
            t = 1.0f;
            direction = -1;
        }
        else if (t <= 0.0f)
        {
            t = 0.0f;
            direction = 1;
        }

        Quaternion currentRot;
        bool useSlerp = toggleSlerp.isOn;
        if (useSlerp)
        {
            currentRot = Quaternion.Slerp(q1, q2, t);
        }
        else
        {
            currentRot = Quaternion.Lerp(q1, q2, t);
        }

        targetObj.transform.rotation = currentRot;

        infoText.text = $"【Slerp Mode：{useSlerp}】\n" +
                        $"t = {t:F3}\n" +
                        $"q1: w={q1.w:F3} x={q1.x:F3} y={q1.y:F3} z={q1.z:F3}\n" +
                        $"q2: w={q2.w:F3} x={q2.x:F3} y={q2.y:F3} z={q2.z:F3}\n" +
                        $"cur: w={currentRot.w:F3} x={currentRot.x:F3} y={currentRot.y:F3} z={currentRot.z:F3}";
    }
}