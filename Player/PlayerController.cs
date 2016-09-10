using UnityEngine;

//[RequireComponent(typeof(ConfigurableJoint))]
[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour {

    [Header("Player Setting:")]
    [SerializeField]
    private float speed = 5f;
    [SerializeField]
    private float lockSensitivty = 3f;
    [SerializeField]
    private float thrustorForce = 1000f;

    [Header("Spring Setting:")]
    [SerializeField]
    private JointDriveMode jointMode = JointDriveMode.Position;
    [SerializeField]
    private float jointSpring = 20f;
    [SerializeField]
    private float jointMaxForce = 40f;

    private PlayerMotor motor;
    private ConfigurableJoint joint;

    void Start()
    {
        motor = GetComponent<PlayerMotor>();
        //joint = GetComponent<ConfigurableJoint>();
    }


    void Update()
    {
        if (PauseMenu.IsOn == true)
        {
            motor.Move(Vector3.zero);
            motor.Rotate(Vector3.zero);
            motor.RotateCamera(0);
            motor.ApplyThrustor(Vector3.zero);
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            return;
        }

        if (!Input.GetKeyDown(KeyCode.LeftControl))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = true;
        }else
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }

        //Calculate Movement Velocity as a 3D Vector
        float _xMove = Input.GetAxisRaw("Horizontal");
        float _yMove = Input.GetAxisRaw("Vertical");
        Vector3 _MoveHorizontal = transform.right * _xMove;
        Vector3 _MoveVertical = transform.forward * _yMove;
        //Final Movement Vector
        Vector3 _veloctiy = (_MoveHorizontal + _MoveVertical).normalized * speed;
        //Apply Movement
        motor.Move(_veloctiy);

        //Calculate Rotation as a 3D Vector: (turing around)
        float _yRot = Input.GetAxis("Mouse X");
        //Final Rotation Vector
        Vector3 _rotation = new Vector3(0f, _yRot, 0f) * lockSensitivty;
        //Apply Rotation
        motor.Rotate(_rotation);

        //Calculate camera as a 3D Vector
        float _xRot = Input.GetAxis("Mouse Y");
        //Final Camera Vector
        float _cameraRotation = -_xRot * lockSensitivty;
        //Apply Camera
        motor.RotateCamera(_cameraRotation);

        //Calculate thrustor as a 3D Vector
        Vector3 _thrustorForce = Vector3.zero;
        if (Input.GetButton("Jump"))
        {
            //_thrustorForce = Vector3.up * thrustorForce;
            //SetJointSetting(0f);
        }else
        {
            //SetJointSetting(jointSpring);
        }

        //Apply thrustor
        //motor.ApplyThrustor(_thrustorForce);

    }

    private void SetJointSetting(float _jointSpring)
    {
        joint.yDrive = new JointDrive {
            mode = jointMode,
            positionSpring = _jointSpring,
            maximumForce = jointMaxForce };
    }

}
