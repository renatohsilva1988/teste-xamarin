﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Android.Graphics;
using MarvelApp.Controls;
using MarvelApp.Droid.Renders;

[assembly: ExportRenderer(typeof(ImageCircle), typeof(ImageCircleRenderer))]
namespace MarvelApp.Droid.Renders
{
    public class ImageCircleRenderer : ImageRenderer
    {
        public ImageCircleRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementPropertyChanged(object sender,
            System.ComponentModel.PropertyChangedEventArgs e)
        {

            base.OnElementPropertyChanged(sender, e);
            if (e.PropertyName == Image.IsLoadingProperty.PropertyName && !this.Element.IsLoading
                && this.Control.Drawable != null)
            {

                using (var sourceBitmap = Bitmap.CreateBitmap(this.Control.Drawable.IntrinsicWidth,
                    this.Control.Drawable.IntrinsicHeight, Bitmap.Config.Argb8888))
                {
                    var canvas = new Canvas(sourceBitmap);
                    this.Control.Drawable.SetBounds(0, 0, canvas.Width, canvas.Height);
                    this.Control.Drawable.Draw(canvas);
                    this.Control.SetImageBitmap(GetRoundedShape(sourceBitmap));
                }
            }
        }

        public Bitmap GetRoundedShape(Bitmap scaleBitmapImage)
        {
            int targetWidth = scaleBitmapImage.Width;
            int targetHeight = scaleBitmapImage.Width;
            Bitmap targetBitmap = Bitmap.CreateBitmap(targetWidth,
                targetHeight, Bitmap.Config.Argb8888);

            Canvas canvas = new Canvas(targetBitmap);
            Path path = new Path();
            path.AddCircle(((float)targetWidth - 1) / 2,
                ((float)targetHeight - 1) / 2,
                (Math.Min(((float)targetWidth),
                    ((float)targetHeight)) / 2),
                Path.Direction.Ccw);

            canvas.ClipPath(path);
            Bitmap sourceBitmap = scaleBitmapImage;
            canvas.DrawBitmap(sourceBitmap,
                new Rect(0, 0, sourceBitmap.Width,
                    sourceBitmap.Height),
                new Rect(0, 0, targetWidth, targetHeight), null);
            return targetBitmap;
        }

    }
}