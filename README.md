# CefGlueScreenshot
Take screenshot from any webpage with the CEF wrapper CefGlue.

## Problem

I wanted to rasterize SVGs to PNGs on my Windows Azure Worker Role. 

MagickNet (https://magick.codeplex.com/ - .NET port of ImageMagick) works fair enough, but not every SVG is rendered perfectly. Especially external fonts and displacement maps are causing problems.

Solution: Open a headless webbrowser and take a screenshot.

## Possible frameworks

There are several possible solutions:
* PhantomJS wrapper for .NET (C#) - http://www.nrecosite.com/phantomjs_wrapper_net.aspx 
* Awesomium - http://www.awesomium.com/ 
* CEF - https://bitbucket.org/chromiumembedded/cef
    * CEFSharp - http://cefsharp.github.io/
    * Xilium.CefGlue - http://xilium.bitbucket.org/cefglue/
    * ChromiumFX - https://bitbucket.org/chromiumfx/chromiumfx

Requirements for my project
* 64bit environment
* based on Chromium

So only CEF seems to fit.

## Problem with CEFSharp

CEFSharp looked good at the beginning (NuGet packages, nice community https://gitter.im/cefsharp/CefSharp ), but it didn't worked in my Azure Worker Role. There where some problems with calls between AppDomains.

* https://github.com/cefsharp/CefSharp/issues/351
* https://github.com/cefsharp/CefSharp/issues/1488
* https://github.com/cefsharp/CefSharp/issues/1127

One contributer (https://github.com/arsher) published a pull request (https://github.com/cefsharp/CefSharp/pull/1556 ) which should fix this problem. But I was never able to build his solution.

## Solution

**CefGlue.**

In contrast to CEFSharp CefGlue is uses P/Invoke calls, which aren't causing any troubles - yet.
Basically I updated the solution posted here (http://joelverhagen.com/blog/2013/12/headless-chromium-in-c-with-cefglue/) and used the packages from MyGet (http://myget.org/gallery/dazaraev). Unfortunately CefGlue has no official NuGet packages yet.

Chromium version: Chrome 51.0.2704.63 | .NET version: 4.5.2 | CEF Version: 3.2704.1409 | Date: 1.6.2016

## Build info
Since the CEF related binaries are quite big, I have excluded them from the repo. Visual Studio will probably show you a warning that several files are missing. To get the missing files you have to download the CEF related packages from [MyGet](http://myget.org/gallery/dazaraev) (add sourcs for the third-party NuGet Server: https://docs.nuget.org/create/hosting-your-own-nuget-feeds ). The missing files can then be found in the directory `.\packages\ChromiumEmbedded.Redistributable-win-x64.51.1.1\build`. Copy them into the solution directory, select `x64` instead of `AnyCPU` and hit start.

## Further reading
* Difference between CEFSharp and CefGlue -  http://stackoverflow.com/questions/12224798/any-reason-to-prefer-cefsharp-over-cefglue-or-vice-versa
* Discussion about the NuGet packages in CefGlue - https://bitbucket.org/xilium/xilium.cefglue/issues/61/make-nuget-packages
