using System.Runtime.InteropServices;

namespace SeleniumV2.MouseMethod
{
    public class MiddleClick
    {
        // WinAPI'den SetCursorPos ve mouse_event fonksiyonlarını import ediyoruz
        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool SetCursorPos(int X, int Y);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, IntPtr dwExtraInfo);

        // Mouse event flag'leri
        private const uint MOUSEEVENTF_MIDDLEDOWN = 0x0020; // Orta tuş basımı
        private const uint MOUSEEVENTF_MIDDLEUP = 0x0040;   // Orta tuş bırakma

        // Ekran çözünürlüğünü almak için P/Invoke
        [DllImport("user32.dll")]
        private static extern IntPtr GetDC(IntPtr hWnd);

        [DllImport("gdi32.dll")]
        private static extern int GetDeviceCaps(IntPtr hdc, int nIndex);

        private const int HORZRES = 8; // Yatay çözünürlük
        private const int VERTRES = 10; // Dikey çözünürlük

        public void MiddleClickAndBottomAction()
        {

            // Ekranın merkez koordinatlarını al
            (int centerX, int centerY) = GetScreenCenter();

            // Fareyi ekranın ortasına taşı
            SetCursorPos(centerX, centerY);

            // Orta tuşa tıklama işlemini gerçekleştir
            mouse_event(MOUSEEVENTF_MIDDLEDOWN, 0, 0, 0, IntPtr.Zero); // Orta tuş bas
            mouse_event(MOUSEEVENTF_MIDDLEUP, 0, 0, 0, IntPtr.Zero);   // Orta tuş bırak

            // Ekranın merkez koordinatlarını al
            (int bottomX, int bottomY) = GetScreenBottomCenter();

            // Fareyi ekranın altına taşı
            SetCursorPos(bottomX, bottomY);
        }

        private static (int, int) GetScreenCenter()
        {
            IntPtr hdc = GetDC(IntPtr.Zero);
            int screenWidth = GetDeviceCaps(hdc, HORZRES);
            int screenHeight = GetDeviceCaps(hdc, VERTRES);

            // Merkez koordinatları hesapla
            int centerX = screenWidth / 2;
            int centerY = screenHeight / 2;

            return (centerX, centerY);
        }

        private static (int, int) GetScreenBottomCenter()
        {
            IntPtr hdc = GetDC(IntPtr.Zero);
            int screenWidth = GetDeviceCaps(hdc, HORZRES);
            int screenHeight = GetDeviceCaps(hdc, VERTRES);

            // Merkez koordinatları hesapla (orta-alt kısım)
            int centerX = screenWidth / 2;
            int bottomY = screenHeight;

            return (centerX, bottomY);
        }
    }
}
