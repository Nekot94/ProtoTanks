using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemScript : MonoBehaviour {

    public static ItemScript instance;

    public enum ItemStates { Heal, Speed, Reverse, Freeze, Bomb, RandomState };

    public ItemStates itemState = ItemStates.Heal;

    public Color[] stateColors;

    delegate void ItemAction(GameObject player);

    private ItemAction[] allActions = { Heal, Speed, Reverse, Freeze, Bomb, RandomState};



	// Use this for initialization
	void Awake () {
        instance = this;
        //ChoseColor();
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            allActions[(int)itemState](other.gameObject);
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            gameObject.GetComponent<BoxCollider>().enabled = false;

        }
		if (other.gameObject.CompareTag("Item")){
			Debug.Log(9);
			Destroy(other.gameObject);
		}

    }

    private static void Heal(GameObject player) {
        ChangeHealth(player, true);
    }

    private static void Bomb(GameObject player) {
        ChangeHealth(player, false);
    }

    private static void ChangeHealth(GameObject player, bool positive) {
        TankHealth tankHealth = player.GetComponent<TankHealth>();
        if (tankHealth != null) {
            float halfHealth = tankHealth.m_StartingHealth / 2;
            if (positive)
                halfHealth = -halfHealth;
            tankHealth.TakeDamage(halfHealth);
        }
    } 

    private static void Speed(GameObject player) {
        TankMovement tankMovement = player.GetComponent<TankMovement>();
        if (tankMovement != null) {
            instance.StartCoroutine(SpeedPerTime(tankMovement, tankMovement.m_Speed * 2, 10f));
        }
    }

    private static IEnumerator SpeedPerTime(TankMovement tankMovement, float newValue, float time) {
        float oldValue = tankMovement.m_Speed;
        tankMovement.m_Speed = newValue;
        yield return new WaitForSeconds(time);
        tankMovement.m_Speed = oldValue;
        Destroy(instance.gameObject);
    }

    private static void Reverse(GameObject player) {
        TankMovement[] playersMovement = GameObject.FindObjectsOfType<TankMovement>();
        TankMovement thisMovement = player.GetComponent<TankMovement>();
        if (thisMovement != null) {
            instance.StartCoroutine(ReversePerTime(playersMovement, thisMovement, 10f));
        }
    }

    private static IEnumerator ReversePerTime(TankMovement[] playersMovement, TankMovement thisMovement, float time) {
        foreach (TankMovement tankMovement in playersMovement) {
            if (tankMovement == thisMovement)
                continue;
            string temp = tankMovement.m_TurnAxisName;
            tankMovement.m_TurnAxisName = tankMovement.m_MovementAxisName;
            tankMovement.m_MovementAxisName = temp;
        }
        yield return new WaitForSeconds(time);
        foreach (TankMovement tankMovement in playersMovement) {
            if (tankMovement == thisMovement)
                continue;
            string temp = tankMovement.m_TurnAxisName;
            tankMovement.m_TurnAxisName = tankMovement.m_MovementAxisName;
            tankMovement.m_MovementAxisName = temp;
        }
    }

    private static void Freeze(GameObject player) {
        TankMovement[] playersMovement = GameObject.FindObjectsOfType<TankMovement>();
        TankMovement thisMovement = player.GetComponent<TankMovement>();
        if (thisMovement != null) {
            instance.StartCoroutine(FreezePerTime(playersMovement, thisMovement, 5f));
        }
    }

    private static IEnumerator FreezePerTime(TankMovement[] playersMovement, TankMovement thisMovement, float time) {
        foreach (TankMovement tankMovement in playersMovement) {
            if (tankMovement == thisMovement)
                continue;
            tankMovement.enabled = false;
            TankShooting tankShooting = tankMovement.GetComponentInParent<TankShooting>();
            if (tankShooting != null) {
                tankShooting.enabled = false;
            }
        }
        yield return new WaitForSeconds(time);
        foreach (TankMovement tankMovement in playersMovement) {
            if (tankMovement == thisMovement)
                continue;
            tankMovement.enabled = true;
            TankShooting tankShooting = tankMovement.GetComponentInParent<TankShooting>();
            if (tankShooting != null) {
                tankShooting.enabled = true;
            }
        }
    }

    private static void RandomState(GameObject player) {
        instance.ChooseRandomState();
        instance.allActions[(int)instance.itemState](player);
        instance.gameObject.GetComponent<MeshRenderer>().enabled = false;
        instance.gameObject.GetComponent<BoxCollider>().enabled = false;
    }


    public void ChooseRandomState() {
        int stateNumber = Random.Range(0, System.Enum.GetNames(typeof(ItemStates)).Length);
        itemState = (ItemStates)stateNumber;
        ChooseColor();
    }

    public void ChooseColor() {
        Color itemColor = stateColors[(int)itemState];        
        Material itemMaterial = GetComponent<Renderer>().material;
        itemMaterial.color =  itemColor;
        itemMaterial.SetColor("_EmissionColor", itemColor);

    }






}
