using UnityEngine;
[AddComponentMenu("HMFPS/PlayerMovement")]

public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;

    public float speed = 12f; // Tốc độ người chơi
    public float gravity = -9.81f * 2;// Trọng lực
    public float jumpHeight = 3f;// Đọ cao khi nhảy

    public Transform groundCheck; // Kiểm tra va chạm với mặt đất
    public float groundDistance = 0.4f;//Khoảng cách ngchoi với mặt đất
    public LayerMask groundMask;

    Vector3 velocity;//Vận tốc cho cú nhảy 
    bool isGrounded;
    bool isMoving; 

    private Vector3 lastPosition = new Vector3(0f,0f,0f);

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        //Kiểm tra xem có đững trên mặt đất
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        // Trả về giá trị mặc định của tốc đọ(velocity)
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // Lấy đầu vào 
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");


        //Tạo một vecto di chuyển
        Vector3 move = transform.right * x + transform.forward * z  ;// (bên phải(right) - mũi tên màu đỏ; phía trước(forward) - mũi tên màu xanh dương

        // Di chuyển người chơi
        controller.Move(move * speed * Time.deltaTime);

        //Kiểm tra ngchoi có nhảy được ko
        if (Input.GetButton("Jump") && isGrounded)
        {
            //Người chơi nhảy lên
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        //Người chơi rơi xuống
        velocity.y += gravity * Time.deltaTime;

        //Thực hiện cú nhảy
        controller.Move(velocity * Time.deltaTime);

        if (lastPosition != gameObject.transform.position && isGrounded == true)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }

        lastPosition = gameObject.transform.position;
    }
}
