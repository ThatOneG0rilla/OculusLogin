using UnityEngine;
public class AchievementTrigger : MonoBehaviour
{
    [SerializeField] private string Name;
    [SerializeField] private AchievementTriggerType Type = AchievementTriggerType.OnStart;
    public enum AchievementTriggerType { OnStart, OnTriggerEnter }
    void Start()
    {
        if (Type == AchievementTriggerType.OnStart)
            TriggerAchievement();
    }
    void OnTriggerEnter(Collider other)
    {
        if (Type == AchievementTriggerType.OnTriggerEnter)
            TriggerAchievement();
    }
    private void TriggerAchievement()
    {
        OculusLogin.UnlockAchievement(Name, success =>
        {
            if (success)
            {
                Debug.Log($"Achievement {Name} unlocked!");
            }
            else
                Debug.LogError($"Failed to unlock {Name}");
        });
    }
}