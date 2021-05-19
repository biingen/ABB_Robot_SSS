using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ModuleLayer.Api
{
    /// <summary>
    /// Monitor Configuration Functions
    /// </summary>
    internal class MonitorConfiguration
    {
        static AccessResult acsRes;
        static PhysicalItem phyItem;
        static LPMC_VCP_CODE_TYPE MC_VCP_CODE_TYPE = new LPMC_VCP_CODE_TYPE();
        static AccessStatus acsSta = new AccessStatus();
        #region Win32

        [DllImport("Dxva2.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetNumberOfPhysicalMonitorsFromHMONITOR(SafePhysicalMonitorHandle hMonitor, out uint pdwNumberOfPhysicalMonitors);

        [DllImport("Dxva2.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetPhysicalMonitorsFromHMONITOR(
            SafePhysicalMonitorHandle hMonitor,
            uint dwPhysicalMonitorArraySize,
            [Out] PHYSICAL_MONITOR[] pPhysicalMonitorArray);

        [DllImport("Dxva2.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool DestroyPhysicalMonitors(
            uint dwPhysicalMonitorArraySize,
            [In] PHYSICAL_MONITOR[] pPhysicalMonitorArray);

        [DllImport("Dxva2.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DestroyPhysicalMonitor(
            IntPtr hMonitor);

        [DllImport("Dxva2.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetMonitorCapabilities(
            SafePhysicalMonitorHandle hMonitor,
            out MC_CAPS pdwMonitorCapabilities,
            out MC_SUPPORTED_COLOR_TEMPERATURE pdwSupportedColorTemperatures);

        [DllImport("Dxva2.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetMonitorBrightness(
            SafePhysicalMonitorHandle hMonitor,
            out uint pdwMinimumBrightness,
            out uint pdwCurrentBrightness,
            out uint pdwMaximumBrightness);

        [DllImport("Dxva2.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetMonitorBrightness(
            SafePhysicalMonitorHandle hMonitor,
            uint dwNewBrightness);

        [DllImport("Dxva2.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetCapabilitiesStringLength(
            SafePhysicalMonitorHandle hMonitor,
            out uint pdwCapabilitiesStringLengthInCharacters);

        [DllImport("Dxva2.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool CapabilitiesRequestAndCapabilitiesReply(
            SafePhysicalMonitorHandle hMonitor,

            [MarshalAs(UnmanagedType.LPStr)]
            [Out] StringBuilder pszASCIICapabilitiesString,

            uint dwCapabilitiesStringLengthInCharacters);

        [DllImport("Dxva2.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetVCPFeatureAndVCPFeatureReply(
            SafePhysicalMonitorHandle hMonitor,
            byte bVCPCode,
            //out LPMC_VCP_CODE_TYPE pvct,
            out LPMC_VCP_CODE_TYPE MC_VCP_CODE_TYPE,
            out uint pdwCurrentValue,
            out uint pdwMaximumValue);

        [DllImport("Dxva2.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetVCPFeature(
            SafePhysicalMonitorHandle hMonitor,
            byte bVCPCode,
            uint dwNewValue);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct PHYSICAL_MONITOR
        {
            //public SafePhysicalMonitorHandle hPhysicalMonitor;
            public IntPtr hPhysicalMonitor;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string szPhysicalMonitorDescription;
        }

        [Flags]
        private enum MC_CAPS
        {
            MC_CAPS_NONE = 0x00000000,
            MC_CAPS_MONITOR_TECHNOLOGY_TYPE = 0x00000001,
            MC_CAPS_BRIGHTNESS = 0x00000002,
            MC_CAPS_CONTRAST = 0x00000004,
            MC_CAPS_COLOR_TEMPERATURE = 0x00000008,
            MC_CAPS_RED_GREEN_BLUE_GAIN = 0x00000010,
            MC_CAPS_RED_GREEN_BLUE_DRIVE = 0x00000020,
            MC_CAPS_DEGAUSS = 0x00000040,
            MC_CAPS_DISPLAY_AREA_POSITION = 0x00000080,
            MC_CAPS_DISPLAY_AREA_SIZE = 0x00000100,
            MC_CAPS_RESTORE_FACTORY_DEFAULTS = 0x00000400,
            MC_CAPS_RESTORE_FACTORY_COLOR_DEFAULTS = 0x00000800,
            MC_RESTORE_FACTORY_DEFAULTS_ENABLES_MONITOR_SETTINGS = 0x00001000
        }

        [Flags]
        private enum MC_SUPPORTED_COLOR_TEMPERATURE
        {
            MC_SUPPORTED_COLOR_TEMPERATURE_NONE = 0x00000000,
            MC_SUPPORTED_COLOR_TEMPERATURE_4000K = 0x00000001,
            MC_SUPPORTED_COLOR_TEMPERATURE_5000K = 0x00000002,
            MC_SUPPORTED_COLOR_TEMPERATURE_6500K = 0x00000004,
            MC_SUPPORTED_COLOR_TEMPERATURE_7500K = 0x00000008,
            MC_SUPPORTED_COLOR_TEMPERATURE_8200K = 0x00000010,
            MC_SUPPORTED_COLOR_TEMPERATURE_9300K = 0x00000020,
            MC_SUPPORTED_COLOR_TEMPERATURE_10000K = 0x00000040,
            MC_SUPPORTED_COLOR_TEMPERATURE_11500K = 0x00000080
        }

        private enum LPMC_VCP_CODE_TYPE
        {
            MC_MOMENTARY,
            MC_SET_PARAMETER
        }

        private const byte LuminanceCode = 0x10; // VCP Code of Luminance
        private const byte ContrastCode = 0x12;
        private const byte SpeakerVolumeCode = 0x62;

        #endregion

        #region Type

        [DataContract]
        public class PhysicalItem
        {
            [DataMember(Order = 0)]
            public string Description { get; }

            [DataMember(Order = 1)]
            public int MonitorIndex { get; }

            public SafePhysicalMonitorHandle Handle { get; }

            public bool IsSupported => IsHighLevelSupported || IsLowLevelSupported;

            [DataMember(Order = 2)]
            public bool IsHighLevelSupported { get; }

            [DataMember(Order = 3)]
            public bool IsLowLevelSupported { get; }

            [DataMember(Order = 4)]
            public string CapabilitiesString { get; }

            [DataMember(Order = 5)]
            public string CapabilitiesReport { get; }

            public PhysicalItem(
                string description,
                int monitorIndex,
                SafePhysicalMonitorHandle handle,
                bool isHighLevelSupported,
                bool isLowLevelSupported = false,
                string capabilitiesString = null,
                string capabilitiesReport = null)
            {
                this.Description = description;
                this.MonitorIndex = monitorIndex;
                this.Handle = handle;
                this.IsHighLevelSupported = isHighLevelSupported;
                this.IsLowLevelSupported = isLowLevelSupported;
                this.CapabilitiesString = capabilitiesString;
                this.CapabilitiesReport = capabilitiesReport;
            }
        }

        #endregion

        public static IEnumerable<PhysicalItem> EnumeratePhysicalMonitors(SafePhysicalMonitorHandle monitorHandle, bool verbose = false)
        {
            uint count = 0;
            MC_CAPS caps;
            MC_SUPPORTED_COLOR_TEMPERATURE colorTemp;
            if (!GetNumberOfPhysicalMonitorsFromHMONITOR(monitorHandle, out count))
            {
                //Debug.WriteLine($"Failed to get the number of physical monitors. {Error.GetMessage()}");
                yield break;
            }
            if (count == 0)
            {
                yield break;
            }

            var physicalMonitors = new PHYSICAL_MONITOR[count];

            try
            {
                if (!GetPhysicalMonitorsFromHMONITOR(
                    monitorHandle,
                    count,
                    physicalMonitors))
                {
                    //Debug.WriteLine($"Failed to get an array of physical monitors. {Error.GetMessage()}");
                    yield break;
                }

                int monitorIndex = 0;
                uint capabilitiesStringLength = 0;
                foreach (var physicalMonitor in physicalMonitors)
                {
                    var handle = new SafePhysicalMonitorHandle(physicalMonitor.hPhysicalMonitor);

                    bool isHighLevelSupported = GetMonitorCapabilities(
                        handle,
                        out caps,
                        out colorTemp);

                    bool isLowLevelSupported = false;
                    string capabilitiesString = null;

                    if (!isHighLevelSupported || verbose)
                    {
                        if (GetCapabilitiesStringLength(
                            handle,
                            out capabilitiesStringLength))
                        {
                            var buffer = new StringBuilder((int)capabilitiesStringLength);

                            if (CapabilitiesRequestAndCapabilitiesReply(
                                handle,
                                buffer,
                                capabilitiesStringLength))
                            {
                                capabilitiesString = buffer.ToString();
                                isLowLevelSupported = IsLowLevelSupported(capabilitiesString);
                            }
                        }
                    }

                    //Debug.WriteLine($"Description: {physicalMonitor.szPhysicalMonitorDescription}");
                    //Debug.WriteLine($"Handle: {physicalMonitor.hPhysicalMonitor}");
                    //Debug.WriteLine($"IsHighLevelSupported: {isHighLevelSupported}");
                    //Debug.WriteLine($"IsLowLevelSupported: {isLowLevelSupported}");
                    //Debug.WriteLine($"CapabilitiesString: {capabilitiesString}");

                    yield return new PhysicalItem(
                        description: physicalMonitor.szPhysicalMonitorDescription,
                        monitorIndex: monitorIndex,
                        handle: handle,
                        isHighLevelSupported: isHighLevelSupported,
                        isLowLevelSupported: isLowLevelSupported,
                        capabilitiesString: (verbose ? capabilitiesString : null),
                        capabilitiesReport: (verbose ? MakeCapabilitiesReport(capabilitiesString) : null));

                    monitorIndex++;
                }
            }
            finally
            {
                // The physical monitor handles should be destroyed at a later stage.
            }
        }

        private static bool IsLowLevelSupported(string source)
        {
            return EnumerateVcpCodes(source).Contains(LuminanceCode);
        }

        private static string MakeCapabilitiesReport(string source)
        {
            var codes = EnumerateVcpCodes(source).ToArray();
            return $"Luminance: {codes.Contains(LuminanceCode)}, Contrast: {codes.Contains(ContrastCode)}, Speaker Volume: {codes.Contains(SpeakerVolumeCode)}";
        }

        private static IEnumerable<byte> EnumerateVcpCodes(string source)
        {
            if (string.IsNullOrEmpty(source))
                yield break;

            int index = source.IndexOf("vcp", StringComparison.OrdinalIgnoreCase);
            if (index < 0)
                yield break;

            int depth = 0;
            var buffer = new StringBuilder(2);

            foreach (char c in source.Skip(index + 3))
            {
                switch (c)
                {
                    case '(':
                        depth++;
                        break;
                    case ')':
                        depth--;
                        if (depth < 1)
                        {
                            if (0 < buffer.Length)
                            {
                                yield return byte.Parse(buffer.ToString(), NumberStyles.HexNumber, NumberFormatInfo.InvariantInfo);
                            }
                            yield break; // End of enumeration
                        }
                        break;
                    default:
                        if (depth == 1)
                        {
                            if (IsHexNumber(c))
                            {
                                buffer.Append(c);
                                if (buffer.Length == 1)
                                    continue;
                            }

                            if (0 < buffer.Length)
                            {
                                yield return byte.Parse(buffer.ToString(), NumberStyles.HexNumber, NumberFormatInfo.InvariantInfo);
                                buffer.Clear();
                            }
                        }
                        break;
                }
            }


        }
        //static bool IsHexNumber(char c) => c is (>= '0' and <= '9') or(>= 'A' and <= 'F') or(>= 'a' and <= 'f');
        static bool IsHexNumber(char c)
        {
            bool retVal;
            if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'F') || (c >= 'a' && c <= 'f'))
                retVal = true;
            else
                retVal = false;

            return retVal;
        }
        /// <summary>
        /// Gets raw brightnesses not represented in percentage.
        /// </summary>
        /// <param name="physicalMonitorHandle">Physical monitor handle</param>
        /// <param name="useHighLevel">Whether to use high level function</param>
        /// <returns>
        /// <para>result: Result</para>
        /// <para>minimum: Raw minimum brightness (not always 0)</para>
        /// <para>current: Raw current brightness (not always 0 to 100)</para>
        /// <para>maximum: Raw maximum brightness (not always 100)</para>
        /// </returns>
        /// <remarks>
        /// Raw minimum and maximum brightnesses will become meaningful when they are not standard
        /// values (0 and 100) and so raw current brightness needs to be converted to brightness
        /// in percentage using those values. They are used to convert brightness in percentage
        /// back to raw brightness when settings brightness as well.
        /// </remarks>
        public static AccessResult GetBrightness(SafePhysicalMonitorHandle physicalMonitorHandle, uint curVal, bool useHighLevel = true)
        {
            uint minimumBrightness, currentBrightness, maximumBrightness;
            uint minimumValue, currentValue, maximumValue;
            int errCode = 0;

            if (physicalMonitorHandle == null)
                throw new ArgumentNullException(nameof(physicalMonitorHandle));

            if (physicalMonitorHandle.IsClosed)
            {
                //Debug.WriteLine("Failed to get brightnesses. The physical monitor handle has been closed.");
                //return (result: AccessResult.Failed, 0, 0, 0);
                acsRes = new AccessResult(AccessStatus.Failed, "Failed to get brightnesses");
                return acsRes;
            }

            if (useHighLevel)
            {
                if (GetMonitorBrightness(physicalMonitorHandle, out minimumBrightness, out curVal, out maximumBrightness))
                {
                    /*
                    return (result: AccessResult.Succeeded,
						minimum: minimumBrightness,
						current: currentBrightness,
						maximum: maximumBrightness);*/
                }
                /*var(errorCode, message) = Error.GetCodeMessage();
                Debug.WriteLine($"Failed to get brightnesses (High level). {message}");
                return (result: new AccessResult(GetStatus(errorCode), $"High level, {message}"), 0, 0, 0);*/
                acsSta = GetStatus(errCode);
                acsRes = new AccessResult(acsSta, "Succeed to get brightness");
                return acsRes;
            }
            else
            {
                if (GetVCPFeatureAndVCPFeatureReply(
                    physicalMonitorHandle,
                    LuminanceCode,
                    out MC_VCP_CODE_TYPE,
                    out currentValue,
                    out maximumValue))
                {
                    minimumValue = 0;
                    acsSta = AccessStatus.Succeeded;
                    /*
                    return (result: AccessResult.Succeeded,
						minimum: 0,
						current: currentValue,
						maximum: maximumValue);*/
                }
                /*
                var(errorCode, message) = Error.GetCodeMessage();
                Debug.WriteLine($"Failed to get brightnesses (Low level). {message}");
                return (result: new AccessResult(GetStatus(errorCode), $"Low level, {message}"), 0, 0, 0);*/
                acsSta = GetStatus(errCode);
                acsRes = new AccessResult(acsSta, "Succeed to get brightness");
                return acsRes;
            }
        }

        /// <summary>
        /// Sets raw brightness not represented in percentage.
        /// </summary>
        /// <param name="physicalMonitorHandle">Physical monitor handle</param>
        /// <param name="brightness">Raw brightness (not always 0 to 100)</param>
        /// <param name="useHighLevel">Whether to use high level function</param>
        /// <returns>Result</returns>
        public static AccessResult SetBrightness(SafePhysicalMonitorHandle physicalMonitorHandle, uint brightness, bool useHighLevel = true)
        {
            if (physicalMonitorHandle == null)
                throw new ArgumentNullException(nameof(physicalMonitorHandle));

            if (physicalMonitorHandle.IsClosed)
            {
                Debug.WriteLine("Failed to set brightness. The physical monitor handle has been closed.");
                return AccessResult.Failed;
            }
            
            if (useHighLevel)
            {
                // SetMonitorBrightness function may return true even when it actually failed.
                if (SetMonitorBrightness(physicalMonitorHandle, brightness))
                {
                    acsSta = AccessStatus.Succeeded;
                    //return AccessResult.Succeeded;
                }
                /*
                var(errorCode, message) = Error.GetCodeMessage();
                Debug.WriteLine($"Failed to set brightness (High level). {message}");
                return new AccessResult(GetStatus(errorCode), $"High level {message}");*/
                acsRes = new AccessResult(acsSta, "Succeed to get brightness");
                return acsRes;
            }
            else
            {
                if (SetVCPFeature(physicalMonitorHandle,
                                                LuminanceCode,
                                                brightness))
                {
                    acsSta = AccessStatus.Succeeded;
                    //return AccessResult.Succeeded;
                }
                /*
                var(errorCode, message) = Error.GetCodeMessage();
                Debug.WriteLine($"Failed to set brightness (Low level). {message}");
                return new AccessResult(GetStatus(errorCode), $"Low level, {message}");*/
                acsRes = new AccessResult(acsSta, "Succeed to get brightness");
                return acsRes;
            }
        }

        #region Error

        // Derived from winerror.h
        private const uint ERROR_GRAPHICS_DDCCI_VCP_NOT_SUPPORTED = 0xC0262584;
        private const uint ERROR_GRAPHICS_DDCCI_INVALID_DATA = 0xC0262585;
        private const uint ERROR_GRAPHICS_DDCCI_MONITOR_RETURNED_INVALID_TIMING_STATUS_BYTE = 0xC0262586;
        private const uint ERROR_GRAPHICS_DDCCI_INVALID_MESSAGE_COMMAND = 0xC0262589;
        private const uint ERROR_GRAPHICS_DDCCI_INVALID_MESSAGE_LENGTH = 0xC026258A;
        private const uint ERROR_GRAPHICS_DDCCI_INVALID_MESSAGE_CHECKSUM = 0xC026258B;
        private const uint ERROR_GRAPHICS_MONITOR_NO_LONGER_EXISTS = 0xC026258D;
        private const uint ERROR_GRAPHICS_DDCCI_CURRENT_CURRENT_VALUE_GREATER_THAN_MAXIMUM_VALUE = 0xC02625D8;

        private static AccessStatus GetStatus(int errorCode)
        {
            switch ((uint)errorCode)
            {
                case ERROR_GRAPHICS_DDCCI_VCP_NOT_SUPPORTED:
                case ERROR_GRAPHICS_DDCCI_INVALID_DATA:
                case ERROR_GRAPHICS_DDCCI_INVALID_MESSAGE_COMMAND:
                case ERROR_GRAPHICS_DDCCI_INVALID_MESSAGE_LENGTH:
                case ERROR_GRAPHICS_DDCCI_INVALID_MESSAGE_CHECKSUM:
                    return AccessStatus.DdcFailed;
                    break;
                case ERROR_GRAPHICS_MONITOR_NO_LONGER_EXISTS:
                    return AccessStatus.NoLongerExist;
                    break;
                default:
                    return AccessStatus.Failed;
                    break;
            }

            /*AccessStatus{ None = 0,
                                            Succeeded,
                                            Failed,
                                            DdcFailed,
                                            NoLongerExist }
            */
        }
        #endregion
    }
}