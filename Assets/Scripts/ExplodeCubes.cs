using UnityEngine;

public class ExplodeCubes : MonoBehaviour
{
    [SerializeField] private float powerForce;
    [SerializeField] private GameObject explosion;

    private bool collisionSet;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Cube") && !collisionSet)
        {
            int countChild = collision.transform.childCount;
            GetComponent<AudioSource>().Play();
            for (int i = countChild - 1; i >= 0; i--)
            {
                Transform child = collision.transform.GetChild(i);
                child.gameObject.AddComponent<Rigidbody>();
                child.gameObject.GetComponent<Rigidbody>().AddExplosionForce(powerForce, Vector3.up, 5f);
                child.SetParent(null);
            }
            Camera.main.transform.localPosition -= new Vector3(0f, 0f, 4f);
            Camera.main.gameObject.AddComponent<CameraShake>();

            Instantiate(explosion, 
                new Vector3(collision.contacts[0].point.x, 
                collision.contacts[0].point.y, collision.contacts[0].point.z), 
                Quaternion.identity);

            Destroy(collision.gameObject);
            collisionSet = true;
        }
    }
}
