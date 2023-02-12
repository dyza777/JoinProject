using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Touch touch;
    private Vector2 touchPosition;

    [SerializeField] private GameObject cinemachineCamera;
    public float CharacterMovingBorder { get; private set; } = 9f;

    private Animator m_Animator;
    private Rigidbody rb;
    public float MovingSpeed = 60f;
    public float TransverseSpeed = 0.3f;
    private bool isPlayer = false;


    static readonly int speedHash = Animator.StringToHash("Speed");
    void Start()
    {
        if (gameObject.CompareTag("Player"))
        {
            isPlayer = true;
        }
        m_Animator = this.GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    public void RunToBoss(Vector3 bossPosition)
    {
        Vector3 target = new Vector3(bossPosition.x, 0, bossPosition.z);
        Vector3 newPos = Vector3.MoveTowards(rb.position, target, Time.deltaTime * MovingSpeed);
        rb.MovePosition(newPos);
        m_Animator.SetFloat(speedHash, 1);

        if (Vector3.Distance(bossPosition, this.transform.position) <= 5f)
        {
            // m_Animator.SetFloat(k_HashSpeed,0);
            m_Animator.SetBool("isWin", true);
            cinemachineCamera.SetActive(false);
        }
    }




    void Update()
    {
        if (isPlayer)
        {
            float xPos = Mathf.Clamp(transform.position.x, -CharacterMovingBorder, CharacterMovingBorder);
            transform.position = new Vector3(xPos, transform.position.y, transform.position.z);
        }

        TouchControl();
        KeyboardControl();

    }

    public void TouchControl()
    {
        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                touchPosition = touch.position;
            }
            else if (touch.phase == TouchPhase.Stationary)
            {
                SetSpeed(MovingSpeed);
                if (isPlayer)
                {
                    var deltaX = touch.position.x - touchPosition.x;
                    var xShift = deltaX * Time.deltaTime * TransverseSpeed;
                    if (xShift < 0 && !GameplayManager.Instance.CanMoveLeft || xShift > 0 && !GameplayManager.Instance.CanMoveRight)
                    {
                        touchPosition = touch.position;
                        return;
                    }
                    transform.Translate(xShift, 0, 0);
                }

                touchPosition = touch.position;
            }
        }
        else
        {
            SetSpeed(0);
        }
    }

    public void KeyboardControl()
    {
        if (Input.GetMouseButtonDown(0))
        {
            touchPosition = Input.mousePosition;
        }
        else if (Input.GetMouseButton(0))
        {
            SetSpeed(MovingSpeed);
            if (isPlayer)
            {
                var deltaX = Input.mousePosition.x - touchPosition.x;
                var xShift = deltaX * Time.deltaTime * TransverseSpeed;
                if (xShift < 0 && !GameplayManager.Instance.CanMoveLeft || xShift > 0 && !GameplayManager.Instance.CanMoveRight) {
                    touchPosition = Input.mousePosition;
                    return;
                } 
                transform.Translate(xShift, 0, 0);
            }
            touchPosition = Input.mousePosition;
        }
        else
        {
            SetSpeed(0);
        }
    }

    public void SetSpeed(float Speed)
    {
        m_Animator.SetFloat(speedHash, Speed);
    }
}

