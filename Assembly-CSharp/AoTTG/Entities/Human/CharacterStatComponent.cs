using UnityEngine;

public class CharacterStatComponent : MonoBehaviour
{
    public GameObject manager;
    public CreateStat type;

    public void nextOption()
    {
        manager.GetComponent<CustomCharacterManager>().NextStatOption(type);
    }

    public void prevOption()
    {
        manager.GetComponent<CustomCharacterManager>().PreviousStatOption(type);
    }
}
