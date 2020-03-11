using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float walkSpeed;

    [SerializeField]
    private float lookSensitivity;

    [SerializeField]
    private float cameraRotationLimit;
    private float currentCameraRotationX = 0;

    [SerializeField]
    private Camera theCamera;




    private Rigidbody myRigid;

    void Start()
    {
        myRigid = GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {
        Move();
        CameraRotation();
        CharacterRotation();
    }

    private void Move()
    {
        float _moveDirX = Input.GetAxisRaw("Horizontal"); //좌우화살표
        float _moveDirZ = Input.GetAxisRaw("Vertical"); //정면 뒤

        Vector3 _moveHorizontal = transform.right * _moveDirX;
        Vector3 _moveVertical = transform.forward * _moveDirZ;

        Vector3 _velocity = (_moveHorizontal + _moveVertical).normalized * walkSpeed; // (1, 0, 1) (0.5, 0, 0.5) y = x

        myRigid.MovePosition(transform.position + _velocity * Time.deltaTime); //움직임 순간이동x Time.deltaTime - 0.0016만큼 조금씩 움직임
    }
    private void CharacterRotation()
    {
        // 좌우 캐릭터 회전
        float _yRotation = Input.GetAxis("Mouse X");
        Vector3 _characterRotationY = new Vector3(0f, _yRotation, 0f) * lookSensitivity;
        myRigid.MoveRotation(myRigid.rotation * Quaternion.Euler(_characterRotationY));
        Debug.Log(myRigid.rotation);
        Debug.Log(myRigid.rotation.eulerAngles);
    }

    private void CameraRotation()
    {
        //상하 카메라 회전
        float _xRotation = Input.GetAxisRaw("Mouse Y"); // 마우스 2차원 x, y 값
        float _cameraRotationX = _xRotation * lookSensitivity;
        currentCameraRotationX -= _cameraRotationX;
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit); //-45, 45에 가둠
        theCamera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f          ); //x, y, z
    }
    
}
