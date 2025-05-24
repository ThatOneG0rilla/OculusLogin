using UnityEngine;
using Oculus.Platform;
using System;
using Photon.Pun;
using PlayFab.ClientModels;
using PlayFab;

public class OculusLogin : MonoBehaviour
{
    public static string OculusId { get; private set; }
    public static string OculusProfile { get; private set; }
    public static string OculusNonce { get; private set; }
    public bool OculusNameToUser;
    void Awake()
    {
        InitializeOculusPlatform();
        DontDestroyOnLoad(gameObject);
    }
    void InitializeOculusPlatform()
    {
        try
        {
            Core.Initialize();
            Entitlements.IsUserEntitledToApplication().OnComplete(msg =>
            {
                if (msg.IsError)
                {
                    Debug.LogError($"Entitlement Check Failed: {msg.GetError().Message}");
                    return;
                }

                Users.GetLoggedInUser().OnComplete(msg =>
                {
                    if (msg.IsError)
                    {
                        Debug.LogError($"User Data Fetch Failed: {msg.GetError().Message}");
                        return;
                    }

                    OculusId = msg.Data.ID.ToString();
                    OculusProfile = msg.Data.OculusID;
                    Users.GetUserProof().OnComplete(msg =>
                    {
                        if (msg.IsError)
                        {
                            Debug.LogError($"Nonce creation Failed: {msg.GetError().Message}");
                            return;
                        }

                        OculusNonce = msg.Data.Value;

                        if (OculusNameToUser)
                            OculusNameToUserName();
                    });
                });
            });
        }
        catch (Exception e)
        {
            Debug.LogError($"Initialization Failed: {e.Message}");
        }
    }
    public static void OculusNameToUserName()
    {
        if (string.IsNullOrEmpty(OculusProfile))
        {
            Debug.LogWarning("OculusProfile is null or empty. Cannot set username.");
            return;
        }

        PhotonNetwork.NickName = OculusProfile;
        PlayerPrefs.SetString("Username", OculusProfile);
        PlayerPrefs.Save();

        var request = new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = OculusProfile
        };

        PlayFabClientAPI.UpdateUserTitleDisplayName(request,
            result => Debug.Log("PlayFab display name changed to Oculus Name."),
            error => Debug.LogError("PlayFab display name changed failed: " + error.GenerateErrorReport())
        );
    }
    public static void UnlockAchievement(string achievementName, Action<bool> callback)
    {
        if (string.IsNullOrEmpty(achievementName))
        {
            callback.Invoke(false);
            return;
        }

        Achievements.Unlock(achievementName).OnComplete(msg =>
        {
            bool success = !msg.IsError;
            callback.Invoke(success);
        });
    }
    public static void BuyProduct(string sku, Action<bool> callback)
    {
        if (string.IsNullOrEmpty(sku))
        {
            callback.Invoke(false);
            return;
        }

        IAP.LaunchCheckoutFlow(sku).OnComplete(msg =>
        {
            bool IsSuccess = !msg.IsError;
            callback.Invoke(IsSuccess);
        });
    }
    public static void CheckOwnership(string SKU, Action<bool> callback)
    {
        if (string.IsNullOrEmpty(SKU))
        {
            callback.Invoke(false);
            return;
        }

        IAP.GetViewerPurchases().OnComplete(Msg =>
        {
            foreach (var purchase in Msg.GetPurchaseList())
            {
                if (purchase.Sku == SKU)
                {
                    if (purchase.Sku.Equals(SKU, StringComparison.OrdinalIgnoreCase))
                    {
                        callback.Invoke(true);
                        return;
                    }
                }
                callback.Invoke(false);
            }
        });
    }
}