using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestDyFontTextMain : MonoBehaviour {

    public Text text;
    public Text text1;
    private char[] strary;
	// Use this for initialization
	void Start () {
	
        string str = "德萨；副科级；撒打飞机啊卡萨了解到发打飞机啊";
        strary = str.ToCharArray();
	    
	}

    private int count = 0;
	// Update is called once per frame
	void Update () {
        if (text == null)
            return;


        if(Input.GetKey(KeyCode.A))
        {
            text.text = @"今天要实现的功能是用lua给UGUI的一个按钮添加一个点击响应~~~~<br/><br/>因为我觉得在使用Lua与C#交互，委托是最重要的部分，这样可以很灵活的响应宿主的事件，其它的姿势都比较明确，也不用多说<br/><br/>首先是一个类&#20284;以前NGUI绑定事件的脚本，大部分来自momo研究院，我给加了一个OnPress的事件网易科技讯5月27日消息，乌镇围棋峰会最后一天，也是万众瞩目的柯洁AlphaGo最后一场，面对“围棋上帝”AlphaGo，柯洁使出全身解数仍无济于事最终投子认负，正应了柯洁在二十手时棋圣聂卫平预判的，“AlphaGo已赢，可以收子了。”本次人机大战最终以柯洁三连败告终。之前应柯洁要求本局要执白，开局AlphaGo执黑先下，第一招再次下在己方右下角。柯洁称这么要求的原因是AlphaGo拿黑棋胜率45%，拿白棋胜率55%。柯洁经过两次失败今天开局后一直想把局面往复杂方向引，这应该也是人类唯一能够想出来能战胜AlphaGo的可能。常昊九段在解说中称：“人类对相对容易量化的感觉好一点，但对中腹部难量化的感觉要差。去年与李世石对战的AlphaGo还有点人类的影子，现在的Alphago则完全走着自己的招法，人类很难预测。本次围棋峰会，人类方无论是柯洁的单挑还是九段联队的群殴，均没能拿下AlphaGo，一种深深的无力感弥漫在围棋界。聂卫平则表示：“跟阿老师比赛没意义，应该让它给我们所有职业棋手上课。”（白鑫）先通过Initialize方法加载总的清单文件，然后我们可以通过LoadPrefab方法来加载AB包，此时会把对这个AB包的请求放在m_LoadRequests中，然后在OnLoadAsset方法对该AB包的所有请求进行处理，通过GetLoadedAssetBundle方法看看内存中有没有这个AB包，如果有的话，再检查一下该AB包的依赖包是否也在内存中，如果都在，就把请求的包内资源加载出来，并回调方法。如果出现缺包情况，会通过OnLoadAssetBundle方法加载AB包及其依赖包(使用递归)。
至于卸载AB包，使用的是UnloadAssetBundle方法。这里还有说一下引用计数的问题，例如有A1A2A3三个包，A2A3依赖A1，然后我们加载A2A3。那么A1的引用计数为2，A2的引用计数为1，A3的引用计数为1，当使用UnloadAssetBundle卸载掉A2包时，A1的引用计数变为1，此时还会留在内存中，再卸载A3包，A1的引用计数变为0，A1包就自动被卸载了。包被加载一次，则引用计数加1，所以上面的A2A3包引用计数为1,；包被依赖一次，则引用计数加1，所以上面的A1包引用计数为2,；每次卸载时计数减1，减到0则从内存中去掉。

2.PanelManager：默认lua创建的panel都要在tag为GuiCamera的物体下，提供创建panel的方法斯蒂芬就三；飞；打飞机；打飞机啊而降温了人家问沃尔沃他；我二姐我请假而为情人节；积分平均分撒地方的批发平时对付暗示法盼复爱上耳机；的说法-245;l大批发价 皮阿道夫";


            text1.text = text.text;
        }
        else
        {
            text.text = strary[Random.Range(0, strary.Length - 1)].ToString();
            text1.text = text.text;

        }
	}
}
