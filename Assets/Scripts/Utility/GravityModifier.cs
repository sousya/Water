using UnityEngine;

public class GravityModifier : MonoBehaviour
{
    public float gravityScale = 1.0f; // 设置重力的缩放比例
    public bool useGravity = false;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if(useGravity)
        {
            // 修改物体的重力
            Vector3 newGravity = Physics.gravity * gravityScale;
            rb.AddForce(newGravity, ForceMode.Acceleration);
        }
    }
}

