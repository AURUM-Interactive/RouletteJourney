using UnityEngine;

public class DamagePopUpScript : MonoBehaviour
{
    public float TimeVisible = 3f;

    private void Start()
    {
        Destroy(this.gameObject, TimeVisible);
    }
}
