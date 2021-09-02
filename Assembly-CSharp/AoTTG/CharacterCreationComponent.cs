using UnityEngine;

public class CharacterCreationComponent : MonoBehaviour
{
    public GameObject manager;
    public CreatePart part;

    public void nextOption()
    {
        manager.GetComponent<CustomCharacterManager>().NextOption(part);
    }

    public void prevOption()
    {
        manager.GetComponent<CustomCharacterManager>().PreviousOption(part);
    }
}
