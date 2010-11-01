// C# wrapper for libpinproc (http://github.com/preble/libpinproc)
// Initial version written 10/31/2010 by Adam Preble
// 
// To compile and run with Mono:
// 
//   gmcs PinPROC.cs
//   mono PinPROC.exe
// 
using System;
using System.Runtime.InteropServices;

namespace pinproc
{
	public enum MachineType {
		Invalid = 0,
		Custom = 1,
		WPCAlphanumeric = 2,
		WPC = 3,
		WPC95 = 4,
		SternWhitestar = 5,
		SternSAM = 6
	};
	
	public enum Result { Success = 1, Failure = 0 };
	
	public enum EventType {
		Invalid = 0,
		SwitchClosedDebounced = 1,
		SwitchOpenDebounced = 2,
		SwitchClosedNondebounced = 3,
		SwitchOpenNondebounced = 4,
		DMDFrameDisplayed = 5
	};
	
	public struct DriverGlobalConfig {
		public bool EnableOutputs;
		public bool GlobalPolarity;
		public bool UseClear;
		public bool StrobeStartSelect;
		public byte StarStrobeTime;
		public byte MatrixRowEnableIndex1;
		public byte MatrixRowEnableIndex0;
		public bool ActiveLowMatrixRows;
		public bool EncodeEnables;
		public bool TickleSternWatchdog;
		public bool WatchdogExpired;
		public bool WatchdogEnable;
		public UInt16 WatchdogResetTime;
	};
	
	public struct DriverGroupConfig {
		public byte GroupNum;
		public UInt16 SlowTime;
		public byte EnableIndex;
		public byte RowActivateIndex;
		public byte RowEnableSelect;
		public bool Matrixed;
		public bool Polarity;
		public bool Active;
		public bool DisableStrobeAfter;
	};
	
	public struct DriverState {
		public UInt16 DriverNum;
		public byte OutputDriveTime;
		public bool Polarity;
		public bool State;
		public bool WaitForFirstTimeSlot;
		public UInt32 Timeslots;
		public byte PatterOnTime;
		public byte PatterOffTime;
		public bool PatterEnable;
	};
	
	[StructLayout(LayoutKind.Sequential, Size=12),Serializable]
	public struct Event {
		public EventType Type;
		public UInt32 Value;
		public UInt32 Time;
	};
	
	public struct SwitchConfig {
		public bool Clear;
		public bool HostEventsEnable;
		public bool UseColumn9;
		public bool UseColumn8;
		public byte DirectMatrixScanLoopTime;
		public byte PulsesBeforeCheckingRX;
		public byte InactivePulsesAfterBurst;
		public byte PulsesPerBurst;
		public byte PulseHalfPeriodTime;
	};
	
	public struct SwitchRule {
		public bool ReloadActive;
		public bool NotifyHost;
	};
	
	
	
	class PinPROC
	{
		[DllImport("libpinproc")]
		private static extern IntPtr PRCreate(int machineType);
		
		[DllImport("libpinproc")]
		private static extern void PRDelete(IntPtr handle);

		[DllImport("libpinproc")]
		private static extern Result PRReset(IntPtr handle, UInt32 flags);

		[DllImport("libpinproc")]
		private static extern Result PRFlushWriteData(IntPtr handle);

		[DllImport("libpinproc")]
		private static extern Result PRDriverUpdateGlobalConfig(IntPtr handle, ref DriverGlobalConfig driverGlobalConfig);

		[DllImport("libpinproc")]
		private static extern Result PRDriverWatchdogTickle(IntPtr handle);
		
		[DllImport("libpinproc")]
		private static extern int PRGetEvents(IntPtr handle, [In, Out] Event[] events, int maxEvents);
		
		[DllImport("libpinproc")]
		private static extern Result PRSwitchUpdateConfig(IntPtr handle, ref SwitchConfig switchConfig);
		
		[DllImport("libpinproc")]
		private static extern Result PRSwitchUpdateRule(IntPtr handle, byte switchNum, EventType eventType, ref SwitchRule rule, DriverState[] linkedDrivers, int numDrivers);
		
		[DllImport("libpinproc")]
		private static extern UInt16 PRDecode(MachineType machineType, string str);
		
		
		
		// Test code:
		static public void Main ()
		{
			Console.WriteLine("Initializing...");
			
			Console.WriteLine("decode {0}", PRDecode(MachineType.WPC, "C08"));
			
			IntPtr h = PRCreate(0);
			Event[] events = new Event[16];
			int numEvents = PRGetEvents(h, events, events.Length);
		}
	}
}

