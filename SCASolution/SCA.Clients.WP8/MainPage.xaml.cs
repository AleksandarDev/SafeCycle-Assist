using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.Rfcomm;
using Windows.Devices.Enumeration;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Networking.Proximity;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace SCA.Clients.WP8
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: Prepare page for display here.

            // TODO: If your application contains multiple pages, ensure that you are
            // handling the hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
            // If you are using the NavigationHelper provided by some templates,
            // this event is handled for you.


			// NOTE: http://developer.nokia.com/community/wiki/Introduction_to_Bluetooth_support_on_Windows_Phone_8#Serial_Port_Profile_.28SSP.29
			// NOTE: http://msdn.microsoft.com/en-us/library/windowsphone/develop/jj207007%28v=vs.105%29.aspx
	        PeerFinder.AlternateIdentities["Bluetooth:PAIRED"] = "";
	        try
	        {
		        var btSelector = BluetoothDevice.GetDeviceSelector();

		        var pairedDevices = await PeerFinder.FindAllPeersAsync();
				if (pairedDevices.Count == 0)
				{
					//return false;  
					System.Diagnostics.Debug.WriteLine("No paired devices");
				}
				else
				{
					System.Diagnostics.Debug.WriteLine("Selecting first device...");
					// Select a paired device. In this example, just pick the first one.
					PeerInformation selectedDevice = pairedDevices[0];

					// Attempt a connection
					var socket = new StreamSocket();

					// Make sure ID_CAP_NETWORKING is enabled in your WMAppManifest.xml, or the next 
					// line will throw an Access Denied exception.
					// In this example, the second parameter of the call to ConnectAsync() is the RFCOMM port number, and can range 
					// in value from 1 to 30.
					string ConnectionChannel = "16";
					System.Diagnostics.Debug.WriteLine("Connecting (ch.{0}) to {1} ({2})", ConnectionChannel, selectedDevice.DisplayName, selectedDevice.Id);
					await socket.ConnectAsync(selectedDevice.HostName, ConnectionChannel);

					// TODO Something useful with streamSocket
					const int DataSize = 2;
					var buffer = new Windows.Storage.Streams.Buffer(DataSize);
					var bufferStream = buffer.AsStream();

					var data = new byte[DataSize] {(byte) 'A', (byte) 'B'};
					bufferStream.Write(data, 0, data.Length);
					bufferStream.Flush();

					System.Diagnostics.Debug.WriteLine("Writing data...");
					await socket.OutputStream.WriteAsync(buffer);
					await socket.OutputStream.FlushAsync();
					System.Diagnostics.Debug.WriteLine("Data sent.");

					socket.Dispose();
				}
			}
			catch (Exception ex)
			{
				if ((uint)ex.HResult == 0x8007048F)
				{
					(new MessageDialog("Bluetooth is turned off")).ShowAsync();
				}
				else System.Diagnostics.Debug.WriteLine(ex.Message);
			}

			//StreamSocket socket = new StreamSocket(); 
			//await socket.ConnectAsync(pi.HostName, "1");
        }
    }
}
