using System.Collections.Generic;
using MMS.AI;
using Photon.Pun;
using UnityEngine;

namespace MMS.PLAYER
{
    public class TimberManController : MonoBehaviourPun
    {
        [Header("Refs")]
        [SerializeField] private Renderer renderer;
        [SerializeField] private Rigidbody rb;
        [SerializeField] private Animator animator;
        [SerializeField] private GameObject selectedVisual;
        
        [Header("Values")]
        [SerializeField] private float walkSpeed = 1f;
        [SerializeField] private float stoppingDistanceToNode = .1f;
        [SerializeField] private float stoppingDistanceToTree = 1f;
        [SerializeField] private float choppingActionDuration = 2f;
        [SerializeField] private int woodEarnAmount = 1;

        private PlayerManager playerManager;
        private PathfindingManager pathManager;

        private TimberManState state;
        private bool isSelected = false;
        private bool isMoving = false;
        private int currentPathNodeIndex = 0;
        private bool choppingActionAvailable = false;
        private Transform targetTreeTransform = null;
        private float choppingTimer = 0f;

        public bool IsSelected => isSelected;

        private List<PathNode> path;

        #region MonoBehaviour Callbacks

        private void Start()
        {
            playerManager = PhotonView.Find((int)photonView.InstantiationData[0]).GetComponent<PlayerManager>();
            pathManager = PathfindingManager.Instance;
            rb.mass = UnityEngine.Random.Range(1f, 3f);
        }

        private void Update()
        {
            switch (state)
            {
                case TimberManState.Idle:
                    break;
                case TimberManState.ChoppingTree:
                    ChopAction();
                    break;
                case TimberManState.MovingToDestination:
                    MoveToDestination();
                    break;
                default:
                    break;
            }
        }

        #endregion
        
        private void SwitchState(TimberManState newState, Vector3 targetPosition = default)
        {
            if (newState == state) return;
            state = newState;
            
            switch (state)
            {
                case TimberManState.Idle:
                    SetChoppingAnimation(false);
                    break;
                case TimberManState.ChoppingTree:

                    choppingTimer = 0f;
                    
                    if (Vector3.Distance(transform.position, targetTreeTransform.position) <= stoppingDistanceToTree)
                    {
                        SetChoppingAnimation(true);
                    }
                    else
                    {
                        SwitchState(TimberManState.MovingToDestination, targetPosition);
                    }
                    
                    break;
                case TimberManState.MovingToDestination:
                    
                    SetChoppingAnimation(false);
                    SetDestination(targetPosition);
                    
                    break;
                default:
                    break;
            }
        }

        private void SetChoppingAnimation(bool isChoppingTree)
        {
            animator.SetBool("IsChopping", isChoppingTree);
        }

        #region Team Color Setting

        public void SetMyColor(Color color)
        {
            photonView.RPC("RPC_SetMyColor", RpcTarget.AllBuffered, color.r, color.g, color.b);
        }

        [PunRPC]
        private void RPC_SetMyColor(float r, float g, float b)
        {
            Color myColor = new Color(r, g, b);
            renderer.material.color = myColor;
        }

        #endregion

        #region Movement Actions

        public void SetDestination(Vector3 destination, bool clearTreeAction = false)
        {
            if (clearTreeAction)
            {
                targetTreeTransform = null;
                choppingActionAvailable = false;
            }
            state = TimberManState.MovingToDestination;
            SetChoppingAnimation(false);
            currentPathNodeIndex = 0;
            path = pathManager.GetPathToPosition(transform.position, destination);
            isMoving = true;
        }

        private void MoveToDestination()
        {
            if (isMoving && path != null)
            {
                Vector3 target = pathManager.GetGrid.GetWorldPosition(path[currentPathNodeIndex].GetX,
                    path[currentPathNodeIndex].GetZ);
                
                if (Vector3.Distance(transform.position, target) > 
                    ((currentPathNodeIndex == path.Count - 1) 
                        ? (choppingActionAvailable ? stoppingDistanceToTree : stoppingDistanceToNode)
                        : stoppingDistanceToNode))
                {
                    transform.position = Vector3.MoveTowards(
                        transform.position,
                        target,
                        walkSpeed * Time.deltaTime);
                }
                else
                {
                    if (currentPathNodeIndex < path.Count - 1)
                    {
                        // Reached the target node
                        currentPathNodeIndex++;
                    }
                    else
                    {
                        // Reached the destination
                        currentPathNodeIndex = 0;
                        TimberManState newState =
                            choppingActionAvailable ? TimberManState.ChoppingTree : TimberManState.Idle;
                        SwitchState(newState, choppingActionAvailable ? targetTreeTransform.position : Vector3.zero);
                    }
                }
            }
        }

        #endregion

        #region Chopping Tree Actions

        public void ChopTree(Transform tree)
        {
            choppingActionAvailable = true;
            targetTreeTransform = tree;
            SwitchState(TimberManState.ChoppingTree, tree.position);
        }

        private void ChopAction()
        {
            if (choppingTimer >= choppingActionDuration)
            {
                choppingTimer = 0f;
                GetWood();
            }

            choppingTimer += Time.deltaTime;
        }

        private void GetWood()
        {
            playerManager.AddWood(woodEarnAmount);
        }

        #endregion

        #region Selection Of The Unit

        public void Select()
        {
            isSelected = true;
            selectedVisual.SetActive(true);
        }

        public void DeSelect()
        {
            isSelected = false;
            selectedVisual.SetActive(false);
        }

        #endregion
        
        
    }
}