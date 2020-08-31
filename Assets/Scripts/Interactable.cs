using UnityEngine;
using UnityEngine.AI;


//[RequireComponent(typeof(ColorOnHover))]
public class Interactable : MonoBehaviour
{

	public static bool bottleFull ;
	//public float radius = 3f;
	//public Transform interactionTransform;

	public void Start()
	{
		bottleFull = false;
	}


	void Update()
	{
	
	}


	// Método para ser subscrito
	public virtual void Interact()
	{

	}

	//void OnDrawGizmosSelected()
	//{
		//Gizmos.color = Color.yellow;
		//Gizmos.DrawWireSphere(interactionTransform.position, radius);
	//}

}