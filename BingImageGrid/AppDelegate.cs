using System;
using System.Collections.Generic;
using System.Linq;

using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace BingImageGrid
{
    [Register ("AppDelegate")]
    public partial class AppDelegate : UIApplicationDelegate
    {
        UIWindow window;
        BingImageGridViewController viewController;
        UICollectionViewFlowLayout flowLayout;

        public override bool FinishedLaunching (UIApplication app, NSDictionary options)
        {
            window = new UIWindow (UIScreen.MainScreen.Bounds);

            flowLayout = new UICollectionViewFlowLayout ();
            viewController = new BingImageGridViewController (flowLayout);

            window.RootViewController = viewController;
            window.MakeKeyAndVisible ();
            
            return true;
        }
    }
}

