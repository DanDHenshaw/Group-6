using UnityEngine;
using Utilities;

public class GrapplingGun : MonoBehaviour
{
  [Header("Weapon Config")]
  [SerializeField] private float maxDistance;
  public Transform muzzle;

  [Header("Prediction Config")]
  [SerializeField] private RaycastHit predictionHit;
  [SerializeField] private float predictionSphereCastRadius;
  [SerializeField] private GameObject predictionPointObject;

  [Header("Moving Object Config")]
  [SerializeField] private GameObject movingObject;
  [SerializeField] private string movingObjectTag;

  [Space(15)]

  public Grapple grapple;
  public Swing swing;

  [Space(15)]

  [SerializeField] private LayerMask whatIsGrappleable;

  [Header("References")]
  [SerializeField] private PlayerController playerController;
  [SerializeField] private InputManager input;
  [SerializeField] private Transform cam;
  [SerializeField] private LineRenderer lineRenderer;

  public Vector3 grapplePoint { get; private set; }
  private Transform predictionPoint;

  private bool isMoving = false;

  private void Awake()
  {
    input = playerController.Input;

    playerController.grapplingGun = this;

    cam = Camera.main.transform;

    lineRenderer = GetComponent<LineRenderer>();
    lineRenderer.enabled = false;

    grapple.cooldownTimer = new CountdownTimer(grapple.cooldown);
  }

  private void OnEnable()
  {
      input.RightClick += HandleGrapple;
      input.LeftClick += HandleSwing;
  }

  private void OnDisable()
  {
      input.RightClick -= HandleGrapple;
      input.LeftClick -= HandleSwing;
  }

  private void Start()
  {
      predictionPoint = Instantiate(predictionPointObject, transform.position, transform.rotation).transform;
      predictionPoint.gameObject.SetActive(false);
  }

  private void Update()
  {
    grapple.cooldownTimer.Tick(Time.deltaTime);

    CheckForSwingPoints();

    if(isMoving && movingObject != null)
    {
      grapplePoint = movingObject.transform.position;

      if(swing.isSwinging)
        swing.joint.connectedAnchor = grapplePoint;

      lineRenderer.SetPosition(1, grapplePoint);
    }
  }

  private void LateUpdate()
  {
      if (swing.isSwinging || grapple.isGrappling)
      {
          lineRenderer.SetPosition(0,muzzle.position);
      }
  }

  /// <summary>
  /// Starts the grapple hook when input has been detected
  /// </summary>
  /// <param name="isDown"> bool - whether right click is pressed or released </param>
  private void HandleGrapple(bool isDown)
  {
      if (isDown)
      {
          StartGrapple();
      }
  }

  /// <summary>
  /// Starts and Stops the Swing when input has been detected
  /// </summary>
  /// <param name="isDown"> bool - whether right click is pressed or released </param>
  private void HandleSwing(bool isDown)
  {
      if (isDown && !swing.isSwinging)
      {
          StartSwinging();
      }
      else
      {
          StopSwinging();
      }
  }

  /// <summary>
  /// Starts the grapple - freeze the player in place and execute the next stage for the grapple
  /// </summary>
  private void StartGrapple()
  {
    if (!grapple.cooldownTimer.IsFinished) return;

    grapple.cooldownTimer.Reset();
    grapple.cooldownTimer.Start();

    grapple.isGrappling = true;

    playerController.freeze = true;

    if (predictionHit.point == Vector3.zero)
    {
      grapplePoint = cam.position + cam.forward * maxDistance;
      Invoke(nameof(StopGrapple), grapple.delayTime);

      isMoving = false;
    }
    else
    {
        grapplePoint = predictionHit.point;

      if (predictionHit.transform.gameObject.CompareTag(movingObjectTag))
      {
        movingObject = predictionHit.transform.gameObject;
        isMoving = true;
      }
      else
      {
        isMoving = false;
      }

      Invoke(nameof(ExecuteGrapple), grapple.delayTime);
    }

    lineRenderer.enabled = true;
    lineRenderer.SetPosition(1,grapplePoint);
  }

  /// <summary>
  /// Executes the grapple - Launches the player towards the grapple point
  /// </summary>
  private void ExecuteGrapple()
  {
      playerController.freeze = false;

      Vector3 lowestPoint = new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z);

      float grapplePointRelativeYPos = grapplePoint.y - lowestPoint.y;
      float hightestPointOnArc = grapplePointRelativeYPos + grapple.overShootYAxis;

      if (grapplePointRelativeYPos < 0) hightestPointOnArc = grapple.overShootYAxis;

      playerController.JumpToPosition(grapplePoint, hightestPointOnArc);

      Invoke(nameof(StopGrapple), 1f);
  }

  /// <summary>
  /// Stops the grapple - Unfreezes the player movement and resets the grapple
  /// </summary>
  private void StopGrapple()
  {
      grapple.isGrappling = false;

      playerController.freeze = false;

      lineRenderer.enabled = false;
  }

  /// <summary>
  /// Starts the swing - attaches a spring joint betweem the player and grapple point allowing the player to swing around
  /// </summary>
  private void StartSwinging()
  {
    // return if predictionHit not found
    if (predictionHit.point == Vector3.zero) return;

    swing.isSwinging = true;

    if (predictionHit.transform.gameObject.CompareTag(movingObjectTag))
    {
      movingObject = predictionHit.transform.gameObject;
      isMoving = true;
    }
    else
    {
      isMoving = false;
    }

    grapplePoint = predictionHit.point;
    swing.joint = playerController.gameObject.AddComponent<SpringJoint>();
    swing.joint.autoConfigureConnectedAnchor = false;
    swing.joint.connectedAnchor = grapplePoint;

    float distanceFromPoint = Vector3.Distance(playerController.transform.position, grapplePoint);

    swing.joint.maxDistance = distanceFromPoint * swing.jointMaxDistance;
    swing.joint.minDistance = distanceFromPoint * swing.jointMinDistance;

    swing.joint.spring = swing.jointSpring;
    swing.joint.damper = swing.jointDamper;
    swing.joint.massScale = swing.jointMassScale;

    lineRenderer.enabled = true;
    lineRenderer.SetPosition(1, grapplePoint);
  }

  /// <summary>
  /// Stops the swing - destroys the spring joint attached between the player and grapple point
  /// </summary>
  private void StopSwinging()
  {
      swing.isSwinging = false;
      Destroy(swing.joint);

      lineRenderer.enabled = false;
  }

  /// <summary>
  /// Creates a prediction for where the player grapples/swings - if the player isnt directly aiming at a grappleable point spherecast to find the nearest grapple point
  /// </summary>
  private void CheckForSwingPoints()
  {
      if (swing.isSwinging) return;

      RaycastHit sphereCastHit;
      Physics.SphereCast(cam.position, predictionSphereCastRadius, cam.forward, out sphereCastHit, maxDistance, whatIsGrappleable);

      RaycastHit raycastHit;
      Physics.Raycast(cam.position, cam.forward, out raycastHit, maxDistance, whatIsGrappleable);

      Vector3 realHitPoint;

      // Option 1 - Direct Hit
      if (raycastHit.point != Vector3.zero)
          realHitPoint = raycastHit.point;

      // Option 2 - Indirect (predicted) Hit
      else if (sphereCastHit.point != Vector3.zero)
          realHitPoint = sphereCastHit.point;

      // Option 3 - Miss
      else
          realHitPoint = Vector3.zero;

      // realHitPoint found
      if (realHitPoint != Vector3.zero)
      {
          predictionPoint.gameObject.SetActive(true);
          predictionPoint.position = realHitPoint;
      }
      // realHitPoint not found
      else
      {
          predictionPoint.gameObject.SetActive(false);
      }

      predictionHit = raycastHit.point == Vector3.zero ? sphereCastHit : raycastHit;
  }
}

[System.Serializable]
public class Grapple
{
  public float delayTime;

  [Space(10)]

  public float cooldown;
  [HideInInspector] public CountdownTimer cooldownTimer;

  [Space(10)]

  [HideInInspector] public bool isGrappling;
  public float overShootYAxis;
}

[System.Serializable]
public class Swing
{
  public float jointMaxDistance = 0.8f;
  public float jointMinDistance = 0.25f;
  public float jointSpring = 4.5f;
  public float jointDamper = 7f;
  public float jointMassScale = 4.5f;

  [HideInInspector] public SpringJoint joint;
  [HideInInspector] public bool isSwinging;
}
