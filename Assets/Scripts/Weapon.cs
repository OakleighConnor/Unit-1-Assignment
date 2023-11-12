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
    HelperScript helper;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        helper = gameObject.AddComponent<HelperScript>();
    }

    // Update is called once per frame
    void Update()
    {
        AimMouse();
    }
    //This is self explainatory
    void AimMouse()
    {
        Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg - 90f;
        Quaternion rotation = Quaternion.AngleAxis(angle, - Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, _rotateSpeed * Time.deltaTime);
        Vector2 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _aimCursor.transform.position = cursorPos;
        if (_aimCursor.transform.position.x <= transform.position.x)
        {
            transform.localScale = new Vector3(0.8f, -0.8f, 1);
        }
        else
        {
            transform.localScale = new Vector3(0.8f, 0.8f, 1);
        }
    }
}
