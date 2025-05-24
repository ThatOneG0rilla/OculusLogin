using Oculus.Platform;
using UnityEngine;
public class IAPTrigger : MonoBehaviour
{
    [SerializeField] private string Sku;
    [SerializeField] private bool Consume = true;
    [SerializeField] private GameObject ObjectToEnable;
    void Start()
    {
        OculusLogin.CheckOwnership(Sku, owns =>
        {
            ObjectToEnable.SetActive(owns);
        });
    }
    void OnTriggerEnter(Collider other)
    {
        OculusLogin.BuyProduct(Sku, success =>
        {
            if (success)
            {
                Debug.Log($"Successfully purchased: {Sku}");

                OculusLogin.CheckOwnership(Sku, owns =>
                {
                    ObjectToEnable.SetActive(owns);
                });
            }
            else
                Debug.LogError($"Purchase failed for: {Sku}");
        });
    }
}