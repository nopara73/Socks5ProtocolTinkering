using Nito.AsyncEx;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Socks5ProtocolTinkering
{
	public class TorSocks5Client : IDisposable
	{
		#region PropertiesAndMembers

		public TcpClient TcpClient { get; private set; }

		public IPEndPoint EndPoint { get; private set; }

		public bool IsConnected
		{
			get
			{
				try
				{
					if (TcpClient == null || TcpClient.GetStream() == null)
					{
						return false;
					}

					return TcpClient.Connected;
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex);
					return false;
				}
			}
		}

		private AsyncLock AsyncLock { get; }

		#endregion

		#region ConstructorsAndInitializers

		public TorSocks5Client(IPEndPoint endPoint)
		{
			EndPoint = endPoint ?? throw new ArgumentNullException(nameof(endPoint));
			TcpClient = new TcpClient();
			AsyncLock = new AsyncLock();
		}

		public async Task ConnectAsync()
		{
			using (await AsyncLock.LockAsync())
			{
				await TcpClient.ConnectAsync(EndPoint.Address, EndPoint.Port).ConfigureAwait(false);
			}
		}

		public async Task HandshakeAsync()
		{
			using (await AsyncLock.LockAsync())
			{
				AssertConnected();

				var stream = TcpClient.GetStream();
			}
		}

		public void AssertConnected()
		{
			if (!IsConnected)
			{
				throw new Exception($"{nameof(TorSocks5Client)} is not connected");
			}
		}

		#endregion

		#region IDisposable Support

		private bool _disposedValue = false; // To detect redundant calls

		protected virtual void Dispose(bool disposing)
		{
			if (!_disposedValue)
			{
				if (disposing)
				{
					DisposeTcpClient();
				}

				// TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
				// TODO: set large fields to null.

				_disposedValue = true;
			}
		}

		// TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
		// ~TorSocks5Client() {
		//   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
		//   Dispose(false);
		// }

		// This code added to correctly implement the disposable pattern.
		public void Dispose()
		{
			// Do not change this code. Put cleanup code in Dispose(bool disposing) above.
			Dispose(true);
			// TODO: uncomment the following line if the finalizer is overridden above.
			// GC.SuppressFinalize(this);
		}

		public void DisposeTcpClient()
		{
			try
			{
				TcpClient?.Dispose();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
			}
			finally
			{
				TcpClient = null; // need to be called, .net bug
			}
		}

		#endregion
	}
}
