using UnityEngine;

public class KameraTakip : MonoBehaviour
{
    public Transform hedef; // Takip edilecek top
    public Vector3 mesafe = new Vector3(0, 5, -8);

    // (Madde 6: LateUpdate) - Kamera takibi en son yapýlmalý
    void LateUpdate()
    {
        if (hedef != null)
        {
            transform.position = hedef.position + mesafe;
            transform.LookAt(hedef);
        }
    }
}