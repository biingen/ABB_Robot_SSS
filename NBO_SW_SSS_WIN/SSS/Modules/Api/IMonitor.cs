using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModuleLayer.Api
{
	public interface IMonitor : IDisposable
	{
		string DeviceInstanceId { get; }
		string Description { get; }
		byte DisplayIndex { get; }
		byte MonitorIndex { get; }
		bool IsReachable { get; }

		int Brightness { get; }
		int BrightnessSystemAdjusted { get; }

		AccessResult UpdateBrightness(int brightness = -1);
		AccessResult SetBrightness(int brightness);
	}

	public enum AccessStatus
	{
		None = 0,
		Succeeded,
		Failed,
		DdcFailed,
		NoLongerExist
	}

    public class AccessResult
	{
		public AccessStatus Status { get; }
		public string Message { get; }

        public AccessResult(AccessStatus status, string message)
        {
            this.Status = status;
            this.Message = message;
        }

		public static readonly AccessResult Succeeded = new AccessResult(AccessStatus.Succeeded, null);
		public static readonly AccessResult Failed = new AccessResult(AccessStatus.Failed, null);
	}
}