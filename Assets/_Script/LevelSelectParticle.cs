using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class LevelSelectParticle : MonoBehaviour
{
    public Vector3 randMultiplier;
    public float fallSpdMultiplier;
    void Start() {
        randMultiplier = new Vector3(
            Random.Range(-2f, 2f), 
            Random.Range(-2f, 2f), 
            Random.Range(-2f, 2f));
        randMultiplier *= Random.Range(25f, 50f);
        fallSpdMultiplier = Random.Range(1f, 3f);
    }

    // Update is called once per frame
    void Update() {
        transform.position += fallSpdMultiplier * Time.deltaTime * Vector3.down;
        transform.rotation = Quaternion.Euler(
            Time.time * randMultiplier.x, 
            Time.time * randMultiplier.y, 
            Time.time * randMultiplier.z);
    }
}
