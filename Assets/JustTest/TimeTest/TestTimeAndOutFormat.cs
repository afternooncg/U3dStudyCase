using UnityEngine;
using System.Collections;
using System;

public class TestTimeAndOutFormat : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

    public void PrintTime()
    {
        Debug.Log("Time.time" + Time.time);
        Debug.Log("Time.realtimeSinceStartup" + Time.realtimeSinceStartup);
        Debug.Log("Time.deltaTime" + Time.deltaTime);
        Debug.Log("Time.fixedTime" + Time.fixedTime);
        Debug.Log("Time.fixedDeltaTime" + Time.fixedDeltaTime);
        Debug.Log("Time.timeScale" + Time.timeScale);

        Time.timeScale = 2;

        Debug.Log("Time.time" + Time.time);
        Debug.Log("Time.realtimeSinceStartup" + Time.realtimeSinceStartup);
        Debug.Log("Time.deltaTime" + Time.deltaTime);
        Debug.Log("Time.fixedTime" + Time.fixedTime);
        Debug.Log("Time.fixedDeltaTime" + Time.fixedDeltaTime);
        Debug.Log("Time.timeScale" + Time.timeScale);
    }
	
	// Update is called once per frame
	void Update () {

        DateTime dtime = DateTime.Now;
        /*
        Debug.Log("Year:" + dtime.Year + " Month:" + dtime.Month + " Day:" + dtime.Day);
        Debug.Log("Hour:" + dtime.Hour + " Minute:" + dtime.Minute + " Second:" + dtime.Second);
        Debug.Log("Week:" + dtime.DayOfWeek);
         * 
         * /
        
        
        /*
        　年份　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　
【格式】
　yy 　　顯示西元，兩位數。
　yyyy　 顯示西元，四位數。

【Java Script】
time = DateTiem.Now.ToString("yy MM dd ");


　月份　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　
【格式】 
　M　　　　顯示月份，一位數。
　MM　　　顯示月份，兩位數。
　MMM　　顯示月份英文，前三個字母。
　MMMM　顯示月份英文，完整單字。


　日期　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　
【格式】
　d　　　　顯示日期，一位數。
　dd　　　顯示日期，兩位數。



　時　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　 
【格式】
　h　　顯示幾點，一位數，12小時制。
　hh　  顯示幾點，兩位數，12小時制。
　H　   顯示幾點，一位數，24小時制。
　HH　顯示幾點，兩位數，24小時制。


　分　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　 
【格式】
　m　　顯示幾分，一位數。
　mm　  顯示幾分，兩位數。


　秒　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　
【格式】
　m　　顯示幾秒，一位數。
　mm　  顯示幾秒，兩位數。
         */

        //Debug.Log( DateTime.Now.ToString("HH mm s "));
        //Debug.Log(DateTime.Now.ToString("yy MM dd "));
        //Debug.Log(DateTime.Now.ToString("yy M d "));    
        //Date date = new Date();
       // DateTime dtime = DateTime.Now;

        CallTimeType();
	}


    public void CallTimeType()
    {
        DateTime EpochTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        DateTime EpochTime1 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Local);
        DateTime dt = DateTime.Now;
        DateTime dt1 = DateTime.SpecifyKind(dt, DateTimeKind.Utc);

        //Debug.Log(EpochTime.ToLongTimeString() + "  " + EpochTime1.ToLongTimeString()  + "  " + dt.Kind);

        Debug.Log("local dt: " + dt.Kind + "  " + dt.ToLocalTime() + "  dt1: " + dt1.Kind + "  " + dt1.ToLocalTime() + "  " + dt.Kind);
        Debug.Log("utc dt: " + dt.Kind + "  " + dt.ToUniversalTime() + "  dt1: " + dt1.Kind + "  " + dt1.ToUniversalTime() + "  " + dt.Kind);
        Debug.Log(  dt.Ticks );
    
    }
}
