using System.Linq;
using System.Xml.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using IFP;

public class DisplayScript : MonoBehaviour {
    
    public int sceneId = 0;
    public string nextSceneName = "none";

    [SerializeField]
    private Text nameText;
    [SerializeField]
    private Text scriptText;
    
    private List<XElement> xElements;
    private IEnumerator<XElement> it;

    private Dictionary<string, string> characters;

    private void Awake()
    {
        sceneId = TempStageManagement.instance.talkSceneMeta.scriptNumber;
        nextSceneName = TempStageManagement.instance.talkSceneMeta.nextSceneName;

        DoSet();

        DoDisplay();
    }

    public void DoSet()
    {
        characters = new Dictionary<string, string>();

        var tAsset = Resources.Load(string.Format("talking_script{0}", sceneId)) as TextAsset;
        var xDoc = XElement.Parse(tAsset.text);

        xDoc.Element("characters").Elements("name").ToList().ForEach(e =>
        {
            characters.Add(e.Attribute("cn").Value, e.Value);
        });

        xElements = xDoc.Element("script").Elements("text").ToList();
        it = xElements.GetEnumerator();
    }

    public void DoDisplay()
    {
        it.MoveNext();

        if (it.Current != null)
        {
            var cName = characters[it.Current.Attribute("cn").Value];
            var cText = it.Current.Value;

            nameText.text = cName;
            scriptText.text = cText;
        }
        else
        {
            MoveNextScene();
        }
    }

    public void MoveNextScene()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}
