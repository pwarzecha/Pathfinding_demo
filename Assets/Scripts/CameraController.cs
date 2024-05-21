using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float speed = 20f;
    [SerializeField] private Vector2 positionLimit;
    [SerializeField] private float scrollSpeed = 20f;
    [SerializeField] private float scrollMinY = 3;
    [SerializeField] private float scrollMaxY = 30;

    private void Update()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        MoveCamera();
        if (Input.GetMouseButtonDown(1))
            HandleInput();
    }

    private void MoveCamera()
    {
        Vector3 pos = transform.position;
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        pos.x += moveX * speed * Time.deltaTime;
        pos.z += moveZ * speed * Time.deltaTime;

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        pos.y -= scroll * scrollSpeed * 100f * Time.deltaTime;

        pos.x = Mathf.Clamp(pos.x, -positionLimit.x, positionLimit.x);
        pos.y = Mathf.Clamp(pos.y, scrollMinY, scrollMaxY);
        pos.z = Mathf.Clamp(pos.z, -positionLimit.y, positionLimit.y);

        transform.position = pos;
    }
    private void HandleInput()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.TryGetComponent(out GridTile tile))
            {
                tile.ToggleState();
            }
        }
    }
}
