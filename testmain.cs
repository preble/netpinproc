using System;
using pinproc;

namespace pinproc
{
	
	
	class Test
	{
		static public void Main()
		{
		
			Console.WriteLine("decode {0}", PinProc.PRDecode(MachineType.WPC, "C08"));
			
			Console.WriteLine("PRCreate...");
			IntPtr h = PinProc.PRCreate(MachineType.WPC);
			
			if (h == IntPtr.Zero)
			{
				Console.WriteLine("PRCreate failed: {0}", PinProc.PRGetLastErrorText());
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
			PinProc.PRDMDUpdateConfig(h, ref dmdConfig);
			
			PinProc.PRFlushWriteData(h);
			
			try
			{
				Console.WriteLine("sleep...");
				System.Threading.Thread.Sleep(100);
				Event[] events = new Event[16];
				int numEvents;
				{
					numEvents = PinProc.PRGetEvents(h, events, events.Length);
					Console.WriteLine("got {0} events", numEvents);
					for (int i = 0; i < numEvents; i++)
					{
						Console.WriteLine("  {0}", events[i]);
					}
				}
			}
			finally
			{
				PinProc.PRDelete(h);
			}
			Console.WriteLine("Done.");
		}
	}
}