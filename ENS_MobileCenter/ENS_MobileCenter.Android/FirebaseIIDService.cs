using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
/*using Firebase.Iid;
using Android.Util;*/

namespace ENS_MobileCenter.Droid
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.INSTANCE_ID_EVENT" })]
    public class FirebaseIIDService /*: FirebaseInstanceIdService*/
    {
        /*//Registration ID가 갱신될 때만 호출됩니다. (한번 호출되면 갱신되기 전까지 호출되지 않음)
        public override void OnTokenRefresh()
        {
            string refreshedToken = FirebaseInstanceId.Instance.Token;
            Log.Debug("FirebaseIIDService ", "Refreshed token: " + refreshedToken);

            SendRegistrationIdToServer(refreshedToken);
        }


        //Registration ID를 앱서버로 전송하는 로직을 구현해주세요.
        private void SendRegistrationIdToServer(string refreshedToken)
        {
        }*/
    }
}
