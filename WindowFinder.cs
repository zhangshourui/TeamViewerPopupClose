using System;
using System.Linq;
using System.Text;

namespace TeamViewerPopupClose
{
    public static class WindowFinder
    {
        public static void Detect(TargetConfig config, string exeName, Func<System.Diagnostics.Process, IntPtr, IntPtr, bool> onSuccess)
        {
            if (NativeMethods.AnyPopup() == 0)
            {
                return;
            }
            //Imports.EnumWindows(new CallBack((hwnd,lPram)=> {
            //    return true;
            //}, 0);
            NativeMethods.EnumWindows((hwnd, lPram) =>
            {
                NativeMethods.GetWindowThreadProcessId(hwnd, out int pId);
                if (pId > 0)
                {
                    var proc = System.Diagnostics.Process.GetProcessById(pId);
                    if (proc != null)
                    {
                        try
                        {
                            var moduleName = proc.MainModule.ModuleName;

                            if (moduleName.Equals(exeName, StringComparison.OrdinalIgnoreCase))
                            {
                                var procWinTitle = proc.MainWindowTitle;

                                // Match main window
                                var matchCfg = config.Targets.Where(cfg =>
                                {
                                    if (string.IsNullOrEmpty(cfg.WindowTitle))
                                    {
                                        return true;
                                    }
                                    if (cfg.TitleContains)
                                    {
                                        return procWinTitle.IndexOf(cfg.WindowTitle, StringComparison.OrdinalIgnoreCase) >= 0;
                                    }
                                    else
                                    {
                                        return procWinTitle.Equals(cfg.WindowTitle, StringComparison.OrdinalIgnoreCase);
                                    }
                                }).FirstOrDefault();

                                // find child window (buttons & labels & links ... )
                                if (matchCfg != null)
                                {
                                    NativeMethods.EnumChildWindows(hwnd, (hChildWin, lparam) =>
                                    {
                                        var title = new StringBuilder(1000);
                                        if (NativeMethods.GetWindowText(hChildWin, title, 1000) != 0)
                                        {
                                            var winTitle = title.ToString();
                                            if (winTitle.Equals(matchCfg.ChildText, StringComparison.OrdinalIgnoreCase))
                                            {
                                                if (onSuccess != null)
                                                {
                                                    onSuccess.Invoke(proc, hwnd, hChildWin);
                                                }
                                                return false;
                                            }
                                        }

                                        return true;
                                    }, IntPtr.Zero);
                                }
                            }
                        }
                        catch (Exception)
                        {
                            //   Console.WriteLine(proc.ProcessName + ": " + ex.Message);

                            //  throw;
                        }
                    }
                }

                return true;
            }, IntPtr.Zero);
        }

      
    }


}