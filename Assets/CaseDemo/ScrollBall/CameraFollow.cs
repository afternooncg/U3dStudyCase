using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
    public GameObject Player;
    private Vector3 m_subOffset;

    // Use this for initialization
    void Start()
    {
        if (Player)
            m_subOffset = Player.transform.position - this.transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (Player)
            this.transform.position = Player.transform.position - m_subOffset;
    }
}
