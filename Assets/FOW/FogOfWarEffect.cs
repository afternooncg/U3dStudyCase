using UnityEngine;

[RequireComponent(typeof(Camera))]
public class FogOfWarEffect : MonoBehaviour
{
	public Shader shader;

	public Color unexploredColor = new Color(0.05f, 0.05f, 0.05f, 1f);
	public Color exploredColor = new Color(0.2f, 0.2f, 0.2f, 1f);

    FogSystem   m_Fog;
	Camera      m_Cam;
	Matrix4x4   m_InverseMVP;
	Material    m_Mat;
    //----------------------------------------------
	void OnEnable ()
	{
        m_Cam = GetComponent<Camera>();
        m_Cam.depthTextureMode = DepthTextureMode.Depth;
		if (shader == null) shader = Shader.Find("CC2/Fog of War");
	}
    //----------------------------------------------
    void OnDisable ()
    {
        if (m_Mat) DestroyImmediate(m_Mat);
    }
    //----------------------------------------------
    void Start ()
	{
		if (!SystemInfo.supportsImageEffects || !shader || !shader.isSupported)
		{
			enabled = false;
		}
	}
    //----------------------------------------------
    void OnRenderImage (RenderTexture source, RenderTexture destination)
	{
        if (m_Fog == null)
        {
            m_Fog = FogSystem.instance;
            if (m_Fog == null) m_Fog = FindObjectOfType(typeof(FogSystem)) as FogSystem;
        }

        if (m_Fog == null || !m_Fog.enabled)
        {
            enabled = false;
            return;
        }

        m_InverseMVP = (m_Cam.projectionMatrix * m_Cam.worldToCameraMatrix).inverse;

		float invScale = 1f / m_Fog.worldSize;
		Transform t = m_Fog.transform;
		float x = t.position.x - m_Fog.worldSize * 0.5f;
		float z = t.position.z - m_Fog.worldSize * 0.5f;

		if (m_Mat == null)
		{
            m_Mat = new Material(shader);
            m_Mat.hideFlags = HideFlags.HideAndDontSave;
		}

		Vector4 camPos = m_Cam.transform.position;

		if (QualitySettings.antiAliasing > 0)
		{
			//RuntimePlatform pl = Application.platform;
			//if (pl == RuntimePlatform.WindowsEditor ||
			//	pl == RuntimePlatform.WindowsPlayer)
			//{
			//	camPos.w = 1f;
			//}
		}

        Vector4 p = Vector4.one;
        p.x = -x * invScale;
        p.y = -z * invScale;
        p.z = invScale;
        p.w = m_Fog.blendFactor;
        m_Mat.SetColor("_Unexplored", unexploredColor);
        m_Mat.SetColor("_Explored", exploredColor);
        m_Mat.SetVector("_CamPos", camPos);
        m_Mat.SetVector("_Params", p);
        m_Mat.SetMatrix("_InverseMVP", m_InverseMVP);
        m_Mat.SetTexture("_FogTex0", m_Fog.texture0);
        m_Mat.SetTexture("_FogTex1", m_Fog.texture1);

		Graphics.Blit(source, destination, m_Mat);
	}
}