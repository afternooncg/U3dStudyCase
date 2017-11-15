using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;


public class XmlTest : MonoBehaviour {

	// Use this for initialization
	void Start () {


        //CreateXmlWithText();
        //RemoveNode();
        SearchNode();
	}
	
	
	void CreateXmlWithNode () {

        XmlDocument xmlDoc = new XmlDocument();
        XmlDeclaration xmldecl;
        xmldecl = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", "yes");
        xmlDoc.AppendChild(xmldecl);


        XmlElement root = xmlDoc.CreateElement("Root");
        xmlDoc.AppendChild(root);


        XmlElement node = xmlDoc.CreateElement("Node");

        root.AppendChild(node);

        XmlElement listNode = xmlDoc.CreateElement("ListNode");
        node.AppendChild(listNode);
        listNode.SetAttribute("num", "10");

        xmlDoc.Save(Application.dataPath + "/testxml.xml");
	}

    void CreateXmlWithText()
    {

        XmlDocument xmlDoc = new XmlDocument();
        XmlDeclaration xmldecl;
        xmldecl = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", "yes");
        xmlDoc.AppendChild(xmldecl);


        XmlElement root = xmlDoc.CreateElement("Root");
        xmlDoc.AppendChild(root);


        XmlElement node = xmlDoc.CreateElement("Node");

        root.AppendChild(node);
        /*
        XmlElement listNode = xmlDoc.CreateElement("ListNode");
        node.AppendChild(listNode);
        listNode.SetAttribute("num", "10");
         */

        //node.InnerText = "<ListNode>textNode</ListNode>";
        //node.InnerXml = "<ListNode>textNode</ListNode>";
        

        Debug.Log(node.OuterXml);
        xmlDoc.Save(Application.dataPath + "/testxml.xml");
    }


    void RemoveNode()
    {
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(Application.dataPath + "/testxml.xml");
        xmlDoc.DocumentElement.RemoveChild(xmlDoc.DocumentElement.FirstChild);
        xmlDoc.Save(Application.dataPath + "/testxml.xml");
        
    }

    void SearchNode()
    {
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(Application.dataPath + "/testxml.xml");
        XmlElement root = xmlDoc.DocumentElement;

        XmlNodeList nodes = xmlDoc.SelectNodes("/Root/Node[@name='n2'");
                
        Debug.Log(nodes.Count);
        if (nodes.Count > 0)
            Debug.Log(nodes[0].OuterXml);


        xmlDoc.Save(Application.dataPath + "/testxml.xml");

    }
}
