using UnityEngine;

public class MouseRemove : MonoBehaviour
{
    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
        Destroy(gameObject);
    }
}
