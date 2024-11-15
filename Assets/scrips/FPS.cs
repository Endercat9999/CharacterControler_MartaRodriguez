using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPS : MonoBehaviour
{
    //---------------Componentes------------------------
    private CharacterController _controller;
    private Transform _camera;
    //-----------------Imputs---------------------------
    private float _horizontal;

    private float _vertical; 
    private float _xRotation; 
    private float _yRotation; 


    [SerializeField] private float _sensitivity = 100;   
    [SerializeField] private float _movementSpeed = 5;
    [SerializeField] private float _pushForce = 10; 
    [SerializeField] private float _jumpHeight = 1; 



   //------------Cosas Gravedad--------------------
    [SerializeField] private float _gravity = -9.81f; 
    [SerializeField] private Vector3 _playerGravity;

    //----------Cosas GroundSensor----------------
    [SerializeField] LayerMask _groundLayer;
    [SerializeField] Transform _sensorPosition;
    [SerializeField] float _sensorRadius = 0.5f;







    void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _camera = Camera.main.transform;
    }
    

    // Update is called once per frame
    void Update()
    {
        _horizontal = Input.GetAxis("Horizontal");
        _vertical = Input.GetAxis("Vertical");

        Movement();

        if(Input.GetButtonDown("Jump") && IsGrouded())
        {
            Jump();
        }

        Gravity();
    }


    void Movement()
    {
        float mouseX = Input.GetAxis("Mouse X") * _sensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * _sensitivity * Time.deltaTime;

        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -90, 90);

        _yRotation += mouseX;


        _camera.localRotation = Quaternion.Euler(_xRotation, 0, 0);

        transform.Rotate(Vector3.up * mouseX);

        Vector3 move = transform.right * _horizontal + transform.forward * _vertical;
        _controller.Move(move * _movementSpeed * Time.deltaTime);
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

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(_sensorPosition.position, _sensorRadius);

    }
}