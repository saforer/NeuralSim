using UnityEngine;
using System.Collections;

public class NeuralSim : MonoBehaviour {
    public static NeuralSim i;
    private Page _page;

	void Start () {
        i = this;

        FutileParams fparams = new FutileParams(true, true, false, false);
        fparams.AddResolutionLevel(768f, 1f, 1f, "");
        fparams.origin = Vector2.zero;

        Futile.instance.Init(fparams);
        Futile.atlasManager.LoadAtlas("Atlases/atlas");

        LoadPage(new TestPage());
    }
	
	void Update () {
        if (_page != null) _page.Update(Time.deltaTime);
	}


    void LoadPage(Page p)
    {
        if (_page != null) _page.RemoveFromContainer();

        _page = p;
        Futile.stage.AddChild(_page);
    }
}
