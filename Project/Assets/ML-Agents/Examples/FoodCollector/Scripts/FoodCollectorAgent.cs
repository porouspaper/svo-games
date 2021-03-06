using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using System;


public class FoodCollectorAgent : Agent
{
    public FoodCollectorSettings m_FoodCollecterSettings;
    public Agent[] allAgents;
    public GameObject area;
    FoodCollectorArea m_MyArea;
    bool m_Shoot;
    bool m_Frozen;
    Rigidbody m_AgentRb;
    float m_LaserLength;
    // Speed of agent rotation.
    public float turnSpeed = 300;
    public int agent_number;
    public float m_FrozenTime;

    // Speed of agent movement.
    public float moveSpeed = 2;
    public Material normalMaterial;
    public Material badMaterial;
    public Material goodMaterial;
    public Material frozenMaterial;
    public GameObject myLaser;
    public bool useVectorObs;
    public bool noRay;

    EnvironmentParameters m_ResetParams;


    public override void Initialize()
    {
        m_AgentRb = GetComponent<Rigidbody>();
        m_MyArea = area.GetComponent<FoodCollectorArea>();
        m_ResetParams = Academy.Instance.EnvironmentParameters;
        SetResetParameters();
        Physics.IgnoreLayerCollision(0, 8);

    }

    public override void CollectObservations(VectorSensor sensor)
    {
        if (useVectorObs)
        {
            var localVelocity = transform.InverseTransformDirection(m_AgentRb.velocity);
            sensor.AddObservation(localVelocity.x);
            sensor.AddObservation(localVelocity.z);
            sensor.AddObservation(System.Convert.ToInt32(m_Shoot));
        }

        /*if (noRay)
        {
            for (int i = 0; i < allAgents.Length; i++)
            {
                var localPositions = allAgents[i].transform.position;
                sensor.AddObservation(localPositions.x);
                sensor.AddObservation(localPositions.y);
                sensor.AddObservation(localPositions.z);
            }

            var foods = GameObject.FindGameObjectsWithTag("food");
            print("food in collect obs " + foods.Length);
            foreach (GameObject food in foods)
            {
                sensor.AddObservation(food.transform.position.x);
                sensor.AddObservation(food.transform.position.y);
                sensor.AddObservation(food.transform.position.z);
            }
        }*/
    }

    public Color32 ToColor(int hexVal)
    {
        var r = (byte)((hexVal >> 16) & 0xFF);
        var g = (byte)((hexVal >> 8) & 0xFF);
        var b = (byte)(hexVal & 0xFF);
        return new Color32(r, g, b, 255);
    }

    public void MoveAgent(float[] act)
    {
        m_Shoot = false;

        if (Time.time > m_FrozenTime + 4f && m_Frozen)
        {
            Unfreeze();
        }

        var dirToGo = Vector3.zero;
        var rotateDir = Vector3.zero;

        if (!m_Frozen)
        {

            var shootCommand = false;
            var forwardAxis = (int)act[0];
            var rightAxis = (int)act[1];
            var rotateAxis = (int)act[2];
            var shootAxis = (int)act[3];

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
            switch (shootAxis)
            {
                case 1:
                    shootCommand = true;
                    break;
            }
            if (shootCommand)
            {
                m_Shoot = true;
                dirToGo *= 0.5f;
                m_AgentRb.velocity *= 0.75f;
            }
            m_AgentRb.AddForce(dirToGo * moveSpeed, ForceMode.VelocityChange);
            transform.Rotate(rotateDir, Time.fixedDeltaTime * turnSpeed);
        }

        if (m_AgentRb.velocity.sqrMagnitude > 25f) // slow it down
        {
            m_AgentRb.velocity *= 0.95f;
        }

        if (m_Shoot)
        {
            logLaser();
            var myTransform = transform;
            myLaser.transform.localScale = new Vector3(1f, 1f, m_LaserLength);
            var rayDir = 25.0f * myTransform.forward;
            Debug.DrawRay(myTransform.position, rayDir, Color.red, 0f, true);
            RaycastHit hit;
            if (Physics.SphereCast(transform.position, 2f, rayDir, out hit, 25f))
            {
                if (hit.collider.gameObject.CompareTag("agent"))
                {
                    AddRewardTemp(-1);
                    hit.collider.gameObject.GetComponent<FoodCollectorAgent>().Hit();
                }
            }
        }
        else
        {
            myLaser.transform.localScale = new Vector3(0f, 0f, 0f);
        }
    }

    void Hit()
    {
        if (!m_FoodCollecterSettings.SchellingCoop)
        {
            Freeze();
            //AddRewardTemp(-50);

        }
    }

    void logLaser()
    {
        m_FoodCollecterSettings.agentLasers[agent_number] += 1;
    }

    void logReward(int val)
    {
        m_FoodCollecterSettings.agentReturns[agent_number] += val;
    }

    void logAppleEaten()
    {
        m_FoodCollecterSettings.applesEaten[agent_number] += 1;
        m_FoodCollecterSettings.totalApples -= 1;
    }

    public override void OnActionReceived(float[] vectorAction)
    {
        MoveAgent(vectorAction);
    }

    void Freeze()
    {
        gameObject.tag = "frozenAgent";
        m_Frozen = true;
        m_FrozenTime = Time.time;
        gameObject.GetComponentInChildren<Renderer>().material = frozenMaterial;
    }

    void Unfreeze()
    {
        m_Frozen = false;
        gameObject.tag = "agent";
        gameObject.GetComponentInChildren<Renderer>().material = normalMaterial;
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
            actionsOut[1] = 0;
        }
        actionsOut[3] = Input.GetKey(KeyCode.Space) ? 1.0f : 0.0f;
    }

    public override void OnEpisodeBegin()
    {
        m_FoodCollecterSettings.EnvironmentReset();
        m_Shoot = false;
        m_AgentRb.velocity = Vector3.zero;
        myLaser.transform.localScale = new Vector3(0f, 0f, 0f);
        transform.position = new Vector3(UnityEngine.Random.Range(-m_MyArea.range, m_MyArea.range),
            2f, UnityEngine.Random.Range(-m_MyArea.range, m_MyArea.range))
            + area.transform.position;
        transform.rotation = Quaternion.Euler(new Vector3(0f, UnityEngine.Random.Range(0, 360)));

        SetResetParameters();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("food"))
        {
            collision.gameObject.GetComponent<FoodLogic>().OnEaten();
            AddRewardTemp(1);
            logAppleEaten();
        }
    }

    public void SetLaserLengths()
    {
        m_LaserLength = m_ResetParams.GetWithDefault("laser_length", 1.0f);
    }

    public void SetAgentScale()
    {
        float agentScale = m_ResetParams.GetWithDefault("agent_scale", 1.0f);
        gameObject.transform.localScale = new Vector3(agentScale, agentScale, agentScale);
    }

    public void SetResetParameters()
    {
        SetLaserLengths();
        SetAgentScale();
    }

    public void AddRewardTemp(int f)
    {
        if (!m_FoodCollecterSettings.SchellingCoop)
        {
            var svoDegrees = m_FoodCollecterSettings.svoDegrees[agent_number];
            double rads = (double) (Math.PI * svoDegrees / 180);
            var weight = 1 / (allAgents.Length - 1);

            for (int i = 0; i < allAgents.Length; i++)
            {
                if (i != agent_number)
                {
                    Agent a = allAgents[i];
                    a.AddReward((float)(weight * Math.Sin(rads) * f));
                }
            }

            AddReward((float)(Math.Cos(rads) * f));
        }
        else
        {
            AddReward(f);
        }
        logReward(f);
    }

    public void FixedUpdate()
    {
        AddReward(-0.0001f);
    }

}
