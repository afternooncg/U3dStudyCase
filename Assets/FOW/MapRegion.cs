using System.Collections.Generic;
using UnityEngine;

public class MapRegion : MonoBehaviour
{
	Transform m_Transform;
	public Vector2 range = new Vector2(2f, 30f);

    public Vector3 center;
    public Vector3[] region;

    public Color mapColor = new Color(0,0,0,1);

    public FogSystem.LOSChecks lineOfSightCheck = FogSystem.LOSChecks.OnlyOnce;
	public bool isActive = true;

    FogSystem.Regioner m_Regioner;
    //----------------------------------------------
    void Awake ()
	{
        
        m_Transform = transform;
        m_Regioner = FogSystem.CreateRevealer();
	}
    //----------------------------------------------
    Texture2D m_pTemp = null;
    Texture m_pRenderTex = null;
    //----------------------------------------------
    private void Start()
    {
        //Texture2D tex = FogSystem.instance.Maping;
        //if(tex)
        //{
        //    float fScaleX = 1f;// FogSystem.instance.textureSize/tex.width;
        //    float fScaleY = 1f;// FogSystem.instance.textureSize/tex.height;
        //    List<Vector2> vRegionL = new List<Vector2>();
        //    List<Vector2> vRegionR = new List<Vector2>();
            
        //    int first_r;
        //    int first_c;
        //    bool bFirst = false;
        //    first_r = 0;
        //    first_c = 0;
        //    Color32[] colors = tex.GetPixels32();
        //    bool bHit = false;
        //    bool bCheck = false;
        //    int width = tex.width;
        //    int height = tex.height;
        //    m_pTemp = new Texture2D(width, height);
        //    for (int c = 0; c < height; ++c)
        //    {
        //        for (int r = 0; r < width; r++)
        //        {
        //            bool bRight = false;
        //            if (mapColor == colors[r + c * height])
        //            {
        //                if (bCheck && r + 1 < width && mapColor!= colors[r+1 + c * height])
        //                {
        //                    bRight = true;
        //                    bCheck = false;
        //                }
        //                bHit = true;
        //            }
        //            else
        //            {
        //                bCheck = false;
        //                bHit = false;
        //            }

        //            if(bHit && !bCheck)
        //            {
        //                if(!bFirst)
        //                {
        //                    first_r = r;
        //                    first_c = c;
        //                    bFirst = true;
        //                }
        //                if(bRight)
        //                    vRegionR.Add(new Vector2(r - first_r,c - first_c));
        //                else
        //                    vRegionL.Add(new Vector2(r - first_r,c - first_c));

        //                m_pTemp.SetPixel(r, c, colors[r + c * height]);
        //                bCheck = true;
        //            }
        //        }

        //        bCheck = false;
        //        bHit = false;
        //    }

        //    m_pTemp.Apply();

        //    region = new Vector3[vRegionL.Count+ vRegionR.Count];
        //    for (int i = 0; i < vRegionL.Count; ++i)
        //    {
        //        region[i].x = vRegionL[i].x * fScaleX;
        //        region[i].z = vRegionL[i].y * fScaleY;
        //    }
        //    for (int i = 0; i < vRegionR.Count; ++i)
        //    {
        //        region[i+ vRegionL.Count].x = vRegionR[vRegionR.Count-i-1].x* fScaleX;
        //        region[i+ vRegionL.Count].z = vRegionR[vRegionR.Count - i - 1].y* fScaleY;
        //    }
        //}
    }
    //----------------------------------------------
    public void OnGUI()
    {
        int ww = 0;
        Vector3 vp = Camera.main.WorldToScreenPoint(transform.position);
        if (m_pTemp != null)
        {
            ww += m_pTemp.width;
            GUI.DrawTexture(new Rect(vp.x, vp.y, m_pTemp.width, m_pTemp.height), m_pTemp);
        }
        if(m_pRenderTex!=null)
            GUI.DrawTexture(new Rect(vp.x+ ww, vp.y, m_pRenderTex.width, m_pRenderTex.height), m_pRenderTex);
    }
    //----------------------------------------------
    void OnDisable ()
	{
        m_Regioner.isActive = false;
	}
    //----------------------------------------------
    void OnDestroy ()
	{
        FogSystem.DeleteRevealer(m_Regioner);
        m_Regioner = null;
	}
    //----------------------------------------------
    void LateUpdate ()
	{
		if (isActive)
		{
			if (lineOfSightCheck != FogSystem.LOSChecks.OnlyOnce) m_Regioner.cachedBuffer = null;

            m_Regioner.pos = m_Transform.position + center;
            if(region != null && m_Regioner.region.Count != region.Length)
            {
                m_Regioner.region = new System.Collections.Generic.List<Vector3>(region);
            }
            m_Regioner.inner = range.x;
            m_Regioner.outer = range.y;
            m_Regioner.los = lineOfSightCheck;
            m_Regioner.isActive = true;
		}
		else
		{
            m_Regioner.isActive = false;
            m_Regioner.cachedBuffer = null;
		}
	}
    //----------------------------------------------
    void OnDrawGizmosSelected ()
	{
        return;
        Renderer render1 = gameObject.GetComponent<Renderer>();

        MeshFilter render = gameObject.GetComponent<MeshFilter>();
        if (render == null || render1 == null) return;

        Gizmos.DrawLine(Camera.main.transform.position, Camera.main.transform.position + Camera.main.transform.forward * 1000f);

        Matrix4x4 model = Camera.main.cameraToWorldMatrix;

        Matrix4x4 mv = model;

        Vector3 viewDir = Camera.main.transform.position - gameObject.transform.position;
        viewDir.Normalize();
        for (int i = 0; i < render.mesh.vertices.Length; ++i)
        {

            Vector3 worldNormal = render.mesh.normals[i];// mv.MultiplyVector(render.mesh.normals[i]);
            worldNormal.Normalize();
            float ff = Vector3.Dot(viewDir, worldNormal);
            if (Mathf.Abs(ff) <= 0.02f)
            {
                Gizmos.color = Color.red;
            }
            else Gizmos.color = Color.white;
            Vector3 vpos = render.mesh.vertices[i];
            vpos.x = vpos.x * gameObject.transform.localScale.x;
            vpos.y = vpos.y * gameObject.transform.localScale.y;
            vpos.z = vpos.z * gameObject.transform.localScale.z;
            vpos += gameObject.transform.position;
            Gizmos.DrawSphere(vpos, 0.1f);
            Gizmos.DrawLine(vpos, vpos + worldNormal * 2f);
        }
    }
    //----------------------------------------------
    public void Rebuild ()
    {
        m_Regioner.cachedBuffer = null;
    }
    //----------------------------------------------
    public void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        m_pRenderTex = source;
    }
}