using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public float RotateSpeed = 10f,Speed=10f,ZoomSpeed=10f;
    private float _mult = 1f;


    private void Update()
    {
        float hor = Input.GetAxis("Horizontal");
        float ver = Input.GetAxis("Vertical");

        float rotate = 0f;
        if(Input.GetKey(KeyCode.Q)) {
            rotate = -1f;
        }else if (Input.GetKey(KeyCode.E))
        {
            rotate = 1f;
        }
        _mult=Input.GetKey(KeyCode.LeftShift) ? 2f : 1f;
        transform.Rotate(Vector3.up* RotateSpeed* Time.deltaTime*rotate*_mult, Space.World);
        transform.Translate(new Vector3(hor,0,ver)*Time.deltaTime*_mult*Speed, Space.Self);

        transform.position += transform.up * ZoomSpeed * Time.deltaTime * Input.GetAxis("Mouse ScrollWheel");
        transform.position=new Vector3(transform.position.x,Mathf.Clamp(transform.position.y,-5f,30f),transform.position.z);
    }
}
