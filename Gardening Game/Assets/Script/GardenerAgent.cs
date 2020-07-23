
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;


public class GardenerAgent : Agent
{
    Rigidbody m_AgentRb;
    public float moveSpeed = 2;
    public float turnSpeed = 300;
    public float scale;
    public int myType;
    public GameObject area;
    GardenArea m_MyArea;
    GardenScene m_scene;
    EnvironmentParameters m_ResetParams;

    public Material ingredient1;
    public Material ingredient2;
    public Material ingredient3;
    public Material ingredient4;


    public override void Initialize()
    {
        m_AgentRb = GetComponent<Rigidbody>();
        m_MyArea = area.GetComponent<GardenArea>();
        m_scene = FindObjectOfType<GardenScene>();
        m_ResetParams = Academy.Instance.EnvironmentParameters;
        scale = m_ResetParams.GetWithDefault("scale", 1.5f);
        switch (myType % 4)
        {
            case 0:
                gameObject.GetComponentInChildren<Renderer>().material = ingredient1;
                break;
            case 1:
                gameObject.GetComponentInChildren<Renderer>().material = ingredient2;
                break;
            case 2:
                gameObject.GetComponentInChildren<Renderer>().material = ingredient3;
                break;
            case 3:
                gameObject.GetComponentInChildren<Renderer>().material = ingredient4;
                break;
        }

    }

    private void Update()
    {
        AddReward(-0.0001f);
    }

    public override void OnEpisodeBegin()
    {
        m_AgentRb.velocity = Vector3.zero;

        transform.position = new Vector3(Random.Range(-m_MyArea.range, m_MyArea.range),
            2f, Random.Range(-m_MyArea.range, m_MyArea.range))
            + area.transform.position;
        transform.rotation = Quaternion.Euler(new Vector3(0f, Random.Range(0, 360)));

    }


    public void MoveAgent(float[] act)
    {
        
        var dirToGo = Vector3.zero;
        var rotateDir = Vector3.zero;


        var forwardAxis = (int)act[0];
        var rightAxis = (int)act[1];
        var rotateAxis = (int)act[2];

        switch (forwardAxis)
        {
            case 1:
                dirToGo = transform.forward;
                break;
            case 2:
                dirToGo = -transform.forward;
                break;
        }

        switch (rightAxis)
        {
            case 1:
                dirToGo = transform.right;
                break;
            case 2:
                dirToGo = -transform.right;
                break;
        }

        switch (rotateAxis)
        {
            case 1:
                rotateDir = -transform.up;
                break;
            case 2:
                rotateDir = transform.up;
                break;
        }



        m_AgentRb.AddForce(dirToGo * moveSpeed);
        transform.Rotate(rotateDir, Time.fixedDeltaTime * turnSpeed);


        if (m_AgentRb.velocity.sqrMagnitude > 25f) // slow it down
        {
            m_AgentRb.velocity *= 0.95f;
        }

    }



    public float speed = 10;
    public override void OnActionReceived(float[] vectorAction)
    {
        // Actions, size = 3
        //print("recieved action!");
        //print(vectorAction[0].ToString());
        //print(vectorAction[1].ToString());
        //print(vectorAction[2].ToString());

        MoveAgent(vectorAction);

        //print(distanceToTarget.ToString());


    }

    void OnTriggerEnter(Collider collision)
    {
        //print("colliding");
        if (collision.gameObject.CompareTag("Food"))
        {
            FoodScript fs = collision.gameObject.GetComponent<FoodScript>();


            int[] types = fs.OnEaten();
            
            if (fs.fromCooperative)
            {
                float rewd = 0;
                foreach (int i in types)
                {
                    if (i != myType)
                    {
                        rewd += types[i] * (scale);
                    }
                }
                AddReward(rewd);
            }
            else
            {
                AddReward(types[myType]);
            }
        }

    }


    public override void Heuristic(float[] actionsOut)
    {
        if (Input.GetKey(KeyCode.D))
        {
            actionsOut[2] = 2f;
        }
        else if (Input.GetKey(KeyCode.W))
        {
            actionsOut[0] = 1f;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            actionsOut[2] = 1f;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            actionsOut[0] = 2f;
        }
        else
        {
            actionsOut[0] = 0;
            actionsOut[2] = 0;
        }
    }
}
