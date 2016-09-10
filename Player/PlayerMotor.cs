using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : MonoBehaviour {

    private Vector3 veloctiy = Vector3.zero;
    private Vector3 rotation = Vector3.zero;
    private float cameraRotationX = 0f;
    private float currentCameraRotationX = 0f;
    private Vector3 thrustor = Vector3.zero;

    [Header("Cursor Settings")]
    [SerializeField]
    private Texture2D cursorIcon;

    [Header("Camera Settings:")]
    [SerializeField]
    private Camera cam;
    [SerializeField]
    private float cameraRotationLimit = 85f;

    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    //Gets A Movement Vector
    public void Move(Vector3 _veloctiy)
    {
        veloctiy = _veloctiy;
    }
    //Gets A Rotation Vector
    public void Rotate(Vector3 _rotation)
    {
        rotation = _rotation;
    }
    //Gets A Camera Rotation Vector
    public void RotateCamera(float _cameraRotateX)
    {
        cameraRotationX = _cameraRotateX;
    }

    //Geta A Thrustor Vector
    public void ApplyThrustor(Vector3 _thurstor)
    {
        thrustor = _thurstor;
    }

    //Run Every Physics Iteration
    void FixedUpdate()
    {
        PerformMovement();
        PerformRotation();
    }

    //Perform Movement based on veloctiy variable
    void PerformMovement()
    {
        if(veloctiy != Vector3.zero)
        {
            rb.MovePosition(rb.position + veloctiy * Time.fixedDeltaTime);
        }

        if(thrustor != Vector3.zero)
        {
            rb.AddForce(thrustor * Time.fixedDeltaTime, ForceMode.Acceleration);
        }
    }
    //Perform Rotation based on veloctiy variable
    void PerformRotation()
    {
        rb.MoveRotation(rb.rotation * Quaternion.Euler(rotation));
        if (cam != null)
        {
            //Set our rotation and clamp it
            currentCameraRotationX += cameraRotationX;
            currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);
            //Apply rotation to the transform of our camera
            cam.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
            //Apply Mouse Return In Zero
            //Cursor.SetCursor(cursorIcon, new Vector2(0, 0), CursorMode.Auto);
            
        }
    }

}
