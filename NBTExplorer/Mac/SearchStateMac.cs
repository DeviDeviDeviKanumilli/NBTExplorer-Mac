using System;
using AppKit;
using NBTExplorer.Model;
using System.Collections.Generic;

namespace NBTExplorer.Mac
{
	public class SearchStateMac : ISearchState
	{
		private NSWindow _sender;
		
		public SearchStateMac (NSWindow sender)
		{
			_sender = sender;
			TerminateOnDiscover = true;
			ProgressRate = 0.5f;
		}
		
		public Action<DataNode> DiscoverCallback { get; set; }
		public Action<DataNode> ProgressCallback { get; set; }
		public Action<DataNode> CollapseCallback { get; set; }
		public Action<DataNode> EndCallback { get; set; }
		
		#region ISearchState
		
		public DataNode RootNode { get; set; }
		public string SearchName { get; set; }
		public string SearchValue { get; set; }
		public bool TerminateOnDiscover { get; set; }
		public bool IsTerminated { get; set; }
		public float ProgressRate { get; set; }
		
		public IEnumerator<DataNode> State { get; set; }
		
		public void InvokeDiscoverCallback (DataNode node)
		{
			if (_sender != null && DiscoverCallback != null)
				_sender.BeginInvokeOnMainThread(delegate { DiscoverCallback(node); });
			//_sender.BeginInvokeOnMainThread(DiscoverCallback, new object[] { node });
		}
		
		public void InvokeProgressCallback (DataNode node)
		{
			if (_sender != null && ProgressCallback != null)
				_sender.BeginInvokeOnMainThread(delegate { ProgressCallback(node); });
			//_sender.BeginInvokeOnMainThread(ProgressCallback, new object[] { node });
		}
		
		public void InvokeCollapseCallback (DataNode node)
		{
			if (_sender != null && CollapseCallback != null)
				_sender.BeginInvokeOnMainThread(delegate { CollapseCallback(node); });
			//_sender.BeginInvokeOnMainThread(CollapseCallback, new object[] { node });
		}
		
		public void InvokeEndCallback (DataNode node)
		{
			if (_sender != null && EndCallback != null)
				_sender.BeginInvokeOnMainThread(delegate { EndCallback(node); });
			//_sender.BeginInvokeOnMainThread(EndCallback, new object[] { node });
		}

		public bool TestNode (DataNode node)
		{
			bool matchesName = SearchName == null;
			bool matchesValue = SearchValue == null;

			if (SearchName != null && node.NodeName != null)
				matchesName = node.NodeName.IndexOf(SearchName, StringComparison.OrdinalIgnoreCase) >= 0;
			if (SearchValue != null && node.NodeDisplay != null)
				matchesValue = node.NodeDisplay.IndexOf(SearchValue, StringComparison.OrdinalIgnoreCase) >= 0;

			return matchesName && matchesValue;
		}
		
		#endregion
	}
}
