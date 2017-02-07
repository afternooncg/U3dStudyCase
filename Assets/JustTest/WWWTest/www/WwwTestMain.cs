using UnityEngine;
using System.Collections;

public class WwwTestMain : MonoBehaviour {

    private WWW www;
    public TestForGetByScript testForGetByScript;
    private Ray shootRay;

    public string url = "http://images.earthcam.com/ec_metros/ourcams/fridays.jpg";
    IEnumerator Start()
    {
        shootRay = new Ray();                                   // A ray from the gun end forwards.
        print("This is printed immediately");
        iTween.MoveBy(this.gameObject, iTween.Hash("x", 2, "easeType", "easeInOutExpo", "loopType", "pingPong", "delay", .1));
        while (true)
        {
            Debug.Log("while 0");
            testForGetByScript = GameObject.Find("Sphere").GetComponent<TestForGetByScript>();      
            Debug.Log("Read SomeInt " + testForGetByScript.SomeInt);

            yield return new WaitForSeconds(3);

            StartCoroutine("DoSome");
            Time.timeScale = 0.5F;
            Debug.Log("while 1");
            yield return new WaitForSeconds(1);

            Debug.Log("while 2");
            yield return new WaitForSeconds(1);

            break;
        }

        www = new WWW(url);        
        yield return www;
        Renderer renderer = GetComponent<Renderer>();
        renderer.material.mainTexture = www.texture;

      
    }

    void Update()
    {
        //if(www != null)
       // Debug.Log("www loaded " + www.progress);

       
        RaycastHit shootHit = new RaycastHit();                            // A raycast hit to get information about what was hit.

         // Set the shootRay so that it starts at the end of the gun and points forward from the barrel.
        shootRay.origin = GameObject.Find("Sphere").transform.position;
        shootRay.direction = GameObject.Find("Sphere").transform.forward;
        if (Physics.Raycast(shootRay, out shootHit, 100f))
        {
            Debug.Log("Raycast" + shootHit.collider.gameObject.name);
        }
    }
       
    
    IEnumerator DoSome()  
    {  
        Debug.Log("do now");
        yield return new WaitForSeconds(2);
        print("Do 2 seconds later");
        Time.timeScale = 0;   //整个游戏停止 update/fixedupdate不再执行
    }

    void FixedUpdate()
    {
        //print("FixedUpdate Call");
    }
}
