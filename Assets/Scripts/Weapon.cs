using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private float _rotateSpeed;
    [SerializeField] GameObject _aimCursor;
    [SerializeField] private LineRenderer lr;
    public GameObject AimCursor;
    Vector3 _startPos;
    Vector3 _endPos;
    Vector3 _mousePos;
    Vector3 _mouseDir;
    float _lineMax = 5;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        AimMouse();
        //AimLine();
    }


    void AimMouse()
    {
        Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg - 90f;
        Quaternion rotation = Quaternion.AngleAxis(angle, - Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, _rotateSpeed * Time.deltaTime);
        Vector2 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _aimCursor.transform.position = cursorPos;
    }

    void AimLine()
    {
        _mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _mouseDir = _mousePos - transform.position;
        _mouseDir.z = 0;
        _mouseDir.Normalize();

        lr.enabled = true;

        _startPos = transform.position;
        _startPos.z = 0;
        lr.SetPosition(0, _startPos);
        _endPos = _mousePos;
        _endPos.z = 0;
        //change the length of the line renderer
        float lineLength = Mathf.Clamp(Vector2.Distance(_startPos, _endPos), 0, _lineMax);
        _endPos = _startPos + (_mouseDir * lineLength);
        lr.SetPosition(1, _endPos);
    }
}
