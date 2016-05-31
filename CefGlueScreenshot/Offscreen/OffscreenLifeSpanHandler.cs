using Xilium.CefGlue;

namespace CefGlueScreenshot.Offscreen {
	internal class OffscreenLifeSpanHandler : CefLifeSpanHandler {
		private readonly OffscreenBrowser offscreenBrowser;

		public OffscreenLifeSpanHandler(OffscreenBrowser offscreenBrowser) {
			this.offscreenBrowser = offscreenBrowser;
		}

		protected override void OnAfterCreated(CefBrowser browser) {
			base.OnAfterCreated(browser);
			offscreenBrowser.BrowserAfterCreated(browser);
		}

		protected override bool DoClose(CefBrowser browser) {
			base.DoClose(browser);
			return false;
		}
	}
}