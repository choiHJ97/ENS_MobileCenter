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
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using static Java.Util.ResourceBundle;

[assembly: ExportRenderer(typeof(BorderlessEntry), typeof(BorderlessEntryRenderer))]

namespace ENS_MobileCenter.Droid
{
    public class BorderlessEntryRenderer : EntryRenderer
    { 
    public BorderlessEntryRenderer(Context context) : base(context)
    {
    }

    protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
    {
        base.OnElementChanged(e);

        //Configure native control (TextBox)
        if (Control != null)
        {
            Control.Background = null;
        }

        // Configure Entry properties
        if (e.NewElement != null)
        {

        }
    }
}
}