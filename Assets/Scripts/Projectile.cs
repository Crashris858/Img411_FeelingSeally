using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    public int NumBoosts = 1; 
    public float Thrust =1; 
    const int LOOKBACK_COUNT = 10;
    static List<Projectile> PROJECTILES = new List<Projectile>();

    [SerializeField]
    private bool _awake = true;
    public bool awake{
        get{return _awake;}
        private set{_awake = value;}
    }

    private Vector3 prevPos;
    private List<float> deltas = new List<float>();
    private Rigidbody rigid;


    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        awake = true;
        prevPos = new Vector3(1000,1000,0);
        deltas.Add(1000);
        PROJECTILES.Add(this);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(rigid.isKinematic || !awake) return;

        Vector3 deltaV3 = transform.position - prevPos;
        deltas.Add(deltaV3.magnitude);
        prevPos = transform.position;

        while (deltas.Count > LOOKBACK_COUNT)
        {
            deltas.RemoveAt(0);
        }
        float maxDelta = 0;
        foreach(float f in deltas)
        {
            if(f>maxDelta) maxDelta = f;
        }
        if(maxDelta <= Physics.sleepThreshold)
        {
            awake = false;
            rigid.Sleep();
        }
    }

    //put input handlinng in update
    public void Update()
    {
        //check for player inputs 
        if(Input.GetButtonDown("Vertical") && 0<NumBoosts)
        {
            if(Input.GetButtonDown("Fire1"))
            {
                //apply up force
                rigid.AddForce(transform.up*Thrust, ForceMode.VelocityChange);
            }
            else
            {
                //apply down force
                rigid.AddForce(Vector3.down *Thrust, ForceMode.VelocityChange);

            }
            //decrease counter 
            NumBoosts=NumBoosts-1; 
            print("boost");

        }
        if(Input.GetButtonDown("Horizontal")&& 0<NumBoosts)
        {
            if(Input.GetButtonDown("Fire2"))
            {
                //right force
                rigid.AddForce(Vector3.right*Thrust*2, ForceMode.VelocityChange);

            }
            else
            {
                //left force
                rigid.AddForce(Vector3.left *Thrust, ForceMode.VelocityChange);

            }
            NumBoosts=NumBoosts-1; 
            print("boost");
        }
    }

    private void OnDestroy()
    {
        PROJECTILES.Remove(this);
    }
    static public void DESTROY_PROJECTILES()
    {
        foreach (Projectile p in PROJECTILES)
        {
            Destroy(p.gameObject);
        } 
    }
}
