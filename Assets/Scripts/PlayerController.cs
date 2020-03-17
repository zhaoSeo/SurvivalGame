using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //스피드
    [SerializeField]
    private float walkSpeed;
    [SerializeField]
    private float runSpeed;
    [SerializeField]
    private float crouchSpeed;

    private float applySpeed;

    //점프
    [SerializeField]
    private float jumpForce;

    //상태
    private bool isRun = false;
    private bool isGround = true;
    private bool isCrouch = false;

    //얼마나 앉을지 결정하는 함수
    [SerializeField]
    private float crouchPosY;
    private float originPosY;
    private float applycrouchPosY;

    //지상과 맞닿을 경우
    private CapsuleCollider capsuleCollider;

    //카메라 민감도
    [SerializeField]
    private float lookSensitivity;

    //카메라 한계
    [SerializeField]
    private float cameraRotationLimit;
    private float currentCameraRotationX = 0;

    //필요한 컴포넌트
    [SerializeField]
    private Camera theCamera;
    private Rigidbody myRigid;
    private GunController theGunController;

    void Start()
    {
        capsuleCollider = GetComponent<CapsuleCollider>(); //Plyer의 component
        myRigid = GetComponent<Rigidbody>(); //Player의 component
        theGunController = FindObjectOfType<GunController>();

        applySpeed = walkSpeed;
        originPosY = theCamera.transform.localPosition.y; //카메라의 위치 이동 localPosition -> Player 기준이기 때문
        applycrouchPosY = originPosY;

    }

    void Update()
    {
        IsGround();
        TryRun();
        TryJump();
        TryCrouch();
        Move();
        CameraRotation();
        CharacterRotation();
    }

    private void TryRun()
    {
        if (Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.LeftControl))
        {
            Running();
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            RunningCancel();
        }
    }

    private void TryJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            Jump();
        }
    }

    private void TryCrouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
            Crouch();
        //if (Input.GetKeyUp(KeyCode.LeftControl))
        //    Crouch();
    }

    private void Running()
    {
        if (isCrouch)
            Crouch();

        theGunController.CancelFineSight();

        isRun = true;
        applySpeed = runSpeed;
    }

    private void RunningCancel()
    {
        isRun = false;
        applySpeed = walkSpeed;
    }

    private void Jump()
    {
        if (isCrouch)
            Crouch();
        myRigid.velocity = transform.up * jumpForce;

    }

    private void Crouch()
    {
        isCrouch = !isCrouch;

        if(isCrouch)
        {
            applySpeed = crouchSpeed;
            applycrouchPosY = crouchPosY;
        } else
        {
            applySpeed = walkSpeed;
            applycrouchPosY = originPosY;
        }

        //theCamera.transform.localPosition =
        //    new Vector3(theCamera.transform.localPosition.x, applycrouchPosY, theCamera.transform.localPosition.z);
        //vector값 조
        StartCoroutine(CrouchCoroutine());
    }

    IEnumerator CrouchCoroutine() //병렬처리
    {
        float _posY = theCamera.transform.localPosition.y;
        int count = 0;

        while(_posY != applycrouchPosY)
        {
            count++;
            _posY = Mathf.Lerp(_posY, applycrouchPosY, 0.3f); //시작점에서 끝점으로 높을수록 빠르게 증가, 근사치값
            theCamera.transform.localPosition = new Vector3(0, _posY, 0);
            if(count > 15)
                break;
            yield return null; //1프레임 대기
        }
        theCamera.transform.localPosition = new Vector3(0, applycrouchPosY, 0);
    }
    
    private void IsGround()
    {
        isGround = Physics.Raycast(transform.position, Vector3.down, capsuleCollider.bounds.extents.y + 0.1f);
        //transform 말고 vector 쓰는 이유는 방향이 바뀔 수 있기 때문
        //capsulecollider.bounds 의 절반의 거리 extends
    }

    private void Move()
    {
        float _moveDirX = Input.GetAxisRaw("Horizontal"); //좌우화살표
        float _moveDirZ = Input.GetAxisRaw("Vertical"); //정면 뒤

        Vector3 _moveHorizontal = transform.right * _moveDirX;
        Vector3 _moveVertical = transform.forward * _moveDirZ;

        Vector3 _velocity = (_moveHorizontal + _moveVertical).normalized * applySpeed; // (1, 0, 1) (0.5, 0, 0.5) y = x

        myRigid.MovePosition(transform.position + _velocity * Time.deltaTime); //움직임 순간이동x Time.deltaTime - 0.0016만큼 조금씩 움직임
    }

    private void CharacterRotation()
    {
        // 좌우 캐릭터 회전
        float _yRotation = Input.GetAxis("Mouse X");
        Vector3 _characterRotationY = new Vector3(0f, _yRotation, 0f) * lookSensitivity;
        myRigid.MoveRotation(myRigid.rotation * Quaternion.Euler(_characterRotationY));
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
