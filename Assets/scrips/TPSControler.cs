using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine; 

public class TPSControler : MonoBehaviour
{
    private CharacterController _controller;
    private Transform _camera;

    private Transform _lookAtPlayer;

    //-----------------Camara---------------------------

    [SerializeField] private GameObject _normalCamara;
    [SerializeField] private GameObject _aimCamara;


    //-----------------Imputs---------------------------
    private float _horizontal;

    private float _vertical; 

    [SerializeField] private float _movementSpeed = 5;

    [SerializeField] private float _jumpHeight = 1; 

   //------------Cosas Gravedad--------------------
    [SerializeField] private float _gravity = -9.81f; 
    [SerializeField] private Vector3 _playerGravity;

    //----------Cosas GroundSensor----------------
    [SerializeField] LayerMask _groundLayer;
    [SerializeField] Transform _sensorPosition;
    [SerializeField] float _sensorRadius = 0.5f;

    [SerializeField] private AxisState xAsis;
    [SerializeField] private AxisState yAsis;


    
    // Start is called before the first frame update
    void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _camera = Camera.main.transform;
        _lookAtPlayer = GameObject.Find("LookAtPlayer").transform;
    }

    // Update is called once per frame
    void Update()
    {
        _horizontal = Input.GetAxis("Horizontal");
        _vertical = Input.GetAxis("Vertical");

        if(Input.GetButtonDown("Fire2"))
        {
            _normalCamara.SetActive(false);
            _aimCamara.SetActive(true);

        }
        else if(Input.GetButtonDown("Fire2"))
        {
            _normalCamara.SetActive(true);
            _aimCamara.SetActive(false);

        }

        if(Input.GetButtonDown("Jump") && IsGrouded())
        {
            Jump();
        }

        
        Movement();
       
        Gravity();
    }

    void Movement()
    {
        Vector3 move = new Vector3(_horizontal, 0, _vertical);

        yAsis.Update(Time.deltaTime);
        xAsis.Update(Time.deltaTime);

        transform.rotation = Quaternion.Euler(0, xAsis.Value, 0);
        _lookAtPlayer.rotation = Quaternion.Euler(yAsis.Value, xAsis.Value, 0);

        if(move != Vector3.zero)
        {
          float targetAngle = Mathf.Atan2(move.x, move.y) * Mathf.Rad2Deg + _camera.eulerAngles.y;
            Vector3 moveDirection = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;

            _controller.Move(moveDirection * _movementSpeed * Time.deltaTime);  
        }
        
    }

    void Gravity()
    {
        if(!IsGrouded())
        {
           _playerGravity.y += _gravity * Time.deltaTime; 
        }
        else if(IsGrouded() && _playerGravity.y < 0)
        {
            _playerGravity.y = -1; 
        }
        

        _controller.Move(_playerGravity * Time.deltaTime);
    }

    void Jump()
    {
        _playerGravity.y = Mathf.Sqrt(_jumpHeight * -2 * _gravity);
    }

    bool IsGrouded()
    {
        
        return Physics.CheckSphere(_sensorPosition.position, _sensorRadius, _groundLayer);
        

    }

}
