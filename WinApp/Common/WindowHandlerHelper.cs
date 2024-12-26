using System;
using Microsoft.UI;
using WinRT.Interop;
using Windows.Graphics;
using Microsoft.UI.Xaml;
using Microsoft.UI.Windowing;

namespace AcceptAFKServer.Common;
public static class WindowHandlerHelper
{
    public static AppWindow GetAppWindowForCurrentWindow(Window window)
    {
        IntPtr hWnd = WindowNative.GetWindowHandle(window);
        WindowId wndId = Win32Interop.GetWindowIdFromWindow(hWnd);
        return AppWindow.GetFromWindowId(wndId);
    }

    public static void ResizeAndCenterWindow(Window window, int width, int height)
    {
        //Get WindowHandler for app navigation
        IntPtr hWnd = WindowNative.GetWindowHandle(window);
        WindowId wndId = Win32Interop.GetWindowIdFromWindow(hWnd);

        AppWindow appWindow = AppWindow.GetFromWindowId(wndId);
        appWindow.Resize(new SizeInt32(width, height));

        CenterWindow(appWindow, width, height);
    }

    public static void CenterWindow(AppWindow appWindow, int width, int height)
    {
        DisplayArea displayArea = DisplayArea.GetFromWindowId(appWindow.Id, DisplayAreaFallback.Nearest);
        RectInt32 displayAreaWorkArea = displayArea.WorkArea;

        int x = (displayAreaWorkArea.Width - width) / 2;
        int y = (displayAreaWorkArea.Height - height) / 2;

        appWindow.MoveAndResize(new RectInt32(x, y, width, height));
    }
}
