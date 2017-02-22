using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectHelper  {

    public static void CopyLightMapData(Transform frome, Transform to)
    {
        if (frome && to)
        {
            if (frome.childCount == to.childCount)
            {
                Renderer f = frome.GetComponent<MeshRenderer>();
                Renderer t = to.GetComponent<MeshRenderer>();
                if (f && t)
                {
                    t.lightmapIndex = f.lightmapIndex;
                    t.lightmapScaleOffset = f.lightmapScaleOffset;
                }
                for (int i = 0; i < frome.childCount; i++)
                {
                    if (frome.childCount == to.childCount)
                    {
                        Transform cf = frome.GetChild(i);
                        Transform ct = to.GetChild(i);
                        if (frome.childCount == to.childCount)
                        {
                            f = cf.GetComponent<MeshRenderer>();
                            t = ct.GetComponent<MeshRenderer>();
                            if (f && t)
                            {
                                t.lightmapIndex = f.lightmapIndex;
                                t.lightmapScaleOffset = f.lightmapScaleOffset;
                            }
                            CopyLightMapData(cf, ct);
                        }
                    }
                }
            }
        }
    }
}
