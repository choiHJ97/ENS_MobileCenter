using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using ENS_MobileCenter.Droid;
using ENS_MobileCenter.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(StatusBar))]

namespace ENS_MobileCenter.Droid
{
    class StatusBar : Views.IStatusBar
    {
        private WindowManagerFlags _originalFlags;
        void IStatusBar.HideStatusBar()
        {
            
            var activity = (Activity)Forms.Context;
            var attributes = activity.Window.Attributes;
            _originalFlags = attributes.Flags;
            attributes.Flags = Android.Views.WindowManagerFlags.Fullscreen;
            activity.Window.Attributes = attributes;
        }

        void IStatusBar.ShowStatusBar()
        {
            var activity = (Activity)Forms.Context;
            var attributes = activity.Window.Attributes;
            attributes.Flags = _originalFlags;
            activity.Window.Attributes = attributes;
        }
    }
}