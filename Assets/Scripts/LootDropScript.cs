using UnityEngine;

public class LootDropScript : MonoBehaviour
{
    public GameObject lootGameObject;
    public void DropLoot()
    {
        Instantiate(lootGameObject, this.transform.position, new Quaternion());
    }
}
