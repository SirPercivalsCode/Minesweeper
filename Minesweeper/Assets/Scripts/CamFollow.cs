using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CamFollow : MonoBehaviour
{
    private float speed;
    public float runSpeed;
    public float walkSpeed;

    private float horizontal;
    private float vertical;

    private Rigidbody2D rb;

    public float minFov;
    public float maxFov;
    public float sensitivity;

    public CinemachineVirtualCamera vcam;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        speed = walkSpeed;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftShift)){
            speed = walkSpeed;
        }
        else if(Input.GetKeyUp(KeyCode.LeftShift))
        {
            speed = runSpeed;
        }

        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");


        float fov = vcam.m_Lens.OrthographicSize;
        fov -= Input.GetAxis("Mouse ScrollWheel") * sensitivity;
        fov = Mathf.Clamp(fov, minFov, maxFov);
        vcam.m_Lens.OrthographicSize = fov;
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(horizontal * speed, vertical * speed);
    }
}
