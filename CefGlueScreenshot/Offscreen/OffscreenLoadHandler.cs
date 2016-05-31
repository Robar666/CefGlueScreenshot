using Xilium.CefGlue;

namespace CefGlueScreenshot.Offscreen {
	delegate void LoadingStateChangeDelegate(CefBrowser browser, bool isLoading, bool canGoBack, bool canGoForward);
	internal class OffscreenLoadHandler : CefLoadHandler {
		private readonly OffscreenBrowser offscreenBrowser;
		public event LoadingStateChangeDelegate LoadingStateChanged;

		public OffscreenLoadHandler(OffscreenBrowser offscreenBrowser) {
			this.offscreenBrowser = offscreenBrowser;
		}
		
		protected override void OnLoadingStateChange(CefBrowser browser, bool isLoading, bool canGoBack, bool canGoForward) {
			LoadingStateChanged?.Invoke(browser, isLoading, canGoBack, canGoForward);
			base.OnLoadingStateChange(browser, isLoading, canGoBack, canGoForward);
		}
	}
}