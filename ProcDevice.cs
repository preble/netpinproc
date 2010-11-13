using System;
using System.Collections.Generic;
namespace pinproc
{
	/// <summary>
	/// Wrapper for netpinproc (libpinproc native interface).
	/// </summary>
	public class ProcDevice
	{
		public IntPtr ProcHandle;
		
		public ProcDevice(MachineType machineType)
		{
			ProcHandle = PinProc.PRCreate(machineType);
			
			if (ProcHandle == IntPtr.Zero)
				throw new InvalidOperationException(PinProc.PRGetLastErrorText());
		}
		
		public void Close()
		{
			if (ProcHandle != IntPtr.Zero)
				PinProc.PRDelete(ProcHandle);
			
			ProcHandle = IntPtr.Zero;
		}
		
		public void Flush()
		{
			PinProc.PRFlushWriteData(ProcHandle);
		}
		
		public List<Event> PollForEvents()
		{
			const int batchSize = 32;
			List<Event> output = new List<Event>(batchSize);
			Event[] events = new Event[batchSize];
			
			while (true)
			{
				int numEvents = PinProc.PRGetEvents(ProcHandle, events, events.Length);
				
				if (numEvents == 0)
					break;
				
				for (int i = 0; i < numEvents; i++)
					output.Add(events[i]);
			}
			return output;
		}
		
		public void TickleWatchdog()
		{
			PinProc.PRDriverWatchdogTickle(ProcHandle);
		}
		
		
		public void PulseDriver(ushort driverNum, byte milliseconds)
		{
			DriverState state = new DriverState();
			state.DriverNum = driverNum;
			PinProc.PRDriverStatePulse(ref state, milliseconds);
			PinProc.PRDriverUpdateState(ProcHandle, ref state);
		}
	}
}

