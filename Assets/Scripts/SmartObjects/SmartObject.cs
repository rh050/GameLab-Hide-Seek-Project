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
        SmartObjectManager.Instance.RegisterSmartObject(this); 
    }
    
    public void Activate(GameObject player)
    {
        if (!isActive) return;

        PlayerController playerController = player.GetComponent<PlayerController>();
        if (playerController == null)
        {
            Debug.LogWarning("Player does not have PlayerController!");
            return;
        }

        if (isTrap)
        {
            Debug.Log("Trap activated!");
            playerController.ModifySpeedTemporary(0.5f, effectDuration); 
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
        if (moveDirection == Vector3.zero)
        {
            Debug.LogWarning($"{gameObject.name}: moveDirection is zero, object won't move!");
            yield break;
        }

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
        isActive = true; // הפיכת האובייקט לפעיל מחדש
    }
}
