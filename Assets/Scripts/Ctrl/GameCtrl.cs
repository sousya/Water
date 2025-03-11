using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCtrl : MonoBehaviour, ICanSendEvent
{

    public BottleCtrl FirstBottle, FirstCake1;
    public BottleCtrl SecondBottle;
    public static GameCtrl Instance;
    [SerializeField] List<Sprite> sprites;
    public bool control = false;
    public IArchitecture GetArchitecture()
    {
        return GameMainArc.Interface;
    }
    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    public void OnSelect(BottleCtrl bottle)
    {
        if(!control)
        {

            if (FirstBottle == null)
            {
                if (bottle.OnSelect(true))
                {
                    FirstBottle = bottle;
                }

            }
            else if (SecondBottle == null)
            {

                if (bottle != FirstBottle && bottle.OnSelect(false))
                {
                    SecondBottle = bottle;
                }
                else
                {
                    FirstBottle.OnCancelSelect();
                    FirstBottle = null;
                }
            }

            if (FirstBottle != null && SecondBottle != null)
            {
                control = true;
                if (FirstBottle.CheckMoveOut() && SecondBottle.CheckMoveIn(FirstBottle.GetMoveOutTop()))
                {
                    //Debug.Log("移动 " + FirstCake.gameObject.name + "->" + SecondCake.gameObject.name);
                    LevelManager.Instance.RecordLast();

                    FirstBottle.MoveTo(SecondBottle);
                    FirstBottle = null;
                    SecondBottle = null;
                    LevelManager.Instance.AddMoveNum();
                    this.SendEvent<MoveCakeEvent>();
                }
                else
                {
                    control = false;
                    FirstBottle.OnCancelSelect();
                    FirstBottle = null;
                    SecondBottle = null;
                }
            }
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {

        if (Input.GetMouseButtonDown(0) && !control)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);


            if (hit.collider != null && hit.collider.GetComponent<BottleCtrl>() != null)
            {
            }
        }


    }



    IEnumerator LerpMove(Vector3 vector3)
    {
        var t = 0f;
        var start = FirstCake1.transform.position;
        var target = vector3;

        while (t < 1)
        {
            t += Time.deltaTime*20;

            if (t > 1) t = 1;

            FirstCake1.transform.position = Vector3.Lerp(start, target, t);

            yield return null;
        }


    }

}
