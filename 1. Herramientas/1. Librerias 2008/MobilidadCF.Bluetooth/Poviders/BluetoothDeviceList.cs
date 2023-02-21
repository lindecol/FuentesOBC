using System;
using System.Collections;

namespace MovilidadCF.Bluetooth
{
	/// <summary>
	/// Clase de colección de datos dispositivos bluetooth
	/// </summary>
	public class BluetoothDeviceList : CollectionBase  
	{
		public BluetoothDevice this[ int index ]  
		{
			get  
			{
				return( (BluetoothDevice) List[index] );
			}
			set  
			{
				List[index] = value;
			}
		}

		public int Add( BluetoothDevice value )  
		{
			return( List.Add( value ) );
		}

		public int IndexOf( BluetoothDevice value )  
		{
			return( List.IndexOf( value ) );
		}

		public void Insert( int index, BluetoothDevice value )  
		{
			List.Insert( index, value );
		}

		public void Remove( BluetoothDevice value )  
		{
			List.Remove( value );
		}

		public bool Contains( BluetoothDevice value )  
		{
			// If value is not of type BluetoothDevice, this will return false.
			return( List.Contains( value ) );
		}

		protected override void OnInsert( int index, Object value )  
		{
			if ( value.GetType() != typeof(BluetoothDevice) )
				throw new ArgumentException( "value must be of type BluetoothDevice.", "value" );
		}

		protected override void OnRemove( int index, Object value )  
		{
			if ( value.GetType() != typeof(BluetoothDevice)  )
				throw new ArgumentException( "value must be of type BluetoothDevice.", "value" );
		}

		protected override void OnSet( int index, Object oldValue, Object newValue )  
		{
			if ( newValue.GetType() != typeof(BluetoothDevice)  )
				throw new ArgumentException( "newValue must be of type BluetoothDevice.", "newValue" );
		}

		protected override void OnValidate( Object value )  
		{
			if ( value.GetType() != typeof(BluetoothDevice) )
				throw new ArgumentException( "value must be of type BluetoothDevice." );
		}
	}

}
