// C# wrapper for libpinproc (http://github.com/preble/libpinproc)
// Initial version written 10/31/2010 by Adam Preble
// 
// To compile and run with Mono:
// 
//   gmcs pinproc.cs testmain.cs
//   mono pinproc.exe
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
	
	[StructLayout(LayoutKind.Sequential),Serializable]
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
	
	[StructLayout(LayoutKind.Sequential),Serializable]
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
	
	[StructLayout(LayoutKind.Sequential, Size=28),Serializable]
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
		
		public override string ToString() { return string.Format("DriverState num={0}", DriverNum); }
	};
	
	// Status: Tested good.
	[StructLayout(LayoutKind.Sequential, Size=12),Serializable]
	public struct Event {
		public EventType Type;
		public UInt32 Value;
		public UInt32 Time;
		
		public override string ToString() { return string.Format("Event type={0} value={1}", Type, Value); }
	};
	
	[StructLayout(LayoutKind.Sequential),Serializable]
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
	
	[StructLayout(LayoutKind.Sequential),Serializable]
	public struct SwitchRule {
		public bool ReloadActive;
		public bool NotifyHost;
	};
	
	// Status: Partial test passed (only checked DeHighCycles)
	[StructLayout(LayoutKind.Sequential, Size=60),Serializable]
	public struct DMDConfig {
		public byte NumRows;
		public UInt16 NumColumns;
		public byte NumSubFrames;
		public byte NumFrameBuffers;
		public bool AutoIncBufferWrPtr;
		public bool EnableFrameEvents;
		public bool Enable;
		[MarshalAs(UnmanagedType.ByValArray, ArraySubType=UnmanagedType.U1, SizeConst = 8)]
		public Byte[] RclkLowCycles;
		[MarshalAs(UnmanagedType.ByValArray, ArraySubType=UnmanagedType.U1, SizeConst = 8)]
		public Byte[] LatchHighCycles;
		[MarshalAs(UnmanagedType.ByValArray, ArraySubType=UnmanagedType.U2, SizeConst = 8)]
		public UInt16[] DeHighCycles;
		[MarshalAs(UnmanagedType.ByValArray, ArraySubType=UnmanagedType.U1, SizeConst = 8)]
		public Byte[] DotclkHalfPeriod;
		
		public DMDConfig(int columns, int rows)
		{
			this.AutoIncBufferWrPtr = true;
			this.NumRows = (byte)rows;
			this.NumColumns = (UInt16)columns;
			this.NumSubFrames = 4;
			this.NumFrameBuffers = 3;
			this.Enable = true;
			this.EnableFrameEvents = true;
			this.RclkLowCycles    = new byte[8];
			this.LatchHighCycles  = new byte[8];
			this.DeHighCycles     = new UInt16[8];
			this.DotclkHalfPeriod = new byte[8];
			for (int i = 0; i < 8; i++)
			{
				this.RclkLowCycles[i] = 15;
				this.LatchHighCycles[i] = 15;
				this.DotclkHalfPeriod[i] = 1;
			}
			// 60 fps timing:
			this.DeHighCycles[0] = 90;
			this.DeHighCycles[1] = 190;
			this.DeHighCycles[2] = 50;
			this.DeHighCycles[3] = 377;
		}
	};
	
	public class PinPROC
	{
		// Status: Good
		[DllImport("libpinproc")]
		private static extern string PRGetLastErrorText();
		
		// Status: Good
		[DllImport("libpinproc")]
		private static extern IntPtr PRCreate(MachineType machineType);
		
		// Status: Good
		[DllImport("libpinproc")]
		private static extern void PRDelete(IntPtr handle);

		[DllImport("libpinproc")]
		private static extern Result PRReset(IntPtr handle, UInt32 flags);

		// Status: Good
		[DllImport("libpinproc")]
		private static extern Result PRFlushWriteData(IntPtr handle);

		// Status: Good
		[DllImport("libpinproc")]
		private static extern Result PRDMDUpdateConfig(IntPtr handle, ref DMDConfig config);

		// Status: Soft tested
		[DllImport("libpinproc")]
		private static extern Result PRDriverGetState(IntPtr handle, byte driverNum, ref DriverState driverState);
		
		// Status: Soft tested
		[DllImport("libpinproc")]
		private static extern Result PRDriverUpdateState(IntPtr handle, ref DriverState driverState);

		[DllImport("libpinproc")]
		private static extern Result PRDriverUpdateGlobalConfig(IntPtr handle, ref DriverGlobalConfig driverGlobalConfig);

		[DllImport("libpinproc")]
		private static extern Result PRDriverWatchdogTickle(IntPtr handle);
		
		// Status: Good
		[DllImport("libpinproc")]
		private static extern int PRGetEvents(IntPtr handle, [In, Out] Event[] events, int maxEvents);
		
		[DllImport("libpinproc")]
		private static extern Result PRSwitchUpdateConfig(IntPtr handle, ref SwitchConfig switchConfig);
		
		[DllImport("libpinproc")]
		private static extern Result PRSwitchUpdateRule(IntPtr handle, byte switchNum, EventType eventType, ref SwitchRule rule, DriverState[] linkedDrivers, int numDrivers);
		
		// Status: Good
		[DllImport("libpinproc")]
		private static extern UInt16 PRDecode(MachineType machineType, string str);
		
		
		
		static public void Test()
		{
			Console.WriteLine("decode {0}", PRDecode(MachineType.WPC, "C08"));
			
			Console.WriteLine("PRCreate...");
			IntPtr h = PRCreate(MachineType.WPC);
			
			if (h == IntPtr.Zero)
			{
				Console.WriteLine("PRCreate failed: {0}", PRGetLastErrorText());
				return;
			}

			Console.WriteLine("PRCreate successful.");
			
			// DriverState state = new DriverState();
			// state.DriverNum = 47;
			// state.PatterEnable = true;
			// PRDriverUpdateState(h, ref state);
			// 
			// state.PatterEnable = false;
			// 
			// PRDriverGetState(h, 47, ref state);
			// Console.WriteLine("get state = {0} patter en = {1}", state, state.PatterEnable);
			
			// Setup the DMD to generate some events:
			DMDConfig dmdConfig = new DMDConfig(128, 32);
			PRDMDUpdateConfig(h, ref dmdConfig);
			
			PRFlushWriteData(h);
			
			try
			{
				Console.WriteLine("sleep...");
				System.Threading.Thread.Sleep(100);
				Event[] events = new Event[16];
				int numEvents;
				{
					numEvents = PRGetEvents(h, events, events.Length);
					Console.WriteLine("got {0} events", numEvents);
					for (int i = 0; i < numEvents; i++)
					{
						Console.WriteLine("  {0}", events[i]);
					}
				}
			}
			finally
			{
				PRDelete(h);
			}
			Console.WriteLine("Done.");
		}
	}
}

