using UnityEngine;

public class CharacterCreateAnimationControl : MonoBehaviour
{
    private HERO_SETUP setup;
    private string currentAnimation;
    private float timeElapsed;
    private float interval = 10f;

    private void Start()
    {
        setup = base.gameObject.GetComponent<HERO_SETUP>();
        currentAnimation = "stand_levi";
        PlayAnimation(currentAnimation);
    }

    public void PlayAttack(string id)
    {
        switch (id)
        {
            case "mikasa":
                currentAnimation = "attack3_1";
                break;
            case "levi":
                currentAnimation = "attack5";
                break;
            case "sasha":
                currentAnimation = "special_sasha";
                break;
            case "jean":
                currentAnimation = "grabbed_jean";
                break;
            case "marco":
                currentAnimation = "special_marco_0";
                break;
            case "armin":
                currentAnimation = "special_armin";
                break;
            case "petra":
                currentAnimation = "special_petra";
                break;
        }
        base.animation.Play(currentAnimation);
    }

    public void Stand()
    {
        if (setup.myCostume.sex == Sex.Female)
        {
            currentAnimation = "stand";
        }
        else
        {
            currentAnimation = "stand_levi";
        }
        base.animation.CrossFade(currentAnimation, 0.1f);
        timeElapsed = 0f;
    }

    private void PlayAnimation(string id)
    {
        currentAnimation = id;
        base.animation.Play(id);
    }

    private void Update()
    {
        if (currentAnimation == "stand" || currentAnimation == "stand_levi")
        {
            timeElapsed += Time.deltaTime;
            if (timeElapsed > interval)
            {
                timeElapsed = 0f;
                if (Random.Range(1, 1000) < 350)
                {
                    PlayAnimation("salute");
                }
                else if (Random.Range(1, 1000) < 350)
                {
                    PlayAnimation("supply");
                }
                else
                {
                    PlayAnimation("dodge");
                }
            }
        }
        else if (base.animation[currentAnimation].normalizedTime >= 1f)
        {
            if (currentAnimation == "attack3_1")
            {
                PlayAnimation("attack3_2");
            }
            else if (currentAnimation == "special_sasha")
            {
                PlayAnimation("run_sasha");
            }
            else
            {
                Stand();
            }
        }
    }
}
