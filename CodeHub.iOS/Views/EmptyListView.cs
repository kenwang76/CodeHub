﻿using UIKit;
using CoreGraphics;

namespace CodeHub.iOS.Views
{
    public class EmptyListView : UIView
    {
        public static UIColor DefaultColor = UIColor.Black;

        public UIImageView ImageView { get; private set; }

        public UILabel Title { get; private set; }

        public EmptyListView(UIImage image, string emptyText)
            : base(new CGRect(0, 0, 320f, 480f * 2f))
        {
            AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;

            ImageView = new UIImageView();
            Title = new UILabel();

            ImageView.Frame = new CGRect(0, 0, 64f, 64f);
            ImageView.TintColor = DefaultColor;
            ImageView.ContentMode = UIViewContentMode.ScaleAspectFit;
            ImageView.Center = new CGPoint(Frame.Width / 2f, (Frame.Height / 4f) - (ImageView.Frame.Height / 2f));
            ImageView.AutoresizingMask = UIViewAutoresizing.FlexibleLeftMargin | UIViewAutoresizing.FlexibleRightMargin | UIViewAutoresizing.FlexibleBottomMargin;
            ImageView.Image = image;
            Add(ImageView);

            Title.Frame = new CGRect(0, 0, 256f, 20f);
            Title.Center = new CGPoint(Frame.Width / 2f, ImageView.Frame.Bottom + 30f);
            Title.AutoresizingMask = UIViewAutoresizing.FlexibleLeftMargin | UIViewAutoresizing.FlexibleRightMargin | 
                                     UIViewAutoresizing.FlexibleBottomMargin | UIViewAutoresizing.FlexibleWidth;
            Title.Text = emptyText;
            Title.TextAlignment = UITextAlignment.Center;
            Title.Font = UIFont.PreferredHeadline;
            Title.TextColor = DefaultColor;
            Add(Title);

            BackgroundColor = UIColor.White;
        }
    }
}

