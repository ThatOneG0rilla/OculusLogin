using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
public class CurrencyPurchase : MonoBehaviour
{
    [SerializeField] private string Sku;
    [SerializeField] private string CurrencyCode = "GC";
    [SerializeField] private int CurrencyAmount = 100;
    void OnTriggerEnter(Collider other)
    {
        OculusLogin.BuyProduct(Sku, success =>
        {
            if (success)
            {
                Debug.Log($"Successfully purchased: {Sku}");

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
                    }
                );
            }
            else
            {
                Debug.LogError($"Purchase failed for: {Sku}");
            }
        });
    }
}