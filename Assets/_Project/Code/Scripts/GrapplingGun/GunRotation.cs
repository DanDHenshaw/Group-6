using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunRotation : MonoBehaviour
{
	[Header("Config")]
	[SerializeField] private float rotationSpeed = 5f;

	[Header("References")]
	[SerializeField] private GrapplingGun grapple;

	private Quaternion desiredRotation;

	private void Awake()
	{
		grapple = GetComponent<GrapplingGun>();
	}

	private void Update()
	{ 
	  if (grapple.IsSwinging)
	  {
		  desiredRotation = Quaternion.LookRotation(grapple.swingPoint - transform.position);
	  }
	  else
	  {
		  desiredRotation = transform.parent.rotation;
	  }

	  transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotation, Time.deltaTime * rotationSpeed);
	}
}
