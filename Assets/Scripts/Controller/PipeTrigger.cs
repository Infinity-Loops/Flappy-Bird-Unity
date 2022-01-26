using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DotGears.Flappy
{
    public class PipeTrigger : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision != null)
            {
                this.gameObject.SetActive(false);
                GameActivity.Instance.Score++;
                GameActivity.Instance.UpdateScore();
                Bird.Instance.PlaySound(Bird.Instance.Point);
            }
        }
    }
}
