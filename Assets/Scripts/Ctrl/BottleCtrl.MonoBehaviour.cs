using UnityEngine;

public partial class BottleCtrl : MonoBehaviour
{
   
    // Start is called before the first frame update
    void Start()
    {
        bottle.onClick.AddListener(OnSelected);
    }
    
    private void LateUpdate()
    {
        fillWaterGoAnim.transform.localRotation = Quaternion.Inverse(fillWaterGoAnim.transform.parent.rotation);
    }
}
