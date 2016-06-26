using UnityEngine;
using System.Collections.Generic;

namespace gkh
{
    public class StarField : MonoBehaviour 
    {
        public GameObject starPrefab;

        List<Star> stars = new List<Star>();


        #region MonoBehaviour
        void Start()
        {
            GameObject g;
            for (var i = 0; i < 200; i++)
            {
                g = (GameObject)GameObject.Instantiate(starPrefab);
                g.transform.position = new Vector3(
                    Random.Range(Screenie.ScreenLeft, Screenie.ScreenRight), 
                    Random.Range(Screenie.ScreenTop, Screenie.ScreenBottom), 0);

                Star s = g.GetComponent<Star>();
                s.SetSize(StarSize.Med);
                stars.Add(s);
            }

            for (var i = 0; i < 275; i++)
            {
                g = (GameObject)GameObject.Instantiate(starPrefab);
                g.transform.position = new Vector3(
                    Random.Range(Screenie.ScreenLeft, Screenie.ScreenRight), 
                    Random.Range(Screenie.ScreenTop, Screenie.ScreenBottom), 0);

                Star s = g.GetComponent<Star>();
                s.SetSize(StarSize.Sml);
                stars.Add(s);
            }
        }

        void Update()
        {
            foreach (var s in stars)
            {
                s.Move();
            }
        }
        #endregion
    }
}