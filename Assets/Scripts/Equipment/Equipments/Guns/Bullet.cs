using UnityEngine;

namespace Equipment.Equipments.Guns
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private float speed;
        [SerializeField] private float lifeTime;
        
        private void Start()
        {
            Destroy(gameObject, lifeTime);
        }
        
        private void Update()
        {
            MoveBullet();
        }
        
        private void MoveBullet()
        {
            transform.Translate(Vector3.forward * (speed * Time.deltaTime));
        }
    }
}