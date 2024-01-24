using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.WSA;

public class GrapplingGun : MonoBehaviour
{
    [Header("Weapon Config")]
    [SerializeField] private float maxDistance = 100f;
    [SerializeField] private Transform gunTip;

    [Header("Grapple Config")]
    [SerializeField] private float maxJointDistance = 0.8f;
    [SerializeField] private float minJointDistance = 0.25f;
    [SerializeField] private float jointSpring = 4.5f;
    [SerializeField] private float jointDamper = 7f;
    [SerializeField] private float jointMassScale = 4.5f;
    
    [Space(15)]

    private LayerMask whatIsGrappleable;

    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private InputManager input;
    [SerializeField] private Transform camera;
    [SerializeField] private Rigidbody playerRigidbody;
    [SerializeField] private LineRenderer lineRenderer;
    
    private Vector3 grapplePoint;
    private SpringJoint joint;
    
    private void Awake()
    {
        input = player.GetComponent<PlayerController>().Input;
        playerRigidbody = player.GetComponent<Rigidbody>();

        camera = Camera.main.transform; 

        lineRenderer = GetComponent<LineRenderer>();
    }

    private void OnEnable()
    {
        input.LeftClick += Shot;
        input.RightClick += Launch;
    }

    private void Launch(bool isDown)
    {
        throw new System.NotImplementedException();
    }

    private void Shot(bool isDown)
    {
        if (isDown)
        {
            StartGrapple();
        }
        else
        {
            StopGrapple();
        }
    }

    private void OnDisable()
    {
        input.LeftClick -= Shot;
        input.RightClick -= Launch;
    }

    private void LateUpdate()
    {
        DrawRope();
    }

    private void StartGrapple()
    {
        RaycastHit hit;
        if (Physics.Raycast(camera.position, camera.forward, out hit, maxDistance, whatIsGrappleable))
        {
            grapplePoint = hit.point;
            joint = player.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = grapplePoint;

            float distanceFromPoint = Vector3.Distance(player.position, grapplePoint);

            joint.maxDistance = distanceFromPoint * maxJointDistance;
            joint.minDistance = distanceFromPoint * minJointDistance;

            joint.spring = jointSpring;
            joint.damper = jointDamper;
            joint.massScale = jointMassScale;

            lineRenderer.positionCount = 2;
        }
    }

    private void DrawRope()
    {
        if (!IsGrappling()) return;

        lineRenderer.SetPosition(0, gunTip.position);
        lineRenderer.SetPosition(1, grapplePoint);
    }

    private void StopGrapple()
    {
        if (!IsGrappling()) return;

        lineRenderer.positionCount = 0;
        Destroy(joint);
    }

    public bool IsGrappling()
    {
        return joint != null;
    }
}