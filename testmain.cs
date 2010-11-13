// The MIT License
// 
// Copyright (c) 2010 Adam Preble
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using pinproc;

namespace pinproc
{
	class Test
	{
		
		static public void Main()
		{
			// Demonstrate how to create a device, collect some events from it, and shut it down.
			
			// Create a device object and connect to the P-ROC hardware.
			// The device's defaults will be set for the given machine type.
			ProcDevice device = new ProcDevice(MachineType.WPC);
			
			try
			{
				// For testing purposes, in order to have some events to display below,
				// we'll configure the device such that it will generate DMD frame events.
				ConfigureDeviceToGenerateDMDEvents(device);
				
				// Allow some time for some events to pile up:
				System.Threading.Thread.Sleep(100);
				
				// Now ask the device for events and print them out:
				foreach (Event evt in device.PollForEvents())
				{
					Console.WriteLine("  {0}", evt);
				}
				
			}
			finally
			{
				// Be sure to release the device handle before we shut down:
				device.Close();
			}
		}
		
		
		
		static public void ConfigureDeviceToGenerateDMDEvents(ProcDevice device)
		{
			// If this were not a test you would probably configure the DMD in a convenience method
			// on ProcDevice.  To tell the device to begin generating DMD frame events we need only
			// to use the default settings (see DMDConfig(w,h)):
			DMDConfig dmdConfig = new DMDConfig(128, 32);
			PinProc.PRDMDUpdateConfig(device.ProcHandle, ref dmdConfig);
			
			// Flush the configuration data from the software buffer down to the device.
			device.Flush();
		}
		
		
		
		static public void PulseKnocker(ProcDevice device)
		{
			// Demonstrate how to pulse an arbitrary coil.
			// In an actual game we would want to tickle the watchdog and flush the output buffer
			// as part of a run loop, probably after processing switch events, and not after every command.
			// I/O will become very slow if each and every command is flushed immediately.
			
			const int knockerDriverNumber = 8; // this value is made up
			const int milliseconds = 30;
			
			device.TickleWatchdog();
			device.PulseDriver(knockerDriverNumber, milliseconds);
			device.Flush();
		}
		
	}
}