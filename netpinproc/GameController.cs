using System;
using pinproc;

namespace pinproc
{
	public class GameController
	{
		public ProcDevice Proc;
		protected bool RunLoopRun;
		
		public GameController(MachineType machineType)
		{
		}
		
		public void Shutdown()
		{
			RunLoopRun = false;
			
		}
		
		public void ProcessEvent(Event e)
		{
			Console.WriteLine("Got event: {0}", evt);
		}
		
		public void RunLoop()
		{
			while (RunLoopRun)
			{
				foreach (Event evt in Proc.PollForEvents())
				{
					ProcessEvent(evt);
				}
				Proc.TickleWatchdog();
				Proc.Flush();
			}
		}
	}
}

