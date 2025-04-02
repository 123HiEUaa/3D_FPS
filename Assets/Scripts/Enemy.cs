using UnityEngine;
using UnityEngine.AI;
[AddComponentMenu("HMFPS/Enemy")]

public class Enemy : MonoBehaviour
{
    [SerializeField] private int HP = 100;
    private Animator animator;

    private NavMeshAgent navAgent;

    public bool isDead ;
    private void Start()
    {
        animator = GetComponent<Animator>();
        navAgent = GetComponent<NavMeshAgent>();
    }

    public void TakeDamage(int damageAmount)
    {
        HP -= damageAmount;
        
        if (HP <= 0) 
        {
            int randomValue = Random.Range(0, 2); // 0 hoac 1

            if (randomValue == 0)
            {
                animator.SetTrigger("Die1");
            }
            else
            {
                animator.SetTrigger("Die2");
            }

            isDead = true;

            SoundManager.Instance.zombieChannel2.PlayOneShot(SoundManager.Instance.zombieDeath);
        }
        else
        {
            animator.SetTrigger("Damage");
            SoundManager.Instance.zombieChannel2.PlayOneShot(SoundManager.Instance.zombieHurt);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, 2.5f);

        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, 50f);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 60f);
    }
}
