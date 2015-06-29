// Jaime Borges 2015
// AdvancedWebBrowser - Windows Control
// Based on code by Earl McNelly - WBrowser 2015

using System;
using System.IO;
using System.Security;
using System.Windows.Forms;
using Microsoft.Win32;

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
			BrowserEmulationVersion emulationCode;

			int ieVersion = GetIEVersion();

			switch (ieVersion)
			{
				case 11:
					emulationCode = BrowserEmulationVersion.Version11;
					break;

				case 10:
					emulationCode = BrowserEmulationVersion.Version10;
					break;
				case 9:
					emulationCode = BrowserEmulationVersion.Version9;
					break;
				case 8:
					emulationCode = BrowserEmulationVersion.Version8;
					break;
				case 7:
					emulationCode = BrowserEmulationVersion.Version7;
					break;
				default:
					emulationCode = BrowserEmulationVersion.DefaultToHighestInstalledVersion;
					break;
			}

			return SetBrowserEmulationVersion(emulationCode);
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


	}   // end static class IEEmulationHelper

}   // end namespace WBrowser
