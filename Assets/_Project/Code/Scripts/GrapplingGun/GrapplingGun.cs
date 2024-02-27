using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utilities;

  public class GrapplingGun : MonoBehaviour
  { 
  [Header("Weapon Config")]
  [SerializeField] private float maxDistance;
  public Transform muzzle;

  [Header("Swing Config")]
  [SerializeField] private float jointMaxDistance = 0.8f;
  [SerializeField] private float jointMinDistance = 0.25f;
  [SerializeField] private float jointSpring = 4.5f;
  [SerializeField] private float jointDamper = 7f;
  [SerializeField] private float jointMassScale = 4.5f;

  [Header("Shorten Config")]
  [SerializeField] private float thrustForce = 3000f;

  [Header("Attack Settings")]
  [SerializeField] private int damage = 10;
  [SerializeField] private float knockbackForce = 1000f;
  [SerializeField] private float attackTime = 0.25f;

  [Header("Prediction Config")]
  [SerializeField] private float predictionSphereCastRadius;
  [SerializeField] private GameObject predictionPointObject;

  [Header("Moving Object Config")]
  [SerializeField] private GameObject movingObject;
  [SerializeField] private string movingObjectTag;

  [Space(15)]

  [SerializeField] private LayerMask whatIsGrappleable;
  [SerializeField] private LayerMask whatIsEnemy;

  [Header("References")]
  [SerializeField] private PlayerController playerController;
  [SerializeField] private InputManager input;
  [SerializeField] private Transform cam;
  [SerializeField] private LineRenderer lineRenderer;

  public Vector3 swingPoint { get; private set; }
  private RaycastHit predictionHit;
  private Transform predictionPoint;

  private SpringJoint joint;
  public bool IsSwinging { get; private set; }

  private bool isMoving = false;

  private bool isShorten = false;

  private GameObject enemyObject = null;
  private bool isBack = false;
  private bool isAttacking = false;

  private Vector3 hitDirection = Vector3.zero;

  private void Awake()
  {
    input = playerController.Input;

    playerController.grapplingGun = this;

    cam = Camera.main.transform;

    lineRenderer = GetComponent<LineRenderer>();
    lineRenderer.enabled = false;
  }

  private void OnEnable()
  {
      input.RightClick += HandleReduceRope;
      input.LeftClick += HandleSwing;
  }

  private void OnDisable()
  {
      input.RightClick -= HandleReduceRope;
      input.LeftClick -= HandleSwing;
  }

  private void Start()
  {
      predictionPoint = Instantiate(predictionPointObject, transform.position, transform.rotation).transform;
      predictionPoint.gameObject.SetActive(false);
  }

  private void Update()
  {
    CheckForSwingPoints();

    HandleShorten();

    if(isMoving && movingObject != null)
    {
      swingPoint = movingObject.transform.position;

      if(IsSwinging)
        joint.connectedAnchor = swingPoint;

      lineRenderer.SetPosition(1, swingPoint);
    }
  }

  private void LateUpdate()
  {
      if (IsSwinging || isAttacking)
      {
          lineRenderer.SetPosition(0, muzzle.position);
      }
  }

  /// <summary>
  /// Starts to reduce the rope of the grapple gun
  /// </summary>
  /// <param name="isDown"> bool - whether right click is pressed or released </param>
  private void HandleReduceRope(bool isDown)
  {
    if (isDown && IsSwinging)
    {
      isShorten = true;
    }
    else
    {
      isShorten = false;
    }
  }

  /// <summary>
  /// Starts and Stops the Swing when input has been detected
  /// </summary>
  /// <param name="isDown"> bool - whether right click is pressed or released </param>
  private void HandleSwing(bool isDown)
  {
      if (isDown && !IsSwinging)
      {
          StartSwinging();
      }
      else
      {
          StopSwinging();
      }
  }


  /// <summary>
  /// Starts the swing - attaches a spring joint betweem the player and grapple point allowing the player to swing around
  /// </summary>
  private void StartSwinging()
  {
    if(enemyObject != null)
    {
      HandleAttack();

      return;
    }

    // return if predictionHit not found
    if (predictionHit.point == Vector3.zero) return;

    IsSwinging = true;

    if (predictionHit.transform.gameObject.CompareTag(movingObjectTag))
    {
      movingObject = predictionHit.transform.gameObject;
      isMoving = true;
    }
    else
    {
      isMoving = false;
    }

    swingPoint = predictionHit.point;
    joint = playerController.gameObject.AddComponent<SpringJoint>();
    joint.autoConfigureConnectedAnchor = false;
    joint.connectedAnchor = swingPoint;

    float distanceFromPoint = Vector3.Distance(playerController.transform.position, swingPoint);

    joint.maxDistance = distanceFromPoint * jointMaxDistance;
    joint.minDistance = distanceFromPoint * jointMinDistance;

    joint.spring = jointSpring;
    joint.damper = jointDamper;
    joint.massScale = jointMassScale;

    lineRenderer.enabled = true;
    lineRenderer.SetPosition(1, swingPoint);
  }

  /// <summary>
  /// Stops the swing - destroys the spring joint attached between the player and grapple point
  /// </summary>
  private void StopSwinging()
  {
      IsSwinging = false;
      Destroy(joint);

      lineRenderer.enabled = false;
  }

  private void HandleShorten()
  {
    if (isShorten && IsSwinging)
    {
      Vector3 directionToPoint = swingPoint - transform.position;
      playerController._Rigidbody.AddForce(directionToPoint.normalized * thrustForce * Time.deltaTime);

      float distanceFromPoint = Vector3.Distance(transform.position, swingPoint);

      joint.maxDistance = distanceFromPoint * jointMaxDistance;
      joint.minDistance = distanceFromPoint * jointMinDistance;
    }
  }

  private void HandleAttack()
  {
    if (enemyObject.name == "level")
    {
      SceneManager.LoadScene("level 1");
    }

    if (isBack)
    {
      enemyObject.GetComponentInParent<HealthSystem>().TakeDamage(damage);
      Destroy(enemyObject);
    }
    else
    {
      if(enemyObject.GetComponentInParent<EnemyController>().TryGetComponent(out IKnockbackable knockbackable))
      {
        Vector3 force = knockbackForce * hitDirection;
        knockbackable.GetKnockedBack(force);
      }
    }

    isAttacking = true;

    lineRenderer.enabled = true;
    lineRenderer.SetPosition(1, predictionHit.point);

    Invoke("OnStopAttack", attackTime);
  }

  private void OnStopAttack()
  {
    isAttacking = false;
    lineRenderer.enabled = false;
  }

  /// <summary>
  /// Creates a prediction for where the player grapples/swings - if the player isnt directly aiming at a grappleable point spherecast to find the nearest grapple point
  /// </summary>
  private void CheckForSwingPoints()
  {
    if (IsSwinging) return;

    RaycastHit sphereCastHit;
    Physics.SphereCast(cam.position, predictionSphereCastRadius, cam.forward, out sphereCastHit, maxDistance, whatIsGrappleable);

    RaycastHit raycastHit;
    Physics.Raycast(cam.position, cam.forward, out raycastHit, maxDistance, whatIsGrappleable);

    RaycastHit raycastHitEnemy;
    Physics.Raycast(cam.position, cam.forward, out raycastHitEnemy, maxDistance, whatIsEnemy);

    Vector3 realHitPoint;

    // If grapple is enemy
    if (raycastHitEnemy.point != Vector3.zero)
    {
      GameObject enemy = raycastHitEnemy.collider.gameObject;

      enemyObject = enemy;

      if(enemy.name.ToLower() == "front")
      {
        isBack = false;
      } else if (enemy.name.ToLower() == "back")
      {
        isBack = true;
        hitDirection = -raycastHitEnemy.normal;
      }
    } 
    else
    {
      enemyObject = null;
    }

    // Option 1 - Direct Hit Enemy
    if (enemyObject != null)
      realHitPoint = raycastHitEnemy.point;

    // Option 2 - Direct Hit
    else if (raycastHit.point != Vector3.zero)
      realHitPoint = raycastHit.point;

    // Option 3 - Indirect (predicted) Hit
    else if (sphereCastHit.point != Vector3.zero)
        realHitPoint = sphereCastHit.point;

    // Option 4 - Miss
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

    if(raycastHit.normal != Vector3.zero)
      predictionPoint.rotation = Quaternion.LookRotation(raycastHit.normal);

    predictionHit = raycastHit.point == Vector3.zero ? sphereCastHit : raycastHit;
  }
}