using System.Collections.Generic;
using UnityEngine;

namespace MMS.PLAYER
{
    public class CommandHandler : MonoBehaviour
    {
        [SerializeField] private LayerMask raycastLayerMask;

        private List<TimberManController> selectedTimbers = new();
        
        private Ray ray;
        private RaycastHit hit;

        private void Update()
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (!Input.GetMouseButtonDown(0))
            {
                return;
            }
            if (!Physics.Raycast(ray, out hit, 100f, raycastLayerMask))
            {
                return;
            }

            if (hit.transform.TryGetComponent(out TimberManController timberManController))
            {
                if (!timberManController.photonView.IsMine)
                {
                    return;
                }
                if (selectedTimbers.Contains(timberManController))
                {
                    timberManController.DeSelect();
                    selectedTimbers.Remove(timberManController);
                }
                else
                {
                    timberManController.Select();
                    selectedTimbers.Add(timberManController);
                }
            }
            else if (hit.transform.CompareTag("Floor"))
            {
                for (int i = 0; i < selectedTimbers.Count; i++)
                {
                    selectedTimbers[i].SetDestination(hit.point, true);
                }
            }
            else if (hit.transform.CompareTag("Tree"))
            {
                for (int i = 0; i < selectedTimbers.Count; i++)
                {
                    selectedTimbers[i].ChopTree(hit.transform);
                }
            }
        }

        
    }
}

