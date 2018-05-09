using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace IF.Debug
{
    public class Controller : MonoBehaviour
    {

        private void OnEnable()
        {
            // start TCP/IP communication
            const int port = 6670;
            const string welcomeMessage = "Hello, IFG!";
            var cli = CLI.Bridge.TryInstall(port, welcomeMessage: welcomeMessage);

            cli.Executer = new CLI.Executer().Bind(typeof(DLadar));

            // bind class
            //cli.Executer = new CLI.Executer()
            //    .Bind(typeof(Binding1))
            //    .Bind(typeof(Binding2));
        }
    }

    public static class DItem
    {
        [CLI.Bind]
        public static CLI.Result AddItem()
        {
            var itemWin = GameObject.Find("Item_Window").GetComponent<ItemWindow>();


            return CLI.Result.Success("Add Item");
        }
    }

    public static class DLadar
    {
        static Ladar ld;

        private static Ladar GetLadar
        {
            get
            {
                if (ld == null)
                {
                    ld = GameObject.Find("LdarCircle").GetComponent<Ladar>();
                }
                return ld;
            }
        }

        [CLI.Bind]
        public static CLI.Result Distance(string i)
        {
            var distance = float.Parse(i, CultureInfo.InvariantCulture.NumberFormat);
            GetLadar.distance = distance;
            return CLI.Result.Success("Ladar Distance Set: " + i);
        }

        [CLI.Bind]
        public static CLI.Result DotsCount()
        {
            return CLI.Result.Success("Dots Count: " + GetLadar.dotsCount);
        }
    }
}