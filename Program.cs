using System;
using System.Runtime.InteropServices;

namespace TeamViewerPopupClose
{
    internal class Program
    {
        private const int WM_CLOSE = 0x0010;

        private const string PROCESS_NAME = "TeamViewer.exe";
        private const int DETECT_INTERVAL = 5000;

        private static void Main(string[] args)
        {
            TargetConfig config = null;
            Console.Title = "TeamViewer弹框自动检测程序，请勿关闭。";
            Console.WriteLine("TeamViewer弹框自动检测程序。TeamViewer Popup Window Auto Detector.");
            Console.WriteLine("按 Ctrl + C 关闭。 Press Ctrl + C to quit.");

            #region Read configuration

            // Read configuration
            try
            {
                var configContent = System.IO.File.ReadAllText(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "tar.json");
                config = Newtonsoft.Json.JsonConvert.DeserializeObject<TargetConfig>(configContent);
                if (config == null)
                {
                    throw new Exception("找不到配置文件");
                }
                if (config.Targets == null || config.Targets.Length == 0)
                {
                    throw new Exception("配置文件中不包含目标窗口项。");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("读取配置文件失败：" + ex.ToString());
                return;
                // throw;
            }

            #endregion Read configuration

            #region Begin detect

            // Begin detect
            while (true)
            {
                WindowFinder.Detect(config, PROCESS_NAME, (proc, hParenWnd, hWnd) =>
                {
                    Console.WriteLine("已经找到目标窗口，发送关闭消息! Target located. Sending WM_CLOSE message. ");
                    NativeMethods.SendMessage(hParenWnd, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
                    return true;
                });

                System.Threading.Thread.Sleep(DETECT_INTERVAL);
            }

            #endregion Begin detect
        }
    }


}