using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Show unit info on special sheet.
/// </summary>
public class ShowInfo : MonoBehaviour
{
	// Name of unit
    public Text unitName;
	// Primary icon for displaing
    public Image primaryIcon;
	// Primary text for displaing
    public Text primaryText;
	// Secondary icon for displaing
    public Image secondaryIcon;
	// Secondary text for displaing
    public Text secondaryText;
	// Hitpoints icon for displaying
	public Sprite hitpointsIcon;
	// Melee attack icon for displaying
	public Sprite meleeAttackIcon;
	// Ranged attack icon for displaying
	public Sprite rangedAttackIcon;
	// Defenders number icon for displaying
	public Sprite defendersNumberIcon;
	// Cooldown icon for displaying
	public Sprite cooldownIcon;

    /// <summary>
    /// Raises the destroy event.
    /// </summary>
    void OnDestroy()
    {
		EventManager.StopListening("UserClick", UserClick);
    }

	/// <summary>
	/// Awake this instance.
	/// </summary>
    void Awake()
    {
        Debug.Assert(unitName && primaryIcon && primaryText && secondaryIcon && secondaryText, "Wrong intial settings");
    }

	/// <summary>
	/// Start this instance.
	/// </summary>
    void Start()
    {
		EventManager.StartListening("UserClick", UserClick);
        HideUnitInfo();
    }

	/// <summary>
	/// Shows the unit info.
	/// </summary>
	/// <param name="info">Info.</param>
	public void ShowUnitInfo(UnitInfo info, GameObject obj)
    {
		if (info.unitName != "")
		{
			unitName.text = info.unitName;
		}
		else
		{
			unitName.text = obj.name;
		}

		if (info.primaryIcon != null || info.secondaryIcon != null || info.primaryText != "" || info.secondaryText != "")
		{
			primaryText.text = info.primaryText;
			secondaryText.text = info.secondaryText;

			if (info.primaryIcon != null)
			{
				primaryIcon.sprite = info.primaryIcon;
				primaryIcon.gameObject.SetActive(true);
			}

			if (info.secondaryIcon != null)
			{
				secondaryIcon.sprite = info.secondaryIcon;
				secondaryIcon.gameObject.SetActive(true);
			}
		}
		else
		{
			DamageTaker damageTaker = obj.GetComponentInChildren<DamageTaker>();
			Attack attack = obj.GetComponentInChildren<Attack>();
			DefendersSpawner spawner = obj.GetComponentInChildren<DefendersSpawner>();

			// Automaticaly set primary icon and text
			if (damageTaker != null)
			{
				primaryText.text = damageTaker.hitpoints.ToString();
				primaryIcon.sprite = hitpointsIcon;
				primaryIcon.gameObject.SetActive(true);
			}
			else
			{
				if (attack != null)
				{
					if (attack != null)
					{
						primaryText.text = attack.cooldown.ToString();
						primaryIcon.sprite = cooldownIcon;
						primaryIcon.gameObject.SetActive(true);
					}
				}
				else if (spawner != null)
				{
					primaryText.text = spawner.cooldown.ToString();
					primaryIcon.sprite = cooldownIcon;
					primaryIcon.gameObject.SetActive(true);
				}
			}

			if (attack != null)
			{
				secondaryText.text = attack.damage.ToString();
				if (attack is AttackMelee)
				{
					secondaryIcon.sprite = meleeAttackIcon;
				}
				else if (attack is AttackRanged)
				{
					secondaryIcon.sprite = rangedAttackIcon;
				}
				secondaryIcon.gameObject.SetActive(true);
			}
			else
			{
				if (spawner != null)
				{
					secondaryText.text = spawner.maxNum.ToString();
					secondaryIcon.sprite = defendersNumberIcon;
					secondaryIcon.gameObject.SetActive(true);
				}
			}
		}
		gameObject.SetActive(true);
    }

	/// <summary>
	/// Hides the unit info.
	/// </summary>
    public void HideUnitInfo()
    {
        unitName.text = primaryText.text = secondaryText.text = "";
        primaryIcon.gameObject.SetActive(false);
        secondaryIcon.gameObject.SetActive(false);
		gameObject.SetActive(false);
    }

	/// <summary>
	/// User click handler.
	/// </summary>
	/// <param name="obj">Object.</param>
	/// <param name="param">Parameter.</param>
    private void UserClick(GameObject obj, string param)
    {
        HideUnitInfo();
        if (obj != null)
        {
			// Cliced object has info for displaing
			UnitInfo unitInfo = obj.GetComponentInChildren<UnitInfo>();
            if (unitInfo != null)
            {
				ShowUnitInfo(unitInfo, obj);
            }
        }
    }
}
