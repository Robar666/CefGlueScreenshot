using System;
using System.Threading.Tasks;
using CefGlueScreenshot.Offscreen;
using Xilium.CefGlue;

namespace CefGlueScreenshot {
	class Program {
		static int Main(string[] args) {
			const string url = "https://upload.wikimedia.org/wikipedia/commons/f/fd/Ghostscript_Tiger.svg";
			return TakeScreenshotAsync(url, 530, 530, args).GetAwaiter().GetResult();
		}

		public static async Task<int> TakeScreenshotAsync(string url, int width, int height, string[] args) {
			try {
				var mainArgs = new CefMainArgs(args);

				// create app
				var app = new OffscreenApp();
				var exitCode = CefRuntime.ExecuteProcess(mainArgs, app, IntPtr.Zero);
				if (exitCode != -1)
					return exitCode;

				// init CEF
				var settings = new CefSettings {
					SingleProcess = false,
					MultiThreadedMessageLoop = true,
					LogSeverity = CefLogSeverity.Error,
					LogFile = "CefGlue.log"
				};
				CefRuntime.Initialize(mainArgs, settings, app, IntPtr.Zero);

				// create browser
				var browser = new OffscreenBrowser(url, width, height);

				// wait for the site to be loaded
				await WaitForBrowserLoadingAsync(browser).ConfigureAwait(false);
				await Task.Delay(1000).ConfigureAwait(false);
				browser.WindowBitmap.Save("output.png");

				// dispose everything
				var host = browser.GetBrowser().GetHost();
				host.CloseBrowser();
				host.Dispose();
				CefRuntime.Shutdown();
				return 0;
			} catch (Exception exc) {
				Console.WriteLine(exc.Message);
			}
			return -1;
		}

		private static Task WaitForBrowserLoadingAsync(OffscreenBrowser browser) {
			var tcs = new TaskCompletionSource<bool>();
			LoadingStateChangeDelegate handler = null;
			handler = (cefBrowser, loading, back, forward) => {
				if (loading) return;
				browser.LoadingStateChanged -= handler;
				tcs.TrySetResult(true);
			};
			browser.LoadingStateChanged += handler;
			return tcs.Task;
		}
	}
}