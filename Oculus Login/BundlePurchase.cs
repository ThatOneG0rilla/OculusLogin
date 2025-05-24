using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
public class BundlePurchase : MonoBehaviour
{
    [SerializeField] private string Sku;
    [SerializeField] private string CurrencyCode = "GC";
    [SerializeField] private int CurrencyAmount = 100;
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

                    if (owns)
                    {
                        var request = new AddUserVirtualCurrencyRequest
                        {
                            VirtualCurrency = CurrencyCode,
                            Amount = CurrencyAmount
                        };
                        PlayFabClientAPI.AddUserVirtualCurrency(request,
                        result =>
                        {
                            Debug.Log($"Currency granted! New balance: {result.Balance} {CurrencyCode}");
                        },
                        error =>
                        {
                           Debug.LogError($"Currency grant failed: {error.GenerateErrorReport()}");
                        });
                    }
                });
            }
            else
            {
                Debug.LogError($"Purchase failed for: {Sku}");
            }
        });
    }
}