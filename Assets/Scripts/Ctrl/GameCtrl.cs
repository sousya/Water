using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCtrl : MonoBehaviour, ICanSendEvent
{

    public CakeCtrl FirstCake, FirstCake1;
    public CakeCtrl SecondCake;
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

    // Update is called once per frame
    void LateUpdate()
    {

        if (Input.GetMouseButtonDown(0) && !control)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);


            if (hit.collider != null && hit.collider.GetComponent<CakeCtrl>() != null)
            {
                var cake = hit.collider.GetComponent<CakeCtrl>();

                if (!cake.isFinish)
                {
                    if (FirstCake == null)
                    {
                        if (cake.OnSelect())
                        {
                            FirstCake = cake;

                            if (LevelManager.Instance.levelId == 1)
                            {
                                var e = new TeachEvent()
                                {
                                    step = 2
                                };
                                this.SendEvent<TeachEvent>(e);
                            }
                        }
                        
                    }
                    else if (SecondCake == null)
                    {
                        SecondCake = cake;
                    }

                    if (FirstCake != null && SecondCake != null)
                    {
                        control = true;
                        if (FirstCake.MoveCake(SecondCake))
                        {
                            //Debug.Log("ÒÆ¶¯ " + FirstCake.gameObject.name + "->" + SecondCake.gameObject.name);
                            FirstCake.OnCancelSelect();
                            FirstCake.CheckConnect();
                            SecondCake.CheckConnect();
                            FirstCake = null;
                            SecondCake = null;
                            LevelManager.Instance.moveNum++;
                            this.SendEvent<MoveCakeEvent>();
                        }
                        else
                        {
                            control = false;
                            FirstCake.OnCancelSelect();
                            FirstCake = null;
                            SecondCake = null;
                        }
                    }
                }
               
               

            }
            else
            {
                //Debug.Log("Nothing");
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
