using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseSignal : MonoBehaviour
{
    public GameObject indicator;
    private List<OpenMenu> menus = new List<OpenMenu>();
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(HitMenus());
    }

    // Update is called once per frame
    void Update()
    {
        var mouspos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(mouspos.x, mouspos.y, 0);
    }

    IEnumerator HitMenus()
    {
        while (true)
        {
            var hit = Physics2D.OverlapCircleAll(transform.position, 10);
            for (int i = 0; i < hit.Length; i++)
            {
                if (hit[i].GetComponent<OpenMenu>() != null && hit[i].GetComponent<OpenMenu>().tiedIndicated == null && (hit[i].GetComponent<PatrolID>() == null || !hit[i].GetComponent<PatrolID>().taken))
                {
                    menus.Add(hit[i].GetComponent<OpenMenu>());
                    var x = 0;
                    var y = hit[i].bounds.size.y / 2 * 1.5f;
                    var scalx = hit[i].bounds.size.x / 2f;
                    scalx = Mathf.Clamp(scalx, 0, 10);
                    //y = Mathf.Clamp(y, hit[i].bounds.size.y, hit[i].bounds.size.y + 1);
                    var createdInd = Instantiate(indicator, hit[i].transform.position + new Vector3(x, y), Quaternion.identity);
                    createdInd.transform.localScale = new Vector3(scalx, scalx);
                    hit[i].GetComponent<OpenMenu>().tiedIndicated = createdInd;
                    createdInd.GetComponent<StayOnObject>().stayOn = hit[i].transform;

                    createdInd.GetComponent<StayOnObject>().offset = new Vector3(x, y);

                }
            }
            for (int i = 0; i < menus.Count; i++)
            {
                if (Vector2.Distance(menus[i].transform.position, transform.position) > 10 || (menus[i].gameObject.GetComponent<PatrolID>() != null && menus[i].gameObject.GetComponent<PatrolID>().taken))
                {
                    Destroy(menus[i].GetComponent<OpenMenu>().tiedIndicated);
                    menus.RemoveAt(i);
                }
            }

            yield return new WaitForSeconds(.1f);
        }

        
    }
}
