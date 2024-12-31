using System.Collections;
using UnityEngine;

public class SmartObject : MonoBehaviour
{
    public bool isTrap; 
    public bool isMovable; 
    public Vector3 moveDirection; 
    public float effectDuration = 5f; 
    private bool isActive = true;

    void Start()
    {
        //Mediator
        GameMediator.Instance.RegisterSmartObject(this);
        
    }

    public void Activate(GameObject player)
    {
        if (!isActive) return;

        if (isTrap)
        {
            Debug.Log("Trap activated!");
            //PLAYER SLOWED DOWN
            PlayerController playerController = player.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.ModifySpeed(-3f, effectDuration);
            }
        }

        if (isMovable)
        {
            Debug.Log("Object is moving!");
            

            StartCoroutine(MoveObject());
        }

        isActive = true; //loop or once
    }

    private IEnumerator MoveObject()
    {
        float elapsedTime = 0f;
        while (elapsedTime < effectDuration)
        {
            transform.position += moveDirection * Time.deltaTime;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}
