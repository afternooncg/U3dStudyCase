using UnityEngine;
using System.Threading;
using System.Collections.Generic;

public class FogSystem : MonoBehaviour
{
    public enum LOSChecks
    {
        None,
        OnlyOnce,
        EveryUpdate,
    }

    public class Regioner
    {
        public bool isActive = false;
        public LOSChecks los = LOSChecks.None;
        public Vector3 pos = Vector3.zero;
        public List<Vector3> region = new List<Vector3>();
        public float inner = 0f;
        public float outer = 0f;
        public bool[] cachedBuffer;
        public int cachedSize = 0;
        public int cachedX = 0;
        public int cachedY = 0;
    }

    public enum State
    {
        Blending,
        NeedUpdate,
        UpdateTexture0,
        UpdateTexture1,
    }

    static public FogSystem instance;


    public Texture2D Maping;

    private Color32 ms_white = new Color32(255, 255, 255, 255);

    // Height map used for visibility checks. Integers are used instead of floats as integer checks are significantly faster.
    protected int[,] mHeights;
    protected Transform mTrans;
    protected Vector3 mOrigin = Vector3.zero;
    protected Vector3 mSize = Vector3.one;

    static FOWRegionList<Regioner> mRevealers = new FOWRegionList<Regioner>();
    static FOWRegionList<Regioner> mAdded = new FOWRegionList<Regioner>();
    static FOWRegionList<Regioner> mRemoved = new FOWRegionList<Regioner>();

    bool m_bDirtyBuffer0 = true;
    bool m_bDirtyBuffer1 = true;
    protected Color32[] mBuffer0;
    protected Color32[] mBuffer1;
    protected Color32[] mBuffer2;

    protected Texture2D mTexture0;
    protected Texture2D mTexture1;

    protected float mBlendFactor = 0f;
    protected float mNextUpdate = 0f;
    protected int mScreenHeight = 0;
    protected State mState = State.Blending;

    Thread mThread;

    public int worldSize = 256;
    public int textureSize = 128;

    public float updateFrequency = 0.1f;
    public float textureBlendTime = 0.5f;
    public int blurIterations = 2;

    public Vector2 heightRange = new Vector2(0f, 10f);

    public LayerMask raycastMask = -1;

    public float raycastRadius = 0f;

    public float margin = 0.4f;


    public bool debug = false;

    public Texture2D texture0 { get { return mTexture0; } }
    public Texture2D texture1 { get { return mTexture1; } }
    public float blendFactor { get { return mBlendFactor; } }
    //-----------------------------------------------------
    static public Regioner CreateRevealer()
    {
        Regioner rev = new Regioner();
        rev.isActive = false;
        lock (mAdded) mAdded.Add(rev);
        return rev;
    }
    //-----------------------------------------------------
    static public void DeleteRevealer(Regioner rev)
    {
        lock (mRemoved) mRemoved.Add(rev);
    }
    //-----------------------------------------------------
    void Awake()
    {
        instance = this;
    }
    //-----------------------------------------------------
    void Start()
    {
        mTrans = transform;
        mHeights = new int[textureSize, textureSize];
        mSize = new Vector3(worldSize, heightRange.y - heightRange.x, worldSize);

        mOrigin = mTrans.position;
        mOrigin.x -= worldSize * 0.5f;
        mOrigin.z -= worldSize * 0.5f;

        int size = textureSize * textureSize;
        mBuffer0 = new Color32[size];
        mBuffer1 = new Color32[size];
        mBuffer2 = new Color32[size];

        // Create the height grid
        CreateGrid();

        // Update the fog of war's visibility so that it's updated right away
        UpdateBuffer();
        UpdateTexture();
        mNextUpdate = Time.time + updateFrequency;

        // Add a thread update function -- all visibility checks will be done on a separate thread
        mThread = new Thread(ThreadUpdate);
        mThread.Start();
    }
    //-----------------------------------------------------
    void OnDestroy()
    {
        if (mThread != null)
        {
            mThread.Abort();
            while (mThread.IsAlive) Thread.Sleep(1);
            mThread = null;
        }
    }
    //-----------------------------------------------------
    void Update()
    {
        if (textureBlendTime > 0f)
        {
            mBlendFactor = Mathf.Clamp01(mBlendFactor + Time.deltaTime / textureBlendTime);
        }
        else mBlendFactor = 1f;

        if (mState == State.Blending)
        {
            float time = Time.time;

            if (mNextUpdate < time)
            {
                mNextUpdate = time + updateFrequency;
                mState = State.NeedUpdate;
            }
        }
        else if (mState != State.NeedUpdate)
        {
            UpdateTexture();
        }
    }

    float mElapsed = 0f;
    //-----------------------------------------------------
    void ThreadUpdate()
    {
        System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

        for (;;)
        {
            if (mState == State.NeedUpdate)
            {
                sw.Reset();
                sw.Start();
                UpdateBuffer();
                sw.Stop();
                if (debug) Debug.Log(sw.ElapsedMilliseconds);
                mElapsed = 0.001f * (float)sw.ElapsedMilliseconds;
                mState = State.UpdateTexture0;
            }
            Thread.Sleep(1);
        }
    }
    //-----------------------------------------------------
    void OnDrawGizmosSelected()
    {
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(new Vector3(0f, (heightRange.x + heightRange.y) * 0.5f, 0f),
            new Vector3(worldSize, heightRange.y - heightRange.x, worldSize));

        UnityEditor.Handles.PositionHandle(mOrigin, Quaternion.identity);

        for (int j = 0; j < mRevealers.size; ++j)
        {
            Regioner r = mRevealers[j];

            Vector3 pos = r.pos;

            Gizmos.DrawCube(pos + Vector3.up * 2f, Vector3.one * 0.1f);
            for (int i = 0; i < r.region.Count; ++i)
            {
                Vector3 vpp = r.region[i];
                if(i+1 < r.region.Count)
                    Gizmos.DrawLine(vpp+Vector3.up*2f, r.region[i+1] + Vector3.up * 2f);
            }
        }
    }
    //-----------------------------------------------------
    bool IsVisible(int sx, int sy, int fx, int fy, float outer, int sightHeight, int variance)
    {
        int dx = Mathf.Abs(fx - sx);
        int dy = Mathf.Abs(fy - sy);
        int ax = sx < fx ? 1 : -1;
        int ay = sy < fy ? 1 : -1;
        int dir = dx - dy;

        float sh = sightHeight;
        float fh = mHeights[fx, fy];

        float invDist = 1f / outer;
        float lerpFactor = 0f;

        for (;;)
        {
            if (sx == fx && sy == fy) return true;

            int xd = fx - sx;
            int yd = fy - sy;

            // If the sampled height is higher than expected, then the point must be obscured
            lerpFactor = invDist * Mathf.Sqrt(xd * xd + yd * yd);
            if (mHeights[sx, sy] > Mathf.Lerp(fh, sh, lerpFactor) + variance) return false;

            int dir2 = dir << 1;

            if (dir2 > -dy)
            {
                dir -= dy;
                sx += ax;
            }

            if (dir2 < dx)
            {
                dir += dx;
                sy += ay;
            }
        }
    }
    //-----------------------------------------------------
    public int WorldToGridHeight(float height)
    {
        int val = Mathf.RoundToInt(height / mSize.y * 255f);
        return Mathf.Clamp(val, 0, 255);
    }
    //-----------------------------------------------------
    protected virtual void CreateGrid()
    {
        return;
        Vector3 pos = mOrigin;
        pos.y += mSize.y;
        float texToWorld = (float)worldSize / textureSize;
        bool useSphereCast = raycastRadius > 0f;

        for (int z = 0; z < textureSize; ++z)
        {
            pos.z = mOrigin.z + z * texToWorld;

            for (int x = 0; x < textureSize; ++x)
            {
                pos.x = mOrigin.x + x * texToWorld;

                RaycastHit hit;

                if (useSphereCast)
                {
                    if (Physics.SphereCast(new Ray(pos, Vector3.down), raycastRadius, out hit, mSize.y, raycastMask))
                    {
                        mHeights[x, z] = WorldToGridHeight(pos.y - hit.distance - raycastRadius);
                        continue;
                    }
                }
                else if (Physics.Raycast(new Ray(pos, Vector3.down), out hit, mSize.y, raycastMask))
                {
                    mHeights[x, z] = WorldToGridHeight(pos.y - hit.distance);
                    continue;
                }
                mHeights[x, z] = 0;
            }
        }
    }
    //-----------------------------------------------------
    void UpdateBuffer()
    {
        // Add all items scheduled to be added
        if (mAdded.size > 0)
        {
            lock (mAdded)
            {
                while (mAdded.size > 0)
                {
                    int index = mAdded.size - 1;
                    mRevealers.Add(mAdded.buffer[index]);
                    mAdded.RemoveAt(index);
                }
            }
        }

        // Remove all items scheduled for removal
        if (mRemoved.size > 0)
        {
            lock (mRemoved)
            {
                while (mRemoved.size > 0)
                {
                    int index = mRemoved.size - 1;
                    mRevealers.Remove(mRemoved.buffer[index]);
                    mRemoved.RemoveAt(index);
                }
            }
        }

        // Use the texture blend time, thus estimating the time this update will finish
        // Doing so helps avoid visible changes in blending caused by the blended result being X milliseconds behind.
        float factor = (textureBlendTime > 0f) ? Mathf.Clamp01(mBlendFactor + mElapsed / textureBlendTime) : 1f;

        // Clear the buffer's red channel (channel used for current visibility -- it's updated right after)
        for (int i = 0, imax = mBuffer0.Length; i < imax; ++i)
        {
            mBuffer0[i] = Color32.Lerp(mBuffer0[i], mBuffer1[i], factor);
            mBuffer1[i].r = 0;
        }

        // For conversion from world coordinates to texture coordinates
        float worldToTex = (float)textureSize / worldSize;

        // Update the visibility buffer, one revealer at a time
        for (int i = 0; i < mRevealers.size; ++i)
        {
            Regioner rev = mRevealers[i];
            if (!rev.isActive) continue;

            if (rev.los == LOSChecks.None)
            {
                RevealUsingRadius(rev, worldToTex);
            }
            else if (rev.los == LOSChecks.OnlyOnce)
            {
                RevealUsingCache(rev, worldToTex);
            }
            else
            {
                RevealUsingLOS(rev, worldToTex);
            }
        }

        // Blur the final visibility data
        for (int i = 0; i < blurIterations; ++i) BlurVisibility();

        // Reveal the map based on what's currently visible
        RevealMap();
    }
    //-----------------------------------------------------
    void RevealUsingRadius(Regioner r, float worldToTex)
    {
        if(r.region.Count > 0)
        {
            RevealUsingPolygon(r, worldToTex, false, LOSChecks.None);
            return;
        }
        // Position relative to the fog of war
        Vector3 pos = r.pos - mOrigin;

        // Coordinates we'll be dealing with
        int xmin = Mathf.RoundToInt((pos.x - r.outer) * worldToTex);
        int ymin = Mathf.RoundToInt((pos.z - r.outer) * worldToTex);
        int xmax = Mathf.RoundToInt((pos.x + r.outer) * worldToTex);
        int ymax = Mathf.RoundToInt((pos.z + r.outer) * worldToTex);

        int cx = Mathf.RoundToInt(pos.x * worldToTex);
        int cy = Mathf.RoundToInt(pos.z * worldToTex);

        cx = Mathf.Clamp(cx, 0, textureSize - 1);
        cy = Mathf.Clamp(cy, 0, textureSize - 1);

        int radius = Mathf.RoundToInt(r.outer * r.outer * worldToTex * worldToTex);

        for (int y = ymin; y < ymax; ++y)
        {
            if (y > -1 && y < textureSize)
            {
                int yw = y * textureSize;

                for (int x = xmin; x < xmax; ++x)
                {
                    if (x > -1 && x < textureSize)
                    {
                        int xd = x - cx;
                        int yd = y - cy;
                        int dist = xd * xd + yd * yd;

                        // Reveal this pixel
                        if (dist < radius) mBuffer1[x + yw].r = 255;
                    }
                }
            }
        }
    }
    //-----------------------------------------------------
    public bool IsPointInPolygon(float pointX, float pointZ, Vector3[] polygon, Vector3 vCenter)
    {
        int i = 0;
        int polygonLength = polygon.Length;
        bool inside = false;
        float startX, startZ, endX, endZ;

        Vector3 endPoint = polygon[polygonLength - 1];
        endX = endPoint.x- vCenter.x;
        endZ = endPoint.z - vCenter.z;
        while (i < polygonLength)
        {
            startX = endX;
            startZ = endZ;
            endPoint = polygon[i++];
            endX = endPoint.x - vCenter.x;
            endZ = endPoint.z - vCenter.z;
            inside ^= (endZ > pointZ ^ startZ > pointZ)&&((pointX - endX) < (pointZ - endZ) * (startX - endX) / (startZ - endZ));
        }
        return inside;
    }
    //-----------------------------------------------------
    void RevealUsingPolygon(Regioner r, float worldToTex, bool bCache = false, LOSChecks types = LOSChecks.None)
    {
        int xmin = 99999;
        int ymin = 99999;
        int xmax = -999999;
        int ymax = -999999;
        Vector3 pos = Vector3.zero;
        List<Vector3> vPolygons = new List<Vector3>();
        //for (int i = 0; i < r.region.Count; ++i)
        //    pos += (r.region[i] - mOrigin);
        //pos /= r.region.Count;
        pos = r.pos - mOrigin;

        for (int i = 0; i < r.region.Count; ++i)
        {
            Vector3 vpp = r.region[i] - mOrigin;
            int tepx = Mathf.RoundToInt((vpp.x) * worldToTex);
            int tepz = Mathf.RoundToInt((vpp.z) * worldToTex);
            xmin = Mathf.Min(tepx, xmin);
            ymin = Mathf.Min(tepz, ymin);
            xmax = Mathf.Max(tepx, xmax);
            ymax = Mathf.Max(tepz, ymax);

            vPolygons.Add(new Vector3(tepx, 0, tepz));
        }

        int cx = Mathf.RoundToInt(pos.x * worldToTex);
        int cy = Mathf.RoundToInt(pos.z * worldToTex);

        cx = Mathf.Clamp(cx, 0, textureSize - 1);
        cy = Mathf.Clamp(cy, 0, textureSize - 1);

        int size = Mathf.RoundToInt(xmax - xmin);
        if (bCache)
        {
            r.cachedBuffer = new bool[size * size];
            r.cachedSize = size;
            r.cachedX = xmin;
            r.cachedY = ymin;

            for (int i = 0, imax = size * size; i < imax; ++i) r.cachedBuffer[i] = false;
        }

        for (int y = ymin; y < ymax; ++y)
        {
            if (y > -1 && y < textureSize)
            {
                int yw = y * textureSize;

                for (int x = xmin; x < xmax; ++x)
                {
                    if (x > -1 && x < textureSize)
                    {
                        int xd = x - cx;
                        int yd = y - cy;
                        int dist = xd * xd + yd * yd;
                        int index = x + y * textureSize;

                        Vector2 v = new Vector2(xd, yd);

                        int sx = cx + Mathf.RoundToInt(v.x);
                        int sy = cy + Mathf.RoundToInt(v.y);

                        if (sx > -1 && sx < textureSize && sx < worldSize &&
                            sy > -1 && sy < textureSize && sy < worldSize &&
                            IsPointInPolygon(x, y, vPolygons.ToArray(), Vector3.zero))
                        {
                            if (bCache)
                                r.cachedBuffer[(x - xmin) + (y - ymin) * size] = true;
                            else
                            {
                                if (types == LOSChecks.EveryUpdate)
                                {
                                    mBuffer1[index] = ms_white;
                                }
                                else mBuffer1[x + yw].r = 255;
                            }
                        }
                    }
                }
            }
        }
    }
    //-----------------------------------------------------
    void RevealUsingLOS(Regioner r, float worldToTex)
    {
        if(r.region.Count > 0)
        {
            RevealUsingPolygon(r, worldToTex, false, LOSChecks.EveryUpdate);
            return;
        }
        // Position relative to the fog of war
        Vector3 pos = r.pos - mOrigin;

        // Coordinates we'll be dealing with
        int xmin = Mathf.RoundToInt((pos.x - r.outer) * worldToTex);
        int ymin = Mathf.RoundToInt((pos.z - r.outer) * worldToTex);
        int xmax = Mathf.RoundToInt((pos.x + r.outer) * worldToTex);
        int ymax = Mathf.RoundToInt((pos.z + r.outer) * worldToTex);

        int cx = Mathf.RoundToInt(pos.x * worldToTex);
        int cy = Mathf.RoundToInt(pos.z * worldToTex);

        cx = Mathf.Clamp(cx, 0, textureSize - 1);
        cy = Mathf.Clamp(cy, 0, textureSize - 1);

        int minRange = Mathf.RoundToInt(r.inner * r.inner * worldToTex * worldToTex);
        int maxRange = Mathf.RoundToInt(r.outer * r.outer * worldToTex * worldToTex);
        int gh = WorldToGridHeight(r.pos.y);
        int variance = Mathf.RoundToInt(Mathf.Clamp01(margin / (heightRange.y - heightRange.x)) * 255);
        Color32 white = new Color32(255, 255, 255, 255);

        for (int y = ymin; y < ymax; ++y)
        {
            if (y > -1 && y < textureSize)
            {
                for (int x = xmin; x < xmax; ++x)
                {
                    if (x > -1 && x < textureSize)
                    {
                        int xd = x - cx;
                        int yd = y - cy;
                        int dist = xd * xd + yd * yd;
                        int index = x + y * textureSize;

                        if (dist < minRange || (cx == x && cy == y))
                        {
                            mBuffer1[index] = white;
                        }
                        else if (dist < maxRange)
                        {
                            Vector2 v = new Vector2(xd, yd);
                            v.Normalize();
                            v *= r.inner;

                            int sx = cx + Mathf.RoundToInt(v.x);
                            int sy = cy + Mathf.RoundToInt(v.y);

                            if (sx > -1 && sx < textureSize &&
                                sy > -1 && sy < textureSize &&
                                IsVisible(sx, sy, x, y, Mathf.Sqrt(dist), gh, variance))
                            {
                                mBuffer1[index] = white;
                            }
                        }
                    }
                }
            }
        }
    }
    //-----------------------------------------------------
    void RevealUsingCache(Regioner r, float worldToTex)
    {
        if (r.cachedBuffer == null) RevealIntoCache(r, worldToTex);

        for (int y = r.cachedY, ymax = r.cachedY + r.cachedSize; y < ymax; ++y)
        {
            if (y > -1 && y < textureSize)
            {
                int by = y * textureSize;
                int cy = (y - r.cachedY) * r.cachedSize;

                for (int x = r.cachedX, xmax = r.cachedX + r.cachedSize; x < xmax; ++x)
                {
                    if (x > -1 && x < textureSize)
                    {
                        int cachedIndex = x - r.cachedX + cy;

                        if (r.cachedBuffer[cachedIndex])
                        {
                            mBuffer1[x + by] = ms_white;
                        }
                    }
                }
            }
        }
    }
    //-----------------------------------------------------
    void RevealIntoCache(Regioner r, float worldToTex)
    {
        if (r.region.Count > 0)
        {
            RevealUsingPolygon(r, worldToTex, true, LOSChecks.OnlyOnce);
            return;
        }

        // Position relative to the fog of war
        Vector3 pos = r.pos - mOrigin;

        // Coordinates we'll be dealing with
        int xmin = Mathf.RoundToInt((pos.x - r.outer) * worldToTex);
        int ymin = Mathf.RoundToInt((pos.z - r.outer) * worldToTex);
        int xmax = Mathf.RoundToInt((pos.x + r.outer) * worldToTex);
        int ymax = Mathf.RoundToInt((pos.z + r.outer) * worldToTex);

        int cx = Mathf.RoundToInt(pos.x * worldToTex);
        int cy = Mathf.RoundToInt(pos.z * worldToTex);

        cx = Mathf.Clamp(cx, 0, textureSize - 1);
        cy = Mathf.Clamp(cy, 0, textureSize - 1);

        // Create the buffer to reveal into
        int size = Mathf.RoundToInt(xmax - xmin);
        r.cachedBuffer = new bool[size * size];
        r.cachedSize = size;
        r.cachedX = xmin;
        r.cachedY = ymin;

        // The buffer should start off clear
        for (int i = 0, imax = size * size; i < imax; ++i) r.cachedBuffer[i] = false;

        int minRange = Mathf.RoundToInt(r.inner * r.inner * worldToTex * worldToTex);
        int maxRange = Mathf.RoundToInt(r.outer * r.outer * worldToTex * worldToTex);
        int variance = Mathf.RoundToInt(Mathf.Clamp01(margin / (heightRange.y - heightRange.x)) * 255);
        int gh = WorldToGridHeight(r.pos.y);

        for (int y = ymin; y < ymax; ++y)
        {
            if (y > -1 && y < textureSize)
            {
                for (int x = xmin; x < xmax; ++x)
                {
                    if (x > -1 && x < textureSize)
                    {
                        int xd = x - cx;
                        int yd = y - cy;
                        int dist = xd * xd + yd * yd;

                        if (dist < minRange || (cx == x && cy == y))
                        {
                            r.cachedBuffer[(x - xmin) + (y - ymin) * size] = true;
                        }
                        else if (dist < maxRange)
                        {
                            Vector2 v = new Vector2(xd, yd);
                            v.Normalize();
                            v *= r.inner;

                            int sx = cx + Mathf.RoundToInt(v.x);
                            int sy = cy + Mathf.RoundToInt(v.y);

                            if (sx > -1 && sx < textureSize &&
                                sy > -1 && sy < textureSize &&
                                IsVisible(sx, sy, x, y, Mathf.Sqrt(dist), gh, variance))
                            {
                                r.cachedBuffer[(x - xmin) + (y - ymin) * size] = true;
                            }
                        }
                    }
                }
            }
        }
    }
    //-----------------------------------------------------
    void BlurVisibility()
    {
        Color32 c;

        for (int y = 0; y < textureSize; ++y)
        {
            int yw = y * textureSize;
            int yw0 = (y - 1);
            if (yw0 < 0) yw0 = 0;
            int yw1 = (y + 1);
            if (yw1 == textureSize) yw1 = y;

            yw0 *= textureSize;
            yw1 *= textureSize;

            for (int x = 0; x < textureSize; ++x)
            {
                int x0 = (x - 1);
                if (x0 < 0) x0 = 0;
                int x1 = (x + 1);
                if (x1 == textureSize) x1 = x;

                int index = x + yw;
                int val = mBuffer1[index].r;

                val += mBuffer1[x0 + yw].r;
                val += mBuffer1[x1 + yw].r;
                val += mBuffer1[x + yw0].r;
                val += mBuffer1[x + yw1].r;

                val += mBuffer1[x0 + yw0].r;
                val += mBuffer1[x1 + yw0].r;
                val += mBuffer1[x0 + yw1].r;
                val += mBuffer1[x1 + yw1].r;

                c = mBuffer2[index];
                c.r = (byte)(val / 9);
                mBuffer2[index] = c;
            }
        }

        // Swap the buffer so that the blurred one is used
        Color32[] temp = mBuffer1;
        mBuffer1 = mBuffer2;
        mBuffer2 = temp;
    }
    //-----------------------------------------------------
    void RevealMap()
    {
        for (int y = 0; y < textureSize; ++y)
        {
            int yw = y * textureSize;

            for (int x = 0; x < textureSize; ++x)
            {
                int index = x + yw;
                Color32 c = mBuffer1[index];

                if (c.g < c.r)
                {
                    c.g = c.r;
                    mBuffer1[index] = c;
                }
            }
        }
    }
    //-----------------------------------------------------
    void UpdateTexture()
    {
        if (mScreenHeight != Screen.height || mTexture0 == null)
        {
            mScreenHeight = Screen.height;

            if (mTexture0 != null) Destroy(mTexture0);
            if (mTexture1 != null) Destroy(mTexture1);

            // Native ARGB format is the fastest as it involves no data conversion
            mTexture0 = new Texture2D(textureSize, textureSize, TextureFormat.ARGB32, false);
            mTexture1 = new Texture2D(textureSize, textureSize, TextureFormat.ARGB32, false);

            mTexture0.wrapMode = TextureWrapMode.Clamp;
            mTexture1.wrapMode = TextureWrapMode.Clamp;

            mTexture0.SetPixels32(mBuffer0);
            mTexture0.Apply();
            mTexture1.SetPixels32(mBuffer1);
            mTexture1.Apply();
            mState = State.Blending;
        }
        else if (mState == State.UpdateTexture0)
        {
            // Texture updates are spread between two frames to make it even less noticeable when they get updated
            mTexture0.SetPixels32(mBuffer0);
            mTexture0.Apply();
            mState = State.UpdateTexture1;
            mBlendFactor = 0f;
        }
        else if (mState == State.UpdateTexture1)
        {
            mTexture1.SetPixels32(mBuffer1);
            mTexture1.Apply();
            mState = State.Blending;
        }
    }
    //-----------------------------------------------------
    public bool IsVisible(Vector3 pos)
    {
        if (mBuffer0 == null) return false;
        pos -= mOrigin;

        float worldToTex = (float)textureSize / worldSize;
        int cx = Mathf.RoundToInt(pos.x * worldToTex);
        int cy = Mathf.RoundToInt(pos.z * worldToTex);

        cx = Mathf.Clamp(cx, 0, textureSize - 1);
        cy = Mathf.Clamp(cy, 0, textureSize - 1);
        int index = cx + cy * textureSize;
        return mBuffer1[index].r > 0 || mBuffer0[index].r > 0;
    }
    //-----------------------------------------------------
    public bool IsExplored(Vector3 pos)
    {
        if (mBuffer0 == null) return false;
        pos -= mOrigin;

        float worldToTex = (float)textureSize / worldSize;
        int cx = Mathf.RoundToInt(pos.x * worldToTex);
        int cy = Mathf.RoundToInt(pos.z * worldToTex);

        cx = Mathf.Clamp(cx, 0, textureSize - 1);
        cy = Mathf.Clamp(cy, 0, textureSize - 1);
        return mBuffer0[cx + cy * textureSize].g > 0;
    }
}