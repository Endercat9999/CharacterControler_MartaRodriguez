using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControler : MonoBehaviour
{
    //---------------Componentes------------------------
    private CharacterController _controller;
    //-----------------Imputs---------------------------
    private float _horizontal;

    private float _vertical; 

    [SerializeField] private float _movementSpeed = 5;

    private float _turnSmoothVelocity;
    [SerializeField] private float _turnSmoothTime = 0.5f;
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
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _horizontal = Input.GetAxis("Horizontal");
        _vertical = Input.GetAxis("Vertical");

        Movement();
        Gravity();
    }


    void Movement()
    {
        Vector3 direction = new Vector3(_horizontal, 0, _vertical);

        if(direction !=Vector3.zero)
        {
          float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
          float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity, _turnSmoothTime);
          transform.rotation = Quaternion.Euler(0, smoothAngle, 0);  

          _controller.Move(direction * _movementSpeed * Time.deltaTime);



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
