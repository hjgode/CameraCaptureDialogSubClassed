using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace SHELLAPI
{
    static class SIP_API{
        const uint SHFS_SHOWTASKBAR = 0x0001;
        const uint SHFS_HIDETASKBAR = 0x0002;
        const uint SHFS_SHOWSIPBUTTON = 0x0004;
        const uint SHFS_HIDESIPBUTTON = 0x0008;
        const uint SHFS_SHOWSTARTICON = 0x0010;
        const uint SHFS_HIDESTARTICON = 0x0020;
        [DllImport("aygshell.dll")]
        static extern uint SHFullScreen(IntPtr hwndRequester, uint dwState);

        const uint SIP_UP = 0;
        const uint SIP_DOWN = 1;
        const uint SIP_FORCEDOWN = 2;
        const uint SIP_UNCHANGED = 3;
        const uint SIP_INPUTDIALOG = 4;
        [DllImport("aygshell.dll")]
        static extern uint SHSipPreference(IntPtr hwndRequester, uint dwState);

        const int SW_HIDE = 0;
        const int SW_SHOW = 5;
        [DllImport("coredll.dll", EntryPoint = "FindWindow")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("coredll.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        private static void SIPdown(IntPtr m_hWnd)
        {
            SHSipPreference(m_hWnd, SIP_FORCEDOWN);
        }
        public static void SIPhide(IntPtr m_hWnd)
        {
            SHSipPreference(m_hWnd, SIP_FORCEDOWN);
            SHFullScreen(m_hWnd, SHFS_HIDESIPBUTTON);
            IntPtr hSIP = FindWindow("MS_SIPBUTTON", null);
            ShowWindow(hSIP, SW_HIDE);
        }
    }
    static class ShellAPI
    {

        [DllImport("coredll.dll", SetLastError = true)]
        private static extern int SHGetSpecialFolderPath(IntPtr hWnd, string path, int iCSIDL, bool bCreate);
        /// <summary>
        /// This function retrieves the path of a special folder, identified
        /// by its CSIDL.
        /// </summary>
        /// <param name="hwndOwner">Handle to the owner window the client should
        /// specify if it displays a dialog box or message box.</param>
        /// <param name="lpszPath">Address of a character buffer that receives
        /// the drive and path of the specified folder. This buffer must be at
        /// least MAX_PATH characters in size.</param>
        /// <param name="nFolder"> CSIDL that identifies the folder of interest.
        /// If a virtual folder is specified, this function fails.</param>
        /// <param name="fCreate">Indicates if the folder should be created if
        /// it does not already exist. If this value is nonzero, the folder will
        /// be created. If this value is zero, the folder will not be
        /// created.</param>
        /// <returns>Returns FALSE even if successful.</returns>
        [DllImport("Coredll.dll")]
        static extern int SHGetSpecialFolderPath
        (
            IntPtr hwndOwner,
            StringBuilder lpszPath,
            int nFolder,
            int fCreate
        );

        public static string getMyPicturesFolder()
        {
            StringBuilder sFolder=new StringBuilder(255);
            SHGetSpecialFolderPath(IntPtr.Zero, sFolder, (int)CSIDL.CSIDL_MYPICTURES, 0);
            return sFolder.ToString();
        }
        enum CSIDL{
            CSIDL_ADMINTOOLS        = 0x0030,
            CSIDL_ALTSTARTUP        = 0x001d,
            CSIDL_APPDATA           = 0x001a,
            CSIDL_BITBUCKET         = 0x000a,
            CSIDL_CDBURN_AREA           = 0x003b,
            CSIDL_COMMON_ADMINTOOLS     = 0x002f,
            CSIDL_COMMON_ALTSTARTUP     = 0x001e,
            CSIDL_COMMON_APPDATA        = 0x0023,
            CSIDL_COMMON_DESKTOPDIRECTORY   = 0x0019,
            CSIDL_COMMON_DOCUMENTS      = 0x002e,
            CSIDL_COMMON_FAVORITES      = 0x001f,
            CSIDL_COMMON_MUSIC          = 0x0035,
            CSIDL_COMMON_OEM_LINKS      = 0x003a,
            CSIDL_COMMON_PICTURES       = 0x0036,
            CSIDL_COMMON_PROGRAMS       = 0X0017,
            CSIDL_COMMON_STARTMENU      = 0x0016,
            CSIDL_COMMON_STARTUP        = 0x0018,
            CSIDL_COMMON_TEMPLATES      = 0x002d,
            CSIDL_COMMON_VIDEO          = 0x0037,
            CSIDL_COMPUTERSNEARME       = 0x003d,
            CSIDL_CONNECTIONS           = 0x0031,
            CSIDL_CONTROLS          = 0x0003,
            CSIDL_COOKIES           = 0x0021,
            CSIDL_DESKTOP           = 0x0000,
            CSIDL_DESKTOPDIRECTORY      = 0x0010,
            CSIDL_DRIVES            = 0x0011,
            CSIDL_FAVORITES         = 0x0006,
            CSIDL_FLAG_CREATE           = 0x8000,
            CSIDL_FLAG_DONT_VERIFY      = 0x4000,
            CSIDL_FLAG_MASK         = 0xFF00,
            CSIDL_FLAG_NO_ALIAS         = 0x1000,
            CSIDL_FLAG_PER_USER_INIT    = 0x0800,
            CSIDL_FONTS             = 0x0014,
            CSIDL_HISTORY           = 0x0022,
            CSIDL_INTERNET          = 0x0001,
            CSIDL_INTERNET_CACHE        = 0x0020,
            CSIDL_LOCAL_APPDATA         = 0x001c,
            CSIDL_MYDOCUMENTS           = 0x000c,
            CSIDL_MYMUSIC           = 0x000d,
            CSIDL_MYPICTURES        = 0x0027,
            CSIDL_MYVIDEO           = 0x000e,
            CSIDL_NETHOOD           = 0x0013,
            CSIDL_NETWORK           = 0x0012,
            CSIDL_PERSONAL          = 0x0005,
            CSIDL_PRINTERS          = 0x0004,
            CSIDL_PRINTHOOD         = 0x001b,
            CSIDL_PROFILE           = 0x0028,
            CSIDL_PROGRAM_FILES         = 0x0026,
            CSIDL_PROGRAM_FILES_COMMON      = 0x002b,
            CSIDL_PROGRAM_FILES_COMMONX86   = 0x002c,
            CSIDL_PROGRAM_FILESX86      = 0x002a,
            CSIDL_PROGRAMS          = 0x0002,
            CSIDL_RECENT            = 0x0008,
            CSIDL_RESOURCES         = 0x0038,
            CSIDL_RESOURCES_LOCALIZED       = 0x0039,
            CSIDL_SENDTO            = 0x0009,
            CSIDL_STARTMENU         = 0x000b,
            CSIDL_STARTUP           = 0x0007,
            CSIDL_SYSTEM            = 0x0025,
            CSIDL_SYSTEMX86         = 0x0029,
            CSIDL_TEMPLATES         = 0x0015,
            CSIDL_WINDOWS           = 0x0024,
        }

    }
}
