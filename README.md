# Introduction
Steps:
1. Import the package.
2. Ensure oculus api features are enabled
3. Add OculusLogin on an empty object
4. Change bool to what you want
5. All other scripts are self explanatory. Tutorial soon

The package offers a range of features, including:
- Access to oculus id, oculus pfp, oculus name.
- Logs the player into oculus
- Syncs oculus username to playfab username, and photon nickname
- Simplified achivements, and iap.
- Ownership checking of iap items (some security)

## Script Usage
### `OculusNameToUserName`
- Updates players username to oculus username
Example:
```csharp
OculusLogin.SetCosmetic();
```

### `UnlockAchievement`
- Unlocks an achivement through oculus
Example:
```csharp
OculusLogin.BuyProduct(Name, success =>
```

### `BuyProduct`
- Buys a sku
Example:
```csharp
OculusLogin.BuyProduct(Sku, success =>
```

### `CheckOwnership`
- Checks ownership of a sku
Example:
```csharp
OculusLogin.BuyProduct(Sku, success =>
```

# Info
I will be posting future versions in [Discord](https://discord.gg/gorillasdevhub). Join to get updates.
