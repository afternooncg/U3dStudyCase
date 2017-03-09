﻿using UnityEngine;

using System.Collections;



[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]

public class CaseSnowMain : MonoBehaviour
{

    //Unity可以支持多达64000个顶点，如果一个雪花有4个顶点组成，则最多有16000个雪花

    const int SNOW_NUM = 16000;

    //顶点

    private Vector3[] m_vertices;

    //顶点构成的三角面

    private int[] triangles_;

    //雪花网格的贴图

    private Vector2[] uvs_;

    //雪花的范围

    private float range;

    //雪花范围的倒数，为了提高计算效率

    private float rangeR_;

    private Vector3 move_ = Vector3.zero;



    void Start()
    {

        range = 16f;

        rangeR_ = 1.0f / range;

        m_vertices = new Vector3[SNOW_NUM * 4];

        for (var i = 0; i < SNOW_NUM; ++i)
        {

            float x = Random.Range(-range, range);

            float y = Random.Range(-range, range);

            float z = Random.Range(-range, range);

            var point = new Vector3(x, y, z);

            m_vertices[i * 4 + 0] = point;

            m_vertices[i * 4 + 1] = point;

            m_vertices[i * 4 + 2] = point;

            m_vertices[i * 4 + 3] = point;

        }



        triangles_ = new int[SNOW_NUM * 6];

        for (int i = 0; i < SNOW_NUM; ++i)
        {

            triangles_[i * 6 + 0] = i * 4 + 0;

            triangles_[i * 6 + 1] = i * 4 + 1;

            triangles_[i * 6 + 2] = i * 4 + 2;

            triangles_[i * 6 + 3] = i * 4 + 2;

            triangles_[i * 6 + 4] = i * 4 + 1;

            triangles_[i * 6 + 5] = i * 4 + 3;

        }



        uvs_ = new Vector2[SNOW_NUM * 4];

        for (var i = 0; i < SNOW_NUM; ++i)
        {

            uvs_[i * 4 + 0] = new Vector2(0f, 0f);

            uvs_[i * 4 + 1] = new Vector2(1f, 0f);

            uvs_[i * 4 + 2] = new Vector2(0f, 1f);

            uvs_[i * 4 + 3] = new Vector2(1f, 1f);

        }

        Mesh mesh = new Mesh();

        mesh.name = "MeshSnowFlakes";

        mesh.vertices = m_vertices;

        mesh.triangles = triangles_;

        mesh.uv = uvs_;

        mesh.bounds = new Bounds(Vector3.zero, Vector3.one * 99999999);

        var mf = GetComponent<MeshFilter>();

        mf.sharedMesh = mesh;

    }



    void LateUpdate()
    {

        var target_position = Camera.main.transform.TransformPoint(Vector3.forward * range);

        var mr = GetComponent<Renderer>();

        mr.material.SetFloat("_Range", range);

        mr.material.SetFloat("_RangeR", rangeR_);

        mr.material.SetFloat("_Size", 0.1f);

        mr.material.SetVector("_MoveTotal", move_);

        mr.material.SetVector("_CamUp", Camera.main.transform.up);

        mr.material.SetVector("_TargetPosition", target_position);

        float x = (Mathf.PerlinNoise(0f, Time.time * 0.1f) - 0.5f) * 10f;

        float y = -2f;

        float z = (Mathf.PerlinNoise(Time.time * 0.1f, 0f) - 0.5f) * 10f;

        move_ += new Vector3(x, y, z) * Time.deltaTime;

        move_.x = Mathf.Repeat(move_.x, range * 2f);

        move_.y = Mathf.Repeat(move_.y, range * 2f);

        move_.z = Mathf.Repeat(move_.z, range * 2f);

    }

}