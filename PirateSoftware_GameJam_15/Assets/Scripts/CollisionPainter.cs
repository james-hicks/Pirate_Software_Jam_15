using UnityEngine;

public class CollisionPainter : MonoBehaviour{
    [ColorUsageAttribute(false, true)]
    public Color paintColor;

    [SerializeField] private GameObject potion;
    
    public float minRadius = 1;
    public float maxRadius = 2;
    public float strength = 1;
    public float hardness = 1;

    private void OnCollisionStay(Collision other) {
        Paintable p = other.collider.GetComponent<Paintable>();
        if(p != null){
            Vector3 pos = other.contacts[0].point;
            PaintManager.instance.paint(p, pos, Random.Range(minRadius, maxRadius), hardness, strength, paintColor);
        }
    }

    private void Update()
    {
        potion.transform.Rotate(0, 1f, 5f);
    }
}
