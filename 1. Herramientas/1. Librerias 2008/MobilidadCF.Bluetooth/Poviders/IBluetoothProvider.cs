using System;

namespace MovilidadCF.Bluetooth
{
	/// <summary>
	/// Interfaz que define la funcionalidad de los proveedores de
	/// acceso a tarjetas bluetooth, y que permite aislar la capa de interfaz
	/// de usuario del los detalles de implementación del mismo. Que son
	/// diferentes dependiendo del Stack Bluetooth que implemente la tarjeta
	/// bluetooth del dispositivo
	/// </summary>
	public interface IBluetoothProvider
	{
		/// <summary>
		/// Indicada si el proveedor es soportado por el dispositivo
		/// </summary>
		bool IsSupported { get; }

		/// <summary>
		/// Indica si hay o no datos disponibles para leer
		/// </summary>
		bool DataAvailable { get; }

		/// <summary>
		/// Indica si la conexión con el equipo remoto esta o no
		/// abierta
		/// </summary>
		bool IsConnected { get; }

		/// <summary>
		/// Permite cambiar el modo de encendido del dispositivo
		/// </summary>
		PowerMode PowerMode { get; set;}

		/// <summary>
		/// Permite obtener la lista de dispositivos cercanos con los cuales
		/// se puede establecer una conexión Bluetooth.
		/// </summary>
		/// <param name="nMaxDevices"></param>
		/// <param name="nTimeOut"></param>
		/// <returns></returns>
		BluetoothDeviceList DiscoverDevices(int nMaxDevices, int nTimeOut);

		void Connect(BluetoothDevice RemoteDevice);

		/// <summary>
		/// En las clases que implemente esta interfaz, se debe abrir
		/// el modo de escucha para permitir que clientes se pueden 
		/// conectar.
		/// </summary>
		void Listen();

		/// <summary>
		/// Espera hasta que un cliente se conecte para aceptar su 
		/// conexión. Se debe generar una excepción si es cancelado
		/// </summary>
		/// <returns></returns>
		void AcceptClient();


		/// <summary>
		/// Permite cancelar el una operación AceptClient en ejecución, la implementación
		/// debe asegurar que AcceptClient, genere una excepción adecuada.
		/// </summary>
		void CancelAcceptClient();
		
		/// <summary>
		/// Permite enviar el byte pasado al equipo remoto.
		/// </summary>
		/// <param name="c"></param>
		void WriteByte(byte c);

		/// <summary>
		/// Permite enviar los datos pasados al equipo remoto
		/// </summary>
		/// <param name="Data"></param>
		/// <param name="offset"></param>
		/// <param name="len"></param>
		void WriteData(byte[] Data, int offset, int len);

		byte ReadByte();

		int ReadData(byte[] Data, int offset, int len);

		string GetRemoteDeviceName();

		void Close();

		
	}
}
