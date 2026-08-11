using System;
using System.Windows.Forms;
using _4RTools.Utils;
using System.Threading;
using System.Runtime.InteropServices;

namespace _4RTools.Model
{
    public class SelfSkillTarget
    {
        private const int WM_LBUTTONDOWN = 0x0201;
        private const int WM_LBUTTONUP = 0x0202;

        [DllImport("user32.dll")]
        private static extern bool GetClientRect(IntPtr hWnd, out RECT lpRect);

        [StructLayout(LayoutKind.Sequential)]
        private struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        public static void CastOnSelf(Keys skillKey)
        {
            IntPtr windowHandle = ClientSingleton.GetClient().process.MainWindowHandle;

            if (skillKey != Keys.None)
            {
                // 1. Pressiona e solta a tecla da skill
                Interop.PostMessage(windowHandle, Constants.WM_KEYDOWN_MSG_ID, skillKey, 0);
                Interop.PostMessage(windowHandle, Constants.WM_KEYUP_MSG_ID, skillKey, 0);

                Thread.Sleep(60); 

                // 2. Descobre o tamanho atual da janela do jogo dinamicamente
                RECT rect;
                GetClientRect(windowHandle, out rect);
                
                // Calcula o centro exato da janela atual (Largura / 2 e Altura / 2)
                int centerX = (rect.Right - rect.Left) / 2;
                int centerY = (rect.Bottom - rect.Top) / 2;
                
                int lParam = (centerY << 16) | (centerX & 0xFFFF);

                // 3. Simula o clique do mouse no centro real da tela
                Interop.PostMessage(windowHandle, WM_LBUTTONDOWN, 1, lParam);
                Interop.PostMessage(windowHandle, WM_LBUTTONUP, 0, lParam);
            }
        }
    }
}
