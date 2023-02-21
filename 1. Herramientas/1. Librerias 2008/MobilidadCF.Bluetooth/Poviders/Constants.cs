using System;

namespace MovilidadCF.Bluetooth
{
	public enum DeviceType
	{
		Printer = 1,
		PDA = 2,
		Computer = 3,
		Unknown = 0
	}

	public enum  PowerMode
	{
		Off,
		Connectable,
		Discoverable
	}	
}
