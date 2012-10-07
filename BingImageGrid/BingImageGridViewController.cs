using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Collections.Generic;
using System.Threading;
using MonoTouch.Dialog.Utilities;

namespace BingImageGrid
{
    public class BingImageGridViewController : UICollectionViewController
    {
        static NSString cellId = new NSString ("ImageCell");
        List<string> imageUrls;
        Bing bing;

        public BingImageGridViewController (UICollectionViewLayout layout) : base (layout)
        {
            imageUrls = new List<string> ();

            bing = new Bing ((results) => {
                InvokeOnMainThread (delegate {   
                    imageUrls = results;
                    CollectionView.ReloadData ();
                });
            });

            bing.ImageSearch ();

            CollectionView.ContentInset = new UIEdgeInsets (10, 10, 10, 10);
        }
            
        public override void ViewDidLoad ()
        {
            base.ViewDidLoad ();
                
            CollectionView.RegisterClassForCell (typeof(ImageCell), cellId);
        }
            
        public override int GetItemsCount (UICollectionView collectionView, int section)
        {
            return imageUrls.Count;
        }
            
        public override UICollectionViewCell GetCell (UICollectionView collectionView, MonoTouch.Foundation.NSIndexPath indexPath)
        {
            var imageCell = (ImageCell)collectionView.DequeueReusableCell (cellId, indexPath);
              
            string imageUrl = imageUrls [indexPath.Row].Replace ("\"", "");

            imageCell.UpdateCell (new Uri (imageUrl));
                
            return imageCell;
        }
         
    }
        
    public class ImageCell : UICollectionViewCell, IImageUpdated
    {
        UIImageView imageView;
            
        [Export ("initWithFrame:")]
        public ImageCell (System.Drawing.RectangleF frame) : base (frame)
        {
            imageView = new UIImageView (new RectangleF (0, 0, 50, 50)); 
            imageView.ContentMode = UIViewContentMode.ScaleAspectFit;
            ContentView.AddSubview (imageView);
        }

        public void UpdateCell (Uri uri)
        {
            imageView.Image = ImageLoader.DefaultRequestImage (uri, this);
        }

        public void UpdatedImage (Uri uri)
        {
            imageView.Image = ImageLoader.DefaultRequestImage (uri, this);
        }
    }
    
}

