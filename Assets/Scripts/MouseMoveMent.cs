using UnityEngine;
[AddComponentMenu("HMFPS/MouseMoveMent")]

public class MouseMoveMent : MonoBehaviour
{
    public float mouseSensitivity = 500f;

    float xRotation = 0f;
    float yRotation = 0f;

    public float topClamp = - 90f;
    public float bottomClamp = 90f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Khóa con trỏ chuột xuất hiện ở giữa màn hình và làm nó biến mất
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        //Tạo đàu vào cho con chuột
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;// Di chuyển lên xuống se la Mouse X
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;// Di chuyển phải trái se la Mouse Y
        
        //Di chuyển xoay quanh trục X
        xRotation -= mouseY;

        // Kẹp vòng quay
        xRotation = Mathf.Clamp(xRotation, topClamp, bottomClamp);// Giới hạn quay trong (-90f,90f)

        //Di chuyển xoay quanh trục Y
        yRotation += mouseX;

        //Áp dụng vòng quanh vào biến đổi cơ thể thực tế
        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);

    }
}
