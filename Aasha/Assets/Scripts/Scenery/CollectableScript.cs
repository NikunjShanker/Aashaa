using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CollectableScript : MonoBehaviour
{
    public CharacterController2D controller;

    private void Awake()
    {
        controller = GameObject.Find("Aashaa").GetComponent<CharacterController2D>();

        validateCollectible();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            ParticleSystem explosionPS;

            if (this.tag == "Heart")
            {
                PlayerHealth.ph.heartContainerCollected();
                explosionPS = GameObject.Find("Pickup PS").GetComponent<ParticleSystem>();

                UniversalScript.instance.heartGained[int.Parse(this.gameObject.name.Substring(16))] = true;

                UpgradeCanvasManager.instance.titleText.text = "Additional Heart";
                UpgradeCanvasManager.instance.infoText.text = "An extra Heart added to your max Health";
                UpgradeCanvasManager.instance.upgradeIcon.sprite = UpgradeCanvasManager.instance.heart;
            }
            else
            {
                explosionPS = GameObject.Find("Upgrade Pickup PS").GetComponent<ParticleSystem>();
            }

            if (this.gameObject.name == "Double Jump")
            {
                controller.canDoubleJump = true;
                UpgradeCanvasManager.instance.titleText.text = "Double Jump";
                UpgradeCanvasManager.instance.infoText.text = "Press the jump button while in the air to Double Jump";
                UpgradeCanvasManager.instance.upgradeIcon.sprite = UpgradeCanvasManager.instance.doubleJump;
            }
            else if (this.gameObject.name == "Dash")
            {
                controller.canDash = true;
                UpgradeCanvasManager.instance.titleText.text = "Dash";
                UpgradeCanvasManager.instance.infoText.text = "Press -K- or Right Click to Dash\nPress -LT- or -RT- to Dash\nDashing is possible on the ground and midair";
                UpgradeCanvasManager.instance.upgradeIcon.sprite = UpgradeCanvasManager.instance.dash;
            }
            else if (this.gameObject.name == "Wall Jump")
            {
                controller.canWallJump = true;
                UpgradeCanvasManager.instance.titleText.text = "Wall Jump";
                UpgradeCanvasManager.instance.infoText.text = "Press the jump button while in contact with a wall to Wall Jump";
                UpgradeCanvasManager.instance.upgradeIcon.sprite = UpgradeCanvasManager.instance.wallJump;
            }
            else if (this.gameObject.name == "Jump Boost")
            {
                controller.jumpIncrease = true;
                UpgradeCanvasManager.instance.titleText.text = "Jump Boost";
                UpgradeCanvasManager.instance.infoText.text = "Your max Jump Height has been permanently increased";
                UpgradeCanvasManager.instance.upgradeIcon.sprite = UpgradeCanvasManager.instance.jumpBoost;
            }
            else if (this.gameObject.name == "Speed Boost")
            {
                controller.speedIncrease = true;
                UpgradeCanvasManager.instance.titleText.text = "Speed Boost";
                UpgradeCanvasManager.instance.infoText.text = "Your Run Speed has been permanently increased";
                UpgradeCanvasManager.instance.upgradeIcon.sprite = UpgradeCanvasManager.instance.speedBoost;
            }

            explosionPS.transform.position = this.transform.position;
            UpgradeCanvasManager.instance.UpgradeCollected();
            UniversalScript.instance.SaveData();
            explosionPS.Play();
            AudioManagerScript.instance.Play("upgrade");

            GameObject.Destroy(this.gameObject);
        }
    }

    public void armorCollected()
    {
        ParticleSystem explosionPS = GameObject.Find("Upgrade Pickup PS").GetComponent<ParticleSystem>();

        UpgradeCanvasManager.instance.titleText.text = "Royal Armor";
        UpgradeCanvasManager.instance.infoText.text = "An expertly crafted suit of armor designed for the strongest of warriors.";
        UpgradeCanvasManager.instance.upgradeIcon.sprite = UpgradeCanvasManager.instance.armor;

        UpgradeCanvasManager.instance.UpgradeCollected();
        UniversalScript.instance.SaveData();
        explosionPS.transform.position = this.transform.position;
        explosionPS.Play();
        AudioManagerScript.instance.Play("upgrade");

        GameObject.Destroy(this.gameObject);
    }

    private void validateCollectible()
    {
        if (this.tag == "Heart")
        {
            if (UniversalScript.instance.heartGained[int.Parse(this.gameObject.name.Substring(16, 1))]) GameObject.Destroy(this.gameObject);
        }

        if (this.gameObject.name == "Double Jump")
        {
            if(UniversalScript.instance.canDoubleJump) GameObject.Destroy(this.gameObject);
        }
        else if (this.gameObject.name == "Dash")
        {
            if (UniversalScript.instance.canDash) GameObject.Destroy(this.gameObject);
        }
        else if (this.gameObject.name == "Wall Jump")
        {
            if (UniversalScript.instance.canWallJump) GameObject.Destroy(this.gameObject);
        }
        else if (this.gameObject.name == "Jump Boost")
        {
            if (UniversalScript.instance.jumpBoost) GameObject.Destroy(this.gameObject);
        }
        else if (this.gameObject.name == "Speed Boost")
        {
            if (UniversalScript.instance.speedBoost) GameObject.Destroy(this.gameObject);
        }
    }
}
