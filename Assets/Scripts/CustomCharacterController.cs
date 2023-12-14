using System.Collections;
using Cinemachine;
using UI;
using UnityEngine;

public class CustomCharacterController : MonoBehaviour{ 	
	public Animator anim;
    public Rigidbody rig;
    public Transform mainCamera;
    public CinemachineVirtualCamera virtualCamera;
    public float jumpForce = 3.5f; 
    public float walkingSpeed = 2f;
    public float runningSpeed = 6f;
    public float currentSpeed;
    private float animationInterpolation = 1f;
    public Transform aimTarget;
    public Transform groundChecker;
    public LayerMask notPlayerMask;
    public InventoryManager inventoryManager;
    public QuickslotInventory quickslotInventory;
    public Indicators indicators;

    private CraftManager _craftManager;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        _craftManager = FindObjectOfType<CraftManager>();

        virtualCamera.GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis.m_MaxSpeed =
            PlayerPrefs.GetFloat("Sensetivity");
        virtualCamera.GetCinemachineComponent<CinemachinePOV>().m_VerticalAxis.m_MaxSpeed =
            PlayerPrefs.GetFloat("Sensetivity");
        

    }
    void Run()
    {
        animationInterpolation = Mathf.Lerp(animationInterpolation, 1.5f, Time.deltaTime * 3);
        // anim.SetFloat("x", Input.GetAxis("Horizontal") * animationInterpolation);
        anim.SetFloat("y", Input.GetAxis("Vertical") * animationInterpolation);
        currentSpeed = Mathf.Lerp(currentSpeed, runningSpeed, Time.deltaTime * 3);
    }
    void Walk()
    {
        // Mathf.Lerp - отвчает за то, чтобы каждый кадр число animationInterpolation(в данном случае) приближалось к числу 1 со скоростью Time.deltaTime * 3.
        // Time.deltaTime - это время между этим кадром и предыдущим кадром. Это позволяет плавно переходить с одного числа до второго НЕЗАВИСИМО ОТ КАДРОВ В СЕКУНДУ (FPS)!!!
        animationInterpolation = Mathf.Lerp(animationInterpolation, 1f, Time.deltaTime * 3);
        // anim.SetFloat("x", Input.GetAxis("Horizontal") * animationInterpolation);
        anim.SetFloat("y", Input.GetAxis("Vertical") * animationInterpolation);
        currentSpeed = Mathf.Lerp(currentSpeed, walkingSpeed, Time.deltaTime * 3);
    }

    public void ChangeLayerWeight(float NewLayerWeight)
    {
        StartCoroutine(SmoothLayerWeightChanged(anim.GetLayerWeight(1), NewLayerWeight, 0.3f));
    }

    IEnumerator SmoothLayerWeightChanged(float oldWeight, float newWeight, float changeDuration)
    {
        float elapsed = 0f;
        while (elapsed < changeDuration)
        {
            float currentWeight = Mathf.Lerp(oldWeight, newWeight, elapsed / changeDuration);
            anim.SetLayerWeight(1, currentWeight);
            elapsed += Time.deltaTime;
            yield return null;
        }
        anim.SetLayerWeight(1, newWeight);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (quickslotInventory.activeSlot != null)
            {
                if (quickslotInventory.activeSlot.item != null)
                {
                    if (quickslotInventory.activeSlot.item.itemType == ItemType.Instrument)
                    {
                        if (inventoryManager.isOpened == false && _craftManager.isOpened == false) 
                            anim.SetBool("Hit",true);
                    }
                }
            }
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            anim.SetBool("Hit",false);
        }
        // Устанавливаем поворот персонажа когда камера поворачивается 
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x,mainCamera.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
        
        // Зажаты ли кнопки W и Shift?
        if(Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.LeftShift))
        {
            // Зажаты ли еще кнопки A S D?
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
            {
                // Если да, то мы идем пешком
                Walk();
            }
            // Если нет, то тогда бежим!
            else
            {
                Run();
            }
        }
        // Если W & Shift не зажаты, то мы просто идем пешком
        else
        {
            Walk();
        }
        //Если зажат пробел, то в аниматоре отправляем сообщение тригеру, который активирует анимацию прыжка
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        if (Physics.CheckSphere(groundChecker.position, 0.3f, notPlayerMask))
        {
            anim.SetBool("IsInAir", false);
        }
        else
        {
            anim.SetBool("IsInAir", true);
        }

        Ray desiredTargetRay = mainCamera.GetComponent<Camera>().ScreenPointToRay(new Vector2(Screen.width/2, Screen.height/2));
        Vector3 desiredTargetPosition = desiredTargetRay.origin + desiredTargetRay.direction * 0.7f;
        aimTarget.position = desiredTargetPosition;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        // Здесь мы задаем движение персонажа в зависимости от направления в которое смотрит камера
        // Сохраняем направление вперед и вправо от камеры 
        Vector3 camF = mainCamera.forward;
        Vector3 camR = mainCamera.right;
        // Чтобы направления вперед и вправо не зависили от того смотрит ли камера вверх или вниз, иначе когда мы смотрим вперед, персонаж будет идти быстрее чем когда смотрит вверх или вниз
        // Можете сами проверить что будет убрав camF.y = 0 и camR.y = 0 :)
        camF.y = 0;
        camR.y = 0;
        Vector3 movingVector;
        // Тут мы умножаем наше нажатие на кнопки W & S на направление камеры вперед и прибавляем к нажатиям на кнопки A & D и умножаем на направление камеры вправо
        movingVector = Vector3.ClampMagnitude(camF.normalized * Input.GetAxis("Vertical") * currentSpeed + camR.normalized * Input.GetAxis("Horizontal") * currentSpeed,currentSpeed);
        // Magnitude - это длинна вектора. я делю длинну на currentSpeed так как мы умножаем этот вектор на currentSpeed на 86 строке. Я хочу получить число максимум 1.
        // anim.SetFloat("magnitude", movingVector.magnitude/currentSpeed);
        // Debug.Log(movingVector.magnitude / currentSpeed);
        // Здесь мы двигаем персонажа! Устанавливаем движение только по x & z потому что мы не хотим чтобы наш персонаж взлетал в воздух
        rig.velocity = new Vector3(movingVector.x, rig.velocity.y,movingVector.z);
        // У меня был баг, что персонаж крутился на месте и это исправил с помощью этой строки
        rig.angularVelocity = Vector3.zero;
    }
    public void Jump()
    {
        if (Physics.CheckSphere(groundChecker.position, 0.3f, notPlayerMask))
        {
            anim.SetTrigger("Jump");
            rig.AddForce(Vector3.up *jumpForce , ForceMode.Impulse);
        }
    }

    public void Hit()
    {
        foreach (Transform weapon in quickslotInventory.allWeapons)
        {
            if (weapon.gameObject.activeSelf)
            {
                weapon.gameObject.GetComponent<GatherResources>().GatherResource();
                _craftManager.currentCraftItemDetails.FillItemDetails();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 4) // water
        {
            indicators.isInWater = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 4) // water
        {
            indicators.isInWater = false;
        }
    }
}