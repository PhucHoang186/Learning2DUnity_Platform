using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Fall : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag =="Player")
        {
            PermanentUI.perma.Reset();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
