using TMPro;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance;

    [SerializeField] float walkingSpeed;

    Rigidbody2D rb;
    Vector3 playerMovement;
    Animator anim;

    [SerializeField] Animator coinAnimator;

    public GameObject[] Clothes; // Naked, Clothes, Robe
    public GameObject[] Hats;// Hair,  Hat, Wizzard Hat

     public InventoryItem EquippedBody;
     public InventoryItem EquippedHead;

    public GameObject Body;
    public GameObject Head;

    bool canInteract = false;

    Collider2D currentColission;

    public GameObject InteractionPanel;
    public TMP_Text InteractionText;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }


    #region Movement
    private void FixedUpdate()
    {
        playerMovement = Vector3.zero;
        playerMovement.x = Input.GetAxisRaw("Horizontal");
        playerMovement.y = Input.GetAxisRaw("Vertical");

        UpdateAnimationOnMove();
    }
    void UpdateAnimationOnMove()
    {
        rb.MovePosition(transform.position + playerMovement * walkingSpeed * Time.deltaTime);

        anim.SetFloat("Horizontal", playerMovement.x);
        anim.SetFloat("Vertical", playerMovement.y);
        anim.SetFloat("Speed", playerMovement.sqrMagnitude);

        if (Body != null)
        {
            Body.GetComponent<Animator>().SetFloat("Horizontal", playerMovement.x);
            Body.GetComponent<Animator>().SetFloat("Vertical", playerMovement.y);
            Body.GetComponent<Animator>().SetFloat("Speed", playerMovement.sqrMagnitude);
        }
        if (Head != null)
        {
            Head.GetComponent<Animator>().SetFloat("Horizontal", playerMovement.x);
            Head.GetComponent<Animator>().SetFloat("Vertical", playerMovement.y);
            Head.GetComponent<Animator>().SetFloat("Speed", playerMovement.sqrMagnitude);
        }
    }

    #endregion

    #region Interaction
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            Inventory.Instance.OpenInventory(false);
        }
    }

    void Interact()
    {
        if (canInteract && currentColission.gameObject.tag == "Chest")
        {
            currentColission.GetComponent<Chest>().OpenChest();
            PlayCoinAnimation();
        }
        if (canInteract && currentColission.gameObject.tag == "Merchant")
        {
            Inventory.Instance.OpenInventory(true);
        }
    }

    void PlayCoinAnimation()
    {
        coinAnimator.gameObject.SetActive(true);
        coinAnimator.Play("Coin Rotate");
    }

    #endregion

    #region Trigger Collision Controller

    private void OnTriggerStay2D(Collider2D collision)
    {
        canInteract = true;
        currentColission = collision;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        canInteract = true;
        currentColission = collision;
        InteractionPanel.SetActive(true);
        InteractionText.text = "Press E to interact with " + collision.tag;

        Debug.Log("Character can interact with " + collision.name);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        canInteract = false;
        currentColission = null;
        InteractionPanel.SetActive(false);
        Debug.Log("Character can't interact with " + collision.name + " anymore");
    }

    #endregion

    #region update appearance

    public void UpdateAppearance()
    {
        UpdateHat();
        UpdateClothes();
    }

    public void UpdateHat()
    {
        Debug.Log("Update Hat");

        for (int i = 0; i < Hats.Length; i++)
        {
            Hats[i].SetActive(false);
        }

        if (EquippedHead == null)
        {
            Debug.Log("No Hat");
            Head = Hats[0];
            Hats[0].SetActive(true);
        }
        else
        {
            Debug.Log("HAT");
            
            int itemID = EquippedHead.item.ID;
            Debug.Log("HAT ID " + itemID);
            Hats[itemID].SetActive(true);
            Head = Hats[EquippedHead.item.ID];
        }
    }
    public void UpdateClothes()
    {
        for (int i = 0; i < Clothes.Length; i++)
        {
            Clothes[i].SetActive(false);
        }

        Debug.Log("Update Clothes");
        if (EquippedBody == null)
        {
            Debug.Log("No CLothes");
            Body = Clothes[0];
            Clothes[0].SetActive(true);
        }
        else
        {
            Debug.Log("CLOTHES");
            
            int itemID = EquippedBody.item.ID;
            Clothes[itemID].SetActive(true);
            Body = Clothes[EquippedBody.item.ID];
        }


        
    }
    #endregion
}

