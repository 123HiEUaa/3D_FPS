using UnityEngine;
using UnityEngine.AI;
[AddComponentMenu("HMFPS/ClickToMove")]

public class ClickToMove : MonoBehaviour
{
    private NavMeshAgent navAgent;

    private void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {

            // Tao ray tu camera den voi vi tri con tro chuot
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            //Kiem tra xem neu va cham vs matdat NavMesh
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, NavMesh.AllAreas))
            {
                //Di chuyen agent bang con chuot
                navAgent.SetDestination(hit.point);
            }
        }
    }
}
