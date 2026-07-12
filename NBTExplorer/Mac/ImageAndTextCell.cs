using System;
using AppKit;
using CoreGraphics;
using Foundation;
using ObjCRuntime;

namespace NBTExplorer.Mac
{
	[Register("ImageAndTextCell")]
	public sealed class ImageAndTextCell : NSTextFieldCell
	{
		private NSImage _image;

		public ImageAndTextCell ()
		{
			Initialize();
		}

		public ImageAndTextCell (IntPtr handle) : base(handle)
		{
			Initialize();
		}

		[Export("initWithCoder:")]
		public ImageAndTextCell (NSCoder coder) : base(coder)
		{
			Initialize();
		}

		private void Initialize ()
		{
			LineBreakMode = NSLineBreakMode.TruncatingTail;
			Selectable = true;
		}

		public new NSImage Image
		{
			get { return _image; }
			set { _image = value; }
		}

		public override CGRect ImageRectForBounds (CGRect rect)
		{
			if (_image == null)
				return CGRect.Empty;

			CGPoint origin = new CGPoint(rect.X + 3, rect.Y + Math.Ceiling((rect.Height - _image.Size.Height) / 2));
			return new CGRect(origin, _image.Size);
		}

		public override CGRect TitleRectForBounds (CGRect rect)
		{
			if (_image == null)
				return base.TitleRectForBounds(rect);

			return new CGRect(
				rect.X + 3 + _image.Size.Width,
				rect.Y,
				rect.Width - 3 - _image.Size.Width,
				rect.Height);
		}

		public override void EditWithFrame (CGRect rect, NSView view, NSText editor, NSObject delegateObject, NSEvent theEvent)
		{
			CGRect imageFrame, textFrame;
			rect.Divide(3 + (_image == null ? 0 : _image.Size.Width), CGRectEdge.MinXEdge, out imageFrame, out textFrame);
			base.EditWithFrame(textFrame, view, editor, delegateObject, theEvent);
		}

		public override void SelectWithFrame (CGRect rect, NSView view, NSText editor, NSObject delegateObject, IntPtr selStart, IntPtr selLength)
		{
			CGRect imageFrame, textFrame;
			rect.Divide(3 + (_image == null ? 0 : _image.Size.Width), CGRectEdge.MinXEdge, out imageFrame, out textFrame);
			base.SelectWithFrame(textFrame, view, editor, delegateObject, selStart, selLength);
		}

		public override void DrawWithFrame (CGRect cellFrame, NSView view)
		{
			if (_image != null) {
				CGRect imageFrame, textFrame;
				cellFrame.Divide(3 + _image.Size.Width, CGRectEdge.MinXEdge, out imageFrame, out textFrame);
				if (DrawsBackground) {
					BackgroundColor.Set();
					NSGraphics.RectFill(imageFrame);
				}

				imageFrame.X += 3;
				imageFrame.Size = _image.Size;
				imageFrame.Y += (nfloat)Math.Ceiling((textFrame.Height - imageFrame.Height) / 2);
				_image.Draw(imageFrame, new CGRect(CGPoint.Empty, _image.Size), NSCompositingOperation.SourceOver, 1.0f, true, null);
				cellFrame = textFrame;
			}

			base.DrawWithFrame(cellFrame, view);
		}

		public override CGSize CellSize
		{
			get {
				CGSize size = base.CellSize;
				return new CGSize(size.Width + 3 + (_image == null ? 0 : _image.Size.Width), size.Height);
			}
		}

		public override NSCellHit HitTest (NSEvent theEvent, CGRect rect, NSView view)
		{
			CGPoint point = view.ConvertPointFromView(theEvent.LocationInWindow, null);
			if (_image != null) {
				CGRect imageFrame, textFrame;
				rect.Divide(3 + _image.Size.Width, CGRectEdge.MinXEdge, out imageFrame, out textFrame);
				imageFrame.X += 3;
				imageFrame.Size = _image.Size;
				if (view.IsMouseInRect(point, imageFrame))
					return NSCellHit.ContentArea;
				rect = textFrame;
			}

			return base.HitTest(theEvent, rect, view);
		}
	}
}
