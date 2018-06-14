using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace IFP.Debug
{
    public class Controller : MonoBehaviour
    {

        private void OnEnable()
        {
            // start TCP/IP communication
            const int port = 6670;
            const string welcomeMessage = "Hello, IFG!";
            var cli = CLI.Bridge.TryInstall(port, welcomeMessage: welcomeMessage);

            cli.Executer = new CLI.Executer()
                .Bind(typeof(DLadar))
                .Bind(typeof(DWeapon));

            // bind class
            //cli.Executer = new CLI.Executer()
            //    .Bind(typeof(Binding1))
            //    .Bind(typeof(Binding2));
        }
    }

    public static class DWeapon
    {
        [CLI.Bind]
        public static CLI.Result AddWeapon()
        {
            return CLI.Result.Success("Add Weapon");
        }

        [CLI.Bind]
        public static CLI.Result Change(int i)
        {
            var info = string.Format("Last Weapon : {0}\n", WeaponCtrl.instance.CurrentWeaponType);

            switch (i)
            {
                case 1:
                    WeaponCtrl.instance.SwitchWeapon(WeaponCtrl.WeaponTypeEnum.weaponType01);
                    break;
                case 2:
                    WeaponCtrl.instance.SwitchWeapon(WeaponCtrl.WeaponTypeEnum.weaponType02);
                    break;
                case 3:
                    WeaponCtrl.instance.SwitchWeapon(WeaponCtrl.WeaponTypeEnum.weaponType03);
                    break;
                case 4:
                    WeaponCtrl.instance.SwitchWeapon(WeaponCtrl.WeaponTypeEnum.weaponType04);
                    break;
            }

            info = info + string.Format("Current Weapon: {0}\n", WeaponCtrl.instance.CurrentWeaponType);

            return CLI.Result.Success(info);
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