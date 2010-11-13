# netpinproc

## What is netpinproc?

netpinproc is a C# wrapper for [libpinproc](http://github.com/preble/libpinproc).
The initial version was started on 10/31/2010 and was written by Adam Preble.

### pinproc.cs

PinProc, a C# wrapper class for libpinproc.  Basically a set of P/Invoke methods and structures to make it pretty easy to call libpinproc functions from .Net.  Note that this requires that libpinproc be available as a DLL (on Windows) or a dylib on Mac or Linux.

### ProcDevice.cs

ProcDevice, a wrapper class for PinProc, which makes controlling the P-ROC hardware even smoother in a .Net environment.  Although it is far from complete, it is a useful as an example of how to call PinProc's methods.  If you improve upon it we would be happy to incorporate your changes.

### testmain.cs

A test Main() implementation that demonstrates how to use the ProcDevice class to read events.  Also discusses how drivers (coils, lamps) should be manipulated.


## Status

netpinproc was developed under Mono and has not been tested in an actual Windows .Net environment.  While portions of PinProc have been tested with actual hardware, much of it has not.  Please use care when testing this library on an actual pinball machine.  Please see the license statement below.


## Usage

To run using MonoDevelop, create a new command line application and add the files testmain.cs, pinproc.cs, and ProcDevice.cs.

To compile and run with Mono:

    gmcs testmain.cs pinproc.cs ProcDevice.cs
    mono testmain.exe

If you happen to be running netpinproc under Mac OS X, due to a bug in Mono/glib, you will need to create a .config file to help Mono find libpinproc.dylib.  In .Net style, if your .exe is pinproc.exe, create a pinproc.exe.config with the following content:

    <configuration>
      <dllmap dll="libpinproc" target="libpinproc.dylib" />
    </configuration>

Read more about this [here](http://www.mono-project.com/Interop_with_Native_Libraries#Library_Names).  You may need to add libpinproc.dylib's folder to DYLD\_LIBRARY\_PATH.


## License

The MIT License

Copyright (c) 2010 Adam Preble

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.