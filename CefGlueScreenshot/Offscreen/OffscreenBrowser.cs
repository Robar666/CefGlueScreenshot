using System;
using System.Drawing;
using Xilium.CefGlue;

namespace CefGlueScreenshot.Offscreen {
	internal class OffscreenBrowser {
		public readonly int Width;
		public readonly int Height;
		private CefBrowser browser;
		public Bitmap WindowBitmap;
		public event LoadingStateChangeDelegate LoadingStateChanged;

		public OffscreenBrowser(string url, int width, int height) {
			Width = width;
			Height = height;
			var parentHandle = IntPtr.Zero;

			var windowInfo = CefWindowInfo.Create();

			windowInfo.SetAsChild(parentHandle, new CefRectangle(0, 0, width, height));
			windowInfo.SetAsWindowless(parentHandle, true);

			var client = new OffscreenCefClient(this);
			var settings = new CefBrowserSettings {
				BackgroundColor = new CefColor(0, 0, 0, 0)
			};

			WindowBitmap = new Bitmap(width, height);

			CefBrowserHost.CreateBrowser(windowInfo, client, settings, url);
			client.LoadingStateChanged += (browser, loading, back, forward) => LoadingStateChanged?.Invoke(browser, loading, back, forward);
		}

		internal void BrowserAfterCreated(CefBrowser browser) {
			this.browser = browser;
		}

		public CefBrowser GetBrowser() {
			return browser;
		}
	}
}
