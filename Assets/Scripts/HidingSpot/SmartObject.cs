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
            //PLAYER SLOWED DOWN
            
            Debug.Log("Trap activated!");
            player.GetComponent<PlayerController>().ModifySpeed(-2f, effectDuration);
            StartCoroutine(ResetTrap());
            
        }

        if (isMovable)
        {
            Debug.Log("Object is moving!");
            StartCoroutine(MoveObject());
            StartCoroutine(ResetTrap());
        }
        isActive = false; 

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
    
    private IEnumerator ResetTrap()
    {
        yield return new WaitForSeconds(effectDuration);
        isActive = true;
    }
}
