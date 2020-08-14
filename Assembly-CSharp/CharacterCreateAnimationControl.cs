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
        play(currentAnimation);
    }

    public void playAttack(string id)
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

    public void toStand()
    {
        if (setup.myCostume.sex == SEX.FEMALE)
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

    private void play(string id)
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
                    play("salute");
                }
                else if (Random.Range(1, 1000) < 350)
                {
                    play("supply");
                }
                else
                {
                    play("dodge");
                }
            }
        }
        else if (base.animation[currentAnimation].normalizedTime >= 1f)
        {
            if (currentAnimation == "attack3_1")
            {
                play("attack3_2");
            }
            else if (currentAnimation == "special_sasha")
            {
                play("run_sasha");
            }
            else
            {
                toStand();
            }
        }
    }
}
