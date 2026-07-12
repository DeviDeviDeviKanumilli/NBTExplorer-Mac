using AppKit;
using Foundation;
using Substrate.Nbt;

namespace NBTExplorer.Mac
{
	/// <summary>
	/// Stores NBT clipboard data using the modern NSPasteboard data API.
	/// The old MonoMac implementation depended on the removed
	/// NSPasteboardReading/NSPasteboardWriting class wrappers.
	/// </summary>
	public sealed class NbtClipboardControllerMac : INbtClipboardController
	{
		private const string DataType = "jaquadro.nbtexplorer.nbt";
		private const string NameType = "jaquadro.nbtexplorer.nbt-name";

		public void CopyToClipboard (NbtClipboardData data)
		{
			NSPasteboard pasteboard = NSPasteboard.GeneralPasteboard;
			pasteboard.ClearContents();
			pasteboard.SetDataForType(NSData.FromArray(NbtClipboardData.SerializeNode(data.Node)), DataType);
			pasteboard.SetStringForType(data.Name ?? string.Empty, NameType);
		}

		public NbtClipboardData CopyFromClipboard ()
		{
			NSPasteboard pasteboard = NSPasteboard.GeneralPasteboard;
			NSData data = pasteboard.GetDataForType(DataType);
			if (data == null)
				return null;

			TagNode node = NbtClipboardData.DeserializeNode(data.ToArray());
			return node == null ? null : new NbtClipboardData(pasteboard.GetStringForType(NameType), node);
		}

		public bool ContainsData
		{
			get { return NSPasteboard.GeneralPasteboard.GetDataForType(DataType) != null; }
		}
	}
}
