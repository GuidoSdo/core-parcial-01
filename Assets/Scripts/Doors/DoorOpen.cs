using UnityEngine;

public class DoorManager : MonoBehaviour
{

    public bool isClosed = true;
    public string keyName;
    private bool isOpen = false;
    public Transform doorPivot;
    public float openAngle = 90f;
    public float moveX = 1f;
    public float speed = 1f;

    private Quaternion closedRotation;
    private Quaternion openRotation;
    private Vector3 closedPosition;
    private Vector3 openPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        closedRotation = doorPivot.localRotation;
        openRotation = Quaternion.Euler(doorPivot.localEulerAngles + new Vector3(0, openAngle, 0));

        closedPosition = doorPivot.localPosition;
        openPosition = closedPosition + new Vector3(0, 0, moveX);
    }



    public void TryOpenDoor(Inventory inventory)
    {
        if (keyName != "")
        {
            if (!inventory.HasKey(keyName))
            {
                Debug.Log("No tenes la llave correspondiente.");
                return;
            }

        }
        if (isClosed)
        {
            isClosed = false;
            ToggleDoor();
        }
        else
        {
            ToggleDoor();
        }
    }


    void ToggleDoor()
    {
        isOpen = !isOpen;
        StopAllCoroutines();
        StartCoroutine(RotateDoor(isOpen ? openRotation : closedRotation, isOpen ? openPosition : closedPosition));
    }
    System.Collections.IEnumerator RotateDoor(Quaternion targetRot, Vector3 targetPos)
    {
        while (Quaternion.Angle(doorPivot.localRotation, targetRot) > 0.1f || Vector3.Distance(doorPivot.localPosition, targetPos) > 0.01f)
        {
            doorPivot.localRotation = Quaternion.Slerp(doorPivot.localRotation, targetRot, Time.deltaTime * speed);
            doorPivot.localPosition = Vector3.Lerp(doorPivot.localPosition, targetPos, Time.deltaTime * speed); 
            yield return null;
        }
        doorPivot.localRotation = targetRot;
        doorPivot.localPosition = targetPos;
    }



}
