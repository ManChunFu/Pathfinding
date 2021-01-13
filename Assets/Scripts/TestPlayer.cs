using UnityEngine;
using UnityEngine.Assertions;


public class TestPlayer : MonoBehaviour
{
    [SerializeField] private float _speed = 2f;
    private float _rotationSpeed = 0.5f;
    private float _rotation = 0f;

    private Animator _anim;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
        Assert.IsNotNull(_anim, "Failed to find Animator component.");

    }

    // Update is called once per frame
    void Update()
    {
        MovementController();
    }

    private void MovementController()
    {
        float rotateInput = Input.GetAxis("Horizontal");

        _rotation += rotateInput * _rotationSpeed;

        transform.eulerAngles = new Vector3(0f, 0f, -_rotation);

        if (Input.GetKey(KeyCode.UpArrow))
        {
            _anim.SetBool("Move_Forward", true);
            transform.Translate(-transform.right * _speed * Time.deltaTime, Space.World);
        }
        else
            _anim.SetBool("Move_Forward", false);

    }
}
