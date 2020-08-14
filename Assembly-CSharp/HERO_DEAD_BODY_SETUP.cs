using UnityEngine;

public class HERO_DEAD_BODY_SETUP : MonoBehaviour
{
    public GameObject col_head;
    public GameObject col_chest;
    public GameObject col_upper_arm_l;
    public GameObject col_lower_arm_l;
    public GameObject col_upper_arm_r;
    public GameObject col_lower_arm_r;
    public GameObject col_thigh_l;
    public GameObject col_thigh_r;
    public GameObject col_shin_l;
    public GameObject col_shin_r;
    public GameObject head;
    public GameObject chest;
    public GameObject leg;
    public GameObject hand_l;
    public GameObject hand_r;
    public GameObject blood_upper;
    public GameObject blood_lower;
    public GameObject blood_upper1;
    public GameObject blood_upper2;
    public GameObject blood_arm_l;
    public GameObject blood_arm_r;
    private float lifetime = 15f;

    private void Update()
    {
        lifetime -= Time.deltaTime;
        if (lifetime <= 0f)
        {
            base.gameObject.GetComponent<HERO_SETUP>().deleteCharacterComponent2();
            Object.Destroy(base.gameObject);
        }
    }

    public void init(string aniname, float time, BODY_PARTS part)
    {
        base.animation.Play(aniname);
        base.animation[aniname].normalizedTime = time;
        base.animation[aniname].speed = 0f;
        switch (part)
        {
            case BODY_PARTS.UPPER:
                col_upper_arm_l.GetComponent<CapsuleCollider>().enabled = false;
                col_lower_arm_l.GetComponent<CapsuleCollider>().enabled = false;
                col_upper_arm_r.GetComponent<CapsuleCollider>().enabled = false;
                col_lower_arm_r.GetComponent<CapsuleCollider>().enabled = false;
                col_thigh_l.GetComponent<CapsuleCollider>().enabled = false;
                col_shin_l.GetComponent<CapsuleCollider>().enabled = false;
                col_thigh_r.GetComponent<CapsuleCollider>().enabled = false;
                col_shin_r.GetComponent<CapsuleCollider>().enabled = false;
                Object.Destroy(leg);
                Object.Destroy(hand_l);
                Object.Destroy(hand_r);
                Object.Destroy(blood_lower);
                Object.Destroy(blood_arm_l);
                Object.Destroy(blood_arm_r);
                base.gameObject.GetComponent<HERO_SETUP>().createHead2();
                base.gameObject.GetComponent<HERO_SETUP>().createUpperBody2();
                break;
            case BODY_PARTS.LOWER:
                col_upper_arm_l.GetComponent<CapsuleCollider>().enabled = false;
                col_lower_arm_l.GetComponent<CapsuleCollider>().enabled = false;
                col_upper_arm_r.GetComponent<CapsuleCollider>().enabled = false;
                col_lower_arm_r.GetComponent<CapsuleCollider>().enabled = false;
                col_head.GetComponent<CapsuleCollider>().enabled = false;
                col_chest.GetComponent<BoxCollider>().enabled = false;
                Object.Destroy(head);
                Object.Destroy(chest);
                Object.Destroy(hand_l);
                Object.Destroy(hand_r);
                Object.Destroy(blood_upper);
                Object.Destroy(blood_upper1);
                Object.Destroy(blood_upper2);
                Object.Destroy(blood_arm_l);
                Object.Destroy(blood_arm_r);
                base.gameObject.GetComponent<HERO_SETUP>().createLowerBody();
                break;
            case BODY_PARTS.ARM_L:
                col_upper_arm_r.GetComponent<CapsuleCollider>().enabled = false;
                col_lower_arm_r.GetComponent<CapsuleCollider>().enabled = false;
                col_thigh_l.GetComponent<CapsuleCollider>().enabled = false;
                col_shin_l.GetComponent<CapsuleCollider>().enabled = false;
                col_thigh_r.GetComponent<CapsuleCollider>().enabled = false;
                col_shin_r.GetComponent<CapsuleCollider>().enabled = false;
                col_head.GetComponent<CapsuleCollider>().enabled = false;
                col_chest.GetComponent<BoxCollider>().enabled = false;
                Object.Destroy(head);
                Object.Destroy(chest);
                Object.Destroy(leg);
                Object.Destroy(hand_r);
                Object.Destroy(blood_lower);
                Object.Destroy(blood_upper);
                Object.Destroy(blood_upper1);
                Object.Destroy(blood_upper2);
                Object.Destroy(blood_arm_r);
                base.gameObject.GetComponent<HERO_SETUP>().createLeftArm();
                break;
            case BODY_PARTS.ARM_R:
                col_upper_arm_l.GetComponent<CapsuleCollider>().enabled = false;
                col_lower_arm_l.GetComponent<CapsuleCollider>().enabled = false;
                col_thigh_l.GetComponent<CapsuleCollider>().enabled = false;
                col_shin_l.GetComponent<CapsuleCollider>().enabled = false;
                col_thigh_r.GetComponent<CapsuleCollider>().enabled = false;
                col_shin_r.GetComponent<CapsuleCollider>().enabled = false;
                col_head.GetComponent<CapsuleCollider>().enabled = false;
                col_chest.GetComponent<BoxCollider>().enabled = false;
                Object.Destroy(head);
                Object.Destroy(chest);
                Object.Destroy(leg);
                Object.Destroy(hand_l);
                Object.Destroy(blood_lower);
                Object.Destroy(blood_upper);
                Object.Destroy(blood_upper1);
                Object.Destroy(blood_upper2);
                Object.Destroy(blood_arm_l);
                base.gameObject.GetComponent<HERO_SETUP>().createRightArm();
                break;
        }
    }
}
