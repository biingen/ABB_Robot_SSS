using System;
using System.Text;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Linq;
using ModuleLayer;


namespace Cheese
{
    public class MonitorControl
    {
        public string Name { get; set; }
        public Capabilities Capabilities { get; set; }
        public SafePhysicalMonitorHandle Handle { get; set; }
        public static NativeMethods.MC_VCP_CODE_TYPE type;
        public static uint currentVal, maxVal;

        public static List<MonitorControl> GetMonitors()
        {
            var result = new List<MonitorControl>();

            var windowHandler = NativeMethods.GetDesktopWindow();
            var monitorHandler = NativeMethods.MonitorFromWindow(windowHandler, NativeMethods.MONITOR_DEFAULT.MONITOR_DEFAULTTOPRIMARY);

            uint physicalMonitorCount;
            if (!NativeMethods.GetNumberOfPhysicalMonitorsFromHMONITOR(monitorHandler, out physicalMonitorCount))
                throw new InvalidOperationException($"{nameof(NativeMethods.GetNumberOfPhysicalMonitorsFromHMONITOR)} returned error 0x{Marshal.GetLastWin32Error():X8}");

            var physicalMonitors = new NativeMethods.PHYSICAL_MONITOR[physicalMonitorCount];
            if (!NativeMethods.GetPhysicalMonitorsFromHMONITOR(monitorHandler, physicalMonitorCount, physicalMonitors))
                throw new InvalidOperationException($"{nameof(NativeMethods.GetPhysicalMonitorsFromHMONITOR)} returned error 0x{Marshal.GetLastWin32Error():X8}");

            foreach (var physicalMonitor in physicalMonitors)
                result.Add(new MonitorControl
                {
                    Name = physicalMonitor.szPhysicalMonitorDescription,
                    Handle = new SafePhysicalMonitorHandle(physicalMonitor.hPhysicalMonitor),
                });

            return result;
        }


        public static List<MonitorControl> GetMonitorsAndFeatures()
        {
            var result = new List<MonitorControl>();

            foreach (var monitor in GetMonitors())
            {
                uint length;
                if (!NativeMethods.GetCapabilitiesStringLength(monitor.Handle, out length))
                    throw new InvalidOperationException($"{nameof(NativeMethods.GetCapabilitiesStringLength)} returned error 0x{Marshal.GetLastWin32Error():X8}");

                var capabilitiesString = new StringBuilder((int)length);
                if (!NativeMethods.CapabilitiesRequestAndCapabilitiesReply(monitor.Handle, capabilitiesString, length))
                    throw new InvalidOperationException($"{nameof(NativeMethods.CapabilitiesRequestAndCapabilitiesReply)} returned error 0x{Marshal.GetLastWin32Error():X8}");

                monitor.Capabilities = Capabilities.Parse(capabilitiesString.ToString());
                GetVCPFeatures(monitor.Handle, monitor.Capabilities);
                AddHiddenFeatures(monitor);

                result.Add(monitor);
            }

            return result;
        }


        private static void GetVCPFeatures(SafePhysicalMonitorHandle handle, Capabilities capabilities)
        {
            foreach (var vcpCode in capabilities.VCPCodes)
            {
                if (NativeMethods.GetVCPFeatureAndVCPFeatureReply(handle, vcpCode.Key, out type, out currentVal, out maxVal))
                {
                    vcpCode.Value.Type = type;
                    vcpCode.Value.MaximumValue = maxVal;
                    vcpCode.Value.CurrentValue = currentVal;
                }
                else
                {
                    vcpCode.Value.Error = true;
                }
            }
        }


        private static void AddHiddenFeatures(MonitorControl monitor)
        {
            var сapabilities = monitor.Capabilities;

            var candidates = Enumerable
                .Range(1, 255)
                .Select(x => (byte)x)
                .Where(x => !сapabilities.Commands.Contains(x) && !сapabilities.VCPCodes.Keys.Contains(x));

            foreach (var candidate in candidates)
                if (NativeMethods.GetVCPFeatureAndVCPFeatureReply(monitor.Handle, candidate, out type, out currentVal, out maxVal))
                    сapabilities.VCPCodes.Add(candidate, new Capabilities.VCPCode
                    {
                        Type = type,
                        MaximumValue = maxVal,
                        CurrentValue = currentVal,
                    });
        }
    }
}
