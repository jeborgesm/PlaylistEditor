// Jaime Borges 2015
// AdvancedWebBrowser - Windows Control
// Based on code by Earl McNelly - WBrowser 2015

using System;
using System.IO;
using System.Security;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Diagnostics;

namespace AdvancedWebBrowser
{
	/// <summary>
	/// Known emulation modes according to MSDN.
	/// </summary>
	public enum BrowserEmulationVersion
	{
		/// <summary>
		/// No emulation versions.
		/// </summary>
		EmulationInformationNotAvailable = -1,
		/// <summary>
		/// This will set emulation mode to the highest IE version installed.
		/// </summary>
		/// <remarks>This setting may come in handy if they have a higher version than IE 11 in the future.</remarks>
		DefaultToHighestInstalledVersion = 0,
		/// <summary>
		/// IE 7
		/// </summary>
		Version7 = 7000,
		/// <summary>
		/// IE 8
		/// </summary>
		Version8 = 8000,
		/// <summary>
		/// IE 8
		/// </summary>
		Version8Standards = 8888,
		/// <summary>
		/// IE 9
		/// </summary>
		Version9 = 9000,
		/// <summary>
		/// IE 9
		/// </summary>
		Version9Standards = 9999,
		/// <summary>
		/// IE 10
		/// </summary>
		Version10 = 10000,
		/// <summary>
		/// IE 10
		/// </summary>
		Version10Standards = 10001,
		/// <summary>
		/// IE 11
		/// </summary>
		Version11 = 11000,
		/// <summary>
		/// IE 11
		/// </summary>
		Version11Edge = 11001
	}


	/// <summary>
	/// Methods to help us set the correct IE Emulation mode so our Web Browser control can work with newer techs like HTML5.
	/// </summary>
	public static class IEEmulationHelper
	{
		private const string BrowserEmulationKey = @"Software\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION";

		/// <summary>
		/// Get the emulation version being used from the registry.
		/// </summary>
		/// <returns></returns>
		public static BrowserEmulationVersion GetBrowserEmulationVersion()
		{
			BrowserEmulationVersion result = BrowserEmulationVersion.EmulationInformationNotAvailable;

			try
			{
				RegistryKey key = Registry.CurrentUser.OpenSubKey(BrowserEmulationKey, true);
				if (key != null)
				{
					string programName = Path.GetFileName(Environment.GetCommandLineArgs()[0]);
					object value = key.GetValue(programName, null);

					if (value != null)
					{
						result = (BrowserEmulationVersion)Convert.ToInt32(value);
					}
				}
			}
			catch (SecurityException)
			{
				// The user does not have the permissions required to read from the registry key.
			}
			catch (UnauthorizedAccessException)
			{
				// The user does not have the necessary registry rights.
			}

			return result;
		}

		/// <summary>
		/// Checks to see if emulation has been set or not.
		/// </summary>
		/// <returns>True if it has, otherwise false.</returns>
		public static bool IsBrowserEmulationSet()
		{
			return GetBrowserEmulationVersion() != BrowserEmulationVersion.EmulationInformationNotAvailable;
		}


		/// <summary>
		/// Sets the registry key to the emulation we want to use.
		/// </summary>
		/// <param name="browserEmulationVersion"></param>
		/// <returns>True if successful otherwise False.</returns>
		public static bool SetBrowserEmulationVersion(BrowserEmulationVersion browserEmulationVersion)
		{
			bool result = false;

			try
			{
				RegistryKey key = Registry.CurrentUser.OpenSubKey(BrowserEmulationKey, true);

				if (key != null)
				{
					string programName = Path.GetFileName(Environment.GetCommandLineArgs()[0]);

					#region Determine if we are running vshost, so that we can add both the exe and vshost

					string applicationname = "";
					string vshostversionname = "";

					if (programName.Contains(".vshost.exe"))
					{
						// it's a vshost app name
						vshostversionname = programName;
						applicationname = programName.Replace(".vshost", "");
					}
					else
					{
						// it's an application name
						applicationname = programName;

						int separator = programName.IndexOf('.');
						string tmp = programName.Substring(0, separator);
						vshostversionname = tmp + ".vshost.exe";
					}

					#endregion

					if (browserEmulationVersion != BrowserEmulationVersion.EmulationInformationNotAvailable)
					{
						// if it's a valid value, update or create the value. unless it's a Delete

						key.SetValue(applicationname, (int)browserEmulationVersion, RegistryValueKind.DWord);
						key.SetValue(vshostversionname, (int)browserEmulationVersion, RegistryValueKind.DWord);
					}
					else
					{
						// otherwise, remove the existing value

						key.DeleteValue(applicationname, false);
						key.DeleteValue(vshostversionname, false);
					}

					result = true;
				}
			}
			catch (SecurityException)
			{
				// The user does not have the permissions required to read from the registry key.
			}
			catch (UnauthorizedAccessException)
			{
				// The user does not have the necessary registry rights.
			}

			return result;
		}


		/// <summary>
		/// Attempts to set the emulation version to the highest IE version installed.
		/// </summary>
		/// <returns>True if successful otherwise False.</returns>
		public static bool SetBrowserEmulationVersionAutomatically()
		{
            //Jaime Borges 6/28/2015 - Test new method to prevent JavaScript Errors
            SetBrowserFeatureControl();
            return true;

            //BrowserEmulationVersion emulationCode;

            //int ieVersion = GetIEVersion();

            //switch (ieVersion)
            //{
            //    case 11:
            //        emulationCode = BrowserEmulationVersion.Version11;
            //        break;

            //    case 10:
            //        emulationCode = BrowserEmulationVersion.Version10;
            //        break;
            //    case 9:
            //        emulationCode = BrowserEmulationVersion.Version9;
            //        break;
            //    case 8:
            //        emulationCode = BrowserEmulationVersion.Version8;
            //        break;
            //    case 7:
            //        emulationCode = BrowserEmulationVersion.Version7;
            //        break;
            //    default:
            //        emulationCode = BrowserEmulationVersion.DefaultToHighestInstalledVersion;
            //        break;
            //}

            //return SetBrowserEmulationVersion(emulationCode);
		}


		/// <summary>
		/// Deletes any browser emulation versions.
		/// </summary>
		/// <returns></returns>
		public static bool DeleteBrowserEmulationVersion()
		{
			bool result = false;

			try
			{
				RegistryKey key = Registry.CurrentUser.OpenSubKey(BrowserEmulationKey, true);

				if (key != null)
				{
					string programName = Path.GetFileName(Environment.GetCommandLineArgs()[0]);

					#region Determine if we are running vshost, so that we can add both the exe and vshost

					string applicationname = "";
					string vshostversionname = "";

					if (programName.Contains(".vshost.exe"))
					{
						// it's a vshost app name
						vshostversionname = programName;
						applicationname = programName.Replace(".vshost", "");
					}
					else
					{
						// it's an application name
						applicationname = programName;

						int separator = programName.IndexOf('.');
						string tmp = programName.Substring(0, separator);
						vshostversionname = tmp + ".vshost.exe";
					}

					#endregion

					key.DeleteValue(applicationname, false);
					key.DeleteValue(vshostversionname, false);
					result = true;
				}
			}
			catch (SecurityException)
			{
				// The user does not have the permissions required to read from the registry key.
			}
			catch (UnauthorizedAccessException)
			{
				// The user does not have the necessary registry rights.
			}

			return result;
		}




		/// <summary>
		/// Get the major version number of the users installed IE.
		/// </summary>
		/// <returns>The major version number.</returns>
		public static int GetIEVersion()
		{
			string version = (new WebBrowser()).Version.ToString();

			int result = 0;
			int separator = version.IndexOf('.');

			if (separator != -1)
			{
				int.TryParse(version.Substring(0, separator), out result);
			}

			return result;
		}

        private static void SetBrowserFeatureControlKey(string feature, string appName, uint value)
        {
            using (var key = Registry.CurrentUser.CreateSubKey(
                String.Concat(@"Software\Microsoft\Internet Explorer\Main\FeatureControl\", feature),
                RegistryKeyPermissionCheck.ReadWriteSubTree))
            {
                key.SetValue(appName, (UInt32)value, RegistryValueKind.DWord);
            }
        }

        private static void SetBrowserFeatureControl()
        {
            // http://msdn.microsoft.com/en-us/library/ee330720(v=vs.85).aspx

            // FeatureControl settings are per-process
            var fileName = System.IO.Path.GetFileName(Process.GetCurrentProcess().MainModule.FileName);

            // make the control is not running inside Visual Studio Designer
            if (String.Compare(fileName, "devenv.exe", true) == 0 || String.Compare(fileName, "XDesProc.exe", true) == 0)
                return;

            SetBrowserFeatureControlKey("FEATURE_BROWSER_EMULATION", fileName, GetBrowserEmulationMode()); // Webpages containing standards-based !DOCTYPE directives are displayed in IE10 Standards mode.
            SetBrowserFeatureControlKey("FEATURE_AJAX_CONNECTIONEVENTS", fileName, 1);
            SetBrowserFeatureControlKey("FEATURE_ENABLE_CLIPCHILDREN_OPTIMIZATION", fileName, 1);
            SetBrowserFeatureControlKey("FEATURE_MANAGE_SCRIPT_CIRCULAR_REFS", fileName, 1);
            SetBrowserFeatureControlKey("FEATURE_DOMSTORAGE ", fileName, 1);
            SetBrowserFeatureControlKey("FEATURE_GPU_RENDERING ", fileName, 1);
            SetBrowserFeatureControlKey("FEATURE_IVIEWOBJECTDRAW_DMLT9_WITH_GDI  ", fileName, 0);
            SetBrowserFeatureControlKey("FEATURE_DISABLE_LEGACY_COMPRESSION", fileName, 1);
            SetBrowserFeatureControlKey("FEATURE_LOCALMACHINE_LOCKDOWN", fileName, 0);
            SetBrowserFeatureControlKey("FEATURE_BLOCK_LMZ_OBJECT", fileName, 0);
            SetBrowserFeatureControlKey("FEATURE_BLOCK_LMZ_SCRIPT", fileName, 0);
            SetBrowserFeatureControlKey("FEATURE_DISABLE_NAVIGATION_SOUNDS", fileName, 1);
            SetBrowserFeatureControlKey("FEATURE_SCRIPTURL_MITIGATION", fileName, 1);
            SetBrowserFeatureControlKey("FEATURE_SPELLCHECKING", fileName, 0);
            SetBrowserFeatureControlKey("FEATURE_STATUS_BAR_THROTTLING", fileName, 1);
            SetBrowserFeatureControlKey("FEATURE_TABBED_BROWSING", fileName, 1);
            SetBrowserFeatureControlKey("FEATURE_VALIDATE_NAVIGATE_URL", fileName, 1);
            SetBrowserFeatureControlKey("FEATURE_WEBOC_DOCUMENT_ZOOM", fileName, 1);
            SetBrowserFeatureControlKey("FEATURE_WEBOC_POPUPMANAGEMENT", fileName, 0);
            SetBrowserFeatureControlKey("FEATURE_WEBOC_MOVESIZECHILD", fileName, 1);
            SetBrowserFeatureControlKey("FEATURE_ADDON_MANAGEMENT", fileName, 0);
            SetBrowserFeatureControlKey("FEATURE_WEBSOCKET", fileName, 1);
            SetBrowserFeatureControlKey("FEATURE_WINDOW_RESTRICTIONS ", fileName, 0);
            SetBrowserFeatureControlKey("FEATURE_XMLHTTP", fileName, 1);
        }

        private static  UInt32 GetBrowserEmulationMode()
        {
            int browserVersion = 7;
            using (var ieKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Internet Explorer",
                RegistryKeyPermissionCheck.ReadSubTree,
                System.Security.AccessControl.RegistryRights.QueryValues))
            {
                var version = ieKey.GetValue("svcVersion");
                if (null == version)
                {
                    version = ieKey.GetValue("Version");
                    if (null == version)
                        throw new ApplicationException("Microsoft Internet Explorer is required!");
                }
                int.TryParse(version.ToString().Split('.')[0], out browserVersion);
            }

            UInt32 mode = 10000; // Internet Explorer 10. Webpages containing standards-based !DOCTYPE directives are displayed in IE10 Standards mode. Default value for Internet Explorer 10.
            switch (browserVersion)
            {
                case 7:
                    mode = 7000; // Webpages containing standards-based !DOCTYPE directives are displayed in IE7 Standards mode. Default value for applications hosting the WebBrowser Control.
                    break;
                case 8:
                    mode = 8000; // Webpages containing standards-based !DOCTYPE directives are displayed in IE8 mode. Default value for Internet Explorer 8
                    break;
                case 9:
                    mode = 9000; // Internet Explorer 9. Webpages containing standards-based !DOCTYPE directives are displayed in IE9 mode. Default value for Internet Explorer 9.
                    break;
                case 10:
                    mode = 10000; // Internet Explorer 10. Webpages containing standards-based !DOCTYPE directives are displayed in IE10 mode. Default value for Internet Explorer 10.
                    break;
                default:
                    mode = 11001; // Internet Explorer 11. Webpages containing standards-based !DOCTYPE directives are displayed in IE11 Standards mode. Default value for Internet Explorer 11.
                    break;
            }

            return mode;
        }

	}   // end static class IEEmulationHelper

}   // end namespace AdvancedWebBrowser
