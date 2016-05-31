using Xilium.CefGlue;

namespace CefGlueScreenshot.Offscreen {
	internal class OffscreenCefClient : CefClient {
		private readonly OffscreenLifeSpanHandler lifeSpanHandler;
		private readonly OffscreenRenderHandler renderHandler;
		private readonly OffscreenLoadHandler loadHandler;
		public event LoadingStateChangeDelegate LoadingStateChanged;

		public OffscreenCefClient(OffscreenBrowser offscreenBrowser) {
			lifeSpanHandler = new OffscreenLifeSpanHandler(offscreenBrowser);
			renderHandler = new OffscreenRenderHandler(offscreenBrowser);
			loadHandler = new OffscreenLoadHandler(offscreenBrowser);
			loadHandler.LoadingStateChanged += (browser, loading, back, forward) => LoadingStateChanged?.Invoke(browser, loading, back, forward);
		}

		protected override CefLifeSpanHandler GetLifeSpanHandler() {
			return lifeSpanHandler;
		}

		protected override CefRenderHandler GetRenderHandler() {
			return renderHandler;
		}

		protected override CefLoadHandler GetLoadHandler() {
			return loadHandler;
		}
	}
}